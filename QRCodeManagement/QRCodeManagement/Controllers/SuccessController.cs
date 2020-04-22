using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCodeManagement.Models;

namespace QRCodeManagement.Controllers
{
    public partial class SuccessController : Controller
    {
        // GET: Success
        [ValidateInput(false)]
        public virtual ActionResult Index(string title, string message)
        {
            var msgModels = new MessageModels
            {
                Title = title,
                Message = message
            };
            return View(MVC.Shared.Views.Success, msgModels);
        }
    }
}