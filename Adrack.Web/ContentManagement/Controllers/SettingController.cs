// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SettingController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service;
using Adrack.Service.Common;
using Adrack.Service.Configuration;
using Adrack.Service.Content;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Message;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Content;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class SettingController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public class SettingController : BaseContentManagementController
    {
        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The SMTP account service
        /// </summary>
        private readonly ISmtpAccountService _smtpAccountService;

        /// <summary>
        /// The email template service
        /// </summary>
        private readonly IEmailTemplateService _emailTemplateService;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService _historyService;

        /// <summary>
        /// The date time helper
        /// </summary>
        private readonly IDateTimeHelper _dateTimeHelper;

        /// <summary>
        /// The report service
        /// </summary>
        private readonly IReportService _reportService;

        private readonly ICountryService _countryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingController"/> class.
        /// </summary>
        /// <param name="settingService">The setting service.</param>
        /// <param name="smtpAccountService">The SMTP account service.</param>
        /// <param name="emailTemplateService">The email template service.</param>
        /// <param name="appContext">The application context.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="historyService">The history service.</param>
        /// <param name="dateTimeHelper">The date time helper.</param>
        /// <param name="reportService">The report service.</param>
        /// <param name="countryService">The report service.</param>
        public SettingController(ISettingService settingService, ISmtpAccountService smtpAccountService, IEmailTemplateService emailTemplateService, IAppContext appContext, IUserService userService, IHistoryService historyService, IDateTimeHelper dateTimeHelper, IReportService reportService, ICountryService countryService)
        {
            this._settingService = settingService;
            this._smtpAccountService = smtpAccountService;
            this._emailTemplateService = emailTemplateService;
            this._appContext = appContext;
            this._userService = userService;
            this._historyService = historyService;
            this._dateTimeHelper = dateTimeHelper;
            this._reportService = reportService;
            this._countryService = countryService;
        }

        // GET: Setting
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / Company Details")]
        public ActionResult Index()
        {
            Setting set = new Setting();

            SettingModel settingModel = new SettingModel();



            settingModel.IsSaved = false;

            if (Request["CompanyName"] != null)
            {
                set = this._settingService.GetSetting("Settings.CompanyName");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.CompanyName";
                    set.Value = Request["CompanyName"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["CompanyName"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["Address"] != null)
            {
                set = this._settingService.GetSetting("Settings.CompanyAddress");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.CompanyAddress";
                    set.Value = Request["Address"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["Address"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }
            if (Request["Address2"] != null)
            {
                set = this._settingService.GetSetting("Settings.CompanyAddress2");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.CompanyAddress2";
                    set.Value = Request["Address2"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["Address2"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }
            if (Request["City"] != null)
            {
                set = this._settingService.GetSetting("Settings.City");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.City";
                    set.Value = Request["City"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["City"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }
            if (Request["State"] != null)
            {
                set = this._settingService.GetSetting("Settings.State");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.State";
                    set.Value = Request["State"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["State"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }
            if (Request["ZipCode"] != null)
            {
                set = this._settingService.GetSetting("Settings.ZipCode");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.ZipCode";
                    set.Value = Request["ZipCode"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["ZipCode"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }
            if (Request["Country"] != null)
            {
                set = this._settingService.GetSetting("Settings.Country");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.Country";
                    set.Value = Request["Country"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["Country"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["CountryId"] != null)
            {
                set = this._settingService.GetSetting("Settings.CountryId");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.CountryId";
                    set.Value = Request["CountryId"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["CountryId"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["Bank"] != null)
            {
                set = this._settingService.GetSetting("Settings.CompanyBank");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.CompanyBank";
                    set.Value = Request["Bank"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["Bank"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["AccountType"] != null)
            {
                set = this._settingService.GetSetting("Settings.AccountType");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.AccountType";
                    set.Value = Request["AccountType"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["AccountType"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["RoutingNumber"] != null)
            {
                set = this._settingService.GetSetting("Settings.RoutingNumber");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.RoutingNumber";
                    set.Value = Request["RoutingNumber"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["RoutingNumber"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["AccountNumber"] != null)
            {
                set = this._settingService.GetSetting("Settings.AccountNumber");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.AccountNumber";
                    set.Value = Request["AccountNumber"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["AccountNumber"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["EMail"] != null)
            {
                set = this._settingService.GetSetting("Settings.CompanyEMail");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "Settings.CompanyEMail";
                    set.Value = Request["EMail"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["EMail"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["WebSiteTitle"] != null)
            {
                set = this._settingService.GetSetting("SeoSetting.PageTitle");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "SeoSetting.PageTitle";
                    set.Value = Request["WebSiteTitle"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["WebSiteTitle"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request.Files["CompanyLogo"] != null)
            {
                string fileName = Request.Files["CompanyLogo"].FileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    string ext = Path.GetExtension(fileName);
                    string targetFolder = Server.MapPath("~/Uploads/");
                    string targetPath = Path.Combine(targetFolder, "logo-" + _appContext.AppUser.Id.ToString() + ext);
                    string targetPathTmp = Path.Combine(targetFolder, "logo-tmp-" + _appContext.AppUser.Id.ToString() + ext);
                    Request.Files["CompanyLogo"].SaveAs(targetPathTmp);

                    System.Drawing.Image img = System.Drawing.Image.FromFile(targetPathTmp);
                    if (img.Width > 200)
                    {
                        settingModel.ErrorMessage = "The maximum width is 200px";
                    }
                    else
                    if (img.Height > 200)
                    {
                        settingModel.ErrorMessage = "The maximum height is 200px";
                    }
                    else
                    {
                        Request.Files["CompanyLogo"].SaveAs(targetPath);
                        set = this._settingService.GetSetting("Settings.CompanyLogoPath");
                        set.Value = "/Uploads/logo-" + _appContext.AppUser.Id.ToString() + ext;
                        this._settingService.UpdateSetting(set);
                        settingModel.IsSaved = true;
                    }
                }
            }

            if (Request["PostingUrl"] != null)
            {
                set = this._settingService.GetSetting("System.PostingUrl");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "System.PostingUrl";
                    set.Value = Request["PostingUrl"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["PostingUrl"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["AffiliateXmlField"] != null)
            {
                set = this._settingService.GetSetting("System.AffiliateXmlField");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "System.AffiliateXmlField";
                    set.Value = Request["AffiliateXmlField"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["AffiliateXmlField"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;
            }

            if (Request["LoginExpire"] != null)
            {
                set = this._settingService.GetSetting("UserSetting.CookieExpireMinutes");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "UserSetting.CookieExpireMinutes";
                    set.Value = Request["LoginExpire"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["LoginExpire"];
                    this._settingService.UpdateSetting(set);
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

            /*if (Request["MaxProcessingLeads"] != null)
            {
                set = this._settingService.GetSetting("System.MaxProcessingLeads");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "System.MaxProcessingLeads";
                    set.Value = Request["MaxProcessingLeads"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["MaxProcessingLeads"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;

                int maxProcessingLeads = 0;
                if (int.TryParse(Request["MaxProcessingLeads"], out maxProcessingLeads))
                {
                    GlobalDataManager.MaxProcessingLeads = maxProcessingLeads;
                }
            }*/

            if (Request["ProcessingDelay"] != null)
            {
                set = this._settingService.GetSetting("System.ProcessingDelay");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "System.ProcessingDelay";
                    set.Value = Request["ProcessingDelay"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["ProcessingDelay"];
                    this._settingService.UpdateSetting(set);
                }
                settingModel.IsSaved = true;

                int processingDelay = 0;
                if (int.TryParse(Request["ProcessingDelay"], out processingDelay))
                {
                    GlobalDataManager.ProcessingDelay = processingDelay;
                }
            }

            if (Request["DublicateMonitor"] != null)
            {
                set = this._settingService.GetSetting("System.DublicateMonitor");
                if (set != null)
                {
                    set.Value = Request["DublicateMonitor"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.DublicateMonitor";
                    set.Value = Request["DublicateMonitor"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["AllowAffiliateRedirect"] != null)
            {
                set = this._settingService.GetSetting("System.AllowAffiliateRedirect");
                if (set != null)
                {
                    set.Value = Request["AllowAffiliateRedirect"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.AllowAffiliateRedirect";
                    set.Value = Request["AllowAffiliateRedirect"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["AffiliateRedirectUrl"] != null)
            {
                set = this._settingService.GetSetting("System.AffiliateRedirectUrl");
                if (set != null)
                {
                    set.Value = Request["AffiliateRedirectUrl"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.AffiliateRedirectUrl";
                    set.Value = Request["AffiliateRedirectUrl"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["LeadEmail"] != null)
            {
                set = this._settingService.GetSetting("System.LeadEmail");
                if (set != null)
                {
                    set.Value = Request["LeadEmail"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.LeadEmail";
                    set.Value = Request["LeadEmail"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["LeadEmailTo"] != null)
            {
                set = this._settingService.GetSetting("System.LeadEmailTo");
                if (set != null)
                {
                    set.Value = Request["LeadEmailTo"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.LeadEmailTo";
                    set.Value = Request["LeadEmailTo"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["LeadEmailFields"] != null)
            {
                set = this._settingService.GetSetting("System.LeadEmailFields");
                if (set != null)
                {
                    set.Value = Request["LeadEmailFields"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.LeadEmailFields";
                    set.Value = Request["LeadEmailFields"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["WhiteIp"] != null)
            {
                set = this._settingService.GetSetting("System.WhiteIp");
                if (set == null)
                {
                    set = new Setting();
                    set.Key = "System.WhiteIp";
                    set.Value = Request["WhiteIp"];
                    this._settingService.InsertSetting(set);
                }
                else
                {
                    set.Value = Request["WhiteIp"];
                    this._settingService.UpdateSetting(set);
                }

                settingModel.IsSaved = true;
            }

            if (Request["MinProcessingMode"] != null)
            {
                set = this._settingService.GetSetting("System.MinProcessingMode");
                if (set != null)
                {
                    set.Value = Request["MinProcessingMode"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.MinProcessingMode";
                    set.Value = Request["MinProcessingMode"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["SystemOnHold"] != null)
            {
                set = this._settingService.GetSetting("System.SystemOnHold");
                if (set != null)
                {
                    set.Value = Request["SystemOnHold"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.SystemOnHold";
                    set.Value = Request["SystemOnHold"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["AutoCacheMode"] != null)
            {
                short sh = 0;
                short.TryParse(Request["AutoCacheMode"], out sh);

                MemoryCacheManager.EnableAutoCacheMode = (sh == 0 ? false : true);

                set = this._settingService.GetSetting("System.AutoCacheMode");
                if (set != null)
                {
                    set.Value = Request["AutoCacheMode"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.AutoCacheMode";
                    set.Value = Request["AutoCacheMode"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["AutoCacheUrls"] != null)
            {
                MemoryCacheManager.AutoCacheUrls = Request["AutoCacheUrls"];

                set = this._settingService.GetSetting("System.AutoCacheUrls");
                if (set != null)
                {
                    set.Value = Request["AutoCacheUrls"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.AutoCacheUrls";
                    set.Value = Request["AutoCacheUrls"];
                    this._settingService.InsertSetting(set);
                }
            }


            if (Request["DebugMode"] != null)
            {
                set = this._settingService.GetSetting("System.DebugMode");
                if (set != null)
                {
                    set.Value = Request["DebugMode"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "System.DebugMode";
                    set.Value = Request["DebugMode"];
                    this._settingService.InsertSetting(set);
                }
            }

            if (Request["AppUrl"] != null)
            {
                set = this._settingService.GetSetting("AppSetting.Url");
                if (set != null)
                {
                    set.Value = Request["AppUrl"];
                    this._settingService.UpdateSetting(set);
                    settingModel.IsSaved = true;
                }
                else
                {
                    set = new Setting();
                    set.Key = "AppSetting.Url";
                    set.Value = Request["AppUrl"];
                    this._settingService.InsertSetting(set);
                }
            }

            try
            {
                Setting PageTitle = this._settingService.GetSetting("SeoSetting.PageTitle");
                settingModel.PageTitle = PageTitle == null ? "" : PageTitle.Value;

                Setting CompanyName = this._settingService.GetSetting("Settings.CompanyName");
                settingModel.CompanyName = CompanyName == null ? "" : CompanyName.Value;

                Setting CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress");
                settingModel.CompanyAddress = CompanyAddress == null ? "" : CompanyAddress.Value;

                Setting CompanyAddress2 = this._settingService.GetSetting("Settings.CompanyAddress2");
                settingModel.CompanyAddress2 = CompanyAddress2 == null ? "" : CompanyAddress2.Value;

                Setting City = this._settingService.GetSetting("Settings.City");
                settingModel.City = City == null ? "" : City.Value;

                Setting State = this._settingService.GetSetting("Settings.State");
                settingModel.State = State == null ? "" : State.Value;

                Setting ZipCode = this._settingService.GetSetting("Settings.ZipCode");
                settingModel.ZipCode = ZipCode == null ? "" : ZipCode.Value;

                Setting Country = this._settingService.GetSetting("Settings.Country");
                settingModel.Country = Country == null ? "" : Country.Value;

                Setting CountryId = this._settingService.GetSetting("Settings.CountryId");
                settingModel.CountryId = CountryId == null ? 0 : long.Parse(CountryId.Value);

                Setting CompanyBank = this._settingService.GetSetting("Settings.CompanyBank");
                settingModel.CompanyBank = CompanyBank == null ? "" : CompanyBank.Value;

                Setting AccountType = this._settingService.GetSetting("Settings.AccountType");
                settingModel.AccountType = AccountType == null ? "" : AccountType.Value;

                Setting RoutingNumber = this._settingService.GetSetting("Settings.RoutingNumber");
                settingModel.RoutingNumber = RoutingNumber == null ? "" : RoutingNumber.Value;

                Setting AccountNumber = this._settingService.GetSetting("Settings.AccountNumber");
                settingModel.AccountNumber = AccountNumber == null ? "" : AccountNumber.Value;

                Setting CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail");
                settingModel.CompanyEmail = CompanyEmail == null ? "" : CompanyEmail.Value;

                Setting CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath");
                settingModel.CompanyLogoPath = CompanyLogoPath == null ? "" : CompanyLogoPath.Value;

                Setting PostingUrl = this._settingService.GetSetting("System.PostingUrl");
                settingModel.PostingUrl = PostingUrl == null ? "" : PostingUrl.Value;

                Setting AffiliateXmlField = this._settingService.GetSetting("System.AffiliateXmlField");
                settingModel.AffiliateXmlField = AffiliateXmlField == null ? "" : AffiliateXmlField.Value;

                Setting CookieExpireMinutes = this._settingService.GetSetting("UserSetting.CookieExpireMinutes");
                settingModel.LoginExpire = CookieExpireMinutes == null ? 0 : int.Parse(CookieExpireMinutes.Value);

                Setting maxProcessingLeads = this._settingService.GetSetting("System.MaxProcessingLeads");
                settingModel.MaxProcessingLeads = (maxProcessingLeads == null ? 10 : int.Parse(maxProcessingLeads.Value));

                Setting processingDelay = this._settingService.GetSetting("System.ProcessingDelay");
                settingModel.ProcessingDelay = (processingDelay == null ? 500 : int.Parse(processingDelay.Value));

                Setting dublMonitor = this._settingService.GetSetting("System.DublicateMonitor");
                settingModel.DublicateMonitor = (dublMonitor == null ? (short)1 : short.Parse(dublMonitor.Value));

                Setting allowAffiliateRedirect = this._settingService.GetSetting("System.AllowAffiliateRedirect");
                settingModel.AllowAffiliateRedirect = (allowAffiliateRedirect == null ? (short)1 : short.Parse(allowAffiliateRedirect.Value));

                Setting affiliateRedirectUrl = this._settingService.GetSetting("System.AffiliateRedirectUrl");
                settingModel.AffiliateRedirectUrl = (affiliateRedirectUrl == null ? "" : affiliateRedirectUrl.Value);

                Setting whiteIp = this._settingService.GetSetting("System.WhiteIp");
                settingModel.WhiteIp = (whiteIp == null ? "" : whiteIp.Value);

                Setting leadEmail = this._settingService.GetSetting("System.LeadEmail");
                settingModel.LeadEmail = (leadEmail == null ? (short)1 : short.Parse(leadEmail.Value));

                Setting leadEmailTo = this._settingService.GetSetting("System.LeadEmailTo");
                settingModel.LeadEmailTo = (leadEmailTo == null ? "" : leadEmailTo.Value);

                Setting leadEmailFields = this._settingService.GetSetting("System.LeadEmailFields");
                settingModel.LeadEmailFields = (leadEmailFields == null ? "" : leadEmailFields.Value);

                Setting minProcessingMode = this._settingService.GetSetting("System.MinProcessingMode");
                settingModel.MinProcessingMode = (minProcessingMode == null ? (short)1 : short.Parse(minProcessingMode.Value));

                Setting autoCacheMode = this._settingService.GetSetting("System.AutoCacheMode");
                settingModel.AutoCacheMode = (autoCacheMode == null ? (short)0 : short.Parse(autoCacheMode.Value));

                Setting autoCacheUrls = this._settingService.GetSetting("System.AutoCacheUrls");
                settingModel.AutoCacheUrls = (autoCacheUrls == null ? "" : autoCacheUrls.Value);

                Setting systemOnHold = this._settingService.GetSetting("System.SystemOnHold");
                settingModel.SystemOnHold = (systemOnHold == null ? (short)0 : short.Parse(systemOnHold.Value));

                Setting debugMode = this._settingService.GetSetting("System.DebugMode");
                settingModel.DebugMode = (debugMode == null ? (short)1 : short.Parse(debugMode.Value));

                Setting appUrl = this._settingService.GetSetting("AppSetting.Url");
                settingModel.AppUrl = (appUrl == null ? "" : appUrl.Value);
                
            }
            catch
            {
            }

            foreach (var value in _countryService.GetAllCountries())
            {
                settingModel.ListCountry.Add(new System.Web.Mvc.SelectListItem
                {
                    Text = value.Name,
                    Value = value.Id.ToString(),
                    Selected = value.Id == settingModel.CountryId
                });
            }


            //return RedirectToAction("index");
            return View(settingModel);
        }

        /// <summary>
        /// Times the zone.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / Time Zone")]
        public ActionResult TimeZone()
        {
            SettingModel settingModel = new SettingModel();

            Setting st = _settingService.GetSetting("TimeZoneStr");

            string timeZone = st != null ? st.Value : "";

            settingModel.TimeZones = _dateTimeHelper.GetSystemTimeZones(timeZone);

            return View(settingModel);
        }

        /// <summary>
        /// Times the zone.
        /// </summary>
        /// <param name="settingModel">The setting model.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult TimeZone(SettingModel settingModel)
        {
            Setting tz = _settingService.GetSetting("TimeZone");
            Setting tzStr = _settingService.GetSetting("TimeZoneStr");

            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(settingModel.SelectedTimeZone);
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
            TimeSpan ts = localDateTime - DateTime.UtcNow;

            if (tz == null)
            {
                tz = new Setting();
                tz.Key = "TimeZone";
                tz.Value = ts.TotalHours.ToString();
                _settingService.InsertSetting(tz);
            }
            else
            {
                tz.Value = ts.TotalHours.ToString();
                _settingService.UpdateSetting(tz);
            }

            if (tzStr == null)
            {
                tzStr = new Setting();
                tzStr.Key = "TimeZoneStr";
                tzStr.Value = settingModel.SelectedTimeZone;
                _settingService.InsertSetting(tzStr);
            }
            else
            {
                tzStr.Value = settingModel.SelectedTimeZone;
                _settingService.UpdateSetting(tzStr);
            }

            string timeZone = settingModel.SelectedTimeZone;
            settingModel.SelectedTimeZone = null;
            settingModel.TimeZones = _dateTimeHelper.GetSystemTimeZones(timeZone);

            return View(settingModel);
        }

        /// <summary>
        /// es the mail templates.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / E-Mail Templates")]
        public ActionResult EMailTemplates()
        {
            EmailTemplateModel etModel = new EmailTemplateModel();
            return View();
        }

        /// <summary>
        /// es the mail template item.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = false, Label = "Edit Item")]
        [ContentManagementAntiForgery(true)]
        public ActionResult EMailTemplateItem(long Id)
        {
            EmailTemplate eTemplate = new EmailTemplate();

            if (Request["Name"] != null &&
                Request.Unvalidated["Body"] != null &&
                Request["Subject"] != null &&
                Request["SmtpAccountId"] != null &&
                Request["AttachmentId"] != null)
            {
                eTemplate = this._emailTemplateService.GetEmailTemplateById(Id);

                eTemplate.Id = Id;
                eTemplate.Name = Request["Name"];
                if (Request["Bcc"] != null)
                {
                    eTemplate.Bcc = Request["Bcc"];
                }

                eTemplate.Body = Request.Unvalidated["Body"];
                eTemplate.Subject = Request["Subject"];
                eTemplate.SmtpAccountId = long.Parse(Request["SmtpAccountId"]);
                eTemplate.AttachmentId = long.Parse(Request["AttachmentId"]);

                _emailTemplateService.UpdateEmailTemplate(eTemplate);
            }

            EmailTemplateModel etModel = new EmailTemplateModel();
            EmailTemplate et = this._emailTemplateService.GetEmailTemplateById(Id);

            etModel.Active = et.Active;
            etModel.AttachmentId = et.AttachmentId;
            etModel.Bcc = et.Bcc;
            etModel.Body = et.Body;
            etModel.Id = et.Id;
            etModel.Name = et.Name;
            etModel.SmtpAccountId = et.SmtpAccountId;
            etModel.Subject = et.Subject;

            return View(etModel);
        }

        /// <summary>
        /// Gets the e mail templates.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetEMailTemplates()
        {
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = 3;
            jd.recordsFiltered = 3;

            IList<EmailTemplate> emailTemplatesList = this._emailTemplateService.GetAllEmailTemplates();
            foreach (EmailTemplate et in emailTemplatesList)
            {
                string[] names1 = {
                                    et.Id.ToString(),
                                    et.Name,
                                    /* et.Bcc, */
                                    et.Subject,
                                    et.Body,
                                    et.Active == false ? "<span class='label label-danger'> Pasive </span>" : "<span class='label label-success'> Active </span>",
                                    permissionService.Authorize(PermissionProvider.SettingsEMailTemplatesModify) ? "<button class='btn btn-info btn-sm' type='button' onclick=\"location.href='/Management/Setting/EMailTemplateItem/"+et.Id.ToString()+"'\">Edit<i class='icon-play3 position-right'></i></button>" : ""
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SMTPs the specified setting model.
        /// </summary>
        /// <param name="settingModel">The setting model.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Settings / SMTP Settings")]
        [HttpPost]
        [PublicAntiForgery]
        [ValidateInput(false)]
        public ActionResult Smtp(SettingModel settingModel)
        {
            SmtpAccount smtpAccountNew = new SmtpAccount();
            smtpAccountNew.Id = 1;
            smtpAccountNew.Email = settingModel.SmtpEmail;
            smtpAccountNew.DisplayName = settingModel.SmtpDisplayName;
            smtpAccountNew.Host = settingModel.SmtpHost;
            smtpAccountNew.Port = settingModel.SmtpPort;
            smtpAccountNew.Username = settingModel.SmtpUsername;
            smtpAccountNew.Password = settingModel.SmtpPassword;

            smtpAccountNew.EnableSsl = (Request["EnableSsl"] == "on" ? true : false);
            smtpAccountNew.UseDefaultCredentials = (Request["UseDefaultCredentials"] == "on" ? true : false);


            this._smtpAccountService.UpdateSmtpAccount(smtpAccountNew);

            return View(settingModel);
        }

        /// <summary>
        /// SMTPs this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Smtp()
        {
            IList<SmtpAccount> SmtpAccountList = this._smtpAccountService.GetAllSmtpAccounts();

            SettingModel settingModel = new SettingModel();

            settingModel.SmtpDisplayName = SmtpAccountList[0].DisplayName;
            settingModel.SmtpEmail = SmtpAccountList[0].Email;
            settingModel.SmtpHost = SmtpAccountList[0].Host;
            settingModel.SmtpPort = SmtpAccountList[0].Port;
            settingModel.SmtpUsername = SmtpAccountList[0].Username;
            settingModel.SmtpPassword = SmtpAccountList[0].Password;
            settingModel.SmtpEnableSsl = SmtpAccountList[0].EnableSsl;
            settingModel.SmtpUseDefaultCredentials = SmtpAccountList[0].UseDefaultCredentials;

            return View(settingModel);
        }

        /// <summary>
        /// Gets the mobile version.
        /// </summary>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="device">The device.</param>
        /// <returns>String.</returns>
        public String GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }

        /// <summary>
        /// Browsers the test.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult BrowserTest()
        {
            HttpRequestBase request = Request;
            var ua = request.UserAgent;
            string retStr = "";

            if (ua.Contains("Android"))
                retStr = string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                retStr = string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                retStr = string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                retStr = "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                retStr = "Black Berry";

            if (ua.Contains("Windows Phone"))
                retStr = string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                retStr = "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                retStr = "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                retStr = "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                retStr = "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                retStr = "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                retStr = "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                retStr = "Windows 10";

            //fallback to basic platform:

            var browser = Request.Browser;
            var platform = retStr;

            retStr = request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");

            ViewBag.Ip = Request.UserHostAddress;
            ViewBag.Browser = browser.Browser + " " + browser.Version;
            ViewBag.OS = platform;
            ViewBag.Device = Request.Browser.IsMobileDevice ? "Mobile " + Request.Browser.MobileDeviceModel : "Desktop PC";

            ViewBag.UA = request.UserAgent;

            if (Request["no"] != null)
            {
                string whatDetected = "Browser: " + ViewBag.Browser + " OS:" + ViewBag.OS + " Device: " + ViewBag.Device;
                this._historyService.AddHistory("BrowserTest", HistoryAction.Unknown, "BrowserTest", 0, whatDetected, request.UserAgent, "", this._appContext.AppUser.Id);
            }

            return View();
        }
    }
}