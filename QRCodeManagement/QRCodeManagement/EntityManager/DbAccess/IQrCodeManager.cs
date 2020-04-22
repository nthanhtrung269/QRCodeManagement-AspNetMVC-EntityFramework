using System.Collections.Generic;
using QRCodeManagement.Constants;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.EntityManager.DbAccess
{
    public interface IQrCodeManager
    {
        bool Insert(QRCode webLinks);

        bool Delete(string id);

        bool Delete(QRCode qrCode);

        bool Update(QRCode qrCode);

        IList<QRCode> GetAll();

        QRCode GetById(string id);

        IList<QRCode> GetByUser(string userId);

        IList<QRCode> GetByQrType(QrType type);

        IList<string> GetLogoByUser(string userId);

        IList<string> GetLogoByLogoName(string logoName);

        IList<QRCode> GetAllArchived();

        IList<QRCode> GetArchivedByUser(string userId);
    }
}