using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QRCodeManagement.Common;
using QRCodeManagement.Constants;
using QRCodeManagement.Helpers;
using QRCodeManagement.Models;
using QRCodeManagement.Services;
using QRCodeManagement.Models.ViewModels;
using QRCodeManagement.EntityManager.DbEntity;
using Webdiyer.WebControls.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;

namespace QRCodeManagement.Controllers
{
    [Authorize]
    public partial class QrManagementController : Controller
    {
        private readonly IQrCodeService _qrManagementService;
        private readonly IStatisticsService _statisticsService;
        private readonly ITemplateService _templateService;
        private readonly IVCardDetailService _qrVCardDetailService;
        private readonly ILogoService _logoService;
        private readonly IConfigurationManagerWrapper _configurationManagerWrapper;
        private string _loginUserId;

        public QrManagementController(): this(null, null, null, null, null, null)
        {
        }

        public QrManagementController(
            IQrCodeService qrManagementService,
            IStatisticsService qrStatisticsService,
            ITemplateService qrTemplateService,
            IVCardDetailService qrVCardDetailService,
            ILogoService logoService,
            IConfigurationManagerWrapper configurationManagerWrapper)
        {
            _qrManagementService = qrManagementService ?? new QrCodeService();
            _statisticsService = qrStatisticsService ?? new StatisticsService();
            _templateService = qrTemplateService ?? new TemplateService();
            _qrVCardDetailService = qrVCardDetailService ?? new VCardDetailService();
            _logoService = logoService ?? new LogoService();
            _configurationManagerWrapper = configurationManagerWrapper ?? new ConfigurationManagerWrapper();
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            _loginUserId = User.Identity.GetUserId();
        }

        public virtual ActionResult Chart(string id, DateTime? fromDate = null, DateTime? toDate = null, int page = 1)
        {
            var qrCode = _qrManagementService.GetQrCodeById(id);
            string msg;
            if (!AllowToOpen(out msg, qrCode))
            {
                return GoToUnAuthorizePage(msg);
            }
            if (!fromDate.HasValue || !toDate.HasValue)
            {
                fromDate = DateTime.Now.AddDays(-30);
                toDate = DateTime.Now;
            }

            var qrCodeModel = new QrCodeModel
            {
                QrType = qrCode.QrType,
                Background = qrCode.Background,
                EmbedLogo = qrCode.EmbedLogo,
                Filename = qrCode.QrType == (int)QrType.File ? qrCode.Filename : null,
                Id = qrCode.Id,
                Foreground = qrCode.Foreground,
                IsArchived = qrCode.IsArchived,
                Logos = _logoService.GetLogoByUser(_loginUserId),
                Templates = _templateService.GetQrTemplateByUser(_loginUserId),
                SelectedLogoId = qrCode.EmbedLogoId,
                SelectedTemplateId = qrCode.TemplateId,
                TargetUrl = qrCode.TargetUrl,
                TemplateId = qrCode.TemplateId,
                Label = qrCode.Title,
                StatisticsUrl = qrCode.StatisticsUrl,
                EmbedLogoId = qrCode.EmbedLogoId
            };

            IList<Statictic> statictics = _statisticsService.GetQrStatisticsByQrId(id);
            IList<ChartRawDataViewModel> chartRawDataViewModels = statictics.Where(s => s.ScanDate.Value.Date >= fromDate.Value.Date && s.ScanDate.Value.Date <= toDate.Value.Date)
                .Select(s => new ChartRawDataViewModel()
                {
                    City = s.City,
                    Country = s.Country,
                    Device = s.Device,
                    OperationSystem = s.Os,
                    ScanDate = s.ScanDate.Value
                }).ToList();

            var chartRawDataPaged = chartRawDataViewModels.ToPagedList(page, AppConstants.NUMBER_RAW_CHART_ITEM_PER_PAGE);

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.QrManagement.Views.RawDataChart, chartRawDataPaged);
            }

            var chartViewModel = new ChartViewModel()
            {
                QrCodeModel = qrCodeModel,
                TotalScans = statictics.Count,
                TotalCities = statictics.GroupBy(s => s.City).Count(),
                TotalScansLast48h = statictics.Where(s => s.ScanDate > DateTime.Now.AddHours(-48)).Count(),
                ScanByOperationSystemViewModels = statictics.GroupBy(s => s.Os)
                                            .Select((s, index) => new ScanByOperationSystemViewModel()
                                            {
                                                OperationSystem = s.First().Os,
                                                Percent = Math.Round((((decimal)s.Count() / statictics.Count()) * 100), 2)
                                            }).ToList(),
                ScanByTopCities = statictics.GroupBy(s => s.City)
                                            .Select((s, index) => new ScanByTopCitiesViewModel()
                                            {
                                                RowIndex = index + 1,
                                                City = s.First().City,
                                                Scans = s.Count(),
                                                Percent = Math.Round((((decimal)s.Count() / statictics.Count()) * 100), 2).ToString() + " %"
                                            }).ToList(),
                ChartRawDataViewModel = chartRawDataPaged,
                ScanOverTime = statictics.Where(s => s.ScanDate.HasValue).GroupBy(s => s.ScanDate.Value.Date).OrderBy(s => s.First().ScanDate)
                                         .ToDictionary(s => s.First().ScanDate.Value, s => s.Count())
            };

            return View(MVC.QrManagement.Views.Chart, chartViewModel);
        }

        public virtual ActionResult Manage(string userId = "", int page = 1, string keyword = "", bool? archived = null)
        {
            ViewBag.Archived = (archived != null & archived == true);
            // If user is not an admin, only get current login user, else load qr by userId
            if (!User.IsInRole("Administrator") || userId == "")
            {
                userId = _loginUserId;
            }

            var qrCodes = (archived == null || archived == false) // if not pass archive or archive = false, load not archived QR 
                ? _qrManagementService.GetQrCodeByUser(userId).AsEnumerable()
                : _qrManagementService.GetArchivedQrCodesByUser(userId).AsEnumerable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                qrCodes = qrCodes.Where(column => column.Title.ToLower().Contains(keyword.ToLower()));
            }
            var model = qrCodes.ToPagedList(page, AppConstants.NUMBER_QR_PER_PAGE);

            if (Request.IsAjaxRequest())
                return PartialView(MVC.QrManagement.Views.Partials.QrListPartial, model);

            return View(MVC.QrManagement.Views.Manage, model);
        }

        [ProtectOwnerQr]
        public virtual ActionResult Archive(string id)
        {
            _qrManagementService.ArchiveQrCode(id);
            var qrCodes = _qrManagementService.GetQrCodeByUser(_loginUserId).AsEnumerable();
            var model = qrCodes.ToPagedList(1, AppConstants.NUMBER_QR_PER_PAGE);

            return PartialView(MVC.QrManagement.Views.Partials.QrListPartial, model);
        }

        public virtual ActionResult ArchiveAndGoToManagePage(string qrId)
        {
            if (_qrManagementService.ArchiveQrCode(qrId))
            {
                return RedirectToAction(MVC.QrManagement.Manage(archived: true));
            }

            return RedirectToAction(MVC.QrManagement.Chart(qrId));
        }

        public virtual ActionResult RestoreAndGoToManagePage(string qrId)
        {
            if (_qrManagementService.RestoreQrCode(qrId))
            {
                return RedirectToAction(MVC.QrManagement.Manage());
            }

            return RedirectToAction(MVC.QrManagement.Chart(qrId));
        }

        public virtual ActionResult DeleteAndGoToManagePage(string qrId)
        {
            if (DeleteQrCode(qrId))
            {
                return RedirectToAction(MVC.QrManagement.Manage(archived: true));
            }

            return RedirectToAction(MVC.QrManagement.Chart(qrId));
        }

        [ProtectOwnerQr]
        public virtual ActionResult Restore(string id)
        {
            ViewBag.Archived = true;
            _qrManagementService.RestoreQrCode(id);
            var qrCodes = _qrManagementService.GetArchivedQrCodesByUser(_loginUserId).AsEnumerable();
            var model = qrCodes.ToPagedList(1, AppConstants.NUMBER_QR_PER_PAGE);

            return PartialView(MVC.QrManagement.Views.Partials.QrListPartial, model);
        }

        [ProtectOwnerQr]
        public virtual ActionResult Delete(string id)
        {
            var qrCode = _qrManagementService.GetQrCodeById(id);
            if (qrCode == null || !qrCode.IsArchived)
            {
                return RedirectToAction(MVC.Error.ErrorMsg("An error occurs", "You need to archive QR code before detele it."));
            }
            ViewBag.Archived = true;
            DeleteQrCode(id);
            var qrCodes = _qrManagementService.GetArchivedQrCodesByUser(_loginUserId).AsEnumerable();
            var model = qrCodes.ToPagedList(1, AppConstants.NUMBER_QR_PER_PAGE);

            return PartialView(MVC.QrManagement.Views.Partials.QrListPartial, model);
        }

        [HttpGet]
        public virtual ActionResult Create(int type = 0)
        {
            ViewBag.Type = type;
            ViewBag.CheckingUrl = $"{_configurationManagerWrapper.StatisticDomain}{AppConstants.CHECKING_QR_URL}";
            ViewBag.QrId = RandomString.GetTextFromDateTime();

            var qrCodeModel = new QrCodeModel
            {
                Templates = _templateService.GetQrTemplateByUser(_loginUserId),
                Logos = _logoService.GetLogoByUser(_loginUserId)
            };

            return View(MVC.QrManagement.Views.Create, qrCodeModel);
        }

        [HttpPost]
        public virtual ActionResult CreateUrl(UrlViewModel urlViewModel)
        {
            var qrCodeModel = new QrCodeModel
            {
                QrType = urlViewModel.QrType,
                Background = urlViewModel.Background,
                EmbedLogo = urlViewModel.EmbedLogo,
                Foreground = urlViewModel.Foreground,
                Id = urlViewModel.Id,
                StatisticsUrl = urlViewModel.StatisticsUrl,
                TargetUrl = urlViewModel.UrlText,
                TemplateId = urlViewModel.TemplateId,
                UserId = _loginUserId,
                Label = urlViewModel.Label,
                EmbedLogoId = urlViewModel.EmbedLogoId
            };

            if (_qrManagementService.InsertQrCode(qrCodeModel))
            {
                return RedirectToAction(MVC.QrManagement.Manage());
            }

            return RedirectToAction(MVC.QrManagement.Create(0));
        }

        [HttpPost]
        public virtual ActionResult CreateVCard(VCardViewModel vCardViewModel)
        {
            var qrCodeModel = new QrCodeModel
            {
                QrType = vCardViewModel.QrType,
                Background = vCardViewModel.Background,
                EmbedLogo = vCardViewModel.EmbedLogo,
                Foreground = vCardViewModel.Foreground,
                Id = vCardViewModel.Id,
                StatisticsUrl = vCardViewModel.StatisticsUrl,
                TargetUrl = Url.Action(MVC.QrInfo.ViewVCard(vCardViewModel.Id)),
                TemplateId = vCardViewModel.TemplateId,
                UserId = _loginUserId,
                Label = vCardViewModel.Label,
                EmbedLogoId = vCardViewModel.EmbedLogoId
            };

            var vCardDetailModel = new VCardDetail
            {
                QrId = vCardViewModel.Id,
                FirstName = vCardViewModel.FirstName,
                LastName = vCardViewModel.LastName,
                Company = vCardViewModel.Company,
                JobTitle = vCardViewModel.JobTitle,
                Street = vCardViewModel.Street,
                City = vCardViewModel.City,
                Country = vCardViewModel.Country,
                State = vCardViewModel.State,
                Zipcode = vCardViewModel.ZipCode,
                MobileNumber = vCardViewModel.MobileNumber,
                Phone = vCardViewModel.Phone,
                Fax = vCardViewModel.Fax,
                Email = vCardViewModel.Email,
                Website = vCardViewModel.Website,
                About = vCardViewModel.About,
                ProfileImage = vCardViewModel.ProfileImage,
                HeaderColor = vCardViewModel.HeaderColor,
                ButtonColor = vCardViewModel.ButtonColor
            };

            if (_qrManagementService.InsertQrCode(qrCodeModel) && _qrVCardDetailService.InsertVCardDetail(vCardDetailModel))
            {
                MoveVCardImageFile(vCardViewModel.ProfileImage);
                return RedirectToAction(MVC.QrManagement.Manage());
            }

            return RedirectToAction(MVC.QrManagement.Create((int)QrType.VCard));
        }

        [HttpPost]
        public virtual ActionResult UpdateVCard(VCardViewModel vCardViewModel)
        {
            var qrCodeModel = new QrCodeModel
            {
                QrType = vCardViewModel.QrType,
                Background = vCardViewModel.Background,
                EmbedLogo = vCardViewModel.EmbedLogo,
                Foreground = vCardViewModel.Foreground,
                Id = vCardViewModel.Id,
                StatisticsUrl = vCardViewModel.StatisticsUrl,
                TemplateId = vCardViewModel.TemplateId,
                UserId = _loginUserId,
                Label = vCardViewModel.Label,
                EmbedLogoId = vCardViewModel.EmbedLogoId
            };

            var vCardDetailModel = new VCardDetail
            {
                QrId = vCardViewModel.Id,
                FirstName = vCardViewModel.FirstName,
                LastName = vCardViewModel.LastName,
                Company = vCardViewModel.Company,
                JobTitle = vCardViewModel.JobTitle,
                Street = vCardViewModel.Street,
                City = vCardViewModel.City,
                Country = vCardViewModel.Country,
                State = vCardViewModel.State,
                Zipcode = vCardViewModel.ZipCode,
                MobileNumber = vCardViewModel.MobileNumber,
                Phone = vCardViewModel.Phone,
                Fax = vCardViewModel.Fax,
                Email = vCardViewModel.Email,
                Website = vCardViewModel.Website,
                About = vCardViewModel.About,
                ProfileImage = vCardViewModel.ProfileImage,
                HeaderColor = vCardViewModel.HeaderColor,
                ButtonColor = vCardViewModel.ButtonColor
            };

            if (_qrManagementService.UpdateQrCode(qrCodeModel) && _qrVCardDetailService.UpdateVCardDetail(vCardDetailModel))
            {
                if(vCardViewModel.ProfileImage != vCardViewModel.OldProfileImage)
                {
                    string filePath = Server.MapPath($"{AppConstants.VCARD_IMG_UPLOAD_FOLDER}{vCardViewModel.OldProfileImage}");

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                MoveVCardImageFile(vCardViewModel.ProfileImage);
                return RedirectToAction(MVC.QrManagement.Manage());
            }

            return RedirectToAction(MVC.QrManagement.Create((int)QrType.VCard));
        }

        [HttpPost]
        public virtual ActionResult CreateFile(FileViewModel fileViewModel)
        {
            HttpPostedFileBase file = fileViewModel.FileUpload;
            string extention = Path.GetExtension(file?.FileName);
            string fileNameReplace = $"{fileViewModel.FileType.ToLower()}_{RandomString.GetTextFromDateTime()}{extention}";
            string uploadPath = fileViewModel.FileType.Equals(QrFileType.PDF.ToString())
                ? AppConstants.PDF_UPLOAD_FOLDER
                : AppConstants.IMG_UPLOAD_FOLDER;
            string fullPath = $"{uploadPath}{fileNameReplace}";
            string savedFileName = Server.MapPath(fullPath);

            try
            {
                CreateFolderIfNotExist(uploadPath);
                file?.SaveAs(savedFileName);

                var qrCodeModel = new QrCodeModel
                {
                    QrType = (int)QrType.File,
                    Filename = fileNameReplace,
                    EmbedLogoId = fileViewModel.EmbedLogoId,
                    EmbedLogo = fileViewModel.EmbedLogo,
                    Foreground = fileViewModel.Foreground,
                    Background = fileViewModel.Background,
                    Id = fileViewModel.Id,
                    StatisticsUrl = fileViewModel.StatisticsUrl,
                    TargetUrl = Url.Action(MVC.QrInfo.ViewFile(fileViewModel.Id)),
                    TemplateId = fileViewModel.TemplateId,
                    UserId = _loginUserId,
                    Label = fileViewModel.Label
                };

                if (!_qrManagementService.InsertQrCode(qrCodeModel))
                {
                    //delete file if insert not sucessfull
                    if (System.IO.File.Exists(savedFileName))
                    {
                        System.IO.File.Delete(savedFileName);
                    }
                    return RedirectToAction(MVC.QrManagement.Create(2));
                }
            }
            catch (Exception e)
            {
                if (System.IO.File.Exists(savedFileName))
                {
                    System.IO.File.Delete(savedFileName);
                }
            }

            return RedirectToAction(MVC.QrManagement.Manage());
        }

        [HttpPost]
        public virtual ActionResult UpdateFile(FileViewModel fileViewModel)
        {
            HttpPostedFileBase file = fileViewModel.FileUpload;
            string uploadPath = string.Empty;
            string savedFileName = string.Empty;
            string fileNameReplace = string.Empty;
            string fullPath = string.Empty;

            // verify there is new file need to upload
            if (file != null)
            {
                string extention = Path.GetExtension(file.FileName);
                fileNameReplace = $"{fileViewModel.FileType.ToLower()}_{RandomString.GetTextFromDateTime()}{extention}";
                uploadPath = fileViewModel.FileType.Equals(QrFileType.PDF.ToString()) ? AppConstants.PDF_UPLOAD_FOLDER
                    : AppConstants.IMG_UPLOAD_FOLDER;
                fullPath = $"{uploadPath}{fileNameReplace}";
                savedFileName = Server.MapPath(fullPath);
                file.SaveAs(savedFileName); // upload new file

            }
            //update db
            try
            {

                var qrCodeModel = new QrCodeModel
                {
                    Id = fileViewModel.Id,
                    EmbedLogoId = fileViewModel.EmbedLogoId,
                    EmbedLogo = fileViewModel.EmbedLogo,
                    Foreground = fileViewModel.Foreground,
                    Background = fileViewModel.Background,
                    TemplateId = fileViewModel.TemplateId,
                    UserId = _loginUserId,
                    Label = fileViewModel.Label
                };

                //If has new file updaload, update new file 
                if (!string.IsNullOrEmpty(fileNameReplace))
                {
                    qrCodeModel.Filename = fileNameReplace;
                }

                if (_qrManagementService.UpdateQrCode(qrCodeModel)) //update successfull
                {
                    // delete old file just updloaded if has file
                    if (file != null)
                    {
                        string fileNeedDelelete = Server.MapPath($"{uploadPath}{fileViewModel.Filename}");
                        if (System.IO.File.Exists(fileNeedDelelete))
                        {
                            System.IO.File.Delete(fileNeedDelelete); // delete old file
                        }
                    }
                    return RedirectToAction(MVC.QrManagement.Manage());
                }
                else //update not succees
                {
                    // delete file just updloaded if existed
                    if (file != null && System.IO.File.Exists(savedFileName))
                    {
                        System.IO.File.Delete(savedFileName);
                    }

                }
            }
            catch (Exception e)
            {
            }

            return RedirectToAction(MVC.QrManagement.Create(2));
        }

        [HttpGet]
        public virtual ActionResult Edit(string id)
        {
            var qrCode = _qrManagementService.GetQrCodeById(id);
            string msg;
            if (!AllowToOpen(out msg, qrCode, true))
            {
                return GoToUnAuthorizePage(msg);
            }
            
            ViewBag.Type = qrCode.QrType;

            var qrCodeModel = new QrCodeModel
            {
                QrType = qrCode.QrType,
                Background = qrCode.Background,
                EmbedLogo = qrCode.EmbedLogo,
                Filename = qrCode.QrType == (int)QrType.File ? qrCode.Filename : null,
                Id = qrCode.Id,
                Foreground = qrCode.Foreground,
                IsArchived = qrCode.IsArchived,
                Logos = _logoService.GetLogoByUser(_loginUserId),
                Templates = _templateService.GetQrTemplateByUser(_loginUserId),
                SelectedLogoId = qrCode.EmbedLogoId,
                SelectedTemplateId = qrCode.TemplateId,
                TargetUrl = qrCode.TargetUrl,
                TemplateId = qrCode.TemplateId,
                Label = qrCode.Title,
                StatisticsUrl = qrCode.StatisticsUrl,
                EmbedLogoId = qrCode.EmbedLogoId
            };

            if (qrCode.QrType == (int)QrType.VCard)
            {
                var vCardDetail = _qrVCardDetailService.GetVCardDetailByQrId(id);
                var vCardViewModel = new VCardViewModel
                {
                    QrType = qrCode.QrType,
                    Background = qrCode.Background,
                    EmbedLogo = qrCode.EmbedLogo,
                    Filename = qrCode.QrType == (int)QrType.File ? qrCode.Filename : null,
                    Id = qrCode.Id,
                    Foreground = qrCode.Foreground,
                    IsArchived = qrCode.IsArchived,
                    Logos = _logoService.GetLogoByUser(_loginUserId),
                    Templates = _templateService.GetQrTemplateByUser(_loginUserId),
                    SelectedLogoId = qrCode.EmbedLogoId,
                    SelectedTemplateId = qrCode.TemplateId,
                    TargetUrl = qrCode.TargetUrl,
                    TemplateId = qrCode.TemplateId,
                    Label = qrCode.Title,
                    StatisticsUrl = qrCode.StatisticsUrl,
                    EmbedLogoId = qrCode.EmbedLogoId,
                    FirstName = vCardDetail.FirstName,
                    LastName = vCardDetail.LastName,
                    Company = vCardDetail.Company,
                    JobTitle = vCardDetail.JobTitle,
                    Street = vCardDetail.Street,
                    ZipCode = vCardDetail.Zipcode,
                    City = vCardDetail.City,
                    Country = vCardDetail.Country,
                    State = vCardDetail.State,
                    MobileNumber = vCardDetail.MobileNumber,
                    Phone = vCardDetail.Phone,
                    Fax = vCardDetail.Fax,
                    Email = vCardDetail.Email,
                    Website = vCardDetail.Website,
                    About = vCardDetail.About,
                    ProfileImage = vCardDetail.ProfileImage,
                    HeaderColor = vCardDetail.HeaderColor,
                    ButtonColor = vCardDetail.ButtonColor
                };

                return View(vCardViewModel);
            }

            return View(qrCodeModel);
        }

        public virtual ActionResult UpdateUrl(UrlViewModel createUrlViewModel)
        {
            var qrCodeModel = new QrCodeModel
            {
                Id = createUrlViewModel.Id,
                Background = createUrlViewModel.Background,
                Foreground = createUrlViewModel.Foreground,
                EmbedLogo = createUrlViewModel.EmbedLogo,
                EmbedLogoId = createUrlViewModel.EmbedLogoId,
                TargetUrl = createUrlViewModel.UrlText,
                TemplateId = createUrlViewModel.TemplateId,
                Label = createUrlViewModel.Label
            };

            if (_qrManagementService.UpdateQrCode(qrCodeModel))
            {
                return RedirectToAction(MVC.QrManagement.Manage());
            }

            return RedirectToAction(MVC.QrManagement.Edit(createUrlViewModel.Id));
        }

        [ProtectOwnerTemplate]
        public virtual ActionResult DeleteTemplate(int id)
        {
            var template = _templateService.GetQrTemplateById(id);
            if (_templateService.DeleteQrTemplate(id))
            {
                if (!string.IsNullOrEmpty(template.EmbedLogo) && !_logoService.LogoIsInUsed(template.EmbedLogo))
                {
                    DeleteFile(Request.MapPath($"~{AppConstants.LOGO_UPLOAD_FOLDER}/{template.EmbedLogo}"));
                }
            }

            var qrCodeModel = new QrCodeModel {Templates = _templateService.GetQrTemplateByUser(_loginUserId)};

            return PartialView(MVC.QrManagement.Views.Partials.TemplateList, qrCodeModel);
        }

        public virtual ActionResult SaveTemplate()
        {
            QrCodeModel qrCodeModel = new QrCodeModel();
            try
            {
                string strJson = new StreamReader(Request.InputStream).ReadToEnd();
                TemplateModel templateModel = JsonHelper.Deserialize<TemplateModel>(strJson);
                templateModel.UserId = _loginUserId;
                var template = _templateService.InsertQrTemplate(templateModel);

                if (template != null)
                {
                    qrCodeModel.SelectedTemplateId = template.Id;
                }

                qrCodeModel.Templates = _templateService.GetQrTemplateByUser(_loginUserId);
                return PartialView(MVC.QrManagement.Views.Partials.TemplateList, qrCodeModel);
            }
            catch (Exception e) { }

            return null;
        }

        [HttpPost]
        public virtual ActionResult UploadLogo()
        {
            QrCodeModel qrCodeModel = new QrCodeModel();
            HttpPostedFileBase file = Request.Files["files[]"] as HttpPostedFileBase;
            if (file?.ContentLength != 0)
            {
                string extention = Path.GetExtension(file?.FileName);
                string fileNameReplace = $"logo_{RandomString.GetTextFromDateTime()}{extention}";
                string savedFileName = Server.MapPath($"{AppConstants.LOGO_UPLOAD_FOLDER}{fileNameReplace}");

                file?.SaveAs(savedFileName);

                if (System.IO.File.Exists(savedFileName))
                {
                    LogoModel logoModel = new LogoModel
                    {
                        UserId = _loginUserId,
                        FileName = fileNameReplace
                    };

                    var logo = _logoService.InsertLogo(logoModel);

                    if (logo != null)
                    {
                        qrCodeModel.SelectedLogoId = logo.Id;
                    }

                    qrCodeModel.Logos = _logoService.GetLogoByUser(_loginUserId);
                    return PartialView(MVC.QrManagement.Views.Partials.LogoList, qrCodeModel);
                }

            }
            return null;
        }

        [ProtectOwnerLogo]
        public virtual ActionResult DeleteLogo(int id)
        {
            QrCodeModel qrCodeModel = new QrCodeModel();
            try
            {
                var logoName = _logoService.GetLogoById(id).FileName;
                if (_logoService.DeleteLogo(id))
                {
                    if (!_logoService.LogoIsInUsed(logoName))
                    {
                        string fullPath = Request.MapPath($"~{AppConstants.LOGO_UPLOAD_FOLDER}/{logoName}");
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }
                qrCodeModel.Logos = _logoService.GetLogoByUser(_loginUserId);

            }
            catch (Exception e) { }

            return PartialView(MVC.QrManagement.Views.Partials.LogoList, qrCodeModel);
        }

        [HttpPost]
        public virtual ActionResult UploadVcardImage()
        {
            // Upload image to temp folder, then submit vcard, we will copy temp file to main folder. So we should have a functional to remove all temp files later.
            var file = Request.Files["files[]"] as HttpPostedFileBase;
            if (file?.ContentLength != 0)
            {
                string extention = Path.GetExtension(file?.FileName);
                string fileNameReplace = $"vcard_profile_image_{RandomString.GetTextFromDateTime()}{extention}";
                string savedFileName = Server.MapPath($"{AppConstants.TEMP_VCARD_IMG_UPLOAD_FOLDER}{fileNameReplace}");
                CreateFolderIfNotExist(AppConstants.TEMP_VCARD_IMG_UPLOAD_FOLDER);

                file?.SaveAs(savedFileName);

                if (System.IO.File.Exists(savedFileName))
                {
                    return Json(new { success = true, value = fileNameReplace });
                }
            }

            return Json(new { success = false, value = string.Empty });
        }

        private void CreateFolderIfNotExist(string folderPath)
        {
            // Check the folder is existing or not before saving
            string uploadFolder = Server.MapPath(folderPath);
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
        }

        private void MoveVCardImageFile(string fileName)
        {
            string tempFilePath = Server.MapPath($"{AppConstants.TEMP_VCARD_IMG_UPLOAD_FOLDER}{fileName}");
            string filePath = Server.MapPath($"{AppConstants.VCARD_IMG_UPLOAD_FOLDER}{fileName}");
            CreateFolderIfNotExist(AppConstants.VCARD_IMG_UPLOAD_FOLDER);

            if (System.IO.File.Exists(tempFilePath))
            {
                System.IO.File.Move(tempFilePath, filePath);
            }

            // Delete file in temp folder
            DeleteAllFilesInFolder(Server.MapPath(AppConstants.TEMP_VCARD_IMG_UPLOAD_FOLDER));
        }

        private void DeleteFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private void DeleteAllFilesInFolder(string folderPath)
        {
            var directoryInfo = new DirectoryInfo(folderPath);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }

        private bool DeleteQrCode(string id)
        {
            //get qr code before delete
            var qrCode = _qrManagementService.GetQrCodeById(id);
            //get vcard detail before delete
            VCardDetail vCardDetail = null;
            if (qrCode.QrType == (int)QrType.VCard)
            {
                vCardDetail = _qrVCardDetailService.GetVCardDetailByQrId(id);
            }
            //delete qr code
            var success = _qrManagementService.DeleteQrCode(id);

            if (success)
            {
                // Delete logo not in use if QR contains logo
                if ((!string.IsNullOrEmpty(qrCode.EmbedLogo) || qrCode.EmbedLogoId > 0) && !_logoService.LogoIsInUsed(qrCode.EmbedLogo))
                {
                    DeleteFile(Request.MapPath($"~{AppConstants.LOGO_UPLOAD_FOLDER}/{qrCode.EmbedLogo}"));
                }

                // Delete vCard profile image
                if (vCardDetail != null)
                {
                    DeleteFile(Server.MapPath($"{AppConstants.VCARD_IMG_UPLOAD_FOLDER}{vCardDetail.ProfileImage}"));
                }
                else if (qrCode.QrType == (int)QrType.File) // Delete file pdf or image
                {
                    string folder = QrHelper.GetFileTypeFromFileName(qrCode.Filename) == QrFileType.PDF
                        ? AppConstants.PDF_UPLOAD_FOLDER
                        : AppConstants.IMG_UPLOAD_FOLDER;
                    DeleteFile(Server.MapPath($"{folder}{qrCode.Filename}"));
                }
            }

            return success;
        }

        private bool IsOwnerOfQr(string qrId)
        {
            var qrCode = _qrManagementService.GetQrCodeById(qrId);
            return qrCode != null && qrCode.UserId.Equals(User.Identity.GetUserId());
        }

        private bool AllowToOpen(out string msg, QRCode qrCode, bool isEditPage = false)
        {
            if (qrCode.IsArchived && isEditPage)
            {
                msg = "You cannot edit an archived QR.";
                return false;
            }

            var isAdmin = User.IsInRole("Administrator");
            //if admin open edit page with QR of other user ==> fail
            if (isAdmin && isEditPage && !IsOwnerOfQr(qrCode.Id))
            {
                msg = "You cannot edit QR of other user.";
                return false;
            }
            //if user is not an admin and open qr of other user ==>fail
            if (!isAdmin && !IsOwnerOfQr(qrCode.Id))
            {
                msg = "You don't have permission for the link.";
                return false;
            }

            msg = String.Empty;
            return true;
        }

        private ActionResult GoToUnAuthorizePage(string msg)
        {
            return RedirectToAction(MVC.Error.ErrorMsg("Request Failed", msg));
        }

    }
}