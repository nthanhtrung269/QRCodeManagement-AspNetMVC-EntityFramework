// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace QRCodeManagement.Controllers
{
    public partial class ManageUserController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ManageUserController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> EditAccount()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditAccount);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> DeleteAccount()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteAccount);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ManageUserController Actions { get { return MVC.ManageUser; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "ManageUser";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "ManageUser";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string UserManagement = "UserManagement";
            public readonly string CreateUser = "CreateUser";
            public readonly string EditAccount = "EditAccount";
            public readonly string DeleteAccount = "DeleteAccount";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string UserManagement = "UserManagement";
            public const string CreateUser = "CreateUser";
            public const string EditAccount = "EditAccount";
            public const string DeleteAccount = "DeleteAccount";
        }


        static readonly ActionParamsClass_UserManagement s_params_UserManagement = new ActionParamsClass_UserManagement();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UserManagement UserManagementParams { get { return s_params_UserManagement; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UserManagement
        {
            public readonly string page = "page";
            public readonly string keyword = "keyword";
            public readonly string isBlock = "isBlock";
            public readonly string isConfirm = "isConfirm";
        }
        static readonly ActionParamsClass_CreateUser s_params_CreateUser = new ActionParamsClass_CreateUser();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CreateUser CreateUserParams { get { return s_params_CreateUser; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CreateUser
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_EditAccount s_params_EditAccount = new ActionParamsClass_EditAccount();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_EditAccount EditAccountParams { get { return s_params_EditAccount; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_EditAccount
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_DeleteAccount s_params_DeleteAccount = new ActionParamsClass_DeleteAccount();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteAccount DeleteAccountParams { get { return s_params_DeleteAccount; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteAccount
        {
            public readonly string userId = "userId";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string CreateUser = "CreateUser";
                public readonly string EditAccount = "EditAccount";
                public readonly string UserManagement = "UserManagement";
            }
            public readonly string CreateUser = "~/Views/ManageUser/CreateUser.cshtml";
            public readonly string EditAccount = "~/Views/ManageUser/EditAccount.cshtml";
            public readonly string UserManagement = "~/Views/ManageUser/UserManagement.cshtml";
            static readonly _PartialsClass s_Partials = new _PartialsClass();
            public _PartialsClass Partials { get { return s_Partials; } }
            [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
            public partial class _PartialsClass
            {
                static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
                public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
                public class _ViewNamesClass
                {
                    public readonly string UserListPartial = "UserListPartial";
                }
                public readonly string UserListPartial = "~/Views/ManageUser/Partials/UserListPartial.cshtml";
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ManageUserController : QRCodeManagement.Controllers.ManageUserController
    {
        public T4MVC_ManageUserController() : base(Dummy.Instance) { }

        [NonAction]
        partial void UserManagementOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int page, string keyword, bool? isBlock, bool? isConfirm);

        [NonAction]
        public override System.Web.Mvc.ActionResult UserManagement(int page, string keyword, bool? isBlock, bool? isConfirm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UserManagement);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "page", page);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "keyword", keyword);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "isBlock", isBlock);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "isConfirm", isConfirm);
            UserManagementOverride(callInfo, page, keyword, isBlock, isConfirm);
            return callInfo;
        }

        [NonAction]
        partial void CreateUserOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult CreateUser()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreateUser);
            CreateUserOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CreateUserOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, QRCodeManagement.Models.ViewModels.CreateUserViewModel model);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> CreateUser(QRCodeManagement.Models.ViewModels.CreateUserViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreateUser);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            CreateUserOverride(callInfo, model);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void EditAccountOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> EditAccount(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditAccount);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            EditAccountOverride(callInfo, id);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void DeleteAccountOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string userId);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> DeleteAccount(string userId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteAccount);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userId", userId);
            DeleteAccountOverride(callInfo, userId);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114