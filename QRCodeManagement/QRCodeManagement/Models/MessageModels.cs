using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRCodeManagement.Models
{
    public class MessageModels
    {
        public string Title { get; set; }

        [AllowHtml]
        public string Message { get; set; }
    }
}