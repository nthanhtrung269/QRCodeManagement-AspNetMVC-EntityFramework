using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public interface ITemplateManager
    {
        bool Insert(Template template);

        bool Delete(int id);

        bool Delete(Template template);

        bool Update(Template template);

        IList<Template> GetAll();

        Template GetById(int id);

        IList<Template> GetByUser(string userId);

    }
}