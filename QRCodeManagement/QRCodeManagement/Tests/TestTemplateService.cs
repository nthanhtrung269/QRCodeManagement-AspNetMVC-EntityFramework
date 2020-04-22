using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRCodeManagement.Constants;
using QRCodeManagement.Models;
using QRCodeManagement.Services;
using System.Web.Mvc;
using QRCodeManagement.Common;

namespace QRCodeManagement.Tests
{
    [TestClass]
    public class TestTemplateService
    {
        private TemplateModel templateModel;
        [TestInitialize]
        public void SetUp()
        {
            templateModel = new TemplateModel()
            {
                Background = "red",
                EmbedLogo = "testLog.jpg",
                Foreground = "blue",
                UserId = "test_user_id"
            };
        }
        [TestMethod]
        public void TestInsertQrTemplate()
        {
            ITemplateService service = new TemplateService();
            //Assert.IsTrue(service.InsertQrTemplate(templateModel));
        }

        [TestMethod]
        public void TestUpdateQr()
        {
            ITemplateService service = new TemplateService();
            templateModel.Id = 1;
            templateModel.EmbedLogo = "test_logo_update";
            Assert.IsTrue(service.UpdateQrTemplate(templateModel));
            Assert.IsTrue(service.GetQrTemplateByUser("test_user_id").FirstOrDefault()?.EmbedLogo == "test_logo_update");
        }

        [TestMethod]
        public void TestDeleteQrById()
        {
            ITemplateService service = new TemplateService();
            Assert.IsTrue(service.DeleteQrTemplate(2));
        }
    }
}