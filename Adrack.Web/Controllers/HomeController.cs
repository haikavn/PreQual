// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="HomeController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Accounting;
using Adrack.Core.Domain.Click;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Message;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Managers;
using Adrack.Service;
using Adrack.Service.Accounting;
using Adrack.Service.Audit;
using Adrack.Service.Click;
using Adrack.Service.Configuration;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Message;
using Adrack.Web.Framework.Security;
using Adrack.Web.Helpers;
using Adrack.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using RequestContext = Adrack.Managers.RequestContext;

namespace Adrack.Web.Controllers
{
    /// <summary>
    ///     Represents a Home Controller
    ///     Implements the <see cref="BasePublicController" />
    /// </summary>
    /// <seealso cref="BasePublicController" />
    public class HomeController : BasePublicController
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        /// <param name="appContext">The application context.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        /// <param name="affiliateResponseService">The affiliate response service.</param>
        /// <param name="buyerResponseService">The buyer response service.</param>
        /// <param name="affiliateChannelTemplateService">The affiliate channel template service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="buyerChannelTemplateService">The buyer channel template service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="leadMainService">The lead main service.</param>
        /// <param name="leadContentService">The lead content service.</param>
        /// <param name="leadFieldsContentService">The lead fields content service.</param>
        /// <param name="leadMainResponseService">The lead main response service.</param>
        /// <param name="leadContentDublicateService">The lead content dublicate service.</param>
        /// <param name="buyerChannelScheduleService">The lead schedule service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="processingLogService">The processing log service.</param>
        /// <param name="buyerChannelFilterConditionService">The buyer channel filter condition service.</param>
        /// <param name="affiliateChannelFilterConditionService">The affiliate channel filter condition service.</param>
        /// <param name="blackListService">The black list service.</param>
        /// <param name="redirectUrlService">The redirect URL service.</param>
        /// <param name="zipCodeRedirectService">The zip code redirect service.</param>
        /// <param name="postedDataService">The posted data service.</param>
        /// <param name="accountingService">The accounting service.</param>
        /// <param name="buyerChannelTemplateMatchingService">The buyer channel template matching service.</param>
        /// <param name="bannerService">The banner service.</param>
        /// <param name="leadBannerService">The lead banner service.</param>
        /// <param name="callCenterSettingService">The call center setting service.</param>
        /// <param name="userSubscribtionService">The user subscribtion service.</param>
        /// <param name="leadSensitiveDataService">The lead sensitive data service.</param>
        /// <param name="reportService">The report service.</param>
        /// <param name="emailService">The email service.</param>
        public HomeController(IAppContext appContext,
            IAffiliateService affiliateService,
            IAffiliateChannelService affiliateChannelService,
            IAffiliateResponseService affiliateResponseService,
            IBuyerResponseService buyerResponseService,
            IAffiliateChannelTemplateService affiliateChannelTemplateService,
            IBuyerService buyerService,
            IBuyerChannelService buyerChannelService,
            IBuyerChannelTemplateService buyerChannelTemplateService,
            ICampaignService campaignService,
            ICampaignTemplateService campaignTemplateService,
            ILeadMainService leadMainService,
            ILeadContentService leadContentService,
            ILeadFieldsContentService leadFieldsContentService,
            ILeadMainResponseService leadMainResponseService,
            ILeadContentDublicateService leadContentDublicateService,
            IBuyerChannelScheduleService buyerChannelScheduleService,
            ISettingService settingService,
            IProcessingLogService processingLogService,
            IBuyerChannelFilterConditionService buyerChannelFilterConditionService,
            IAffiliateChannelFilterConditionService affiliateChannelFilterConditionService,
            IBlackListService blackListService,
            IRedirectUrlService redirectUrlService,
            IZipCodeRedirectService zipCodeRedirectService,
            IPostedDataService postedDataService,
            IAccountingService accountingService,
            IBuyerChannelTemplateMatchingService buyerChannelTemplateMatchingService,
            IUserSubscribtionService userSubscribtionService,
            ILeadSensitiveDataService leadSensitiveDataService,
            IReportService reportService,
            IEmailService emailService,
            IDoNotPresentService doNotPresentService,
            ISubIdWhiteListService subIdWhiteListService,
            ICachedUrlService cachedUrlService,
            IClickService clickService
        )
        {
            _affiliateService = affiliateService;
            _affiliateChannelService = affiliateChannelService;
            _affiliateResponseService = affiliateResponseService;
            _buyerResponseService = buyerResponseService;
            _affiliateChannelTemplateService = affiliateChannelTemplateService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _buyerChannelTemplateService = buyerChannelTemplateService;
            _buyerChannelTemplateMatchingService = buyerChannelTemplateMatchingService;
            _campaignService = campaignService;
            _campaignTemplateService = campaignTemplateService;
            _leadContentDublicateService = leadContentDublicateService;
            _leadContentService = leadContentService;
            _leadFieldsContentService = leadFieldsContentService;
            _leadMainResponseService = leadMainResponseService;
            _leadMainService = leadMainService;
            _blackListService = blackListService;
            _redirectUrlService = redirectUrlService;
            _buyerChannelScheduleService = buyerChannelScheduleService;
            _settingService = settingService;
            _processingLogService = processingLogService;
            _buyerChannelFilterConditionService = buyerChannelFilterConditionService;
            _affiliateChannelFilterConditionService = affiliateChannelFilterConditionService;
            _zipCodeRedirectService = zipCodeRedirectService;
            _postedDataService = postedDataService;
            _accountingService = accountingService;

            _userSubscribtionService = userSubscribtionService;
            _leadSensitiveDataService = leadSensitiveDataService;
            _appContext = appContext;
            _reportService = reportService;
            _emailService = emailService;
            _doNotPresentService = doNotPresentService;
            _subIdWhiteListService = subIdWhiteListService;
            _cachedUrlService = cachedUrlService;
            _clickService = clickService;
        }

        #endregion Constructor

        #region Fields

        /// <summary>
        ///     The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        ///     The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        ///     The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        ///     The affiliate response service
        /// </summary>
        private readonly IAffiliateResponseService _affiliateResponseService;

        /// <summary>
        ///     The buyer response service
        /// </summary>
        private readonly IBuyerResponseService _buyerResponseService;

        /// <summary>
        ///     The affiliate channel template service
        /// </summary>
        private readonly IAffiliateChannelTemplateService _affiliateChannelTemplateService;

        /// <summary>
        ///     The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        ///     The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        ///     The buyer channel template service
        /// </summary>
        private readonly IBuyerChannelTemplateService _buyerChannelTemplateService;

        /// <summary>
        ///     The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        ///     The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        ///     The lead main service
        /// </summary>
        private readonly ILeadMainService _leadMainService;

        /// <summary>
        ///     The lead content service
        /// </summary>
        private readonly ILeadContentService _leadContentService;

        /// <summary>
        ///     The lead fields content service
        /// </summary>
        private readonly ILeadFieldsContentService _leadFieldsContentService;

        /// <summary>
        ///     The lead main response service
        /// </summary>
        private readonly ILeadMainResponseService _leadMainResponseService;

        /// <summary>
        ///     The lead content dublicate service
        /// </summary>
        private readonly ILeadContentDublicateService _leadContentDublicateService;

        /// <summary>
        ///     The lead schedule service
        /// </summary>
        private readonly IBuyerChannelScheduleService _buyerChannelScheduleService;

        /// <summary>
        ///     The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        ///     The processing log service
        /// </summary>
        private readonly IProcessingLogService _processingLogService;

        /// <summary>
        ///     The buyer channel filter condition service
        /// </summary>
        private readonly IBuyerChannelFilterConditionService _buyerChannelFilterConditionService;

        /// <summary>
        ///     The affiliate channel filter condition service
        /// </summary>
        private readonly IAffiliateChannelFilterConditionService _affiliateChannelFilterConditionService;

        /// <summary>
        ///     The buyer channel template matching service
        /// </summary>
        private readonly IBuyerChannelTemplateMatchingService _buyerChannelTemplateMatchingService;

        /// <summary>
        ///     The black list service
        /// </summary>
        private readonly IBlackListService _blackListService;

        /// <summary>
        ///     The redirect URL service
        /// </summary>
        private readonly IRedirectUrlService _redirectUrlService;

        /// <summary>
        ///     The zip code redirect service
        /// </summary>
        private readonly IZipCodeRedirectService _zipCodeRedirectService;

        /// <summary>
        ///     The posted data service
        /// </summary>
        private readonly IPostedDataService _postedDataService;

        /// <summary>
        ///     The accounting service
        /// </summary>
        private readonly IAccountingService _accountingService;

        /// <summary>
        ///     The user subscribtion service
        /// </summary>
        private readonly IUserSubscribtionService _userSubscribtionService;

        /// <summary>
        ///     The lead sensitive data service
        /// </summary>
        private readonly ILeadSensitiveDataService _leadSensitiveDataService;

        /// <summary>
        ///     The report service
        /// </summary>
        private readonly IReportService _reportService;

        /// <summary>
        ///     The email service
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        ///     The do not present service
        /// </summary>
        private readonly IDoNotPresentService _doNotPresentService;

        private readonly ISubIdWhiteListService _subIdWhiteListService;

        private readonly ICachedUrlService _cachedUrlService;

        private readonly IClickService _clickService;


        /// <summary>
        ///     The click data
        /// </summary>
        public string ClickData = "";

        /// <summary>
        ///     The click state
        /// </summary>
        public short ClickState = 0;

        /// <summary>
        ///     The context
        /// </summary>
        public RequestContext context = new RequestContext();

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        [Authorize]
        public ActionResult Index()
        {
            /*_emailService.SendTimeoutNotification("TestUser", "TestBuyer", "TestBuyerChannel", "", "margarita.m@dot818.com");
            _emailService.SendCapReachNotification("TestUser", "TestBuyer", "TestBuyerChannel", "margarita.m@dot818.com");*/

            /*bool isHoliday = false;
            isHoliday = Utils.IsFederalHoliday(new DateTime(2019, 1, 21));
            isHoliday = Utils.IsFederalHoliday(new DateTime(2019, 2, 18));
            isHoliday = Utils.IsFederalHoliday(new DateTime(2019, 7, 4));
            isHoliday = Utils.IsFederalHoliday(new DateTime(2020, 10, 12));
            isHoliday = Utils.IsHoliday(new DateTime(2020, 10, 8));
            isHoliday = Utils.IsFederalHoliday(new DateTime(2019, 11, 11));
            isHoliday = Utils.IsFederalHoliday(new DateTime(2019, 12, 25));
            isHoliday = Utils.IsHoliday(new DateTime(2019, 10, 8));*/

            return Redirect("/management");
        }

        /// <summary>
        ///     Class TestData.
        /// </summary>
        public class TestData
        {
            /// <summary>
            ///     The first name
            /// </summary>
            public string FirstName;

            /// <summary>
            ///     The identifier
            /// </summary>
            public int ID;

            /// <summary>
            ///     The last name
            /// </summary>
            public string LastName;
        }

        /// <summary>
        ///     The data string
        /// </summary>
        private List<string> dataStr = new List<string>();

        /// <summary>
        ///     Gets the test data.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetTestData()
        {
            var dt = new TestData
            {
                ID = 1,
                FirstName = "Arman",
                LastName = "Zakaryan"
            };

            var jd = new JsonData
            {
                draw = 1,
                recordsTotal = 3,
                recordsFiltered = 3
            };
            string[] names1 = { "Arman", "Zakaryan", "<span class=\"label label-default\">Developer</span>" };
            jd.data.Add(names1);

            string[] names2 = { "Grigor", "Barsegyan", "<span class=\"label label-danger\">Director</span>" };
            jd.data.Add(names2);

            string[] names3 = { "Hayk", "Ayvazyan", "<span class=\"label label-success\">Manager</span>" };
            jd.data.Add(names3);

            var dtList = new List<TestData>
            {
                /*
                {
                   "draw": 3,
                   "recordsTotal": 57,
                   "recordsFiltered": 57,
                   "data": [
                       [
                           "Airi",
                           "Satou",
                           "Accountant",
                           "Tokyo",
                           "28th Nov 08",
                           "$162,700"
                       ]
                   ]
               }
               */

                dt
            };

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        // UserAgent Parser Functions //////////////////////////////////////

        /// <summary>
        ///     Gets the user platform.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>String.</returns>
        public string GetUserPlatform(HttpRequestBase request)
        {
            var ua = request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || ua.Contains("BB") && ua.Contains("Mobile"))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        /// <summary>
        ///     Gets the mobile version.
        /// </summary>
        /// <param name="userAgent">The user agent.</param>
        /// <param name="device">The device.</param>
        /// <returns>String.</returns>
        public string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test;
                if (int.TryParse(character.ToString(), out test))
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

        ///////////////////////////////////

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns>Action Result Item</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult Test()
        {
            return Content("Test");
        }

        /// <summary>
        ///     Currents the leads.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult CurrentLeads()
        {
            return Content("current=" + GlobalDataManager.CurrentLeadCount + ";waiting=" +
                           GlobalDataManager.WaitingLeads);
        }

        /// <summary>
        ///     Listen
        /// </summary>
        /// <returns>Action Result Item</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult Listen()
        {
            return View();
        }

        /*/// <summary>
        ///     Imports the c.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult ImportC()
        {
            var m = new RequestManager();

            context.BuyerService = _buyerService;
            context.AffiliateService = _affiliateService;
            context.AffiliateChannelService = _affiliateChannelService;
            context.AffiliateResponseService = _affiliateResponseService;
            context.BuyerResponseService = _buyerResponseService;
            context.AffiliateChannelTemplateService = _affiliateChannelTemplateService;
            context.BuyerChannelService = _buyerChannelService;
            context.BuyerChannelTemplateService = _buyerChannelTemplateService;
            context.CampaignService = _campaignService;
            context.CampaignTemplateService = _campaignTemplateService;
            context.LeadContentDublicateService = _leadContentDublicateService;
            context.LeadContentService = _leadContentService;
            context.LeadFieldsContentService = _leadFieldsContentService;
            context.LeadMainResponseService = _leadMainResponseService;
            context.LeadMainService = _leadMainService;
            context.LeadScheduleService = _leadScheduleService;
            context.SettingService = _settingService;
            context.BlackListService = _blackListService;
            context.RedirectUrlService = _redirectUrlService;
            context.ProcessingLogService = _processingLogService;
            context.PostedDataService = _postedDataService;
            context.AccountingService = _accountingService;
            context.BuyerChannelTemplateMatchingService = _buyerChannelTemplateMatchingService;

            context.AffiliateChannelFilterConditionService = _affiliateChannelFilterConditionService;
            context.BuyerChannelFilterConditionService = _buyerChannelFilterConditionService;

            context.LeadSensitiveDataService = _leadSensitiveDataService;

            try
            {
                MemoryCacheManager.EnableRemoteCacheCleaner = false;
                m.ProcessData(Request, Response, context, false);
                MemoryCacheManager.EnableRemoteCacheCleaner = true;
                return Content(m.Response.Response, "text/xml");
            }
            catch (Exception ex)
            {
                MemoryCacheManager.EnableRemoteCacheCleaner = true;
                var e = ex.InnerException;
                var s = "";

                while (e != null)
                {
                    s += e.Message;
                    e = e.InnerException;
                }

                return Content(string.IsNullOrEmpty(s) ? ex.Message : s);
            }
            finally
            {
                MemoryCacheManager.EnableRemoteCacheCleaner = true;
            }
        }*/

        /// <summary>
        ///     Imports this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        /// <exception cref="Exception"></exception>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult Import()
        {
            //return Content("XML");

            var m = new RequestManager();
            try
            {
                //return Content("XML");

                //Interlocked.Increment(ref GlobalDataManager.CurrentLeadCount);
                context.BuyerService = _buyerService;
                context.AffiliateService = _affiliateService;
                context.AffiliateChannelService = _affiliateChannelService;
                context.AffiliateResponseService = _affiliateResponseService;
                context.BuyerResponseService = _buyerResponseService;
                context.AffiliateChannelTemplateService = _affiliateChannelTemplateService;
                context.BuyerChannelService = _buyerChannelService;
                context.BuyerChannelTemplateService = _buyerChannelTemplateService;
                context.CampaignService = _campaignService;
                context.CampaignTemplateService = _campaignTemplateService;
                context.LeadContentDublicateService = _leadContentDublicateService;
                context.LeadContentService = _leadContentService;
                context.LeadFieldsContentService = _leadFieldsContentService;
                context.LeadMainResponseService = _leadMainResponseService;
                context.LeadMainService = _leadMainService;
                context.BuyerChannelScheduleService = _buyerChannelScheduleService;
                context.SettingService = _settingService;
                context.BlackListService = _blackListService;
                context.RedirectUrlService = _redirectUrlService;
                context.ProcessingLogService = _processingLogService;
                context.ZipCodeRedirectService = _zipCodeRedirectService;
                context.PostedDataService = _postedDataService;
                context.AccountingService = _accountingService;
                context.BuyerChannelTemplateMatchingService = _buyerChannelTemplateMatchingService;
                context.AffiliateChannelFilterConditionService = _affiliateChannelFilterConditionService;
                context.BuyerChannelFilterConditionService = _buyerChannelFilterConditionService;
                context.LeadSensitiveDataService = _leadSensitiveDataService;
                context.EmailService = _emailService;
                context.DoNotPresentService = _doNotPresentService;
                context.SubIdWhiteListService = _subIdWhiteListService;
                context.CachedUrlService = _cachedUrlService;

                if (ClickData.Length > 0) m.Import.CustomData = ClickData;

                context.ClickState = ClickState;

                MemoryCacheManager.EnableRemoteCacheCleaner = false;
                m.ProcessData(Request, Response, context, true);

                // Interlocked.Decrement(ref GlobalDataManager.CurrentLeadCount);
                GlobalDataManager.CurrentLeadCount--;
                MemoryCacheManager.EnableRemoteCacheCleaner = true;

                
                try
                {
                    //try update one more time, in case DB exception throws
                    this._affiliateResponseService.UpdateDatabase(); //this function will clear invalid entities in case error
                }
                catch
                {
                    this._affiliateResponseService.UpdateDatabase(); //try one more time
                    //we do not handle this case
                }

                return Content(m.Response.Response, "text/xml");
            }
            catch (Exception ex)
            {
            

                MemoryCacheManager.EnableRemoteCacheCleaner = true;
                GlobalDataManager.CurrentLeadCount--;

                

                m.GenerateErrorAffiliateResponse(context,ex.Message);

                try
                {
                    //try update one more time, in case DB exception throws
                    this._affiliateResponseService.UpdateDatabase();
                }
                catch
                {
                    //we do not handle this case
                }

                return Content(m.Response.Response, "text/xml");

                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [AllowCrossSiteJson]
        [AppHttpsRequirement(SslRequirement.No)] 
        public ActionResult Click()
        {
            bool result = false;

            try
            {
                string key = Request["key"];
                string clickTypeStr = Request["type"];

                clickTypeStr = char.ToUpper(clickTypeStr[0]) + clickTypeStr.ToLower().Substring(1);

                ClickTypes clickType = (ClickTypes)Enum.Parse(typeof(ClickTypes), clickTypeStr);

                ClickChannel clickChannel = _clickService.GetClickChannelByAccessKey(key);

                if (clickChannel != null)
                {
                    ClickMain clickMain = new ClickMain();
                    clickMain.CreatedAt = DateTime.UtcNow;
                    clickChannel.AccessKey = key;
                    clickMain.ClickType = clickType;
                    clickMain.IpAddress = Request.ServerVariables["REMOTE_ADDR"];
                    clickMain.ClickChannelId = clickChannel.Id;
                    clickMain.ClickPrice = clickChannel.ClickPrice;
                    _clickService.InsertClickMain(clickMain);
                    result = true;
                }
            }
            catch
            {

            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Sets the fake context.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="data">The data.</param>
        public void SetFakeContext(Controller controller, string queryString = "", string data = "")
        {
            var request = new HttpRequest("test", "http://localhost", queryString);
            var stream = new MemoryStream();
            var csvWriter = new StreamWriter(stream, Encoding.UTF8);
            csvWriter.Write(data);
            var responce = new HttpResponse(csvWriter);
            var context = new HttpContext(request, responce);
            System.Web.HttpContext.Current = context;

            var wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);

            var contextController =
                new ControllerContext(
                    new System.Web.Routing.RequestContext(wrapper,
                        new RouteData()), controller);

            controller.ControllerContext = contextController;
        }

        /// <summary>
        ///     Cleans this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Clean()
        {
            return Content("");
        }

        /// <summary>
        ///     Tests the import.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult TestImport(long id)
        {
            var channelid = Request["channelid"];
            var password = Request["password"];

            long bChannelId;
            long.TryParse(Request["BuyerChannelId"], out bChannelId);

            var bChannel = _buyerChannelService.GetBuyerChannelById(bChannelId);

            var aChannel = _affiliateChannelService.GetAffiliateChannelById(id);

            if (aChannel == null)
            {
                var aChannels =
                    (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannelsByCampaignId(
                        bChannel.CampaignId);
                if (aChannels.Count > 0) aChannel = aChannels[0];
            }

            if (aChannel == null) return Json(new { response = "" });

            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(Request.Unvalidated["xml"]);

            /*CampaignTemplate ctpl = _campaignTemplateService.GetCampaignTemplateByValidator(aChannel.CampaignId, 13);
            string AffiliateXmlField = ctpl != null ? ctpl.TemplateField : "";

            if (string.IsNullOrEmpty(AffiliateXmlField))
            {
                Setting st = _settingService.GetSetting("System.AffiliateXmlField");
                if (st != null) AffiliateXmlField = st.Value;
            }

            XmlNodeList channelIdnodes = xmldoc.GetElementsByTagName(AffiliateXmlField);
            XmlNodeList passwordNodes = xmldoc.GetElementsByTagName("PASSWORD");

            if (channelIdnodes.Count == 0 || passwordNodes.Count == 0)
            {
                return Json(new { response = "" });
            }
            else
            {
                channelIdnodes[0].InnerText = channelid;
                passwordNodes[0].InnerText = password;
            }  */

            var response =
                Adrack.Helpers.PostXml(
                    Adrack.Helpers.GetBaseUrl(Request) + "/home/import" + "?mode=test&buyerChannelId=" + bChannelId,
                    xmldoc.OuterXml, 30000, "application/xml", new Dictionary<string, string>(), "");

            return Json(new { response });
        }

        /// <summary>
        ///     Navigates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult Navigate(string id)
        {
            var ru = _redirectUrlService.GetRedirectUrlByKey(id.ToLower());

            if (ru != null)
            {
                var clickDate = DateTime.UtcNow;

                if ((clickDate - ru.Created).TotalSeconds >= 180 && Request["hihi"] != "haha") return RedirectToAction("LinkExpired");

                var browser = Request.Browser;
                var platform = GetUserPlatform(Request);
                ru.Clicked = true;
                ru.ClickDate = clickDate;
                ru.Ip = Request.UserHostAddress;

                ru.Browser = browser.Browser + " " + browser.Version;

                ru.OS = platform;

                ru.Device = Request.Browser.IsMobileDevice
                    ? "Mobile " + Request.Browser.MobileDeviceModel
                    : "Desktop PC";

                _redirectUrlService.UpdateRedirectUrl(ru);

                if (ru.Url != "#" && !string.IsNullOrEmpty(ru.Url)) return Redirect(ru.Url);

                var address = ru.Address.Split(new string[1] { "#@#" }, StringSplitOptions.None);

                var model = new BuyerViewModel
                {
                    Address = address[0]
                };
                if (address.Length > 1)
                    model.Address2 = address[1];
                else
                    model.Address2 = "";
                model.Description = ru.Description;
                model.Title = ru.Title;
                model.ZipCode = ru.ZipCode;
                return PartialView("BuyerView", model);
            }

            return RedirectToAction("LinkExpired");
        }

        /// <summary>
        ///     Tests the response.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult TestResp()
        {
            int d;
            if (int.TryParse(Request["d"], out d)) Thread.Sleep(d * 1000);

            string[] responses =
            {
                "<Response><status>sold</status><message>sold</message><price>30</price><redirect></redirect></Response>",
                "<Response><status>reject</status><price>0</price><redirect></redirect></Response>",
                "<Response><status>error</status><price>0</price><redirect></redirect></Response>",
                "<Response><status>test</status><price>0</price><redirect></redirect></Response>"
            };

            var rnd = new Random();

            var n = rnd.Next(0, 100);
            var i = 0;

            if (n <= 20)
                i = 0;
            else if (n > 20 && n <= 25)
                i = 2;
            else if (n > 25)
                i = 1;

            return Content(responses[i], "text/xml");
        }

        public ActionResult TestRespCustom()
        {
            return Content("PRICE_REJECT,15.00,http://localhost:7457/home/testresp");
        }

        public ActionResult TestRespCustom2()
        {
            return Content("<?xml version=\"1.0\" encoding=\"UTF-8\"?><lead_response>   <lead_accepted>false</lead_accepted>   <response>      <response_id>102</response_id>      <response_message>Application was rejected, willing to purchase at different price point.</response_message><bid_amount>40.00</bid_amount><lead_id>8013727</lead_id></response></lead_response>", "text/xml");
        }

        public ActionResult TestRespAccountId()
        {
            string[] responses =
            {
                "<Response><status>sold</status><message>sold</message><price>100</price><redirect></redirect><accountid>1234567</accountid></Response>",
            };

            return Content(responses[0], "text/xml");
        }

        /// <summary>
        ///     Tests the json resp.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult TestJsonResp()
        {
            string[] responses =
            {
                "{\"messageType\": 0,\"timestamp\": \"2016-11-22T11:55:23.282Z\",\"leadOfferId\": \"20160912-21EC2020-3AEA-4069-A2DD-08002B30309D\",\"offerStatus\": 1,\"lenderContactInfo\": {\"lenderName\": \"OppLoans\",\"lenderEmail\": \"support@opploans.com\",\"lenderPhone\": \"312.212.8079\",\"lenderFax\": \"855.920.5000\"},\"personalizedUrl\": \"https://www.opploans.com/ptr/59/?lead_id=e182\",\"offers\": [{\"interestRateType\": 1,\"loanAmount\": 2000,\"interestRate\": 98.72,\"term\": 10,\"monthlyPayment\": 301}]}",
                "{\"messageType\": 1,\"timestamp\": \"2016-09-19T10:31:02.364Z\",\"leadOfferId\": \"20160912-21EC2020-3AEA-4069-A2DD-08002B30309D\",\"offerStatus\": 2,\"lenderContactInfo\": {\"lenderName\": \"OppLoans\",\"lenderEmail\": \"bizops@opploans.com\",\"lenderPhone\": \"855.408.5000\",\"lenderFax\": \"855.920.5000\"}}",
                "{\"messageType\": 2,\"leadOfferId\": \"20160912-21EC2020-3AEA-4069-A2DD-08002B30309D\",\"errorMsg\": \"partnerId not valid, please contact support.\",\"timestamp\": \"2016-11-22T12:53:00.776Z\"}"
            };

            var rnd = new Random();
            var n = rnd.Next(0, 2);

            return Content(responses[n], "application/json");
        }

        /// <summary>
        ///     Links the expired.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult LinkExpired()
        {
            return Content("Link expired");
        }

        /// <summary>
        ///     Buyers the view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AllowAnonymous]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult BuyerView()
        {
            var model = new BuyerViewModel
            {
                Address = Request["address1"], //"212 N Progress Dr, Ste 30, Perryville, MO 63775-1209";
                Address2 = Request["address2"],
                Description =
                    "<b>Demo text</b> Demo text<br> Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text Demo text ",
                Title = "Demo Bank",
                ZipCode = "48326"
            };
            return PartialView("BuyerView", model);
        }

        /// <summary>
        ///     Unixes the time stamp to date time.
        /// </summary>
        /// <param name="unixTimeStamp">The unix time stamp.</param>
        /// <returns>DateTime.</returns>
        [NonAction]
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        /// <summary>
        /// Clear Cache Manager
        /// </summary>
        /// <param name="returnUrl">Return Url</param>
        /// <returns>Action Result</returns>
        public ActionResult ClearCacheManager()
        {
            SharedData.ClearBuyerChannelLeadsCount();
            ClearCachedUrls();
            var cacheManager = AppEngineContext.Current.ContainerManager.Resolve<ICacheManager>("Application.Cache.Manager_Static");

            cacheManager.Clear();

            if (!string.IsNullOrEmpty("redirect") && Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.LocalPath);

            return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearCachedUrls()
        {
            var cachedUrlService = AppEngineContext.Current.Resolve<ICachedUrlService>();

            cachedUrlService.DeleteCheckCachedUrls();

            return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
        }

        static int indexFake = 0;
        public string FakeTest()
        {
            
            System.IO.File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory+"\\log.txt",Request.QueryString.ToString());
            //return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            string[] values =new string[3];

            values[0] = "REJECTED,failed";
            values[1] = "PRICE_REJECT,2.00,https://pr_redirecturl.com?leadid=12456";
            values[2] = "ACCEPTED,3.0,https://redirecturl.com?leadid=12345&test=2";
            indexFake++;
            if (indexFake > 3)
                indexFake = 1;
            return values[indexFake-1];
        }

        /// <summary>
        ///     Fills the main report.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult FillMainReport()
        {
            var cache = false;
            var emails = false;

            var t = Task.Run(() =>
            {
                MemoryCacheManager.EnableRemoteCacheCleaner = false;
                try
                {
                    GlobalDataManager.LockFillReport = true;

                    if (Request["password"] != "d7DVEkPMRWrnYbp227PawkH")
                        return Json(new { cache, emails }, JsonRequestBehavior.AllowGet);

                    _reportService.FillMainReport();

                    GlobalDataManager.LockFillReport = false;

                    cache = true;

                    var emailTask = new EmailQueueTask(AppEngineContext.Current.Resolve<EmailQueueSetting>(),
                        AppEngineContext.Current.Resolve<ILogService>(),
                        AppEngineContext.Current.Resolve<IEmailService>(),
                        AppEngineContext.Current.Resolve<IEmailQueueService>());
                    try
                    {
                        emailTask.Execute();
                        emails = true;
                    }
                    catch
                    {
                    }
                }
                finally
                {
                    MemoryCacheManager.EnableRemoteCacheCleaner = true;
                }
                return Json(new { cache, emails }, JsonRequestBehavior.AllowGet);
            });
            t.Wait();
            return Json(new { cache, emails }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Fills the main report.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult AddDoNotPresent()
        {
            if (Request["password"] != "d7DVEkPMRWrnYbp227PawkH")
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);

            long buyerId = 0;
            long.TryParse(Request["buyerid"], out buyerId);

            if (string.IsNullOrEmpty(Request["email"]) ||
                string.IsNullOrEmpty(Request["ssn"]) ||
                _buyerService.GetBuyerById(buyerId) == null
                ) return Json(new { result = false }, JsonRequestBehavior.AllowGet);

            DoNotPresent doNotPresent = new DoNotPresent();
            doNotPresent.BuyerId = buyerId;

            doNotPresent.Ssn = Request["ssn"];
            doNotPresent.Email = Request["email"];

            DateTime expirationDate = DateTime.UtcNow;
            try
            {
                doNotPresent.ExpirationDate = DateTime.ParseExact(Request["expiration"], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                doNotPresent.ExpirationDate = null;
            }

            doNotPresent.Phone = "";
            _doNotPresentService.InsertDoNotPresent(doNotPresent);

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckDoNotPresent()
        {
            long buyerId = 0;
            long.TryParse(Request["buyerid"], out buyerId);

            if (string.IsNullOrEmpty(Request["email"]) ||
                string.IsNullOrEmpty(Request["ssn"]) || buyerId == 0) return Json(new { result = false }, JsonRequestBehavior.AllowGet);

            if (_doNotPresentService.CheckDoNotPresent(Request["email"], Request["ssn"], buyerId) == 1)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Posts the XML.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="postingHeaders">The posting headers.</param>
        /// <param name="method">The method.</param>
        /// <returns>System.String.</returns>
        public string PostXml(string url, string xml, int timeout, string contentType,
            Dictionary<string, string> postingHeaders, string method = "POST")
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(xml);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol =
                    //SecurityProtocolType.Tls
                    //| SecurityProtocolType.Tls11
                    SecurityProtocolType.Tls12;
                //| SecurityProtocolType.Ssl3;

                //ServicePointManager.DefaultConnectionLimit = 500;

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Timeout = timeout == 0 ? 30000 : timeout;
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = data.Length;

                foreach (var key in postingHeaders.Keys) request.Headers.Add(key, postingHeaders[key]);

                //string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("Opploans:LeadsHello"));
                //request.Headers.Add("Authorization", "Basic " + svcCredentials);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GenerateString(int len)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[len];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

            var finalString = new string(stringChars);
            return finalString;
        }

        public bool NextBool(int truePercentage = 50)
        {
            Random r = new Random();
            return r.Next(100) < truePercentage;
        }

        public ActionResult GenerateData()
        {
            string prefix = DateTime.Now.Ticks.ToString();
            string name = Request["name"];
            if (string.IsNullOrEmpty(name)) return Content("Name parameter is required.");

            bool addFilterValues = false;
            bool.TryParse(Request["addFilterValues"], out addFilterValues);

            List<string> validators = new List<string>();

            validators.Clear();
            validators.Add("NONE");
            validators.Add("String");
            validators.Add("Number");
            validators.Add("EMail");
            validators.Add("UnsignedNumber");
            validators.Add("AccountNumber");
            validators.Add("SSN");
            validators.Add("ZIP");
            validators.Add("Phone");
            validators.Add("Password");
            validators.Add("DateTime");
            validators.Add("State");
            validators.Add("RoutingNumber");
            validators.Add("AffiliateRefferalField");
            validators.Add("DateOfBirth");
            validators.Add("RegularExpression");
            validators.Add("Decimal");
            validators.Add("SubId");

            List<string> filters = new List<string>();
            filters.Add("CONTAINS");
            filters.Add("DOES NOT CONTAIN");
            filters.Add("STARTS WITH");
            filters.Add("ENDS WITH");
            filters.Add("EQUAL");
            filters.Add("NOT EQUAL");
            filters.Add("GREATER");
            filters.Add("GREATER EQUAL");
            filters.Add("LESS");
            filters.Add("LESS EQUAL");
            filters.Add("RANGE");
            filters.Add("NO SAME DIGITS");


            Random random = new Random();

            Type stringType = typeof(String);
            Type intType = typeof(int);
            Type decimalType = typeof(decimal);

            Dictionary<string, List<string>> dbFields = new Dictionary<string, List<string>>();
            List<string> addedDbFields = new List<string>();

            PropertyInfo[] properties = typeof(LeadContent).GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.Name.ToLower() == "id" ||
                    pi.Name.ToLower() == "leadid" ||
                    pi.Name.ToLower() == "affiliateid" ||
                    pi.Name.ToLower() == "campaigntype" ||
                    pi.Name.ToLower() == "minpricestr" ||
                    pi.Name.ToLower() == "created") continue;


                string[] vals = null;

                string type = "unknown";

                if (pi.PropertyType == stringType)
                {
                    vals = new string[3] { "String", "ZIP", "Phone" };
                    int randomNumber = random.Next(0, vals.Length - 1);
                    type = vals[randomNumber];
                }
                else if (pi.PropertyType == intType || pi.Name.ToLower() == "age" || pi.Name.ToLower() == "emptime" || pi.Name.ToLower() == "addressmonth")
                {
                    vals = new string[2] { "Number", "UnsignedNumber" };
                    int randomNumber = random.Next(0, vals.Length - 1);
                    type = vals[randomNumber];
                }
                else if (pi.PropertyType == intType || pi.Name.ToLower() == "minprice" || pi.Name.ToLower() == "requestedamount" || pi.Name.ToLower() == "netmonthlyincome")
                {
                    type = "Decimal";
                }
                else if (pi.Name.ToLower() == "dob")
                {
                    type = "DateOfBirth";
                }

                if (string.IsNullOrEmpty(type)) continue;

                if (!dbFields.ContainsKey(type)) dbFields[type] = new List<string>();
                dbFields[type].Add(pi.Name);
            }

            Dictionary<long, short> fieldFilters = new Dictionary<long, short>();

            string campaignXml = "<REQUEST>";

            var campaign = new Campaign
            {
                Name = name + " C-" + prefix,
                IsTemplate = false,
                CreatedOn = DateTime.UtcNow,
                CampaignType = 0,
                Start = DateTime.UtcNow,
                Finish = DateTime.UtcNow,
                Status = ActivityStatuses.Active,
                VerticalId = 1
            };
            _campaignService.InsertCampaign(campaign);

            List<CampaignField> campaignFields = new List<CampaignField>();

            var cfield = new CampaignField
            {
                CampaignId = campaign.Id,
                MinLength = 0,
                MaxLength = 150,
                Required = true,
                TemplateField = "REQUEST",
                Validator = 0,
                SectionName = "root",
                DatabaseField = "NONE",
                PossibleValue = "",
                IsFilterable = false,
                IsHash = false,
                IsHidden = false,
                ValidatorSettings = "",
                Description = ""
            };
            _campaignTemplateService.InsertCampaignTemplate(cfield);

            CampaignField channelIdField = null;
            cfield = new CampaignField
            {
                CampaignId = campaign.Id,
                MinLength = 0,
                MaxLength = 150,
                Required = true,
                TemplateField = "CHANNELID",
                Validator = (short)13,
                SectionName = "REQUEST",
                DatabaseField = "NONE",
                PossibleValue = "",
                IsFilterable = false,
                IsHash = false,
                IsHidden = false,
                ValidatorSettings = "",
                Description = ""
            };
            campaignXml += "<CHANNELID></CHANNELID>";
            _campaignTemplateService.InsertCampaignTemplate(cfield);
            channelIdField = cfield;

            CampaignField passwordField = null;
            cfield = new CampaignField
            {
                CampaignId = campaign.Id,
                MinLength = 0,
                MaxLength = 150,
                Required = true,
                TemplateField = "PASSWORD",
                Validator = (short)9,
                SectionName = "REQUEST",
                DatabaseField = "NONE",
                PossibleValue = "",
                IsFilterable = false,
                IsHash = false,
                IsHidden = false,
                ValidatorSettings = "",
                Description = ""
            };
            campaignXml += "<PASSWORD></PASSWORD>";
            _campaignTemplateService.InsertCampaignTemplate(cfield);
            passwordField = cfield;

            Dictionary<long, string> filterValues = new Dictionary<long, string>();

            bool canMapWrongDataFormat = false;
            bool.TryParse(Request["canMapWrongDataFormat"], out canMapWrongDataFormat);

            for (int i = 1; i < validators.Count; i++)
            {
                if (i > 14 || i == 9 || i == 13) continue;

                for (short f = 0; f < filters.Count; f++)
                {
                    if (i == 1 && f > 5) continue;

                    int validator = i;

                    if (canMapWrongDataFormat && NextBool(20))
                    {
                        if (i == 1)
                            validator = 2;
                        else
                        if (i == 2 || i == 16)
                            validator = 10;
                        else
                          if (i == 10)
                            validator = 2;
                    }
                    string dbFieldName = "NONE";

                    if (dbFields.ContainsKey(validators[i]))
                    {
                        var dbFieldsArray = dbFields[validators[i]];
                        int randomIndex = random.Next(0, dbFieldsArray.Count);
                        dbFieldName = dbFieldsArray[randomIndex];
                    }
                    else if (dbFields.ContainsKey("unknown"))
                    {
                        var dbFieldsArray = dbFields["unknown"];
                        int randomIndex = random.Next(0, dbFieldsArray.Count);
                        dbFieldName = dbFieldsArray[randomIndex];
                    }

                    if (addedDbFields.Contains(dbFieldName)) dbFieldName = "NONE";
                    if (dbFieldName != "NONE") addedDbFields.Add(dbFieldName);

                    cfield = new CampaignField
                    {
                        CampaignId = campaign.Id,
                        MinLength = 0,
                        MaxLength = 150,
                        Required = (random.Next(0, 1) == 1 ? true : false),
                        TemplateField = "Field_" + validators[i] + "_" + filters[f].Replace(" ", "_"),
                        Validator = (short)validator,
                        SectionName = "REQUEST",
                        DatabaseField = dbFieldName,
                        PossibleValue = "",
                        IsFilterable = true,
                        IsHash = (random.Next(0, 1) == 1 ? true : false),
                        IsHidden = false,
                        ValidatorSettings = (validator == 1 ? "1;150" : ""),
                        Description = ""
                    };

                    string value = "";

                    if (i == 4 || i == 2 || i == 16)
                    {
                        value = random.Next(1, 1000).ToString();
                    }
                    else if (i == 10 || i == 14)
                    {
                        value = DateTime.UtcNow.ToString("MM/dd/yyyy");
                    }
                    else if (i == 3)
                    {
                        value = GenerateString(5) + "@" + GenerateString(4) + ".com";
                    }
                    else if (i == 11)
                    {
                        value = "CA";
                    }
                    else if (i == 6)
                    {
                        value = "123456789";
                    }
                    else if (i == 7)
                    {
                        value = "99501";
                    }
                    else if (i == 8)
                    {
                        value = "(555) 555-1234";
                    }
                    else if (i == 12)
                    {
                        value = "091300023";
                    }
                    else
                    {
                        value = GenerateString(5);
                    }
                    campaignXml += "<" + cfield.TemplateField + ">" + value + "</" + cfield.TemplateField + ">";

                    _campaignTemplateService.InsertCampaignTemplate(cfield);

                    filterValues[cfield.Id] = value;
                    campaignFields.Add(cfield);

                    fieldFilters[cfield.Id] = (short)(f + 1);

                }
            }
            campaignXml += "</REQUEST>";

            campaign.DataTemplate = campaignXml;
            _campaignService.UpdateCampaign(campaign);

            var affiliate = new Affiliate
            {
                Name = name + " A-" + prefix,
                CreatedOn = DateTime.UtcNow,
                CountryId = 80,
                StateProvinceId = 3,
                Status = 1
            };
            _affiliateService.InsertAffiliate(affiliate);

            var affiliateChannel = new AffiliateChannel
            {
                Name = name+ " AC-" + prefix,
                AffiliateId = affiliate.Id,
                CampaignId = campaign.Id,
                ChannelKey = GenerateString(7),
                Status = 1,
                XmlTemplate = campaignXml
            };
            _affiliateChannelService.InsertAffiliateChannel(affiliateChannel);

            var afield = new AffiliateChannelTemplate
            {
                AffiliateChannelId = affiliateChannel.Id,
                CampaignTemplateId = 0,
                SectionName = "root",
                TemplateField = "REQUEST"
            };
            _affiliateChannelTemplateService.InsertAffiliateChannelTemplate(afield);

            afield = new AffiliateChannelTemplate
            {
                AffiliateChannelId = affiliateChannel.Id,
                CampaignTemplateId = channelIdField.Id,
                SectionName = "REQUEST",
                TemplateField = "CHANNELID",
                DefaultValue = ""
            };
            _affiliateChannelTemplateService.InsertAffiliateChannelTemplate(afield);

            afield = new AffiliateChannelTemplate
            {
                AffiliateChannelId = affiliateChannel.Id,
                CampaignTemplateId = passwordField.Id,
                SectionName = "REQUEST",
                TemplateField = "PASSWORD",
                DefaultValue = GenerateString(7)
            };
            _affiliateChannelTemplateService.InsertAffiliateChannelTemplate(afield);

            for (int i = 0; i < campaignFields.Count; i++)
            {
                if (fieldFilters[campaignFields[i].Id] == 11) continue;

                int index = i;

               /* if (canMapWrongDataFormat && NextBool(10) && campaignFields[i].Validator == 1)
                {
                    for (int j = 0; j < campaignFields.Count; j++)
                    {
                        if (j != i && campaignFields[j].Validator == 2)
                        {
                            index = j;
                            break;
                        }
                    }
                }*/

                afield = new AffiliateChannelTemplate
                {
                    AffiliateChannelId = affiliateChannel.Id,
                    CampaignTemplateId = campaignFields[index].Id,
                    SectionName = "REQUEST",
                    TemplateField = campaignFields[i].TemplateField,
                    DefaultValue = ""
                };
                _affiliateChannelTemplateService.InsertAffiliateChannelTemplate(afield);

                AffiliateChannelFilterCondition filter = new AffiliateChannelFilterCondition();
                filter.AffiliateChannelId = affiliateChannel.Id;
                filter.CampaignTemplateId = campaignFields[i].Id;
                filter.ParentId = 0;
                filter.Condition = fieldFilters[campaignFields[i].Id];

                string value = "";
                if (filterValues.ContainsKey(campaignFields[i].Id) && addFilterValues)
                {
                    value = filterValues[campaignFields[i].Id];

                    switch (filter.Condition)
                    {
                        case 2:
                            value = GenerateString(5);
                            break;
                        case 1:
                        case 3:
                        case 4:
                        case 5:
                            if (campaignFields[i].Validator == 14)
                            {
                                DateTime dob;
                                var res = Validator.IsValidDateTime(value, campaignFields[i].ValidatorSettings, out dob);
                                if (res)
                                {
                                    var tsage = DateTime.Now - dob;
                                    value = ((short)(tsage.TotalDays / 365)).ToString();
                                }
                            }
                            break;
                        case 6: value = value + "1"; break;
                        case 7:
                            if (campaignFields[i].Validator == 4 || campaignFields[i].Validator == 2 || campaignFields[i].Validator == 16)
                            {
                                int intVal = 0;
                                if (int.TryParse(value, out intVal))
                                {
                                    value = (intVal - 1).ToString();
                                }

                            }
                            else value = "";
                            break;
                        case 9:
                            if (campaignFields[i].Validator == 4 || campaignFields[i].Validator == 2 || campaignFields[i].Validator == 16)
                            {
                                int intVal = 0;
                                if (int.TryParse(value, out intVal))
                                {
                                    value = (intVal + 1).ToString();
                                }
                            }
                            else value = "";
                            break;
                        case 11:
                            if (campaignFields[i].Validator == 4 || campaignFields[i].Validator == 2 || campaignFields[i].Validator == 16)
                            {
                                int intVal = 0;
                                if (int.TryParse(value, out intVal))
                                {
                                    value = (intVal - 10).ToString() + "-" + (intVal + 10).ToString();
                                }
                            }
                            else value = "";
                            break;
                        default: value = ""; break;
                    }
                }


                filter.Value = value;

                _affiliateChannelFilterConditionService.InsertFilterCondition(filter);
            }

            /* html += '</select>';
             html += '</td>';
             html += '<td>';
             html += '<select class="form-control">';
             html += '<option value="1" ' + (condition == '1' ? ' selected' : '') + '>CONTAINS</option>';
             html += '<option value="2" ' + (condition == '2' ? ' selected' : '') + '>DOES NOT CONTAIN</option>';
             html += '<option value="3" ' + (condition == '3' ? ' selected' : '') + '>STARTS WITH</option>';
             html += '<option value="4" ' + (condition == '4' ? ' selected' : '') + '>ENDS WITH</option>';
             html += '<option value="5" ' + (condition == '5' ? ' selected' : '') + '>EQUAL</option>';
             html += '<option value="6" ' + (condition == '6' ? ' selected' : '') + '>NOT EQUAL</option>';
             html += '<option value="7" ' + (condition == '7' ? ' selected' : '') + '>GREATER</option>';
             html += '<option value="8" ' + (condition == '8' ? ' selected' : '') + '>GREATER EQUAL</option>';
             html += '<option value="9" ' + (condition == '9' ? ' selected' : '') + '>LESS</option>';
             html += '<option value="10" ' + (condition == '10' ? ' selected' : '') + '>LESS EQUAL</option>';
             html += '<option value="11" ' + (condition == '11' ? ' selected' : '') + '>RANGE</option>';
             html += '<option value="12" ' + (condition == '12' ? ' selected' : '') + '>NO SAME DIGITS</option>';
             html += '</select>';
             html += '</td>';*/


            var buyer = new Buyer
            {
                Name = name + " B-" + prefix,
                CreatedOn = DateTime.UtcNow,
                CountryId = 80,
                StateProvinceId = 3,
                Status = 1,
                AlwaysSoldOption = 0,
                DailyCap = 10000,
                MaxDuplicateDays = 30,
                FrequencyValue = 0,
                BillFrequency = "m",
                ManagerId = 2
            };
            _buyerService.InsertBuyer(buyer);

            var buyerBalance = new BuyerBalance();
            buyerBalance.BuyerId = buyer.Id;
            buyerBalance.Credit = 50000;
            buyerBalance.Balance = 50000;
            _accountingService.InsertBuyerBalance(buyerBalance);

            int bcCount = 1;
            if (!int.TryParse(Request["bccount"], out bcCount)) bcCount = 1;

            for (int i = 1; i <= bcCount; i++)
            {
                var buyerChannel = new BuyerChannel
                {
                    Name = name + " BC-" + i.ToString() + "-" + prefix,
                    BuyerId = buyer.Id,
                    CampaignId = campaign.Id,
                    Status = BuyerChannelStatuses.Active,
                    XmlTemplate = campaignXml
                };

                buyerChannel.AlwaysSoldOption = 0;
                buyerChannel.BuyerId = buyer.Id;
                buyerChannel.PostingUrl = "auto";
                buyerChannel.AcceptedField = "status";
                buyerChannel.AcceptedValue = "sold";
                buyerChannel.AcceptedFrom = 0;
                buyerChannel.ErrorField = "status";
                buyerChannel.ErrorValue = "error";
                buyerChannel.ErrorFrom = 0;
                buyerChannel.RejectedField = "status";
                buyerChannel.RejectedValue = "reject";
                buyerChannel.RejectedFrom = 0;
                buyerChannel.TestField = "status";
                buyerChannel.TestValue = "sold";
                buyerChannel.TestFrom = 0;
                buyerChannel.AllowedAffiliateChannels = ":" + affiliateChannel.Id + ";";
                buyerChannel.AffiliatePriceOption = 0;
                buyerChannel.AffiliatePrice = 3;
                buyerChannel.BuyerPriceOption = 0;
                buyerChannel.BuyerPrice = 10;

                _buyerChannelService.InsertBuyerChannel(buyerChannel);

                var bfield = new BuyerChannelTemplate
                {
                    BuyerChannelId = buyerChannel.Id,
                    CampaignTemplateId = 0,
                    SectionName = "root",
                    TemplateField = "REQUEST"
                };
                _buyerChannelTemplateService.InsertBuyerChannelTemplate(bfield);

                for (int j = 0; j < campaignFields.Count; j++)
                {
                    if (fieldFilters[campaignFields[j].Id] == 11) continue;

                    bfield = new BuyerChannelTemplate
                    {
                        BuyerChannelId = buyerChannel.Id,
                        CampaignTemplateId = campaignFields[j].Id,
                        SectionName = "REQUEST",
                        TemplateField = campaignFields[j].TemplateField
                    };
                    _buyerChannelTemplateService.InsertBuyerChannelTemplate(bfield);

                    BuyerChannelFilterCondition filter = new BuyerChannelFilterCondition();
                    filter.BuyerChannelId = buyerChannel.Id;
                    filter.CampaignTemplateId = campaignFields[j].Id;
                    filter.ParentId = 0;
                    filter.Condition = fieldFilters[campaignFields[j].Id];

                    string value = "";
                    if (filterValues.ContainsKey(campaignFields[j].Id) && addFilterValues)
                    {
                        value = filterValues[campaignFields[j].Id];


                        switch (filter.Condition)
                        {
                            case 2:
                                value = GenerateString(5);
                                break;
                            case 1:
                            case 3:
                            case 4:
                            case 5:
                                if (campaignFields[i].Validator == 14)
                                {
                                    DateTime dob;
                                    var res = Validator.IsValidDateTime(value, campaignFields[i].ValidatorSettings, out dob);
                                    if (res)
                                    {
                                        var tsage = DateTime.Now - dob;
                                        value = ((short)(tsage.TotalDays / 365)).ToString();
                                    }
                                }
                                break;
                            case 6: value = value + "1"; break;
                            case 7:
                                if (campaignFields[i].Validator == 4 || campaignFields[i].Validator == 2 || campaignFields[i].Validator == 16)
                                {
                                    int intVal = 0;
                                    if (int.TryParse(value, out intVal))
                                    {
                                        value = (intVal - 1).ToString();
                                    }

                                }
                                else value = "";
                                break;
                            case 9:
                                if (campaignFields[i].Validator == 4 || campaignFields[i].Validator == 2 || campaignFields[i].Validator == 16)
                                {
                                    int intVal = 0;
                                    if (int.TryParse(value, out intVal))
                                    {
                                        value = (intVal + 1).ToString();
                                    }
                                }
                                else value = "";
                                break;
                            case 11:
                                if (campaignFields[i].Validator == 4 || campaignFields[i].Validator == 2 || campaignFields[i].Validator == 16)
                                {
                                    int intVal = 0;
                                    if (int.TryParse(value, out intVal))
                                    {
                                        value = (intVal - 10).ToString() + "-" + (intVal + 10).ToString();
                                    }
                                }
                                else value = "";
                                break;
                            default: value = ""; break;
                        }
                    }

                    filter.Value = value;

                    _buyerChannelFilterConditionService.InsertFilterCondition(filter);
                }
            }

            return Content(campaign.DataTemplate, "text/xml");
        }

        /// <summary>
        /// Adds All Data from lead short form
        /// </summary>
        /// <returns>ActionResult.</returns>
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult AddShortFormData(string amount, string firstname, string zip, string email)
        {
            int amountVal = 0;
            Int32.TryParse(amount, out amountVal);
            _settingService.AddShortFormData(amountVal, firstname, zip, email);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods
    }
}