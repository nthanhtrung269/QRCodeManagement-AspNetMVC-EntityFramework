using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRCodeManagement.Models
{
    public class TemplateModel
    {
        public int Id { get; set; }
        public string Foreground { get; set; }
        public string Background { get; set; }
        public string EmbedLogo { get; set; }
        public string UserId { get; set; }
    }
}