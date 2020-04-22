using QRCodeManagement.Constants;
using QRGenerator;
using QRGenerator.Windows.Render;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace QRCodeManagement.Helpers
{
    /// <summary>
    /// Class containing the description of the QR code and wrapping encoding and rendering.
    /// </summary>
    public class CodeDescriptor
    {
        public ErrorCorrectionLevel Ecl;
        public string Content;
        public QuietZoneModules QuietZones;
        public int ModuleSize;
        public BitMatrix Matrix;
        public string ContentType;
        public string Logo;
        public string Background;
        public string Foreground;
        public string FileName;
        public string FileType;

        /// <summary>
        /// Parse QueryString that define the QR code properties
        /// </summary>
        /// <param name="request">HttpRequest containing HTTP GET data</param>
        /// <returns>A QR code descriptor object</returns>
        public static CodeDescriptor Init(HttpRequest request)
        {
            var cp = new CodeDescriptor();

            // Error correction level
            if (!Enum.TryParse(request.QueryString["e"], out cp.Ecl))
                cp.Ecl = ErrorCorrectionLevel.H;

            // Code content to encode
            cp.Content = request.QueryString["c"];
            // Size of the quiet zone
            if (!Enum.TryParse(request.QueryString["q"], out cp.QuietZones))
            {
                cp.QuietZones = QuietZoneModules.Two;
            }

            // Module size
            if (!int.TryParse(request.QueryString["s"], out cp.ModuleSize))
            {
                cp.ModuleSize = 10;
            }

            // Allowed module sizes to avoid memory hacking
            if(cp.ModuleSize != 12 && cp.ModuleSize != 67)
            {
                cp.ModuleSize = 10;
            }

            // Logo
            cp.Logo = request.QueryString["l"];
            // Background color
            cp.Background = request.QueryString["b"];
            // Foreground color
            cp.Foreground = request.QueryString["f"];
            // File name
            cp.FileName = request.QueryString["fn"];
            // File type
            cp.FileType = request.QueryString["ft"];

            return cp;
        }

        /// <summary>
        /// Encode the content with desired parameters and save the generated Matrix
        /// </summary>
        /// <returns>True if the encoding succeeded, false if the content is empty or too large to fit in a QR code</returns>
        public bool TryEncode()
        {
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (!encoder.TryEncode(Content, out qr))
                return false;

            Matrix = qr.Matrix;
            return true;
        }

        /// <summary>
        /// Render the Matrix as a PNG image
        /// </summary>
        /// <param name="ms">MemoryStream to store the image bytes into</param>
        internal void RenderGraphics(MemoryStream ms)
        {
            var bg = new SolidBrush(ColorTranslator.FromHtml(Background));
            var fg = new SolidBrush(ColorTranslator.FromHtml(Foreground));

            var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones), fg, bg);

            if (!string.IsNullOrEmpty(Logo) && File.Exists(HttpContext.Current.Server.MapPath($"~{AppConstants.LOGO_UPLOAD_FOLDER}{Logo}")))
            {
                MemoryStream qrStream = new MemoryStream();
                render.WriteToStream(Matrix, ImageFormat.Png, qrStream);
                Image qrImage = Image.FromStream(qrStream);

                //create a border image of the right size
                int borderWidth = Convert.ToInt32(Math.Floor(qrImage.Width * 0.3));
                int borderHeight = Convert.ToInt32(Math.Floor(qrImage.Height * 0.3));
                Image border = new Bitmap(borderWidth, borderHeight);

                Graphics borderGraphics = Graphics.FromImage(border);
                borderGraphics.FillRectangle(bg, 0, 0, borderWidth, borderHeight);
                //add logo to border image
                Image newLogo = QrHelper.ResizeImage(borderWidth - 10, borderHeight - 10, HttpContext.Current.Server.MapPath($"~{AppConstants.LOGO_UPLOAD_FOLDER}{Logo}"));
                int left = (border.Width / 2) - (newLogo.Width / 2);
                int top = (border.Height / 2) - (newLogo.Height / 2);
                borderGraphics.DrawImage(newLogo, left, top, newLogo.Width, newLogo.Height);

                //add border image to QR code
                int left1 = (qrImage.Width / 2) - (border.Width / 2);
                int top1 = (qrImage.Height / 2) - (border.Height / 2);
                Graphics g = Graphics.FromImage(qrImage);
                g.DrawImage(border, left1, top1, border.Width, border.Height);
                qrImage.Save(ms, ImageFormat.Png);
            }
            else
            {
                render.WriteToStream(Matrix, ImageFormat.Png, ms);
            }

            ContentType = "image/png";
        }

        /// <summary>
        /// Render the Matrix as a PNG image
        /// </summary>
        /// <param name="ms">MemoryStream to store the image bytes into</param>
        internal void RenderSVG(MemoryStream ms)
        {
            Color bgColor = ColorTranslator.FromHtml(Background);
            Color fgColor = ColorTranslator.FromHtml(Foreground);
            GColor bg = new FormColor(Color.FromArgb(255, bgColor.R, bgColor.G, bgColor.B));
            GColor fg = new FormColor(Color.FromArgb(255, fgColor.R, fgColor.G, fgColor.B));

            var render = new SVGRenderer(new FixedModuleSize(ModuleSize, QuietZones), fg, bg);
            render.WriteToStream(Matrix, ms);
            ContentType = "image/svg+xml";
        }
    }
}