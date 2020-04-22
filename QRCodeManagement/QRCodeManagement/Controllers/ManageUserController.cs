using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using QRCodeManagement.Common;
using QRCodeManagement.Models;
using QRCodeManagement.Models.ViewModels;
using QRCodeManagement.Services;
using Webdiyer.WebControls.Mvc;

namespace QRCodeManagement.Controllers
{
    [Authorize(Roles = "Administrator")]
    public partial class ManageUserController : Controller
    {
        private ApplicationUserManager _userManager;

        public ManageUserController()
        {
        }

        public ManageUserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        public virtual ActionResult UserManagement(int page = 1, string keyword = "", bool? isBlock = null, bool? isConfirm = null)
        {
            var context = new AdminDbContext();
            // Get all users exclude supper admin
            var allUsers = context.Users.Where(x => !x.Roles.Select(r => r.RoleId).Contains("admin")).ToList();

            //Filter by blocked
            if (isBlock != null)
            {
                allUsers = allUsers.Where(col => col.LockoutEndDateUtc.HasValue == isBlock).ToList();
            }

            //Filter by email confirmed
            if (isConfirm != null)
            {
                allUsers = allUsers.Where(col => col.EmailConfirmed == isConfirm).ToList();
            }

            //filter by email
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                allUsers = allUsers.Where(col => col.Email.ToLower().Contains(keyword.ToLower())).ToList();
            }

            var model = allUsers.ToPagedList(page, 15);

            if (Request.IsAjaxRequest())
                return PartialView(MVC.ManageUser.Views.Partials.UserListPartial, model);

            return View(MVC.ManageUser.Views.UserManagement, model);
        }

        /// <summary>
        /// Block or unblock account
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="isBlock"></param>
        /// <returns></returns>
        public Task<IdentityResult> BlockUnBlockAccount(string id, bool isBlock)
        {
            Task<IdentityResult> result;
            if (isBlock)
            {
                result = UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.MaxValue);
            }
            else
            {
                var user = UserManager.FindById(id);
                result = UserManager.ResetLockOutEndDateUtc(user);
            }
            
            return result;
        }

        public virtual async Task<bool> ResendComfirmationEmail(string id)
        {
            string newPassword = RandomString.GeneratePassword();
            var user = UserManager.FindById(id);
            var removePassResult = UserManager.RemovePassword(id);

            if (removePassResult.Succeeded)
            {
                if (UserManager.AddPassword(id, newPassword).Succeeded)
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code = code},
                        protocol: Request.Url.Scheme);
                    string emailContent =
                        $"<p>Dear {user.FirstName} {user.LastName}, </p> You have been added (by admin) as a user of the QR Code System. " +
                        $"<p>You must have a confirmed email to log on by clicking <a href='{callbackUrl}'>here</a>. Then you can login to system.</p>" +
                        $"<p><b>Your account:<b></p> <p>Email: {user.Email}<p><p>Password: {newPassword}</p></br><p>Thanks,</p>QR Code System";
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", emailContent);

                    return true;
                }
            }

            return false;
        }

        public async Task<string> ResetPassword(string id)
        {
            string newPassword = RandomString.GeneratePassword();
            var user = UserManager.FindById(id);
            var removePassResult = UserManager.RemovePassword(id);

            if (removePassResult.Succeeded)
            {
                if (UserManager.AddPassword(id, newPassword).Succeeded)
                {
                    string emailContent =
                        $"<p>Dear {user.FirstName} {user.LastName}, </p> Your password is reset by Administrator on QR Code System. " +
                        $"<p><b>Account:<b></p> <p>Email: {user.Email}<p><p>New password: {newPassword}</p></br><p>Thanks,</p>QR Code System";
                    await UserManager.SendEmailAsync(user.Id, "Password reset", emailContent);
                    return newPassword;
                }
            }
            return string.Empty;
        }

        public virtual ActionResult CreateUser()
        {
            return View(MVC.ManageUser.Views.CreateUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedDate = model.CreatedDate
                };
                model.Password = RandomString.GeneratePassword();
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    string emailContent = $"<p>Dear {user.FirstName} {user.LastName}, </p> You have been added (by admin) as a user of the QR Code System. " +
                                          $"<p>You must have a confirmed email to log on by clicking <a href='{callbackUrl}'>here</a>. Then you can login to system.</p>" +
                                          $"<p><b>Your account:<b></p> <p>Email: {user.Email}<p><p>Password: {model.Password}</p></br><p>Thanks,</p>QR Code System";
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", emailContent);
                    var msg = $"User <b>{model.Email}</b> was created, an email contain randomly password and confirmation link is sent to user's mail box";
                    return RedirectToAction(MVC.Success.Index("Create User Confirmation", msg));
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(MVC.ManageUser.Views.CreateUser, model);
        }

        public virtual async Task<ActionResult> EditAccount(string id)
        {
            ViewBag.CanDelete = false;
            var user = await UserManager.FindByIdAsync(id);
            var qrCodeService = new QrCodeService();
            var templateService = new TemplateService();
            var logoService = new LogoService();

            if (!user.EmailConfirmed ||
                (!templateService.GetQrTemplateByUser(id).Any()
                && !logoService.GetLogoByUser(id).Any()
                && !qrCodeService.GetQrCodeByUser(id).Any()))
            {
                ViewBag.CanDelete = true;
            }
           
            return View(MVC.ManageUser.Views.EditAccount, user);
        }

        [HttpPost]
        public virtual async Task<ActionResult> DeleteAccount(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var result = await UserManager.DeleteAsync(user);
            return result.Succeeded ? RedirectToAction(MVC.ManageUser.UserManagement()) 
                : RedirectToAction(MVC.Error.ErrorMsg("Cannot delete user", "Has an error when delete user. Please try again"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
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
                ModelState.AddModelError("", error);
            }
        }
        #endregion
    }
}