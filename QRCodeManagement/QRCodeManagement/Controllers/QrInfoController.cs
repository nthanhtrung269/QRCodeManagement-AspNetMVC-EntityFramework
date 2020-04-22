using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCodeManagement.Constants;
using QRCodeManagement.Extensions;
using QRCodeManagement.Helpers;
using QRCodeManagement.Services;

namespace QRCodeManagement.Controllers
{
    public partial class QrInfoController : Controller
    {
        private readonly IQrCodeService _qrManagementService;
        private readonly IVCardDetailService _qrVCardDetailService;

        public QrInfoController()
        {
            _qrManagementService = new QrCodeService();
            _qrVCardDetailService = new VCardDetailService();
        }

        public virtual ActionResult ViewFile(string id)
        {
            var qrCode = _qrManagementService.GetQrCodeById(id);
            var fileFolder = AppConstants.PDF_UPLOAD_FOLDER;
            var contentType = "application/pdf";
            if (qrCode.IsArchived)
            {
                return RedirectToAction(MVC.Error.NotFound404());
            }
            if (QrHelper.GetFileTypeFromFileName(qrCode.Filename) == QrFileType.Image)
            {
                fileFolder = AppConstants.IMG_UPLOAD_FOLDER;
                contentType = $"image/{Path.GetExtension(qrCode.Filename).RemoveNonAlphaNumeric()}";
            }
            var filePath = Server.MapPath($"~{fileFolder}{qrCode.Filename}");

            if (!System.IO.File.Exists(filePath))
            {
                return RedirectToAction(MVC.Error.NotFound404());
            }

            return File(filePath, contentType);
        }

        [HttpGet]
        public virtual ActionResult ViewVCard(string id)
        {
            var vCardDetail = _qrVCardDetailService.GetVCardDetailByQrId(id);
            return View(MVC.QrManagement.Views.ViewVCard, vCardDetail);
        }
    }
}