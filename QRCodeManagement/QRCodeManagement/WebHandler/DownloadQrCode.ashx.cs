using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Helpers;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace QRCodeManagement.WebHandler
{
    /// <summary>
    /// HTTP Handler outputing vCard file
    /// </summary>
    public class DownloadQrCode : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // Retrieve the parameters from the QueryString
            var codeParams = CodeDescriptor.Init(context.Request);

            // Encode the content
            if (codeParams == null || !codeParams.TryEncode())
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            using (var ms = new MemoryStream())
            {
                if (codeParams.FileType == "png")
                {
                    codeParams.RenderGraphics(ms);
                }
                else if (codeParams.FileType == "svg")
                {
                    codeParams.RenderSVG(ms);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;
                }

                context.Response.ClearContent();
                context.Response.Clear();
                context.Response.ContentType = codeParams.ContentType;
                var fileName = !string.IsNullOrEmpty(codeParams.FileName) ? $"{codeParams.FileName}_{ DateTime.Now.ToShortTimeString()}" : "qr_code_download";
                fileName = $"{fileName}.{codeParams.FileType}";
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
                context.Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
                context.Response.Flush();
                context.Response.End();
            }
        }

        /// <summary>
        /// Return true to indicate that the handler is thread safe because it is stateless
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }
    }
}