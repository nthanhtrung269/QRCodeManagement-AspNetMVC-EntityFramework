using QRCodeManagement.Constants;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Services;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;

namespace QRCodeManagement.WebHandler
{
    /// <summary>
    /// HTTP Handler outputing vCard file
    /// </summary>
    public class DownloadVCard : IHttpHandler
    {
        private readonly IVCardDetailService _qrVCardDetailService;

        public DownloadVCard()
            : this(null)
        {
        }

        public DownloadVCard(
            IVCardDetailService qrVCardDetailService)
        {
            _qrVCardDetailService = qrVCardDetailService ?? new VCardDetailService();
        }

        public void ProcessRequest(HttpContext context)
        {
            string qrId = context.Request.QueryString["qrId"];
            var vCardDetail = _qrVCardDetailService.GetVCardDetailByQrId(qrId);
            string content = GetVCardContent(vCardDetail);
            var fileName = $"vcard_{vCardDetail.FirstName}_{vCardDetail.LastName}.vcf";

            context.Response.ClearContent();
            context.Response.Clear();
            context.Response.ContentType = "text/x-vcard";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
            context.Response.Write(content);
            context.Response.Flush();
            context.Response.End();
        }

        /// <summary>
        /// Return true to indicate that the handler is thread safe because it is stateless
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        private string GetVCardContent(VCardDetail vCardDetail)
        {
            bool hasProfileImage = false;
            Image profileImage = null;

            if (!string.IsNullOrEmpty(vCardDetail.ProfileImage) && File.Exists(HttpContext.Current.Server.MapPath($"{AppConstants.VCARD_IMG_UPLOAD_FOLDER}{vCardDetail.ProfileImage}")))
            {
                hasProfileImage = true;
                profileImage = Image.FromFile(HttpContext.Current.Server.MapPath($"{AppConstants.VCARD_IMG_UPLOAD_FOLDER}{vCardDetail.ProfileImage}"));
            }

            var sb = new StringBuilder();
            sb.Append("BEGIN:VCARD\r\n");
            sb.Append("VERSION:3.0\r\n");
            sb.Append($"{vCardDetail.LastName};{vCardDetail.FirstName};;\r\n");
            sb.Append($"FN:{vCardDetail.FirstName} {vCardDetail.LastName}\r\n");
            sb.Append($"ORG:{vCardDetail.Company}\r\n");
            sb.Append($"TITLE:{vCardDetail.JobTitle}\r\n");
            sb.Append($"TEL;TYPE=WORK,VOICE:{vCardDetail.MobileNumber}\r\n");
            sb.Append($"TEL;TYPE=HOME,VOICE:{vCardDetail.Phone}\r\n");
            sb.Append($"ADR;TYPE=WORK:;;{vCardDetail.Street};{vCardDetail.City};{vCardDetail.State};{vCardDetail.Zipcode};{vCardDetail.Country}\r\n");
            sb.Append($"EMAIL; TYPE = PREF,INTERNET:{vCardDetail.Email}\r\n");
            if (hasProfileImage)
            {
                sb.Append($"PHOTO;TYPE=JPG;ENCODING=B:{ImageToString(profileImage)}\r\n");
            }
            sb.Append("REV:2008-04-24T19:52:43Z\r\n");
            sb.Append("END:VCARD\r\n");

            return sb.ToString();
        }

        private string ImageToString(Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}