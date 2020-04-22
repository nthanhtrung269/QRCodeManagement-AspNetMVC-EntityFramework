using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public interface ILogoService
    {
        Logo InsertLogo(LogoModel logoModel);

        bool DeleteLogo(int id);
        
        IList<Logo> GetAllLogoes();

        Logo GetLogoById(int id);
        
        IList<Logo> GetLogoByUser(string userId);

        bool LogoIsInUsed(string logoName);
    }
}