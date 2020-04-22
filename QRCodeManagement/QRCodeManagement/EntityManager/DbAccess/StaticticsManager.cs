using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public class StaticticsManager : IStaticticsManager
    {
        private readonly QRCodeManagementEntities _entity;

        public StaticticsManager()
        {
            _entity = new QRCodeManagementEntities();
        }

        public bool Insert(Statictic statictics)
        {
            _entity.Statictics.Add(statictics);
            _entity.Entry(statictics).State = EntityState.Added;
            _entity.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            return Delete(GetById(id));
        }

        public bool Delete(Statictic statictics)
        {
            if (statictics != null)
            {
                _entity.Statictics.Remove(statictics);
                _entity.Entry(statictics).State = EntityState.Deleted;
                _entity.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(Statictic statictics)
        {
            _entity.Entry(statictics).State = EntityState.Modified;
            _entity.SaveChanges();
            return true;
        }
        
        public IList<Statictic> GetAll()
        {
            return _entity.Statictics?.ToList();
        }

        public Statictic GetById(int id)
        {
            return _entity.Statictics?.Find(id);
        }

        public IList<Statictic> GetByQrCode(string qrCode)
        {
            return _entity.Statictics?.Where(e => e.QrId == qrCode).ToList();
        }
    }
}