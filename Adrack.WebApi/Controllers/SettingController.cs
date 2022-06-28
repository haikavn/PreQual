using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Message;
using Adrack.Service.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using Adrack.Core;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.WebApi.Models.Settings;
using Adrack.Service.Security;
using Adrack.Service;
using Adrack.Core.Cache;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.Service.Content;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/setting")]
    public class SettingController : BaseApiController
    {
        #region fields

        private readonly IAppContext _appContext;

        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly IEmailTemplateService _emailTemplateService;

        private readonly IFileUploadService _fileUploadService;

        private readonly ISettingService _settingService;

        private readonly ISmtpAccountService _smtpAccountService;

        private readonly IPermissionService _permissionService;

        private readonly IStorageService _storageService;

        private string _uploadFolderUrl => $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/Content/Uploads/";
        #endregion

        #region constructors

        public SettingController(IAppContext appContext,
            IDateTimeHelper dateTimeHelper,
            IEmailTemplateService emailTemplateService,
            IFileUploadService fileUploadService,
            IPermissionService permissionService,
            ISettingService settingService,
            ISmtpAccountService smtpAccountService,
            IStorageService storageService)
        {
            _appContext = appContext;
            _dateTimeHelper = dateTimeHelper;
            _emailTemplateService = emailTemplateService;
            _fileUploadService = fileUploadService;
            _permissionService = permissionService;
            _settingService = settingService;
            _smtpAccountService = smtpAccountService;
            _storageService = storageService;
        }

        #endregion

        #region methods

        // POST /api/<controller>
        [Route("")]
        public IHttpActionResult Post([FromBody] SettingModel settingModel)
        {
            var permission = _permissionService.Authorize("view-edit-system-settings");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            var setting = new Setting();

            if (settingModel == null)
            {
                throw new ArgumentNullException(nameof(settingModel), "Setting model cannot be null");
            }

            settingModel.IsSaved = false;

            if (settingModel.CompanyName != null)
            {
                setting = this._settingService.GetSetting("Settings.CompanyName");
                if (setting == null)
                {
                    setting = new Setting
                    {
                        Key = "Settings.CompanyName",
                        Value = settingModel.CompanyName
                    };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.CompanyName;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.CompanyAddress != null)
            {
                setting = this._settingService.GetSetting("Settings.CompanyAddress");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.CompanyAddress", Value = settingModel.CompanyAddress };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.CompanyAddress;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.CompanyAddress2 != null)
            {
                setting = this._settingService.GetSetting("Settings.CompanyAddress2");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.CompanyAddress2", Value = settingModel.CompanyAddress2 };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.CompanyAddress2;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.City != null)
            {
                setting = this._settingService.GetSetting("Settings.City");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.City", Value = settingModel.City };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.City;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.State != null)
            {
                setting = this._settingService.GetSetting("Settings.State");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.State", Value = settingModel.State };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.State;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.ZipCode != null)
            {
                setting = this._settingService.GetSetting("Settings.ZipCode");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.ZipCode", Value = settingModel.ZipCode };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.ZipCode;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.Country != null)
            {
                setting = this._settingService.GetSetting("Settings.Country");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.Country", Value = settingModel.Country };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.Country;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.CompanyBank != null)
            {
                setting = this._settingService.GetSetting("Settings.CompanyBank");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.CompanyBank", Value = settingModel.CompanyBank };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.CompanyBank;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.AccountType != null)
            {
                setting = this._settingService.GetSetting("Settings.AccountType");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.AccountType", Value = settingModel.AccountType };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.AccountType;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.RoutingNumber != null)
            {
                setting = this._settingService.GetSetting("Settings.RoutingNumber");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.RoutingNumber", Value = settingModel.RoutingNumber };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.RoutingNumber;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.AccountNumber != null)
            {
                setting = this._settingService.GetSetting("Settings.AccountNumber");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.AccountNumber", Value = settingModel.AccountNumber };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.AccountNumber;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.CompanyEmail != null)
            {
                setting = this._settingService.GetSetting("Settings.CompanyEMail");
                if (setting == null)
                {
                    setting = new Setting { Key = "Settings.CompanyEMail", Value = settingModel.CompanyEmail };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.CompanyEmail;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.PageTitle != null)
            {
                setting = this._settingService.GetSetting("SeoSetting.PageTitle");
                if (setting == null)
                {
                    setting = new Setting { Key = "SeoSetting.PageTitle", Value = settingModel.PageTitle };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.PageTitle;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            #region fileupload

            string fileName = string.Empty; // Request.Files["CompanyLogo"].FileName

            if (!string.IsNullOrEmpty(fileName))
            {
                string extension = System.IO.Path.GetExtension(fileName);
                string targetFolder = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
                string targetFileName = $"logo-{_appContext.AppUser.Id.ToString()}{extension}";
                string targetTempFileName = $"logo-tmp-{_appContext.AppUser.Id.ToString()}{extension}";
                string targetPath = System.IO.Path.Combine(targetFolder, targetFileName);
                string targetPathTmp = System.IO.Path.Combine(targetFolder, targetTempFileName);


                bool isSaved =
                    _fileUploadService.UploadLogoFile(fileName, _appContext.AppUser.Id, out var errorMessage);
                if (!isSaved)
                {
                    settingModel.ErrorMessage = errorMessage;
                }
                else
                {
                    setting = this._settingService.GetSetting("Settings.CompanyLogoPath");
                    setting.Value = $"/Uploads/{targetFileName}";
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
            }

            #endregion

            if (settingModel.PostingUrl != null)
            {
                setting = this._settingService.GetSetting("System.PostingUrl");
                if (setting == null)
                {
                    setting = new Setting { Key = "System.PostingUrl", Value = settingModel.PostingUrl };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.PostingUrl;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }


            if (settingModel.AffiliateXmlField != null)
            {
                setting = this._settingService.GetSetting("System.AffiliateXmlField");
                if (setting == null)
                {
                    setting = new Setting { Key = "System.AffiliateXmlField", Value = settingModel.AffiliateXmlField };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.AffiliateXmlField;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.LoginExpire != null)
            {
                setting = this._settingService.GetSetting("UserSetting.CookieExpireMinutes");
                if (setting == null)
                {
                    setting = new Setting
                    { Key = "UserSetting.CookieExpireMinutes", Value = settingModel.LoginExpire.ToString() };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.LoginExpire.ToString();
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
                try
                {
                    //_appContext.SetUserCookie(Guid.Parse(_appContext.AppUser.GuId));
                    _appContext.SetUserExpire(_appContext.AppUser.Id);
                }
                catch
                {
                }
            }

            /*if (settingModel.MaxProcessingLeads != null)
            {
                set = this._settingService.GetSetting("System.MaxProcessingLeads");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "System.MaxProcessingLeads";
                    set.Value = settingModel.MaxProcessingLeads;
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = settingModel.MaxProcessingLeads;
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;

                int maxProcessingLeads = 0;
                if (int.TryParse(Request["MaxProcessingLeads"], out maxProcessingLeads))
                {
                    GlobalDataManager.MaxProcessingLeads = maxProcessingLeads;
                }
            }*/

            if (settingModel.ProcessingDelayString != null)
            {
                setting = this._settingService.GetSetting("System.ProcessingDelay");
                if (setting == null)
                {
                    setting = new Setting { Key = "System.ProcessingDelay", Value = settingModel.ProcessingDelayString };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.ProcessingDelayString;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;

                if (int.TryParse(settingModel.ProcessingDelayString, out var processingDelay))
                {
                    GlobalDataManager.ProcessingDelay = processingDelay;
                }
            }

            if (settingModel.DuplicateMonitor != null)
            {
                setting = this._settingService.GetSetting("System.DublicateMonitor");
                if (setting != null)
                {
                    setting.Value = settingModel.DuplicateMonitor.ToString();
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting
                    { Key = "System.DublicateMonitor", Value = settingModel.DuplicateMonitor.ToString() };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.AllowAffiliateRedirect != null)
            {
                setting = this._settingService.GetSetting("System.AllowAffiliateRedirect");
                if (setting != null)
                {
                    setting.Value = settingModel.AllowAffiliateRedirect.ToString();
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting
                    {
                        Key = "System.AllowAffiliateRedirect",
                        Value = settingModel.AllowAffiliateRedirect.ToString()
                    };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.AffiliateRedirectUrl != null)
            {
                setting = this._settingService.GetSetting("System.AffiliateRedirectUrl");
                if (setting != null)
                {
                    setting.Value = settingModel.AffiliateRedirectUrl;
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting
                    { Key = "System.AffiliateRedirectUrl", Value = settingModel.AffiliateRedirectUrl };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.LeadEmail != null)
            {
                setting = this._settingService.GetSetting("System.LeadEmail");
                if (setting != null)
                {
                    setting.Value = settingModel.LeadEmail.ToString();
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "System.LeadEmail", Value = settingModel.LeadEmail.ToString() };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.LeadEmailTo != null)
            {
                setting = this._settingService.GetSetting("System.LeadEmailTo");
                if (setting != null)
                {
                    setting.Value = settingModel.LeadEmailTo;
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "System.LeadEmailTo", Value = settingModel.LeadEmailTo };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.LeadEmailFields != null)
            {
                setting = this._settingService.GetSetting("System.LeadEmailFields");
                if (setting != null)
                {
                    setting.Value = settingModel.LeadEmailFields;
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "System.LeadEmailFields", Value = settingModel.LeadEmailFields };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.WhiteIp != null)
            {
                setting = this._settingService.GetSetting("System.WhiteIp");
                if (setting == null)
                {
                    setting = new Setting { Key = "System.WhiteIp", Value = settingModel.WhiteIp };
                    this._settingService.InsertSetting(setting);
                }
                else
                {
                    setting.Value = settingModel.WhiteIp;
                    this._settingService.UpdateSetting(setting);
                }

                settingModel.IsSaved = true;
            }

            if (settingModel.MinProcessingMode != null)
            {
                setting = this._settingService.GetSetting("System.MinProcessingMode");
                if (setting != null)
                {
                    setting.Value = settingModel.MinProcessingMode.ToString();
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting
                    { Key = "System.MinProcessingMode", Value = settingModel.MinProcessingMode.ToString() };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.SystemOnHold != null)
            {
                setting = this._settingService.GetSetting("System.SystemOnHold");
                if (setting != null)
                {
                    setting.Value = settingModel.SystemOnHold.ToString();
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "System.SystemOnHold", Value = settingModel.SystemOnHold.ToString() };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.AutoCacheModeString != null)
            {
                short.TryParse(settingModel.AutoCacheModeString, out short enabledAutoCacheMode);

                MemoryCacheManager.EnableAutoCacheMode = (enabledAutoCacheMode != 0);

                setting = this._settingService.GetSetting("System.AutoCacheMode");
                if (setting != null)
                {
                    setting.Value = settingModel.AutoCacheModeString;
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "System.AutoCacheMode", Value = settingModel.AutoCacheModeString };
                    this._settingService.InsertSetting(setting);
                }
            }

            if (settingModel.AutoCacheUrls != null)
            {
                MemoryCacheManager.AutoCacheUrls = settingModel.AutoCacheUrls;

                setting = this._settingService.GetSetting("System.AutoCacheUrls");
                if (setting != null)
                {
                    setting.Value = settingModel.AutoCacheUrls;
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "System.AutoCacheUrls", Value = settingModel.AutoCacheUrls };
                    this._settingService.InsertSetting(setting);
                }
            }

            if(settingModel.EmailProvider!=0)
            {
                setting = this._settingService.GetSetting("System.EmailProvider");
                if (setting != null)
                {
                    setting.Value = settingModel.EmailProvider.ToString();
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "System.EmailProvider", Value = settingModel.EmailProvider.ToString() };
                    this._settingService.InsertSetting(setting);
                }
            }

            setting = this._settingService.GetSetting("System.DebugMode");
            if (setting != null)
            {
                setting.Value = settingModel.DebugMode.ToString();
                this._settingService.UpdateSetting(setting);
                settingModel.IsSaved = true;
            }
            else
            {
                setting = new Setting { Key = "System.DebugMode", Value = settingModel.DebugMode.ToString() };
                this._settingService.InsertSetting(setting);
            }

            if (settingModel.AppUrl != null)
            {
                setting = this._settingService.GetSetting("AppSetting.Url");
                if (setting != null)
                {
                    setting.Value = settingModel.AppUrl;
                    this._settingService.UpdateSetting(setting);
                    settingModel.IsSaved = true;
                }
                else
                {
                    setting = new Setting { Key = "AppSetting.Url", Value = settingModel.AppUrl };
                    this._settingService.InsertSetting(setting);
                }
            }
            return Ok();
        }

        // GET /api/<controller>
        [Route("")]
        public IHttpActionResult Get()
        {

            var permission = _permissionService.Authorize("view-edit-system-settings");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            var settingModel = new SettingModel();

            try
            {
                Setting pageTitle = this._settingService.GetSetting("SeoSetting.PageTitle");
                settingModel.PageTitle = pageTitle.Value ?? string.Empty;

                Setting companyName = this._settingService.GetSetting("Settings.CompanyName");
                settingModel.CompanyName = companyName?.Value ?? string.Empty;

                Setting companyAddress = this._settingService.GetSetting("Settings.CompanyAddress");
                settingModel.CompanyAddress = companyAddress?.Value ?? string.Empty;

                Setting companyAddress2 = this._settingService.GetSetting("Settings.CompanyAddress2");
                settingModel.CompanyAddress2 = companyAddress2?.Value ?? string.Empty;

                Setting city = this._settingService.GetSetting("Settings.City");
                settingModel.City = city?.Value ?? string.Empty;

                Setting state = this._settingService.GetSetting("Settings.State");
                settingModel.State = state?.Value ?? string.Empty;

                Setting zipCode = this._settingService.GetSetting("Settings.ZipCode");
                settingModel.ZipCode = zipCode?.Value ?? string.Empty;

                Setting country = this._settingService.GetSetting("Settings.Country");
                settingModel.Country = country?.Value ?? string.Empty;

                Setting companyBank = this._settingService.GetSetting("Settings.CompanyBank");
                settingModel.CompanyBank = companyBank?.Value ?? string.Empty;

                Setting accountType = this._settingService.GetSetting("Settings.AccountType");
                settingModel.AccountType = accountType?.Value ?? string.Empty;

                Setting routingNumber = this._settingService.GetSetting("Settings.RoutingNumber");
                settingModel.RoutingNumber = routingNumber?.Value ?? string.Empty;

                Setting accountNumber = this._settingService.GetSetting("Settings.AccountNumber");
                settingModel.AccountNumber = accountNumber?.Value ?? string.Empty;

                Setting companyEmail = this._settingService.GetSetting("Settings.CompanyEmail");
                settingModel.CompanyEmail = companyEmail?.Value ?? string.Empty;

                Setting companyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath");
                settingModel.CompanyLogoPath = companyLogoPath?.Value ?? string.Empty;

                Setting postingUrl = this._settingService.GetSetting("System.PostingUrl");
                settingModel.PostingUrl = postingUrl?.Value ?? string.Empty;

                Setting affiliateXmlField = this._settingService.GetSetting("System.AffiliateXmlField");
                settingModel.AffiliateXmlField = affiliateXmlField?.Value ?? string.Empty;

                Setting cookieExpireMinutes = this._settingService.GetSetting("UserSetting.CookieExpireMinutes");
                settingModel.LoginExpire = cookieExpireMinutes == null ? 0 : int.Parse(cookieExpireMinutes.Value);

                Setting maxProcessingLeads = this._settingService.GetSetting("System.MaxProcessingLeads");
                settingModel.MaxProcessingLeads =
                    (maxProcessingLeads == null ? 10 : int.Parse(maxProcessingLeads.Value));

                Setting processingDelay = this._settingService.GetSetting("System.ProcessingDelay");
                settingModel.ProcessingDelay = (processingDelay == null ? 500 : int.Parse(processingDelay.Value));

                Setting duplicateMonitor = this._settingService.GetSetting("System.DublicateMonitor");
                settingModel.DuplicateMonitor =
                    (duplicateMonitor == null ? (short)1 : short.Parse(duplicateMonitor.Value));

                Setting allowAffiliateRedirect = this._settingService.GetSetting("System.AllowAffiliateRedirect");
                settingModel.AllowAffiliateRedirect = (allowAffiliateRedirect == null
                    ? (short)1
                    : short.Parse(allowAffiliateRedirect.Value));

                Setting affiliateRedirectUrl = this._settingService.GetSetting("System.AffiliateRedirectUrl");
                settingModel.AffiliateRedirectUrl = affiliateRedirectUrl?.Value ?? string.Empty;

                Setting whiteIp = this._settingService.GetSetting("System.WhiteIp");
                settingModel.WhiteIp = whiteIp?.Value ?? string.Empty;

                Setting leadEmail = this._settingService.GetSetting("System.LeadEmail");
                settingModel.LeadEmail = (leadEmail == null ? (short)1 : short.Parse(leadEmail.Value));

                Setting leadEmailTo = this._settingService.GetSetting("System.LeadEmailTo");
                settingModel.LeadEmailTo = leadEmailTo?.Value ?? string.Empty;

                Setting leadEmailFields = this._settingService.GetSetting("System.LeadEmailFields");
                settingModel.LeadEmailFields = leadEmailFields?.Value ?? string.Empty;

                Setting minProcessingMode = this._settingService.GetSetting("System.MinProcessingMode");
                settingModel.MinProcessingMode =
                    (minProcessingMode == null ? (short)1 : short.Parse(minProcessingMode.Value));

                Setting autoCacheMode = this._settingService.GetSetting("System.AutoCacheMode");
                settingModel.AutoCacheMode = (autoCacheMode == null ? (short)0 : short.Parse(autoCacheMode.Value));

                Setting autoCacheUrls = this._settingService.GetSetting("System.AutoCacheUrls");
                settingModel.AutoCacheUrls = autoCacheUrls?.Value ?? string.Empty;

                Setting systemOnHold = this._settingService.GetSetting("System.SystemOnHold");
                settingModel.SystemOnHold = (systemOnHold == null ? (short)0 : short.Parse(systemOnHold.Value));

                Setting debugMode = this._settingService.GetSetting("System.DebugMode");
                settingModel.DebugMode = (debugMode == null ? (short)1 : short.Parse(debugMode.Value));

                Setting appUrl = this._settingService.GetSetting("AppSetting.Url");
                settingModel.AppUrl = appUrl?.Value ?? string.Empty;

                Setting emailProvider = this._settingService.GetSetting("System.EmailProvider");
                settingModel.EmailProvider = emailProvider!=null?Convert.ToInt16(emailProvider.Value) : Convert.ToInt16(1);

            }
            catch
            {
            }

            return Ok(settingModel);
        }



        // GET: api/<controller>/timezone
        [HttpGet]
        [Route("timezone")]
        public SettingTimeZoneModel GetTimeZone()
        {
            Setting setting = _settingService.GetSetting("TimeZoneStr");

            if (setting == null){
                setting = _settingService.GetSetting("TimeZone");
            }

            string timeZone = setting?.Value ?? string.Empty;

            var settingTimeZoneModel = new SettingTimeZoneModel
            {
                TimeZones = _dateTimeHelper.GetSystemTimeZones(timeZone)
            };

            return settingTimeZoneModel;
        }

        // GET: api/<controller>/email/templates
        [Route("email/templates")]
        public SettingEmailTemplatesModel GetEmailTemplates()
        {
            var emailsTemplateModel = new SettingEmailTemplatesModel();

            var emailTemplatesList = this._emailTemplateService.GetAllEmailTemplates();

            if (emailTemplatesList == null)
            {
                throw new ArgumentNullException(nameof(emailTemplatesList),
                    $"Email template has not found.");
            }

            foreach (var emailTemplate in emailTemplatesList)
            {
                var emailTemplateModel = new EmailTemplateModel
                {
                    Id = emailTemplate.Id,
                    Name = emailTemplate.Name,
                    Subject = emailTemplate.Subject,
                    Body = emailTemplate.Body,
                    Active = emailTemplate.Active,
                    IsAuthorized = _permissionService.Authorize(PermissionProvider.SettingsEMailTemplatesModify)
                };

                emailsTemplateModel.EmailTemplates.Add(emailTemplateModel);
            }

            return emailsTemplateModel;
        }

        // POST: api/<controller>/email/template/5
        [Route("email/template/{id}")]
        public void Post([FromUri] long id, [FromBody] EmailTemplateModel templateModel)
        {
            if (templateModel == null || string.IsNullOrEmpty(templateModel.Name)
                                      || string.IsNullOrEmpty(templateModel.Body)
                                      || string.IsNullOrEmpty(templateModel.Subject)
                                      || string.IsNullOrEmpty(templateModel.SmtpAccountId)
                                      || string.IsNullOrEmpty(templateModel.AttachmentId))
            {
                throw new ArgumentNullException(nameof(templateModel),
                    "Please, fill the Name, Subject, Body, SmtpAccount, Attachment fields.");
            }

            if (!long.TryParse(templateModel.SmtpAccountId, out var smtpAccountId)
                || !long.TryParse(templateModel.AttachmentId, out var attachmentId))
            {
                throw new ArgumentNullException(nameof(templateModel),
                    "Please, fill correct SmtpAccount, Attachment field values.");
            }

            var emailTemplate = this._emailTemplateService.GetEmailTemplateById(id);

            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(id),
                    $"there is no email template for given id equals to {id}");
            }

            emailTemplate.Id = id;
            emailTemplate.Name = templateModel.Name;
            if (templateModel.Bcc != null)
            {
                emailTemplate.Bcc = templateModel.Bcc;
            }

            emailTemplate.Body = templateModel.Body;
            emailTemplate.Subject = templateModel.Subject;
            emailTemplate.SmtpAccountId = smtpAccountId;
            emailTemplate.AttachmentId = attachmentId;

            _emailTemplateService.UpdateEmailTemplate(emailTemplate);
        }

        // GET: api/<controller>/email/template/5
        [Route("email/template/{id}")]
        public EmailTemplateModel GetEmailTemplate([FromUri] long id)
        {
            var emailTemplate = this._emailTemplateService.GetEmailTemplateById(id);

            if (emailTemplate == null)
            {
                throw new ArgumentNullException(nameof(id),
                    $"there is no email template for given id equals to {id}");
            }

            var emailTemplateModel = new EmailTemplateModel
            {
                Active = emailTemplate.Active,
                AttachmentId = emailTemplate.AttachmentId.ToString(),
                Bcc = emailTemplate.Bcc,
                Body = emailTemplate.Body,
                Id = emailTemplate.Id,
                Name = emailTemplate.Name,
                SmtpAccountId = emailTemplate.SmtpAccountId.ToString(),
                Subject = emailTemplate.Subject
            };

            return emailTemplateModel;
        }



        // POST: api/<controller>/timezone
        [HttpPost]
        [Route("timezone")]
        public void Post([FromBody] SettingTimeZoneInsertModel settingTimeZoneModel)
        {
            if (settingTimeZoneModel == null || string.IsNullOrEmpty(settingTimeZoneModel.SelectedTimeZone))
            {
                throw new ArgumentNullException(nameof(settingTimeZoneModel),
                    "Setting time zone parameters are empty.");
            }

            Setting timezoneSetting = this._settingService.GetSetting("TimeZone");
            Setting timezoneStringSetting = this._settingService.GetSetting("TimeZoneStr");

            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(settingTimeZoneModel.SelectedTimeZone);
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            TimeSpan timeSpan = localDateTime - DateTime.UtcNow;
            var totalHoursString = timeSpan.TotalHours.ToString();

            if (timezoneSetting == null)
            {
                timezoneSetting = new Setting { Key = "TimeZone", Value = totalHoursString };
                this._settingService.InsertSetting(timezoneSetting);
            }
            else
            {
                timezoneSetting.Value = totalHoursString;
                this._settingService.UpdateSetting(timezoneSetting);
            }

            if (timezoneStringSetting == null)
            {
                timezoneStringSetting = new Setting
                { Key = "TimeZoneStr", Value = settingTimeZoneModel.SelectedTimeZone };
                this._settingService.InsertSetting(timezoneStringSetting);
            }
            else
            {
                timezoneStringSetting.Value = settingTimeZoneModel.SelectedTimeZone;
                this._settingService.UpdateSetting(timezoneStringSetting);
            }
        }

        /// <summary>
        /// Get posting details 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getSystemSettings")]
        public IHttpActionResult GetSystemSettings()
        {
            var permission = _permissionService.Authorize("view-edit-system-settings");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            var systemSettings = new SystemSettingsModel();
            var postingDetailsModel = GetPostingDetails();
            var loginSession = GetLoginSession();
            var smtpSettings = GetSmtpSettings();

            systemSettings.loginSession = loginSession;
            systemSettings.postingDetails = postingDetailsModel;
            systemSettings.settingsSmtpModel = smtpSettings;


            return Ok(systemSettings);
        }

        /// <summary>
        /// Get company settings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getCompanyInfoSettings")]
        public IHttpActionResult GetCompanyInfoSettings()
        {
            var permission = _permissionService.Authorize("view-edit-company-information");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            var companySettings = GetCompanySettings();

            return Ok(companySettings);
        }

        /// <summary>
        /// Insert or update posting details settings
        /// </summary>
        /// <param name="systemSettingsModel">SystemSettingsModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateSystemSettings")]
        public IHttpActionResult UpdateSystemSettings([FromBody] SystemSettingsModel systemSettingsModel)
        {
            var permission = _permissionService.Authorize("view-edit-system-settings");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                InsertOrUpdateLoginSession(systemSettingsModel.loginSession);
                InsertOrUpdatePostingDetails(systemSettingsModel.postingDetails);
                InsertOrUpdateSmtpSettings(systemSettingsModel.settingsSmtpModel);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(systemSettingsModel);
        }



        /// <summary>
        /// Insert or update company information settings
        /// </summary>
        /// <param name="companyInfoModel">CompanyInfoModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateCompanyInfoSettings")]
        public IHttpActionResult UpdateCompanyInfoSettings([FromBody] CompanyInfoModel companyInfoModel)
        {
            var permission = _permissionService.Authorize("view-edit-company-information");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            if (companyInfoModel == null || !ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Company information parameters are empty");
                return HttpBadRequest(null);
            }

            InsertOrUpdateCompanyInfo(companyInfoModel);

            return Ok(companyInfoModel);
        }

        [Route("uploadCompanyLogo")]
        public IHttpActionResult UploadCompanyLogo()
        {
            var permission = _permissionService.Authorize("view-edit-company-information");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files["CompanyLogo"] != null)
                {
                    var file = httpRequest.Files.Get("CompanyLogo");
                    var ext = Path.GetExtension(file.FileName);
                    var logoName = $"company_logo_{_appContext.AppUser.Id}{ext}";
                    var relativePath = "~/Content/Uploads/";
                    if (file != null)
                    {
                        var blobPath = "uploads";
                        var uri = _storageService.Upload(blobPath, file.InputStream, file.ContentType, logoName);
                        InsertOrUpdateSetting("Settings.CompanyLogoPath", uri.AbsoluteUri);

                        return Ok(uri.AbsoluteUri);

                        Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(fs);
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                        var targetPath = Path.Combine(targetFolder, logoName);


                        var validationResult = ValidationHelper.ValidateImage(bytes, file.FileName.Split('.')[file.FileName.Split('.').Length - 1]
                            , new List<string> { "png", "jpg", "jpeg", "gif" }, 1024, 768, 1048576);

                        if (!validationResult.Item1)
                        {
                            return HttpBadRequest(validationResult.Item2);
                        }


                        if (File.Exists(targetPath))
                        {
                            File.Delete(targetPath);
                        }

                        file.SaveAs(targetPath);
                    }
                    else
                        HttpBadRequest("File not attached");

                    InsertOrUpdateSetting("Settings.CompanyLogoPath", logoName);
                    return Ok($"{_uploadFolderUrl}{logoName}");
                }
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("removeCompanyLogo")]
        public IHttpActionResult RemoveCompanyLogo()
        {
            var permission = _permissionService.Authorize("view-edit-company-information");
            if (!permission)
            {
                return HttpBadRequest("access-denied");
            }
            try
            {
                var companyLogoPath = GetCompanySettings().LogoPath;

                if (string.IsNullOrWhiteSpace(companyLogoPath))
                {
                    return HttpBadRequest("No company logo to delete");
                }
                var relativePath = "~/Content/Uploads/";
                var targetFolder = HttpContext.Current.Server.MapPath(relativePath);
                var splittedFileName = companyLogoPath.Split(new[] { "company_logo_" }, StringSplitOptions.None);
                if (splittedFileName.Length != 0)
                {
                    var logoName = $"company_logo_{splittedFileName[1]}";
                    var fileName = Path.Combine(targetFolder, logoName);
                    File.Delete(fileName);
                }

                InsertOrUpdateSetting("Settings.CompanyLogoPath", string.Empty);


                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("saveCustomSettings")]
        public IHttpActionResult SaveCustomSettings([FromBody] List<CustomSettingModel> customSettings)
        {
            var output = JsonConvert.SerializeObject(customSettings);

            var setting = _settingService.GetSetting("CustomSetting-" + _appContext.AppUser.Id.ToString());
            if (setting == null)
            {
                setting = new Setting();
                setting.Key = "CustomSetting-" + _appContext.AppUser.Id.ToString();
                setting.Value = output;
                _settingService.InsertSetting(setting);
            }
            else
            {
                setting.Value = output;
                _settingService.UpdateSetting(setting);
            }

            return Ok(customSettings);
        }

        [HttpGet]
        [Route("getCustomSettings")]
        public IHttpActionResult GetCustomSetting()
        {
            List<CustomSettingModel> customSettings = new List<CustomSettingModel>();
            var setting = _settingService.GetSetting("CustomSetting-" + _appContext.AppUser.Id.ToString());
            if (setting == null)
            {
                return Ok(customSettings);
            }

            try
            {
                customSettings = JsonConvert.DeserializeObject<List<CustomSettingModel>>(setting.Value);
            }
            catch
            {

            }

            return Ok(customSettings);
        }

        #endregion methods

        #region PrivateMethods
        private SettingSmtpModel GetSmtpSettings()
        {

            var smtpSetting = this._smtpAccountService.GetSmtpAccount();
            var settingSmtpModel = new SettingSmtpModel()
            {
                Id = 78,
                SmtpDisplayName = smtpSetting.DisplayName,
                SmtpEmail = smtpSetting.Email,
                SmtpHost = smtpSetting.Host,
                SmtpPort = smtpSetting.Port,
                SmtpUsername = smtpSetting.Username,
                SmtpPassword = smtpSetting.Password,
                SmtpEnableSsl = smtpSetting.EnableSsl,
                SmtpUseDefaultCredentials = smtpSetting.UseDefaultCredentials,
            };

            return settingSmtpModel;
        }
        private PostingDetailsModel GetPostingDetails()
        {
            try
            {
                var postingDetailsModel = new PostingDetailsModel();

                var appUrl = _settingService.GetSetting("AppSetting.Url");
                postingDetailsModel.ApplicationUrl = appUrl?.Value ?? string.Empty;

                var postingUrl = _settingService.GetSetting("System.PostingUrl");
                postingDetailsModel.PostingUrl = postingUrl?.Value ?? string.Empty;

                var affiliateXmlField = _settingService.GetSetting("System.AffiliateXmlField");
                postingDetailsModel.AffiliateChannelXmlField = affiliateXmlField?.Value ?? string.Empty;

                var duplicateMonitor = _settingService.GetSetting("System.DublicateMonitor");
                postingDetailsModel.IsDuplicatedMonitor = (duplicateMonitor == null || Convert.ToBoolean(Convert.ToInt16(duplicateMonitor.Value)));

                var allowAffiliateRedirect = _settingService.GetSetting("System.AllowAffiliateRedirect");
                postingDetailsModel.IsAllowedAffiliateRedirect = (allowAffiliateRedirect == null || Convert.ToBoolean(Convert.ToInt16(allowAffiliateRedirect.Value)));

                var affiliateRedirectUrl = _settingService.GetSetting("System.AffiliateRedirectUrl");
                postingDetailsModel.AffiliateRedirectUrl = affiliateRedirectUrl?.Value ?? string.Empty;

                var whiteIp = _settingService.GetSetting("System.WhiteIp");
                postingDetailsModel.WhiteIp = whiteIp?.Value ?? string.Empty;

                var minProcessingMode = _settingService.GetSetting("System.MinProcessingMode");
                postingDetailsModel.MinProcessingMode = (minProcessingMode == null || Convert.ToBoolean(Convert.ToInt16(minProcessingMode.Value)));

                var systemOnHold = _settingService.GetSetting("System.SystemOnHold");
                postingDetailsModel.SystemOnHold = (systemOnHold == null || Convert.ToBoolean(Convert.ToInt16(systemOnHold.Value)));

                var debugMode = _settingService.GetSetting("System.DebugMode");
                postingDetailsModel.DebugMode = (debugMode == null || Convert.ToBoolean(Convert.ToInt16(debugMode.Value)));

                return postingDetailsModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int GetLoginSession()
        {
            try
            {
                var loginSession = 0;
                var loginSessionString = _settingService.GetSetting("System.LoginSession");
                loginSession = Convert.ToInt32(loginSessionString?.Value ?? "20");
                return loginSession;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private CompanyInfoModel GetCompanySettings()
        {
            try
            {
                var companySettings = new CompanyInfoModel();

                var companyName = this._settingService.GetSetting("Settings.CompanyName");
                companySettings.Name = companyName?.Value ?? string.Empty;

                var companyAddress = this._settingService.GetSetting("Settings.CompanyAddress");
                companySettings.Address = companyAddress?.Value ?? string.Empty;

                var companyAddress2 = this._settingService.GetSetting("Settings.CompanyAddress2");
                companySettings.SecondaryAddress = companyAddress2?.Value ?? string.Empty;

                var companyEmail = this._settingService.GetSetting("Settings.CompanyEmail");
                companySettings.Email = companyEmail?.Value ?? string.Empty;

                var companyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath");
                companySettings.LogoPath = !string.IsNullOrWhiteSpace(companyLogoPath?.Value)? $"{companyLogoPath?.Value}" : string.Empty;

                var companyPhoneNum = this._settingService.GetSetting("Settings.CompanyPhoneNumber");
                companySettings.PhoneNumber = companyPhoneNum?.Value ?? string.Empty;

                var companyAccountManager = this._settingService.GetSetting("Settings.CompanyAccountManager");
                companySettings.AccountManager = companyAccountManager?.Value ?? string.Empty;

                return companySettings;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void InsertOrUpdateLoginSession(int loginSession)
        {
            try
            {
                InsertOrUpdateSetting("System.LoginSession", loginSession.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void InsertOrUpdatePostingDetails(PostingDetailsModel postingDetailsModel)
        {
            try
            {
                InsertOrUpdateSetting("AppSetting.Url", postingDetailsModel.ApplicationUrl);
                InsertOrUpdateSetting("System.PostingUrl", postingDetailsModel.PostingUrl);
                InsertOrUpdateSetting("System.AffiliateXmlField", postingDetailsModel.AffiliateChannelXmlField);
                InsertOrUpdateSetting("System.DublicateMonitor", Convert.ToInt16(postingDetailsModel.IsDuplicatedMonitor).ToString());
                InsertOrUpdateSetting("System.AllowAffiliateRedirect", Convert.ToInt16(postingDetailsModel.IsAllowedAffiliateRedirect).ToString());
                InsertOrUpdateSetting("System.AffiliateRedirectUrl", postingDetailsModel.AffiliateRedirectUrl);
                InsertOrUpdateSetting("System.WhiteIp", postingDetailsModel.WhiteIp);
                InsertOrUpdateSetting("System.MinProcessingMode", Convert.ToInt16(postingDetailsModel.MinProcessingMode).ToString());
                InsertOrUpdateSetting("System.SystemOnHold", Convert.ToInt16(postingDetailsModel.SystemOnHold).ToString());
                InsertOrUpdateSetting("System.DebugMode", Convert.ToInt16(postingDetailsModel.DebugMode).ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void InsertOrUpdateCompanyInfo(CompanyInfoModel companyInfoModel)
        {
            try
            {
                InsertOrUpdateSetting("Settings.CompanyName", companyInfoModel.Name);
                InsertOrUpdateSetting("Settings.CompanyAddress", companyInfoModel.Address);
                InsertOrUpdateSetting("Settings.CompanyAddress2", companyInfoModel.SecondaryAddress);
                InsertOrUpdateSetting("Settings.CompanyEmail", companyInfoModel.Email);
                InsertOrUpdateSetting("Settings.CompanyPhoneNumber", companyInfoModel.PhoneNumber);
                InsertOrUpdateSetting("Settings.CompanyAccountManager", companyInfoModel.AccountManager);
                InsertOrUpdateSetting("Settings.CompanyAccountManager", companyInfoModel.AccountManager);
                InsertOrUpdateSetting("Settings.CompanyLogoPath", companyInfoModel.LogoPath);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void InsertOrUpdateSetting(string key, string value)
        {
            if (value != null)
            {
                var setting = _settingService.GetSetting(key);
                if (setting != null)
                {
                    setting.Value = value;
                    _settingService.UpdateSetting(setting);
                }
                else
                {
                    setting = new Setting
                    {
                        Key = key,
                        Value = value
                    };
                    _settingService.InsertSetting(setting);
                }
            }
        }
        private void InsertOrUpdateSmtpSettings(SettingSmtpModel settingSmtpModel)
        {
            var smtpAccountNew = new SmtpAccount
            {
                Id = settingSmtpModel.Id,
                Email = settingSmtpModel.SmtpEmail,
                DisplayName = settingSmtpModel.SmtpDisplayName,
                Host = settingSmtpModel.SmtpHost,
                Port = settingSmtpModel.SmtpPort,
                Username = settingSmtpModel.SmtpUsername,
                Password = settingSmtpModel.SmtpPassword,
                EnableSsl = settingSmtpModel.SmtpEnableSsl,
                UseDefaultCredentials = settingSmtpModel.SmtpUseDefaultCredentials
            };

            this._smtpAccountService.UpdateSmtpAccount(smtpAccountNew);

        }
        #endregion
    }
}
