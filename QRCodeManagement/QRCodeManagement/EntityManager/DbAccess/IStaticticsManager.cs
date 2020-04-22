using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public interface IStaticticsManager
    {
        bool Insert(Statictic statictics);

        bool Delete(int id);

        bool Delete(Statictic statictics);

        bool Update(Statictic statictics);

        IList<Statictic> GetAll();

        Statictic GetById(int id);

        IList<Statictic> GetByQrCode(string qrCode);
    }
}