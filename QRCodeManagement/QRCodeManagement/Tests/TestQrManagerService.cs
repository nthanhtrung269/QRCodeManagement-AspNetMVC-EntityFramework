using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRCodeManagement.Constants;
using QRCodeManagement.Models;
using QRCodeManagement.Services;
using System.Web.Mvc;
using QRCodeManagement.Common;

namespace QRCodeManagement.Tests
{
    [TestClass]
    public class TestQrManagerService
    {
        private QrCodeModel model;
        [TestInitialize]
        public void SetUp()
        {
            model = new QrCodeModel
            {
                Id = "test",
                Background = "red",
                EmbedLogo = "test.jpg",
                Filename = "test.gif",
                Foreground = "blue",
                QrType = (int)QrType.File,
                StatisticsUrl = "http://test.com",
                TargetUrl = "http://test.com",
                TemplateId = 1,
                Label = "test_title",
                UserId = "test_user_id"
        };
        }
        [TestMethod]
        public void TestInsertQr()
        {
            IQrCodeService service = new QrCodeService();
            model.Id = model.Id +"-"+ RandomString.GeneratePassword();
            model.QrType = (int) QrType.VCard;
            model.Filename = "test.png";
            Assert.IsTrue(service.InsertQrCode(model));
        }

        [TestMethod]
        public void TestUpdateQr()
        {
            IQrCodeService service = new QrCodeService();
            model.Id = "test-2k*Aj?N4&Y";
            model.Label = "test_update1";
            model.Filename = "updated1.jpg";
            model.QrType = (int) QrType.File;
            Assert.IsTrue(service.UpdateQrCode(model));
        }

        [TestMethod]
        public void TestDeleteQrById()
        {
            IQrCodeService service = new QrCodeService();
            Assert.IsTrue(service.DeleteQrCode("test-sA_6E5n!2?"));
        }

        [TestMethod]
        public void TestDeleteQrEntry()
        {
            IQrCodeService service = new QrCodeService();
            Assert.IsTrue(service.DeleteQrCode(service.GetQrCodeById("test-7p$As&J4")));
        }

        [TestMethod]
        public void TestGetQrCodeByUserId()
        {
            IQrCodeService service = new QrCodeService();
            Assert.IsTrue(service.GetQrCodeByUser("test_user_id").Count == 3);
        }

        [TestMethod]
        public void TestGetQrCodeByQrType()
        {
            IQrCodeService service = new QrCodeService();
            Assert.IsTrue(service.GetQrCodeByQrType(QrType.File).Count == 1);
        }

        [TestMethod]
        public void TestGetPhotoByUser()
        {
            IQrCodeService service = new QrCodeService();
            Assert.IsTrue(service.GetPhotoListByUser("test_user_id").Contains("test.jpg"));
        }
    }
}