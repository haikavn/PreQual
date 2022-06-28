// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RequestContext.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Service.Accounting;
using Adrack.Service.Click;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Lead;
using Adrack.Service.Message;
using Adrack.WebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace Adrack.Managers
{
    /// <summary>
    ///     Class RequestContext.
    /// </summary>
    public class RequestContext
    {
        #region Private properties

        /// <summary>
        ///     The manager
        /// </summary>
        private RequestManager manager;

        /// <summary>
        ///     The HTTP request
        /// </summary>
        private HttpRequestMessage httpRequest;

        /// <summary>
        ///     The extra
        /// </summary>
        private readonly Dictionary<string, object> extra = new Dictionary<string, object>();

        /// <summary>
        ///     The campaign service
        /// </summary>
        private ICampaignService _campaignService;

        /// <summary>
        ///     The campaign template service
        /// </summary>
        private ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        ///     The buyer channel service
        /// </summary>
        private IBuyerChannelService _buyerChannelService;

        /// <summary>
        ///     The buyer channel template service
        /// </summary>
        private IBuyerChannelTemplateService _buyerChannelTemplateService;

        /// <summary>
        ///     The affiliate channel service
        /// </summary>
        private IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        ///     The affiliate service
        /// </summary>
        private IAffiliateService _affiliateService;

        /// <summary>
        ///     The affiliate response service
        /// </summary>
        private IAffiliateResponseService _affiliateResponseService;

        /// <summary>
        ///     The buyer response service
        /// </summary>
        private IBuyerResponseService _buyerResponseService;

        /// <summary>
        ///     The affiliate channel template service
        /// </summary>
        private IAffiliateChannelTemplateService _affiliateChannelTemplateService;

        /// <summary>
        ///     The buyer service
        /// </summary>
        private IBuyerService _buyerService;

        /// <summary>
        ///     The lead content service
        /// </summary>
        private ILeadContentService _leadContentService;

        /// <summary>
        ///     The lead content dublicate service
        /// </summary>
        private ILeadContentDublicateService _leadContentDublicateService;

        /// <summary>
        ///     The lead main service
        /// </summary>
        private ILeadMainService _leadMainService;

        /// <summary>
        ///     The lead main response service
        /// </summary>
        private ILeadMainResponseService _leadMainResponseService;

        /// <summary>
        ///     The lead schedule service
        /// </summary>
        private IBuyerChannelScheduleService _buyerChannelScheduleService;

        /// <summary>
        ///     The setting service
        /// </summary>
        private ISettingService _settingService;

        /// <summary>
        ///     The processing log service
        /// </summary>
        private IProcessingLogService _processingLogService;

        /// <summary>
        ///     The buyer channel filter condition service
        /// </summary>
        private IBuyerChannelFilterConditionService _buyerChannelFilterConditionService;

        /// <summary>
        ///     The affiliate channel filter condition service
        /// </summary>
        private IAffiliateChannelFilterConditionService _affiliateChannelFilterConditionService;

        /// <summary>
        ///     The black list service
        /// </summary>
        private IBlackListService _blackListService;

        /// <summary>
        ///     The redirect URL service
        /// </summary>
        private IRedirectUrlService _redirectUrlService;

        /// <summary>
        ///     The zip code redirect service
        /// </summary>
        private IZipCodeRedirectService _zipCodeRedirectService;

        /// <summary>
        ///     The posted data service
        /// </summary>
        private IPostedDataService _postedDataService;

        /// <summary>
        ///     The accounting service
        /// </summary>
        private IAccountingService _accountingService;

        /// <summary>
        ///     The buyer channel template matching service
        /// </summary>
        private IBuyerChannelTemplateMatchingService _buyerChannelTemplateMatchingService;

        /// <summary>
        ///     The start date UTC
        /// </summary>
        private DateTime startDateUtc = DateTime.UtcNow;

        /// <summary>
        ///     The time zone setting
        /// </summary>
        private Setting timeZoneSetting;

        /// <summary>
        ///     The lead sensitive data service
        /// </summary>
        private ILeadSensitiveDataService _leadSensitiveDataService;

        /// <summary>
        ///     The dublicate monitor
        /// </summary>
        private bool dublicateMonitor = true;

        #endregion Private properties

        #region Public properties

        /// <summary>
        ///     Gets or sets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public RequestManager Manager
        {
            get => manager;
            set => manager = value;
        }

        /// <summary>
        ///     Gets or sets the HTTP request.
        /// </summary>
        /// <value>The HTTP request.</value>
        public HttpRequestMessage HttpRequest
        {
            get => httpRequest;
            set => httpRequest = value;
        }

        /// <summary>
        ///     Gets or sets the HTTP response.
        /// </summary>
        /// <value>The HTTP response.</value>
       /* public HttpResponseBase HttpResponse
        {
            get => httpResponse;
            set => httpResponse = value;
        }*/

        /// <summary>
        ///     Gets the extra.
        /// </summary>
        /// <value>The extra.</value>
        public Dictionary<string, object> Extra => extra;

        /// <summary>
        ///     Gets or sets the campaign service.
        /// </summary>
        /// <value>The campaign service.</value>
        public ICampaignService CampaignService
        {
            get => _campaignService;
            set => _campaignService = value;
        }

        /// <summary>
        ///     Gets or sets the campaign template service.
        /// </summary>
        /// <value>The campaign template service.</value>
        public ICampaignTemplateService CampaignTemplateService
        {
            get => _campaignTemplateService;
            set => _campaignTemplateService = value;
        }

        /// <summary>
        ///     Gets or sets the buyer channel service.
        /// </summary>
        /// <value>The buyer channel service.</value>
        public IBuyerChannelService BuyerChannelService
        {
            get => _buyerChannelService;
            set => _buyerChannelService = value;
        }

        /// <summary>
        ///     Gets or sets the buyer channel template service.
        /// </summary>
        /// <value>The buyer channel template service.</value>
        public IBuyerChannelTemplateService BuyerChannelTemplateService
        {
            get => _buyerChannelTemplateService;
            set => _buyerChannelTemplateService = value;
        }

        /// <summary>
        ///     Gets or sets the affiliate channel service.
        /// </summary>
        /// <value>The affiliate channel service.</value>
        public IAffiliateChannelService AffiliateChannelService
        {
            get => _affiliateChannelService;
            set => _affiliateChannelService = value;
        }

        /// <summary>
        ///     Gets or sets the affiliate service.
        /// </summary>
        /// <value>The affiliate service.</value>
        public IAffiliateService AffiliateService
        {
            get => _affiliateService;
            set => _affiliateService = value;
        }

        /// <summary>
        ///     Gets or sets the affiliate response service.
        /// </summary>
        /// <value>The affiliate response service.</value>
        public IAffiliateResponseService AffiliateResponseService
        {
            get => _affiliateResponseService;
            set => _affiliateResponseService = value;
        }

        /// <summary>
        ///     Gets or sets the buyer response service.
        /// </summary>
        /// <value>The buyer response service.</value>
        public IBuyerResponseService BuyerResponseService
        {
            get => _buyerResponseService;
            set => _buyerResponseService = value;
        }

        /// <summary>
        ///     Gets or sets the affiliate channel template service.
        /// </summary>
        /// <value>The affiliate channel template service.</value>
        public IAffiliateChannelTemplateService AffiliateChannelTemplateService
        {
            get => _affiliateChannelTemplateService;
            set => _affiliateChannelTemplateService = value;
        }

        /// <summary>
        ///     Gets or sets the buyer service.
        /// </summary>
        /// <value>The buyer service.</value>
        public IBuyerService BuyerService
        {
            get => _buyerService;
            set => _buyerService = value;
        }

        /// <summary>
        ///     Gets or sets the lead content service.
        /// </summary>
        /// <value>The lead content service.</value>
        public ILeadContentService LeadContentService
        {
            get => _leadContentService;
            set => _leadContentService = value;
        }

        /// <summary>
        ///     Gets or sets the lead fields content service.
        /// </summary>
        /// <value>The lead fields content service.</value>
        public ILeadFieldsContentService LeadFieldsContentService { get; set; }

        /// <summary>
        ///     Gets or sets the lead content dublicate service.
        /// </summary>
        /// <value>The lead content dublicate service.</value>
        public ILeadContentDublicateService LeadContentDublicateService
        {
            get => _leadContentDublicateService;
            set => _leadContentDublicateService = value;
        }

        /// <summary>
        ///     Gets or sets the lead main service.
        /// </summary>
        /// <value>The lead main service.</value>
        public ILeadMainService LeadMainService
        {
            get => _leadMainService;
            set => _leadMainService = value;
        }

        /// <summary>
        ///     Gets or sets the lead main response service.
        /// </summary>
        /// <value>The lead main response service.</value>
        public ILeadMainResponseService LeadMainResponseService
        {
            get => _leadMainResponseService;
            set => _leadMainResponseService = value;
        }

        /// <summary>
        ///     Gets or sets the setting service.
        /// </summary>
        /// <value>The setting service.</value>
        public ISettingService SettingService
        {
            get => _settingService;
            set => _settingService = value;
        }

        /// <summary>
        ///     Gets or sets the processing log service.
        /// </summary>
        /// <value>The processing log service.</value>
        public IProcessingLogService ProcessingLogService
        {
            get => _processingLogService;
            set => _processingLogService = value;
        }

        /// <summary>
        ///     Gets or sets the lead schedule service.
        /// </summary>
        /// <value>The lead schedule service.</value>
        public IBuyerChannelScheduleService BuyerChannelScheduleService
        {
            get => _buyerChannelScheduleService;
            set => _buyerChannelScheduleService = value;
        }

        /// <summary>
        ///     Gets or sets the buyer channel filter condition service.
        /// </summary>
        /// <value>The buyer channel filter condition service.</value>
        public IBuyerChannelFilterConditionService BuyerChannelFilterConditionService
        {
            get => _buyerChannelFilterConditionService;
            set => _buyerChannelFilterConditionService = value;
        }

        /// <summary>
        ///     Gets or sets the affiliate channel filter condition service.
        /// </summary>
        /// <value>The affiliate channel filter condition service.</value>
        public IAffiliateChannelFilterConditionService AffiliateChannelFilterConditionService
        {
            get => _affiliateChannelFilterConditionService;
            set => _affiliateChannelFilterConditionService = value;
        }

        /// <summary>
        ///     Gets or sets the black list service.
        /// </summary>
        /// <value>The black list service.</value>
        public IBlackListService BlackListService
        {
            get => _blackListService;
            set => _blackListService = value;
        }

        /// <summary>
        ///     Gets or sets the redirect URL service.
        /// </summary>
        /// <value>The redirect URL service.</value>
        public IRedirectUrlService RedirectUrlService
        {
            get => _redirectUrlService;
            set => _redirectUrlService = value;
        }

        /// <summary>
        ///     Gets or sets the zip code redirect service.
        /// </summary>
        /// <value>The zip code redirect service.</value>
        public IZipCodeRedirectService ZipCodeRedirectService
        {
            get => _zipCodeRedirectService;
            set => _zipCodeRedirectService = value;
        }

        /// <summary>
        ///     Gets or sets the posted data service.
        /// </summary>
        /// <value>The posted data service.</value>
        public IPostedDataService PostedDataService
        {
            get => _postedDataService;
            set => _postedDataService = value;
        }

        /// <summary>
        ///     Gets or sets the accounting service.
        /// </summary>
        /// <value>The accounting service.</value>
        public IAccountingService AccountingService
        {
            get => _accountingService;
            set => _accountingService = value;
        }

        /// <summary>
        ///     Gets or sets the buyer channel template matching service.
        /// </summary>
        /// <value>The buyer channel template matching service.</value>
        public IBuyerChannelTemplateMatchingService BuyerChannelTemplateMatchingService
        {
            get => _buyerChannelTemplateMatchingService;
            set => _buyerChannelTemplateMatchingService = value;
        }

        /// <summary>
        ///     Gets or sets the lead sensitive data service.
        /// </summary>
        /// <value>The lead sensitive data service.</value>
        public ILeadSensitiveDataService LeadSensitiveDataService
        {
            get => _leadSensitiveDataService;
            set => _leadSensitiveDataService = value;
        }

        /// <summary>
        ///     Gets or sets the email service.
        /// </summary>
        /// <value>The email service.</value>
        public IEmailService EmailService { get; set; }

        /// <summary>
        ///     Gets or sets the do not present service.
        /// </summary>
        /// <value>The do not present service.</value>
        public IDoNotPresentService DoNotPresentService { get; set; }

        /// <summary>
        ///     Gets or sets the sub id white list service service.
        /// </summary>
        /// <value>The sub id white list service service.</value>

        public ISubIdWhiteListService SubIdWhiteListService { get; set; }

        public ICachedUrlService CachedUrlService { get; set; }

        public ICountryService CountryService { get; set; }

        public IClickService ClickService { get; set; }

        /// <summary>
        ///     Gets or sets the start date UTC.
        /// </summary>
        /// <value>The start date UTC.</value>
        public DateTime StartDateUtc
        {
            get => startDateUtc;
            set => startDateUtc = value;
        }

        public int PastSeconds(DateTime? fromDate = null)
        {
            if (fromDate.HasValue)
            {
                return (int)(Helpers.UtcNow() - fromDate.Value).TotalSeconds;
            }

            return (int)(Helpers.UtcNow() - startDateUtc).TotalSeconds;

        }

        /// <summary>
        ///     Gets or sets the time zone setting.
        /// </summary>
        /// <value>The time zone setting.</value>
        public Setting TimeZoneSetting
        {
            get => timeZoneSetting;
            set => timeZoneSetting = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [dublicate monitor].
        /// </summary>
        /// <value><c>true</c> if [dublicate monitor]; otherwise, <c>false</c>.</value>
        public bool DublicateMonitor
        {
            get => dublicateMonitor;
            set => dublicateMonitor = value;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [allow affiliate redirect].
        /// </summary>
        /// <value><c>true</c> if [allow affiliate redirect]; otherwise, <c>false</c>.</value>
        public bool AllowAffiliateRedirect { get; set; }

        /// <summary>
        ///     Gets or sets the affiliate redirect URL.
        /// </summary>
        /// <value>The affiliate redirect URL.</value>
        public string AffiliateRedirectUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [maximum duplicate days].
        /// </summary>
        /// <value><c>true</c> if [maximum duplicate days]; otherwise, <c>false</c>.</value>
        public bool MaxDuplicateDays { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [lead email].
        /// </summary>
        /// <value><c>true</c> if [lead email]; otherwise, <c>false</c>.</value>
        public bool LeadEmail { get; set; }

        /// <summary>
        ///     Gets or sets the lead email to.
        /// </summary>
        /// <value>The lead email to.</value>
        public string LeadEmailTo { get; set; }

        /// <summary>
        ///     Gets or sets the lead email fields.
        /// </summary>
        /// <value>The lead email fields.</value>
        public string LeadEmailFields { get; set; }

        /// <summary>
        ///     Gets or sets the white ip.
        /// </summary>
        /// <value>The white ip.</value>
        public string WhiteIp { get; set; }

        /// <summary>
        ///     Gets or sets the affiliate.
        /// </summary>
        /// <value>The affiliate.</value>
        public Affiliate Affiliate { get; set; }

        public AffiliateChannel AffiliateChannel { get; set; }


        /// <summary>
        ///     Gets or sets the state of the click.
        /// </summary>
        /// <value>The state of the click.</value>
        public short ClickState { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [minimum processing mode].
        /// </summary>
        /// <value><c>true</c> if [minimum processing mode]; otherwise, <c>false</c>.</value>
        public bool MinProcessingMode { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [system on hold].
        /// </summary>
        /// <value><c>true</c> if [system on hold]; otherwise, <c>false</c>.</value>
        public bool SystemOnHold { get; set; }

        /// <summary>
        ///     Gets or sets the lead fields content list.
        /// </summary>
        /// <value>The lead fields content list.</value>
        public List<LeadFieldsContent> LeadFieldsContentList { get; set; }

        /// <summary>
        ///     Gets or sets the lead created.
        /// </summary>
        /// <value>The lead created.</value>
        public DateTime? LeadCreated { get; set; }

        /// <summary>
        /// Gets or Sets the ping tree test mode value
        /// </summary>
        /// <value>0-off, 1-price reject mode, 2-no price reject mode, 3-random mode</value>
        public short PingTreeTestMode { get; set; }

        public bool PrioritizedEnabled { get; set; }

        /// <summary>
        ///     Gets or sets debug mode.
        /// </summary>
        /// <value><c>true</c> if [in debug mode]; otherwise, <c>false</c>.</value>
        public bool DebugMode { get; set; }

        public Dictionary<string, string> HashedFieldValues { get; set; }
        public Dictionary<string, string> AllFieldValues { get; set; }

        public Dictionary<long, int> BuyerDailyCaps { get; set; }

        public string PostedDataBody { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RequestContext" /> class.
        /// </summary>
        public RequestContext()
        {
            LeadFieldsContentList = new List<LeadFieldsContent>();
            LeadCreated = null;
            DebugMode = true;
            HashedFieldValues = new Dictionary<string, string>();
            AllFieldValues = new Dictionary<string, string>();
            BuyerDailyCaps = new Dictionary<long, int>();
            PrioritizedEnabled = true;
        }

        #endregion Public properties
    }
}