using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCodeManagement.Models;

namespace QRCodeManagement.Controllers
{
    public partial class ErrorController : Controller
    {
        // GET: Error
        public virtual ActionResult NotFound404()
        {
            //Response.StatusCode = 404;
            return View(MVC.Shared.Views.ErrorExternal);
        }

        public virtual ActionResult Error500()
        {
            //Response.StatusCode = 500;
            ViewBag.Title = "Error Page";
            ViewBag.errorMessage = "There are error occur. Please try again";
            return View(MVC.Shared.Views.ErrorExternal);
        }

        public virtual ActionResult ErrorMsg(string title, string message)
        {
            var msgModels = new MessageModels
            {
                Title = title,
                Message = message
            };
            return View(MVC.Shared.Views.ErrorMsg, msgModels);
        }
    }
}