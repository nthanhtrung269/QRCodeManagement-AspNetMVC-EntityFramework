using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using QRCodeManagement.Constants;
using QRCodeManagement.EntityManager;
using QRCodeManagement.EntityManager.DbAccess;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Models;

namespace QRCodeManagement.Services
{
    public class VCardDetailService : IVCardDetailService
    {
        private readonly IVCardDetailManager _vCardDetailManager;
        /// <summary>
        /// The constructor is used for the production environment.
        /// </summary>
        public VCardDetailService()
            : this(null)
        {
        }

        /// <summary>
        /// The constructor is used for Unit Testing & DI Container.
        /// </summary>
        /// <param name="vCardDetailManager"></param>
        public VCardDetailService(IVCardDetailManager vCardDetailManager)
        {
            _vCardDetailManager = vCardDetailManager ?? new VCardDetailManager();
        }
        public bool InsertVCardDetail(VCardDetail vCardDetailModel)
        {
            // Make sure only one QRCode with one VCard
            if (_vCardDetailManager.GetByQrId(vCardDetailModel.QrId) != null)
            {
                return false;
            }

            return _vCardDetailManager.Insert(vCardDetailModel);
        }

        public bool DeleteVCardDetailById(int id)
        {
            try
            {
                return _vCardDetailManager.Delete(id);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool UpdateVCardDetail(VCardDetail vCardDetailModel)
        {
            VCardDetail vCardDetail = _vCardDetailManager.GetByQrId(vCardDetailModel.QrId);

            if (vCardDetail != null)
            {
                vCardDetail.City = vCardDetailModel.City;
                vCardDetail.Company = vCardDetailModel.Company;
                vCardDetail.Country = vCardDetailModel.Country;
                vCardDetail.Email = vCardDetailModel.Email;
                vCardDetail.Fax = vCardDetailModel.Fax;
                vCardDetail.FirstName = vCardDetailModel.FirstName;
                vCardDetail.JobTitle = vCardDetailModel.JobTitle;
                vCardDetail.LastName = vCardDetailModel.LastName;
                vCardDetail.MobileNumber = vCardDetailModel.MobileNumber;
                vCardDetail.Phone = vCardDetailModel.Phone;
                vCardDetail.State = vCardDetailModel.State;
                vCardDetail.Street = vCardDetailModel.Street;
                vCardDetail.Website = vCardDetailModel.Website;
                vCardDetail.Zipcode = vCardDetailModel.Zipcode;
                vCardDetail.ProfileImage = vCardDetailModel.ProfileImage;
                vCardDetail.About = vCardDetailModel.About;
                vCardDetail.HeaderColor = vCardDetailModel.HeaderColor;
                vCardDetail.ButtonColor = vCardDetailModel.ButtonColor;

                return _vCardDetailManager.Update(vCardDetail);
            }

            return false;
        }

        public VCardDetail GetVCardDetailById(int id)
        {
            try
            {
                return _vCardDetailManager.GetById(id);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public VCardDetail GetVCardDetailByQrId(string qrId)
        {
            try
            {
                return _vCardDetailManager.GetByQrId(qrId);
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}