using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public class TemplateManager : ITemplateManager
    {
        private readonly QRCodeManagementEntities _entity;

        public TemplateManager()
        {
            _entity = new QRCodeManagementEntities();
        }

        public bool Insert(Template template)
        {
            _entity.Templates.Add(template);
            _entity.Entry(template).State = EntityState.Added;
            _entity.SaveChanges();
            return true;
        }

        public bool Delete(Template template)
        {
            if (template != null)
            {
                _entity.Templates.Remove(template);
                _entity.Entry(template).State = EntityState.Deleted;
                _entity.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            return Delete(GetById(id));
        }

        public bool Update(Template template)
        {
            _entity.Entry(template).State = EntityState.Modified;
            _entity.SaveChanges();
            return true;
        }

        public IList<Template> GetAll()
        {
            return _entity.Templates?.ToList();
        }

        public Template GetById(int id)
        {
            return _entity.Templates?.Find(id);
        }

        public IList<Template> GetByUser(string userId)
        {
            return _entity.Templates?.Where(e => e.UserId == userId).ToList();
        }
    }
}