using System;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Web.SessionState;

namespace QRCodeManagement.WebHandler
{
    /// <summary>
    /// Summary description for GetCaptcha
    /// </summary>
    public class GetCaptcha : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = $"{a} + {b} = ?";

            //store answer 
            context.Session["CaptchaResult"] = a + b;

            //image stream 
            //FileContentResult img = null;

            int imgW = 150;
            int imgH = 35;
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(imgW, imgH))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise 
                int i, r, x, y;
                var pen = new Pen(Color.Yellow);
                for (i = 1; i < 30; i++)
                {
                    pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                    r = rand.Next(0, (imgW / 3));
                    x = rand.Next(0, imgW);
                    y = rand.Next(0, imgH);
                    gfx.DrawEllipse(pen, x - r, y - r, r, r);
                }
                //add question 
                gfx.DrawString(captcha, new Font("Tahoma", 18), Brushes.DarkGreen, 2, 2);

                //render as Jpeg 
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                context.Response.OutputStream.Write(mem.GetBuffer(), 0, (int)mem.Length);
                //img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            //return img;
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            using (var ms = new MemoryStream())
            {
                context.Response.ContentType = "image/Jpeg";
                context.Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}