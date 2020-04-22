using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public class VCardDetailManager : IVCardDetailManager
    {
        private readonly QRCodeManagementEntities _entity;

        public VCardDetailManager()
        {
            _entity = new QRCodeManagementEntities();
        }

        public bool Insert(VCardDetail vCardDetail)
        {
            _entity.VCardDetails.Add(vCardDetail);
            _entity.Entry(vCardDetail).State = EntityState.Added;
            _entity.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            return Delete(GetById(id));
        }

        public bool Delete(VCardDetail vCardDetail)
        {
            if (vCardDetail != null)
            {
                _entity.VCardDetails.Remove(vCardDetail);
                _entity.Entry(vCardDetail).State = EntityState.Deleted;
                _entity.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(VCardDetail vCardDetail)
        {
            _entity.Entry(vCardDetail).State = EntityState.Modified;
            _entity.SaveChanges();
            return true;
        }

        public IList<VCardDetail> GetAll()
        {
            return _entity.VCardDetails?.ToList();
        }

        public VCardDetail GetById(int id)
        {
            return _entity.VCardDetails?.Find(id);
        }

        public VCardDetail GetByQrId(string qrId)
        {
            return _entity.VCardDetails.FirstOrDefault(e => e.QrId == qrId);
        }
    }
}