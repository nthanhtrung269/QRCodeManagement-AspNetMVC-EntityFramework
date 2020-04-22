using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public interface ILogoManager
    {
        bool Insert(Logo logo);

        bool Delete(int id);

        IList<Logo> GetAll();

        Logo GetById(int id);

        IList<Logo> GetByUser(string userId);

    }
}