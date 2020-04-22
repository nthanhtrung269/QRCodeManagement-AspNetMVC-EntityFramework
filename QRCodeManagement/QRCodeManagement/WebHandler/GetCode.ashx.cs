using QRCodeManagement.Helpers;
using System.IO;
using System.Net;
using System.Web;

namespace QRCodeManagement.WebHandler
{
    /// <summary>
    /// HTTP Handler outputing QR codes as PNG images
    /// </summary>
    public class GetCode : IHttpHandler
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

            // Render the QR code as an image
            using (var ms = new MemoryStream())
            {
                codeParams.RenderGraphics(ms);
                context.Response.ContentType = codeParams.ContentType;
                context.Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
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