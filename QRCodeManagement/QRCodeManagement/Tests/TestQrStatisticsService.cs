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
    public class TestQrStatisticsService
    {
        private StatisticsModel statisticsModel;
        [TestInitialize]
        public void SetUp()
        {
            statisticsModel = new StatisticsModel()
            {
                City = "Ho chi minh ",
                Country = "Viet nam",
                Device = "Samsung Galaxy S6",
                LatTitude = "12.3783348",
                LongTitude = "9845.6767676",
                Os = "Android",
                QrId = "test",
            };
        }
        [TestMethod]
        public void TestInsertQrStatistics()
        {
            IStatisticsService service = new StatisticsService();
            Assert.IsTrue(service.InsertQrStatistics(statisticsModel));
        }

        [TestMethod]
        public void TestDeleteQrById()
        {
            IStatisticsService service = new StatisticsService();
            Assert.IsTrue(service.DeleteQrStatistics(0));
        }
    }
}