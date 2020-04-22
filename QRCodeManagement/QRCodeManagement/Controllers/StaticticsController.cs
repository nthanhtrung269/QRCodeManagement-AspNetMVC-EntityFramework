using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRCodeManagement.Controllers
{
    public partial class StaticticsController : Controller
    {
        // GET: Statictics
        public virtual ActionResult Checking(string id)
        {
            return View();
        }
    }
}
