using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public interface IVCardDetailService
    {
        bool InsertVCardDetail(VCardDetail vCardDetailModel);

        bool DeleteVCardDetailById(int id);

        bool UpdateVCardDetail(VCardDetail vCardDetailModel);

        VCardDetail GetVCardDetailById(int id);

        VCardDetail GetVCardDetailByQrId(string qrId);
    }
}