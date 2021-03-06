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
    public partial class ErrorController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ErrorController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ErrorController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult ErrorMsg()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ErrorMsg);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ErrorController Actions { get { return MVC.Error; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Error";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Error";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string NotFound404 = "NotFound404";
            public readonly string Error500 = "Error500";
            public readonly string ErrorMsg = "ErrorMsg";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string NotFound404 = "NotFound404";
            public const string Error500 = "Error500";
            public const string ErrorMsg = "ErrorMsg";
        }


        static readonly ActionParamsClass_ErrorMsg s_params_ErrorMsg = new ActionParamsClass_ErrorMsg();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ErrorMsg ErrorMsgParams { get { return s_params_ErrorMsg; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ErrorMsg
        {
            public readonly string title = "title";
            public readonly string message = "message";
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
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ErrorController : QRCodeManagement.Controllers.ErrorController
    {
        public T4MVC_ErrorController() : base(Dummy.Instance) { }

        [NonAction]
        partial void NotFound404Override(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult NotFound404()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NotFound404);
            NotFound404Override(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void Error500Override(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Error500()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Error500);
            Error500Override(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ErrorMsgOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string title, string message);

        [NonAction]
        public override System.Web.Mvc.ActionResult ErrorMsg(string title, string message)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ErrorMsg);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "title", title);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "message", message);
            ErrorMsgOverride(callInfo, title, message);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
