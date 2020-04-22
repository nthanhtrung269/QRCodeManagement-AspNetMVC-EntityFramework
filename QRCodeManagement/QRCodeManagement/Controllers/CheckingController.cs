using System.IO;
using System.Web.Mvc;
using QRCodeManagement.Constants;
using QRCodeManagement.EntityManager.DbEntity;
using QRCodeManagement.Helpers;
using QRCodeManagement.Models;
using QRCodeManagement.Services;

namespace QRCodeManagement.Controllers
{
    public partial class CheckingController : Controller
    {
        public virtual ActionResult Index(string id)
        {
            return View();
        }

        // POST: Checking
        public string ProcessScannerInfor()
        {
            var qrCodeService = new QrCodeService();
            string targeUrl = string.Empty;
            string strJson = new StreamReader(Request.InputStream).ReadToEnd();
            StatisticsModel infor = JsonHelper.Deserialize<StatisticsModel>(strJson);

            //Check QR code is archived
            var qrCode = qrCodeService.GetQrCodeById(infor.QrId);
            if (qrCode.IsArchived)
            {
                return string.Empty;
            }

            var statisticsService = new StatisticsService();
            if (statisticsService.InsertQrStatistics(infor))
            {
                targeUrl = new QrCodeService().GetQrCodeById(infor.QrId).TargetUrl;
            }

            return targeUrl;
        }
    }
}