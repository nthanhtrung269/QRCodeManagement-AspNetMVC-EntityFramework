using System;
using System.Collections.Generic;
using QRCodeManagement.Constants;
using QRCodeManagement.EntityManager.DbAccess;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;


namespace QRCodeManagement.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly IQrCodeManager _qrCodeManager;

        /// <summary>
        /// The constructor is used for the production environment.
        /// </summary>
        public QrCodeService()
            : this(null)
        {
        }

        /// <summary>
        /// The constructor is used for Unit Testing & DI Container.
        /// </summary>
        /// <param name="qrCodeManager"></param>
        public QrCodeService(IQrCodeManager qrCodeManager)
        {
            _qrCodeManager = qrCodeManager ?? new QrCodeManager();
        }

        public bool InsertQrCode(QrCodeModel qrCodeModel)
        {
            QRCode qrCode = new QRCode
            {
                Id = qrCodeModel.Id,
                Title = qrCodeModel.Label,
                TargetUrl = qrCodeModel.TargetUrl,
                StatisticsUrl = qrCodeModel.StatisticsUrl,
                Foreground = qrCodeModel.Foreground,
                Background = qrCodeModel.Background,
                EmbedLogo = qrCodeModel.EmbedLogo,
                CreatedDate = qrCodeModel.CreatedDate, // this field no need to pass from view, this field is gotten automatically
                UserId = qrCodeModel.UserId,
                TemplateId = qrCodeModel.TemplateId,
                Filename = qrCodeModel.QrType == (int)QrType.File ? qrCodeModel.Filename : null, // only insert filename if qr type is File
                QrType = qrCodeModel.QrType,
                EmbedLogoId = qrCodeModel.EmbedLogoId
            };

            return _qrCodeManager.Insert(qrCode);
        }

        public bool DeleteQrCode(string id)
        {
            return _qrCodeManager.Delete(id);
        }

        public bool DeleteQrCode(QRCode qrCode)
        {
            return _qrCodeManager.Delete(qrCode);
        }

        public bool UpdateQrCode(QrCodeModel qrCodeModel)
        {
            QRCode qrCode = _qrCodeManager.GetById(qrCodeModel.Id);
            qrCode.Id = qrCodeModel.Id;
            qrCode.Title = string.IsNullOrEmpty(qrCodeModel.Label) ? qrCode.Title : qrCodeModel.Label;
            qrCode.TargetUrl = string.IsNullOrEmpty(qrCodeModel.TargetUrl) ? qrCode.TargetUrl : qrCodeModel.TargetUrl;
            qrCode.Foreground = string.IsNullOrEmpty(qrCodeModel.Foreground) ? qrCode.Foreground : qrCodeModel.Foreground;
            qrCode.Background = string.IsNullOrEmpty(qrCodeModel.Background) ? qrCode.Background : qrCodeModel.Background;
            qrCode.EmbedLogo = qrCodeModel.EmbedLogo;
            qrCode.TemplateId = qrCodeModel.TemplateId;
            qrCode.EmbedLogoId = qrCodeModel.EmbedLogoId;
            if (qrCode.QrType == (int)QrType.File)
            {
                qrCode.Filename = string.IsNullOrEmpty(qrCodeModel.Filename) ? qrCode.Filename : qrCodeModel.Filename; 
            }

            return _qrCodeManager.Update(qrCode);
        }

        public IList<QRCode> GetAllQrCode()
        {
            return _qrCodeManager.GetAll();
        }

        public QRCode GetQrCodeById(string id)
        {
            return _qrCodeManager.GetById(id);
        }

        public IList<QRCode> GetQrCodeByUser(string userId)
        {

            return _qrCodeManager.GetByUser(userId);
        }

        public IList<QRCode> GetQrCodeByQrType(QrType type)
        {
            return _qrCodeManager.GetByQrType(type);
        }

        public IList<string> GetPhotoListByUser(string userId)
        {
            return _qrCodeManager.GetLogoByUser(userId);
        }

        public bool ArchiveQrCode(string qrId)
        {
            var qrCode = _qrCodeManager.GetById(qrId);
            qrCode.IsArchived = true;
            return _qrCodeManager.Update(qrCode);
        }

        public bool RestoreQrCode(string qrId)
        {
            var qrCode = _qrCodeManager.GetById(qrId);
            qrCode.IsArchived = false;
            return _qrCodeManager.Update(qrCode);
        }

        public IList<QRCode> GetAllArchivedQrCodes()
        {
            return _qrCodeManager.GetAllArchived();
        }

        public IList<QRCode> GetArchivedQrCodesByUser(string userId)
        {
            return _qrCodeManager.GetArchivedByUser(userId);
        }
    }
}