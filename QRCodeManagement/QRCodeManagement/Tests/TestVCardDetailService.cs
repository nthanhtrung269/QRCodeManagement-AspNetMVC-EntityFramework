using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRCodeManagement.Constants;
using QRCodeManagement.Models;
using QRCodeManagement.Services;
using System.Web.Mvc;
using QRCodeManagement.Common;
using QRCodeManagement.EntityManager.DbEntity;

namespace QRCodeManagement.Tests
{
    [TestClass]
    public class TestVCardDetailService
    {
        private VCardDetail vCardDetailModel;
        [TestInitialize]
        public void SetUp()
        {
            vCardDetailModel = new VCardDetail()
            {
                City = "Phan Thiet",
                Company = "Company",
                Country = "VietNam",
                Email = "test@test.com",
                Fax = "123456789",
                FirstName = "Tuan",
                JobTitle = "Senior Dev",
                LastName = "Nguyen Minh",
                MobileNumber = "012345673",
                Phone = "085638463",
                QrId = "test",
                State = "70000",
                Street = "Đường 12",
                Website = "http://test.com",
                Zipcode = string.Empty,
                About = "about"
            };
        }
        [TestMethod]
        public void TestInsertVCardDetail_ShouldTrue_WhenNotExistQrCode()
        {
            IVCardDetailService service = new VCardDetailService();
            Assert.IsTrue(service.InsertVCardDetail(vCardDetailModel));
        }

        [TestMethod]
        public void TestInsertVCardDetail_ShouldFalse_WhenQrCodeExisted()
        {
            IVCardDetailService service = new VCardDetailService();
            Assert.IsFalse(service.InsertVCardDetail(vCardDetailModel));
        }

        [TestMethod]
        public void TestUpdateVCard()
        {
            IVCardDetailService service = new VCardDetailService();

            vCardDetailModel.Id = 1;
            vCardDetailModel.FirstName = "test_tuan";
            //Assert.IsTrue(service.UpdateVCardDetail(vCardDetailModel));
            //Assert.IsTrue(service.GetVCardDetailById(1)?.FirstName == "test_tuan");
        }

        [TestMethod]
        public void TestGetByQrId()
        {
            IVCardDetailService service = new VCardDetailService();
            Assert.IsTrue(service.GetVCardDetailByQrId("test").Id == 1);
        }
    }
}