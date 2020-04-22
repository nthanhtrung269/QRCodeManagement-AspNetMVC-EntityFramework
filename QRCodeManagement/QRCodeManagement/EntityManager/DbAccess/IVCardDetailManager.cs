using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public interface IVCardDetailManager
    {
        bool Insert(VCardDetail template);

        bool Delete(int id);

        bool Delete(VCardDetail template);

        bool Update(VCardDetail template);

        IList<VCardDetail> GetAll();

        VCardDetail GetById(int id);

        VCardDetail GetByQrId(string qrId);
    }
}