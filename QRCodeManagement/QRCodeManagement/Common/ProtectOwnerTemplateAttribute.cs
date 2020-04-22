using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using QRCodeManagement.Helpers;

namespace QRCodeManagement.Common
{
    public class ProtectOwnerTemplateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParams = filterContext.ActionParameters;
            int id = 0;

            foreach (KeyValuePair<string, object> entry in actionParams)
            {
                if (entry.Key == "id")
                {
                    id = (int)entry.Value;
                    break;
                }
            }

            if (id != 0)
            {
                if (!QrHelper.IsOwnerOfTemplate(id, filterContext.HttpContext.User.Identity.GetUserId()) && !filterContext.HttpContext.User.IsInRole("Administrator"))
                {
                    filterContext.Result = new RedirectToRouteResult(
                                                    new RouteValueDictionary
                                                    {
                                                        { "controller", "Error" },
                                                        { "action", "NotFound404" }
                                                    });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}