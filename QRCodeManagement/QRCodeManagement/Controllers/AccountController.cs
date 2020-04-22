using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using QRCodeManagement.Models;
using QRCodeManagement.Models.ViewModels;

namespace QRCodeManagement.Controllers
{
    [Authorize]
    public partial class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(MVC.QrManagement.Manage());
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (Session["CaptchaResult"] == null || Session["CaptchaResult"].ToString() != model.Captcha)
            {
                ModelState.AddModelError("Captcha", "The captcha inputed result does not match");
                //dispay error and generate a new captcha 
                return View(MVC.Account.Views.Login, model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ViewBag.Title = "An Error Occur";
                    ViewBag.errorMessage = "You must have a confirmed email to log on.";
                    return View(MVC.Shared.Views.ErrorExternal);
                }
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            // This doesn't count login failures towards account lockout
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    await SignInManager.UpdateLastLogin(user);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    ViewBag.Title = "Locked Account";
                    ViewBag.errorMessage = "This account has been locked. Please contact the system administrator.";
                    return View(MVC.Shared.Views.ErrorExternal);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Login failed! Wrong email or password.");
                    return View(MVC.Account.Views.Login, model);
            }
        }

        public virtual async Task<ActionResult> MyProfile()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var identityModel = new ApplicationUser
            {
                CreatedDate = user.CreatedDate,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LastLogin = user.LastLogin
            };
            return View(MVC.Account.Views.MyProfile, identityModel);
        }

        public virtual ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(MVC.Account.Views.ChangePassword, model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction(MVC.Success.Index("Update password confirmation", "Your password was updated"));
            }
            AddErrors(result);
            return View(MVC.Account.Views.ChangePassword,model);
        }

        public virtual async Task<ActionResult> UpdateMyProfile(MyProfileViewModel userModel)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;

            try
            {
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(MVC.Success.Index("Update profile confirmation", "Your profile was updated"));
                }
            }
            catch (Exception e){}
            return RedirectToAction(MVC.Error.ErrorMsg());
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public virtual async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View(MVC.Shared.Views.ErrorExternal);
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View(MVC.Account.Views.ConfirmEmail);
            }

            return RedirectToAction(MVC.Error.Error500());
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
            return View(MVC.Account.Views.ForgotPassword);
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(MVC.Account.ForgotPasswordConfirmation());
                }

                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request?.Url?.Scheme);

                string emailContent = $"<p>Dear {user.FirstName} {user.LastName}, </p> You requested forgot password on QR Code System. " +
                                          "<p>Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a></p>" +
                                          "</br><p>Thanks,</p>QR Code System";

                await UserManager.SendEmailAsync(user.Id, "Reset Password Request ", emailContent);

                return RedirectToAction(MVC.Account.ForgotPasswordConfirmation());
            }
            return RedirectToAction(MVC.Account.Login());
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ForgotPasswordConfirmation()
        {
            return View(MVC.Account.Views.ForgotPasswordConfirmation);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View(MVC.Shared.Views.ErrorExternal);
            }
            return View(MVC.Account.Views.ResetPassword);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(MVC.Account.Views.ResetPassword, model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(MVC.Account.ResetPasswordConfirmation());
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(MVC.Account.ResetPasswordConfirmation());
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction(MVC.Account.Login());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Equals("Incorrect password."))
                {
                    ModelState.AddModelError("", "Current password is incorrect.");
                    continue;
                }
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(MVC.QrManagement.Manage());
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}