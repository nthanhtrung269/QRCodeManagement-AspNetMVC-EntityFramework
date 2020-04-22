using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public class LogoManager : ILogoManager
    {
        private readonly QRCodeManagementEntities _entity;

        public LogoManager()
        {
            _entity = new QRCodeManagementEntities();
        }

        public bool Insert(Logo logo)
        {
            _entity.Logoes.Add(logo);
            _entity.Entry(logo).State = EntityState.Added;
            _entity.SaveChanges();
            return true;
        }

        public bool Delete(Logo logo)
        {
            if (logo != null)
            {
                _entity.Logoes.Remove(logo);
                _entity.Entry(logo).State = EntityState.Deleted;
                _entity.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            return Delete(GetById(id));
        }

        public IList<Logo> GetAll()
        {
            return _entity.Logoes?.ToList();
        }

        public Logo GetById(int id)
        {
            return _entity.Logoes?.Find(id);
        }

        public IList<Logo> GetByUser(string userId)
        {
            return _entity.Logoes?.Where(e => e.UserId == userId).ToList();
        }
    }
}