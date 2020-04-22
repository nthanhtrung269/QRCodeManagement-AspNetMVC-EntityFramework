using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRCodeManagement.Common;
using QRCodeManagement.Helpers;

namespace QRCodeManagement.Constants
{
    public static class AppConstants
    {
        public const string CHECKING_QR_URL = "/ck-";
        public const string LOGO_UPLOAD_FOLDER = "/FileUploads/Logos/";
        public const string PDF_UPLOAD_FOLDER = "/FileUploads/PDF/";
        public const string IMG_UPLOAD_FOLDER = "/FileUploads/Images/";
        public const string VCARD_IMG_UPLOAD_FOLDER = "/FileUploads/Images/Vcard/";
        public const string TEMP_VCARD_IMG_UPLOAD_FOLDER = "/FileUploads/Images/Vcard/Temp/";
        public const int NUMBER_QR_PER_PAGE = 5;
        public const int NUMBER_RAW_CHART_ITEM_PER_PAGE = 10;
    }
}