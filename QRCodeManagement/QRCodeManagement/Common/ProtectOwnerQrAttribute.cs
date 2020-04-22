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
    public class ProtectOwnerQrAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParams = filterContext.ActionParameters;
            string id = string.Empty;

            foreach (KeyValuePair<string, object> entry in actionParams)
            {
                if (entry.Key == "id" || entry.Key == "qrId")
                {
                    id = entry.Value.ToString();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(id))
            {
                if (!QrHelper.IsOwnerOfQr(id, filterContext.HttpContext.User.Identity.GetUserId()) && !filterContext.HttpContext.User.IsInRole("Administrator"))
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