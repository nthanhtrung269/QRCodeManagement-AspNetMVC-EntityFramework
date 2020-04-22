using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using QRCodeManagement.Constants;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public class QrCodeManager : IQrCodeManager
    {
        private readonly QRCodeManagementEntities _entity;
        public QrCodeManager()
        {
            _entity = new QRCodeManagementEntities();
        }
        public bool Insert(QRCode qrCode)
        {
            _entity.QRCodes.Add(qrCode);
            _entity.Entry(qrCode).State = EntityState.Added;
            _entity.SaveChanges();
            return true;
        }

        public bool Delete(string id)
        {
            return Delete(GetById(id));
        }

        public bool Delete(QRCode webLinks)
        {
            if (webLinks != null)
            {
                _entity.QRCodes.Remove(webLinks);
                _entity.Entry(webLinks).State = EntityState.Deleted;
                _entity.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(QRCode qrCode)
        {
            _entity.Entry(qrCode).State = EntityState.Modified;
            _entity.SaveChanges();
             return true;
        }

        public IList<QRCode> GetAll()
        {
            return _entity.QRCodes?.Where(e => e.IsArchived == false).OrderByDescending(e => e.CreatedDate ).ToList();
        }

        public IList<QRCode> GetAllArchived()
        {
            return _entity.QRCodes?.Where(e => e.IsArchived == true).OrderByDescending(e => e.CreatedDate).ToList();
        }

        public QRCode GetById(string id)
        {
            return _entity.QRCodes?.Find(id);
        }

        public IList<QRCode> GetByUser(string userId)
        {
            return _entity.QRCodes?.Where(e => e.UserId == userId && e.IsArchived == false).OrderByDescending(e=>e.CreatedDate).ToList();
        }

        public IList<QRCode> GetArchivedByUser(string userId)
        {
            return _entity.QRCodes?.Where(e => e.UserId == userId && e.IsArchived == true).OrderByDescending(e => e.CreatedDate).ToList();
        }

        public IList<QRCode> GetByQrType(QrType type)
        {
            return _entity.QRCodes?.Where(e => e.QrType == (int)type).ToList();
        }

        // Logo manager
        public IList<string> GetLogoByUser(string userId)
        {
            return _entity.QRCodes?.Where(e => e.UserId == userId).Select(c => c.EmbedLogo).ToList();
        }

        public IList<string> GetLogoByLogoName(string logoName)
        {
            return _entity.QRCodes?.Where(e => e.EmbedLogo == logoName).Select(c => c.EmbedLogo).ToList();
        }
    }
}