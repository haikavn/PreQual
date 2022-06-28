using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Click;
using Adrack.Core.Infrastructure;
using Adrack.Managers;
using Adrack.Service;
using Adrack.Service.Accounting;
using Adrack.Service.Click;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Message;
using Adrack.WebApi.Helpers;
using Adrack.WebApi.Models.Lead;

namespace Adrack.WebApi.Controllers
{
    //[RoutePrefix("api/processing")]
    public class ProcessingController : BaseApiPublicController
    {
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

        private readonly ICountryService _countryService;

        private readonly IClickService _clickService;

        public RequestContext context = new RequestContext();

        public ProcessingController(IAppContext appContext,
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
            IClickService clickService,
            ICountryService countryService)
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
            _countryService = countryService;
        }

        /// <summary>
        /// Home Page controller's default method
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult Index()
        {
            return Ok();
        }

        /// <summary>
        /// Clear Cache Manager
        /// </summary>
        /// <returns>Action Result</returns>
        public IHttpActionResult ClearCacheManager()
        {
            SharedData.ClearBuyerChannelLeadsCount();
            
            DeleteCheckCachedUrls();

            var cacheManager = AppEngineContext.Current.ContainerManager.Resolve<ICacheManager>("Application.Cache.Manager_Static");

            cacheManager.Clear();

            return Ok();
        }

        /// <summary>
        /// Clear Cache Urls
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult ClearCachedUrls()
        {
            DeleteCheckCachedUrls();

            return Ok();
        }

        private void DeleteCheckCachedUrls()
        {
            var cachedUrlService = AppEngineContext.Current.Resolve<ICachedUrlService>();

            cachedUrlService.DeleteCheckCachedUrls();
        }

        private string GetMobileVersion(string userAgent, string device)
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


        private string GetUserPlatform()
        {
            var request = HttpContext.Current.Request;
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

        [HttpGet]
        [HttpPost]
        [Route("navigate")]
        public IHttpActionResult Navigate([FromUri] string id)
        {
            var ru = _redirectUrlService.GetRedirectUrlByKey(id.ToLower());

            if (ru != null)
            {
                var clickDate = DateTime.UtcNow;

                if ((clickDate - ru.Created).TotalSeconds >= 180) return HttpBadRequest("LinkExpired");

                var browser = HttpContext.Current.Request.Browser;

                var platform = GetUserPlatform();
                ru.Clicked = true;
                ru.ClickDate = clickDate;
                ru.Ip = HttpContext.Current.Request.UserHostAddress;

                ru.Browser = browser.Browser + " " + browser.Version;

                ru.OS = platform;

                ru.Device = HttpContext.Current.Request.Browser.IsMobileDevice
                    ? "Mobile " + HttpContext.Current.Request.Browser.MobileDeviceModel
                    : "Desktop PC";

                _redirectUrlService.UpdateRedirectUrl(ru);

                if (ru.Url != "#" && !string.IsNullOrEmpty(ru.Url)) return Redirect(ru.Url);

                return Ok();
            }

            return HttpBadRequest("LinkExpired");
        }

        [HttpPost]
        [Route("import")]
        public IHttpActionResult Import([FromBody] string body)
        {
            DateTime now = DateTime.UtcNow;

            /*XmlDocument xmlDocument = new XmlDocument();
            now = DateTime.UtcNow;
            xmlDocument.Load("d:\\sample.xml");
            TimeSpan ts = DateTime.UtcNow - now;
            var elements = xmlDocument.GetElementsByTagName("Author");

            now = DateTime.UtcNow;
            XDocument xDocument = XDocument.Parse(System.IO.File.ReadAllText("d:\\sample.xml"));
            TimeSpan ts2 = DateTime.UtcNow - now;
            var elements2 = xDocument.Descendants("Author");

            now = DateTime.UtcNow;
            XmlReader xmlReader = XmlReader.Create("d:\\sample.xml");
            TimeSpan ts3 = DateTime.UtcNow - now;
            int count = 0;
            while (xmlReader.ReadToFollowing("Author"))
            {
                count++;
            }*/

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
                context.CountryService = _countryService;
                context.ClickService = _clickService;

                MemoryCacheManager.EnableRemoteCacheCleaner = false;
                context.PostedDataBody = body;
                m.ProcessData(Request, context, true);

                // Interlocked.Decrement(ref GlobalDataManager.CurrentLeadCount);
                GlobalDataManager.CurrentLeadCount--;
                MemoryCacheManager.EnableRemoteCacheCleaner = true;

                try
                {
                    this._affiliateResponseService.UpdateDatabase(); //this function will clear invalid entities in case error
                }
                catch
                {
                    this._affiliateResponseService.UpdateDatabase(); //try one more time
                    //we do not handle this case
                }

                return base.Content(HttpStatusCode.OK, m.Response.Response, new TextMediaTypeFormatter(), "text/plain");

                return Ok(m.Response.Response);
            }
            catch (Exception ex)
            {
                MemoryCacheManager.EnableRemoteCacheCleaner = true;
                GlobalDataManager.CurrentLeadCount--;

                m.GenerateErrorAffiliateResponse(context, ex.Message);

                try
                {
                    //try update one more time, in case DB exception throws
                    this._affiliateResponseService.UpdateDatabase();
                }
                catch
                {
                    //we do not handle this case
                }

                return base.Content(HttpStatusCode.OK, m.Response.Response, new TextMediaTypeFormatter(), "text/plain");

                return Ok(m.Response.Response);

                //return Content(HttpStatusCode.OK, m.Response.Response, MediaTypeFormatter.);

                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("click")]
        public IHttpActionResult Click()
        {
            bool result = false;

            try
            {
                string key = Request.Properties["key"]?.ToString();
                string clickTypeStr = Request.Properties["type"]?.ToString();

                clickTypeStr = char.ToUpper(clickTypeStr[0]) + clickTypeStr.ToLower().Substring(1);

                ClickTypes clickType = (ClickTypes)Enum.Parse(typeof(ClickTypes), clickTypeStr);

                ClickChannel clickChannel = _clickService.GetClickChannelByAccessKey(key);

                if (clickChannel != null)
                {
                    ClickMain clickMain = new ClickMain();
                    clickMain.CreatedAt = DateTime.UtcNow;
                    clickChannel.AccessKey = key;
                    clickMain.ClickType = clickType;
                    clickMain.IpAddress = Request.RequestUri.Host;
                    clickMain.ClickChannelId = clickChannel.Id;
                    clickMain.ClickPrice = clickChannel.ClickPrice;
                    _clickService.InsertClickMain(clickMain);

                    foreach (var paramKey in Request.Properties.Keys)
                    {
                        if (string.IsNullOrEmpty(paramKey) ||
                            (paramKey.ToLower() == "key" || paramKey.ToLower() == "type" || paramKey.ToLower() == "clickid"))
                        {
                            continue;
                        }

                        _clickService.InsertClickContent(new ClickContent()
                        {
                            AffiliateChannelId = clickChannel.AffiliateChannelId,
                            ClickChannelId = clickChannel.Id,
                            ParamName = paramKey,
                            ParamValue = Request.Properties[paramKey]?.ToString(),
                            ClickMainId = clickMain.Id
                        });
                    }

                    result = true;

                    if (Request.Method == HttpMethod.Get && clickType == ClickTypes.Click && !string.IsNullOrEmpty(clickChannel.RedirectUrl))
                        return Redirect(clickChannel.RedirectUrl);
                }
            }
            catch (Exception ex)
            {

            }

            return Ok(new { result = result });
        }

        [HttpPost]
        [Route("postToBuyer/{buyerChannelId}")]
        public IHttpActionResult PostToBuyer(long buyerChannelId, [FromBody] string body)
        {
            var buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerChannelId);

            if (buyerChannel ==  null)
            {
                return HttpBadRequest("buyer channel not found");
            }

            var contentType = "application/x-www-form-urlencoded";

            if (buyerChannel.DataFormat == (short)PostFormat.Json)
                contentType = "application/json";
            else
            if (buyerChannel.DataFormat == (short)PostFormat.XML)
                contentType = "application/xml";

            var postingHeaders = Adrack.WebApi.Helpers.Helpers.ReadPostingHeaders(buyerChannel);
            var postingHeadersStr = buyerChannel.PostingHeaders;

            string response = Adrack.WebApi.Helpers.Helpers.PostXml(buyerChannel.PostingUrl, body, 60000, contentType, postingHeaders, postingHeadersStr, buyerChannel.DataFormat != 3 ? "POST" : "GET");

            return Ok(response);
        }
    }
}
