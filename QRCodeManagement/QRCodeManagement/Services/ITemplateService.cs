using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public interface ITemplateService
    {
        Template InsertQrTemplate(TemplateModel templateModel);

        bool DeleteQrTemplate(int id);

        bool DeleteQrTemplate(Template template);

        bool UpdateQrTemplate(TemplateModel templateModel);

        IList<Template> GetAllQrTemplate();

        Template GetQrTemplateById(int id);
        
        IList<Template> GetQrTemplateByUser(string userId);

    }
}