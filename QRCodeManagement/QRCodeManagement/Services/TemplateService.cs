using System;
using System.Collections.Generic;
using QRCodeManagement.EntityManager.DbAccess;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public class TemplateService: ITemplateService
    {
        private readonly ITemplateManager _templateManager;
        /// <summary>
        /// The constructor is used for the production environment.
        /// </summary>
        public TemplateService()
            : this(null)
        {
        }

        /// <summary>
        /// The constructor is used for Unit Testing & DI Container.
        /// </summary>
        /// <param name="templateManager"></param>
        public TemplateService(ITemplateManager templateManager)
        {
            _templateManager = templateManager ?? new TemplateManager();
        }

        public Template InsertQrTemplate(TemplateModel templateModel)
        {
            try
            {
                Template template = new Template
                {
                    Background = templateModel.Background,
                    EmbedLogo = templateModel.EmbedLogo,
                    Foreground = templateModel.Foreground,
                    UserId = templateModel.UserId
                };

                if (_templateManager.Insert(template))
                {
                    return template;
                }
            }
            catch (Exception exception)
            {

            }
            return null;
        }

        public bool DeleteQrTemplate(int id)
        {
            try
            {
                return _templateManager.Delete(id);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool DeleteQrTemplate(Template template)
        {
            try
            {
                return _templateManager.Delete(template);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool UpdateQrTemplate(TemplateModel templateModel)
        {
            try
            {
                Template template = _templateManager.GetById(templateModel.Id);

                template.Background = templateModel.Background;
                template.EmbedLogo = templateModel.EmbedLogo;
                template.Foreground = templateModel.Foreground;
                template.UserId = templateModel.UserId;

                return _templateManager.Update(template);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public IList<Template> GetAllQrTemplate()
        {
            try
            {
                return _templateManager.GetAll();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public Template GetQrTemplateById(int id)
        {
            try
            {
                return _templateManager.GetById(id);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public IList<Template> GetQrTemplateByUser(string userId)
        {
            try
            {
                return _templateManager.GetByUser(userId);
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}