using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QRCodeManagement.Constants;
using QRCodeManagement.Helpers;

namespace QRCodeManagement.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string RenderQrSource(this HtmlHelper helper, string content, string foreground, string background,
            string logo)
        {
            content = string.IsNullOrEmpty(content) ? "nthanhtrung269@gmail.com" : content;
            return QrHelper.GetQrImageSource( content, foreground, background, logo);
        }

        public static string GetIconClass(this HtmlHelper helper, int type, string filename = "")
        {
            if (type == (int)QrType.File)
            {
                return ((!string.IsNullOrEmpty(filename) && Path.GetExtension(filename).ToLower() == ".pdf") ? "fa-file-pdf-o" : "fa-picture-o");
            }
            else if (type == (int)QrType.VCard)
            {
                return "fa-credit-card";
            }
            return "fa-link";
        }
    }
}