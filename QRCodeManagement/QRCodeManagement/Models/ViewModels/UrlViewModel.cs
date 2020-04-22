using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QRCodeManagement.Common;

namespace QRCodeManagement.Models.ViewModels
{
    public class UrlViewModel : QrCodeModel
    {
        public string UrlText { get; set; }
    }
}
