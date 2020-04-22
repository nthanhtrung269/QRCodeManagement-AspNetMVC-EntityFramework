using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using QRCodeManagement.Common;

namespace QRCodeManagement.Models.ViewModels
{
    public class FileViewModel : QrCodeModel
    {
        public string FileType { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
    }
}
