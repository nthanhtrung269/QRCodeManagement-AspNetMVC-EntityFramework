using System;
using System.Collections.Generic;
using System.Linq;
using QRCodeManagement.EntityManager.DbAccess;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public class LogoService : ILogoService
    {
        private readonly ILogoManager _logoManager;
        /// <summary>
        /// The constructor is used for the production environment.
        /// </summary>
        public LogoService()
            : this(null)
        {
        }

        /// <summary>
        /// The constructor is used for Unit Testing & DI Container.
        /// </summary>
        /// <param name="logoManager"></param>
        public LogoService(ILogoManager logoManager)
        {
            _logoManager = logoManager ?? new LogoManager();
        }


        public Logo InsertLogo(LogoModel logoModel)
        {
            try
            {
                Logo logo = new Logo
                {
                    FileName = logoModel.FileName,
                    UserId = logoModel.UserId
                };

                if (_logoManager.Insert(logo))
                {
                    return logo;
                }
            }
            catch (Exception exception)
            {
                
            }
            return null;
        }

        public bool DeleteLogo(int id)
        {
            try
            {
                return _logoManager.Delete(id);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public IList<Logo> GetAllLogoes()
        {
            try
            {
                return _logoManager.GetAll();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public Logo GetLogoById(int id)
        {
            try
            {
                return _logoManager.GetById(id);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public IList<Logo> GetLogoByUser(string userId)
        {
            try
            {
                return _logoManager.GetByUser(userId);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public bool LogoIsInUsed(string logoName)
        {
            try
            {
                if (logoName.ToLower().Equals("default_logo.png") || logoName.ToLower().Equals("no_logo.png"))
                {
                    return true;
                }
                //Check logo in QR table
                if (new QrCodeManager().GetLogoByLogoName(logoName).Count > 0)
                {
                    return true;
                }
                //Check logo in logo table or template
                if (new LogoService().GetAllLogoes().Any(x => x.FileName == logoName))
                {
                    return true;
                }
                //Check logo in template table
                if (new TemplateService().GetAllQrTemplate().Any(x => x.EmbedLogo == logoName))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return true;
            }
            return false;
        }
    }
}