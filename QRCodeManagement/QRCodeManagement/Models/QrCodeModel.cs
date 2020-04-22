using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QRCodeManagement.Common;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.Models
{
    public class QrCodeModel
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string TargetUrl { get; set; }

        public string StatisticsUrl { get; set; }

        public string Foreground { get; set; }

        public string Background { get; set; }

        public string EmbedLogo { get; set; }

        public System.DateTime CreatedDate => DateTime.Now;

        public string UserId { get; set; }

        public int? TemplateId { get; set; }

        public string Filename { get; set; }

        public byte QrType { get; set; }

        public bool IsArchived { get; set; }

        public int? EmbedLogoId { get; set; }

        public int? SelectedLogoId { get; set; }

        public int? SelectedTemplateId { get; set; }

        public IList<Logo> Logos { get; set; }

        public IList<Template> Templates { get; set; }

    }
}
