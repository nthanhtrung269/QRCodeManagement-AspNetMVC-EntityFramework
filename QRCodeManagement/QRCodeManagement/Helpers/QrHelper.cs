using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCodeManagement.Constants;
using System;
using QRCodeManagement.Services;

namespace QRCodeManagement.Helpers
{
    public class QrHelper
    {
        /// <summary>
        /// Get the string of QR handler url
        /// </summary>
        /// <param name="content"></param>
        /// <param name="foreground"></param>
        /// <param name="background"></param>
        /// <param name="logo"></param>
        /// <returns></returns>
        public static string GetQrImageSource(string content, string foreground, string background, string logo)
        {
            return $"/webhandler/getcode.ashx?c={content}&b={background.Replace("#","%23")}&f={foreground.Replace("#", "%23")}&l={logo}";
        }

        public static QrFileType GetFileTypeFromFileName(string filename)
        {
            return (!string.IsNullOrEmpty(filename) && Path.GetExtension(filename).ToLower() == ".pdf" ? QrFileType.PDF : QrFileType.Image);
        }

        public static bool IsOwnerOfQr(string qrId, string userId)
        {
            var qrCode = new QrCodeService().GetQrCodeById(qrId);
            return qrCode != null && qrCode.UserId.Equals(userId);
        }

        public static bool IsOwnerOfTemplate(int templateId, string userId)
        {
            var template = new TemplateService().GetQrTemplateById(templateId);
            return template != null && template.UserId.Equals(userId);
        }

        public static bool IsOwnerOfLogo(int logoId, string userId)
        {
            var logo = new LogoService().GetLogoById(logoId);
            return logo != null && logo.UserId.Equals(userId);
        }

        public static Image ResizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            Image imgPhoto = Image.FromFile(stPhotoPath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

        public static long GetJavascriptTimestamp(DateTime input)
        {
            TimeSpan span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            DateTime time = input.Subtract(span);
            return (long)(time.Ticks / 10000);
        }
    }
}