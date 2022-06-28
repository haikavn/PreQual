using Adrack.Core;
using Adrack.Core.Helpers;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Domain.Membership;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.WebApi.Infrastructure.Web.Helpers;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Lead;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Http;
using System.Web.Hosting;
using System.Web;
using Adrack.Web.Framework.Security;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/leads_old")]
    public class LeadController_OLD : BaseApiController
    {
        #region readonly properties

        private int StateUsaCode => 80;
        private int TotalRowsCount => 1000;
        private int RowsPerPage => 100;
        private string DateFormatString => "MM/dd/yyyy";

        private string VisibilityCookiePrefix => "AdrackColumnVisibility";
        private string MissingNoteTitle => "Pending contact";

        private string ColumnsCookieName
        {
            get
            {
                var uri = new Uri(Request.GetBaseUrl());
                var cookieName = $"{VisibilityCookiePrefix}-{uri.Host}-{_appContext.AppUser.Id.ToString()}";
                return cookieName;
            }
        }

        #endregion readonly properties

        #region fields

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The HTTP context
        /// </summary>
        private readonly HttpContextBase _httpContext;

        /// <summary>
        /// The accounting service
        /// </summary>
        private readonly IAccountingService _accountingService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The affiliate response service
        /// </summary>
        private readonly IAffiliateResponseService _affiliateResponseService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The black list service
        /// </summary>
        private readonly IBlackListService _blackListService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        /// The encryption service
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        /// The lead content duplicate service
        /// </summary>
        private readonly ILeadContentDublicateService _leadContentDuplicateService;

        /// <summary>
        /// The lead main response service
        /// </summary>
        private readonly ILeadMainResponseService _leadMainResponseService;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly ILeadMainService _leadMainService;

        /// <summary>
        /// The lead sensitive data service
        /// </summary>
        private readonly ILeadSensitiveDataService _leadSensitiveDataService;

        /// <summary>
        /// The note title service
        /// </summary>
        private readonly INoteTitleService _noteTitleService;

        /// <summary>
        /// The user permission service
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// The redirect URL service
        /// </summary>
        private readonly IRedirectUrlService _redirectUrlService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The state province service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        ///// <summary>
        ///// The lead model
        ///// </summary>
        //public LeadModel _leadModel;

        #endregion fields

        #region constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="appContext">Application Context</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="accountingService">The accounting service.</param>
        /// <param name="affiliateResponseService">The affiliate response service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="blackListService">The black list service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="leadContentDuplicateService">The lead content duplicate service.</param>
        /// <param name="leadMainResponseService">The lead main response service.</param>
        /// <param name="leadMainService">The lead main service.</param>
        /// <param name="leadSensitiveDataService">The lead sensitive data service.</param>
        /// <param name="noteTitleService">The note title service.</param>
        /// <param name="permissionService">The user permission service</param>
        /// <param name="redirectUrlService">The redirect URL service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="stateProvinceService">The state province service</param>
        /// <param name="userService">The user service.</param>
        public LeadController_OLD(
                            IAppContext appContext,
                            HttpContextBase httpContext,
                            IAccountingService accountingService,
                            IAffiliateChannelService affiliateChannelService,
                            IAffiliateResponseService affiliateResponseService,
                            IAffiliateService affiliateService,
                            IBlackListService blackListService,
                            IBuyerChannelService buyerChannelService,
                            IBuyerService buyerService,
                            ICampaignService campaignService,
                            ICampaignTemplateService campaignTemplateService,
                            IEncryptionService encryptionService,
                            ILeadContentDublicateService leadContentDuplicateService,
                            ILeadMainResponseService leadMainResponseService,
                            ILeadMainService leadMainService,
                            ILeadSensitiveDataService leadSensitiveDataService,
                            INoteTitleService noteTitleService,
                            IPermissionService permissionService,
                            IRedirectUrlService redirectUrlService,
                            ISettingService settingService,
                            IStateProvinceService stateProvinceService,
                            IUserService userService//,
                            )
        {
            //this._leadModel = new LeadModel();
            this._appContext = appContext;
            this._httpContext = httpContext;

            this._accountingService = accountingService;
            this._affiliateResponseService = affiliateResponseService;
            this._campaignService = campaignService;
            this._campaignTemplateService = campaignTemplateService;
            this._affiliateService = affiliateService;
            this._affiliateChannelService = affiliateChannelService;
            this._blackListService = blackListService;
            this._buyerService = buyerService;
            this._buyerChannelService = buyerChannelService;
            this._encryptionService = encryptionService;
            this._leadContentDuplicateService = leadContentDuplicateService;
            this._leadMainResponseService = leadMainResponseService;
            this._leadMainService = leadMainService;
            this._leadSensitiveDataService = leadSensitiveDataService;
            this._noteTitleService = noteTitleService;
            this._permissionService = permissionService;
            this._redirectUrlService = redirectUrlService;
            this._settingService = settingService;
            this._stateProvinceService = stateProvinceService;
            this._userService = userService;
        }

        #endregion constructor

        #region methods

        #region route methods

        // GET: Leads List
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[NavigationBreadCrumb(Clear = true, Label = "Logs / Leads")]
        [Route("")]
        public LeadExtendedModel Index()
        {
            var type = "a"; // Affiliate
            var allBuyerChannels = true;
            var allCampaigns = true;

            if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId) //Admin & Network
            {
                type = string.Empty;
            } 
            else if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId) //Buyer
            {
                type = "b";
                allBuyerChannels = false;
                allCampaigns = false;
            }

            return GetLeadExtendedInfo(0L, type, allBuyerChannels, allCampaigns);
        }

        /// <summary>
        /// Affiliates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [Route("{id}/type/affiliate")]
        public LeadExtendedModel GetByAffiliateType(long id)
        {
            return GetLeadExtendedInfo(id, "a", false);
        }

        /// <summary>
        /// Buyers the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [Route("{id}/type/buyer")]
        public LeadExtendedModel GetByBuyerType(long id)
        {
            return GetLeadExtendedInfo(id, "b", false);
        }

        // GET: Leads List
        /// <summary>
        /// Indexes the partial.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type a-affiliate or b-buyer.</param>
        /// <returns>ActionResult.</returns>
        [Route("{id}/type/{type}")]
        public LeadExtendedModel GetByType(long id, string type)
        {
            return GetLeadExtendedInfo(id, type, false);
        }

        /// <summary>
        /// Indexes the partial aff.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>ActionResult.</returns>
        [Route("{id}/type/{type}/all")]
        public LeadExtendedModel GetByAffiliateAndAllBuyerChannels(long id, string type)
        {
            return GetLeadExtendedInfo(id, type);
        }

        /// <summary>
        /// Gets the leads ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("paged")]
        [ContentManagementAntiForgery(true)]
        public LeadMainContentPagedModel GetLeadsAjax([FromBody]LeadFilterModel filterModel)
        {
            var leadMainContentModels = new List<LeadMainContentModel>();

            bool.TryParse(filterModel.IsReport, out var report);

            var leadMainContents = GetLeadMainContents(filterModel, out var filter);
            var total = filter.LeadId == 0
                ? this._leadMainService.GetLeadsCount(filter.DateFrom, filter.DateTo, filter.Email,
                    filter.AffiliateId, filter.AffiliateChannelId, filter.BuyerId, filter.BuyerChannelId,
                    filter.CampaignId, filter.Status, filter.IP, filter.State, (filter.Notes ?? string.Empty))
                : 1;

            var leadMainContentPagedModel = new LeadMainContentPagedModel
            {
                RecordsStart = filter.Page,
                RecordsTotal = total,
                RecordsFiltered = total,
                TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser)
                    .ToString(DateFormatString)
            };

            bool allowAffiliateRedirect = GetAllowAffiliateRedirect();

            foreach (LeadMainContent leadMainContent in leadMainContents)
            {
                var affiliateName = string.Empty;
                var buyerName = string.Empty;
                var campaignName = string.Empty;
                var buyerChannelName = string.Empty;
                var affiliateChannelName = string.Empty;

                Affiliate affiliate = this._affiliateService.GetAffiliateById(leadMainContent.AffiliateId, true);
                if (affiliate != null)
                {
                    affiliateName = affiliate.Name;
                }

                if (leadMainContent.BuyerId != null)
                {
                    Buyer buyer = this._buyerService.GetBuyerById((long)leadMainContent.BuyerId);
                    if (buyer != null)
                    {
                        buyerName = buyer.Name;
                    }
                }
                if (leadMainContent.BuyerChannelId != null)
                {
                    var buyerChannel = this._buyerChannelService.GetBuyerChannelById((long)leadMainContent.BuyerChannelId);
                    if (buyerChannel != null)
                    {
                        buyerChannelName = buyerChannel.Name;
                    }
                }

                if (leadMainContent.AffiliateChannelId != 0)
                {
                    var affiliateChannel = this._affiliateChannelService.GetAffiliateChannelById(leadMainContent.AffiliateChannelId);
                    if (affiliateChannel != null)
                    {
                        affiliateChannelName = affiliateChannel.Name;
                    }
                }

                if (leadMainContent.CampaignId != 0)
                {
                    campaignName = this._campaignService.GetCampaignById(leadMainContent.CampaignId).Name;
                }

                string emailResult = string.Empty;
                if (leadMainContent.Email != null)
                {
                    var maskEmail = _appContext.AppUser?.MaskEmail ?? true;

                    emailResult = maskEmail ? Regex.Replace(leadMainContent.Email, @"(?<=[\w]{2})[\w-\._\+%]*(?=[\w]{1}@)", m => new string('*', m.Length)) : leadMainContent.Email;
                }

                DateTime created = _settingService.GetTimeZoneDate(leadMainContent.Created.Value);
                DateTime? updated = null;
                if (leadMainContent.UpdateDate.HasValue)
                    updated = _settingService.GetTimeZoneDate(leadMainContent.UpdateDate.Value);

                var leadNoteUiString = GetLeadNoteUiString(leadMainContent.Id, leadMainContent.LeadId);
                var statusUiString = GetStatusUiString(leadMainContent.Status);
                var monitorUiString = GetMonitorUiString(leadMainContent.Id, leadMainContent.Warning);
                var redirectUiString = (leadMainContent.Status == 1 && allowAffiliateRedirect)
                    ? GetRedirectUiString(leadMainContent.Id, leadMainContent.Clicked ?? false)
                    : string.Empty;

                if (report)
                {
                    leadMainContentModels.Add(new LeadMainContentModel
                    {
                        HasPermission = this._permissionService.Authorize(PermissionProvider.LeadsIinfoView),
                        Id = leadMainContent.Id,
                        Created = created,
                        EmailResult = emailResult,
                        RedirectUrl = redirectUiString,
                        StatusString = statusUiString,
                        AffiliateId = leadMainContent.AffiliateId,
                        AffiliateChannelId = leadMainContent.AffiliateChannelId,
                        AffiliateName = affiliateName,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerPrice = Math.Round(leadMainContent.BuyerPrice ?? 0, 2),
                        LeadNoteString = leadNoteUiString
                    });

                }
                else if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId ||
                         _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
                {
                    leadMainContentModels.Add(new LeadMainContentModel
                    {
                        HasPermission = this._permissionService.Authorize(PermissionProvider.LeadsIinfoView),
                        Id = leadMainContent.Id,
                        EmailResult = emailResult,
                        BuyerPrice = Math.Round(leadMainContent.BuyerPrice ?? 0, 2),
                        AffiliatePrice = Math.Round(leadMainContent.AffiliatePrice ?? 0, 2),
                        Created = created,
                        Updated = updated,
                        FirstName = leadMainContent.Firstname,
                        LastName = leadMainContent.Lastname,
                        IP = leadMainContent.Ip,
                        Zip = leadMainContent.Zip,
                        State = leadMainContent.State,
                        Status = leadMainContent.Status,
                        StatusString = statusUiString,
                        AffiliateId = leadMainContent.AffiliateId,
                        AffiliateName = affiliateName,
                        AffiliateChannelId = leadMainContent.AffiliateChannelId,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerId = leadMainContent.BuyerId,
                        BuyerName = buyerName,
                        BuyerChannelId = leadMainContent.BuyerChannelId,
                        BuyerChannelName = buyerChannelName,
                        CampaignId = leadMainContent.CampaignId,
                        CampaignName = campaignName,
                        Monitor = monitorUiString,
                        RedirectUrl = redirectUiString,
                        ProcessingTime = leadMainContent.ProcessingTime
                    });
                }
                else if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                {
                    leadMainContentModels.Add(new LeadMainContentModel
                    {
                        HasPermission = this._permissionService.Authorize(PermissionProvider.LeadsIinfoView),
                        Id = leadMainContent.Id,
                        EmailResult = emailResult,
                        BuyerPrice = Math.Round(leadMainContent.BuyerPrice ?? 0, 2),
                        AffiliatePrice = Math.Round(leadMainContent.AffiliatePrice ?? 0, 2),
                        Created = created,
                        Updated = updated,
                        FirstName = leadMainContent.Firstname,
                        LastName = leadMainContent.Lastname,
                        IP = leadMainContent.Ip,
                        Zip = leadMainContent.Zip,
                        State = leadMainContent.State,
                        Status = leadMainContent.Status,
                        StatusString = statusUiString,
                        AffiliateId = leadMainContent.AffiliateId,
                        AffiliateName = affiliateName,
                        AffiliateChannelId = leadMainContent.AffiliateChannelId,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerId = leadMainContent.BuyerId,
                        BuyerName = buyerName,
                        BuyerChannelId = leadMainContent.BuyerChannelId,
                        BuyerChannelName = buyerChannelName,
                        CampaignId = leadMainContent.CampaignId,
                        CampaignName = campaignName,
                        ProcessingTime = leadMainContent.ProcessingTime
                    });
                }
                else
                {
                    var noNotes = false;
                    if (_appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                    {
                        Buyer buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
                        if (buyer != null && buyer.AlwaysSoldOption == 0) noNotes = true;
                    }

                    leadMainContentModels.Add(new LeadMainContentModel
                    {
                        HasNotes = noNotes,
                        HasPermission = this._permissionService.Authorize(PermissionProvider.LeadsIinfoView),
                        Id = leadMainContent.Id,
                        EmailResult = emailResult,
                        BuyerPrice = Math.Round(leadMainContent.BuyerPrice ?? 0, 2),
                        AffiliatePrice = Math.Round(leadMainContent.AffiliatePrice ?? 0, 2),
                        Created = created,
                        Updated = updated,
                        FirstName = leadMainContent.Firstname,
                        LastName = leadMainContent.Lastname,
                        IP = leadMainContent.Ip,
                        Zip = leadMainContent.Zip,
                        State = leadMainContent.State,
                        Status = leadMainContent.Status,
                        StatusString = statusUiString,
                        AffiliateId = leadMainContent.AffiliateId,
                        AffiliateName = affiliateName,
                        AffiliateChannelId = leadMainContent.AffiliateChannelId,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerId = leadMainContent.BuyerId,
                        BuyerName = buyerName,
                        BuyerChannelId = leadMainContent.BuyerChannelId,
                        BuyerChannelName = buyerChannelName,
                        CampaignId = leadMainContent.CampaignId,
                        CampaignName = campaignName,
                        ProcessingTime = leadMainContent.ProcessingTime,
                        LeadNoteString = leadNoteUiString
                    });

                }
            }

            leadMainContentPagedModel.LeadMainContents = leadMainContentModels;
            return leadMainContentPagedModel;
        }

        /// <summary>
        /// Gets the lead by identifier ajax.
        /// </summary>
        /// <returns>System.String.</returns>
        [HttpPost]
        [Route("{id}")]
        [ContentManagementAntiForgery(true)]
        public string GetLeadByIdAjax(string id)
        {
            if (id == null)
                return null;
            long leadIdResult = long.Parse(id);
            LeadMainContent lead = this._leadMainService.GetLeadsAllById(leadIdResult);

            if (lead == null) return null;

            return lead.ReceivedData;
        }

        /// <summary>
        /// Gets the email item.
        /// </summary>
        /// <param name="lead">The lead.</param>
        /// <param name="leadEmailFields">The lead email fields.</param>
        /// <returns>System.String.</returns>
        [Route("email")]
        public LeadResponseModel GetEmailItem([FromBody]LeadMainModel lead)
        {
            var responseModel = new LeadResponseModel();
            var xmlNodes = new List<XmlNode>();

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(lead.ReceivedData);
                GetNodes(xmlDocument.DocumentElement, xmlNodes, lead.CampaignId);
            }
            catch(Exception)
            {
            }

            //using (var sw = new StringWriter())
            //{
                //ViewDataDictionary viewData = new ViewDataDictionary();
                responseModel.Nodes = xmlNodes;
                responseModel.AllowedNodes = this._campaignTemplateService.CampaignTemplateAllowedNames(lead.CampaignId);
                responseModel.SensitiveData = this._leadSensitiveDataService.GetLeadSensitiveDataByLeadId(lead.Id);
                responseModel.LeadEmailFields = (!string.IsNullOrEmpty(lead.EmailFields) ? lead.EmailFields.Split(new char[1] { ',' }) : new string[0]);
                return responseModel;
                //    TempDataDictionary tempData = new TempDataDictionary();

                //    var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                //                                                             "ItemEmail");
                //    var viewContext = new ViewContext(ControllerContext, viewResult.View,
                //                                 viewData, tempData, sw);
                //    viewResult.View.Render(viewContext, sw);
                //    viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                //    return sw.GetStringBuilder().ToString();
                //}
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        //[ContentManagementAntiForgery(true)]
        [Route("{id}/response")]
        public LeadResponseModel Item(long id)
        {
            var leadResponseModel = new LeadResponseModel(); // TODO as parameter

            if (_appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                LeadMain leadMain = this._leadMainService.GetLeadMainById(id);
                if (leadMain != null)
                {
                    leadMain.ViewDate = DateTime.UtcNow;
                    this._leadMainService.UpdateLeadMain(leadMain);
                }
            }

            LeadMainContent lead = this._leadMainService.GetLeadsAllById(id);

            if (lead != null)
            {
                var affiliateResponses = (List<AffiliateResponse>)this._affiliateResponseService.GetAffiliateResponsesByLeadId(id);

                leadResponseModel.AffiliateResponses = affiliateResponses;
                AffiliateResponse affiliateResponse = null; ;

                if (affiliateResponses.Count > 0)
                {
                    affiliateResponse = affiliateResponses.FirstOrDefault(x => !string.IsNullOrEmpty(x.Message));
                }

                leadResponseModel.AffiliateResponseMessage = (affiliateResponse?.Message ?? "");

                leadResponseModel.ReceivedData = lead.ReceivedData;

                leadResponseModel.Lead = lead;
                leadResponseModel.LeadCreated = _settingService.GetTimeZoneDate(lead.Created.Value);

                if (lead.CampaignId != 0)
                {
                    leadResponseModel.CampaignName = this._campaignService.GetCampaignById(lead.CampaignId).Name;
                }

                Affiliate affiliate = this._affiliateService.GetAffiliateById(lead.AffiliateId, true);
                if (affiliate != null)
                {
                    leadResponseModel.AffiliateName = affiliate.Name;
                }
                AffiliateChannel affiliateChannel = this._affiliateChannelService.GetAffiliateChannelById(lead.AffiliateChannelId);
                if (affiliateChannel != null)
                {
                    leadResponseModel.AffiliateChannelName = affiliateChannel.Name;
                }

                leadResponseModel.StatusValue = lead.Status.ToString();
                leadResponseModel.Status = GetStatusUiString(lead.Status);

                List<XmlNode> xmlNodes = new List<XmlNode>();

                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(lead.ReceivedData);
                    GetNodes(xmlDocument.DocumentElement, xmlNodes, lead.CampaignId);
                }
                catch(Exception)
                {
                }

                leadResponseModel.Nodes = xmlNodes;
                leadResponseModel.AllowedNodes = this._campaignTemplateService.CampaignTemplateAllowedNames(lead.CampaignId);
                //this._campaignTemplateService.IsCampaignTemplateHidden(lead.CampaignId)

                IList<LeadResponse> leadResponseList = this._leadMainResponseService.GetLeadMainResponseByLeadId(id);

                Hashtable distinct = new Hashtable();
                Hashtable postedFilter = new Hashtable();

                foreach (LeadResponse leadResponse in leadResponseList)
                {
                    if (distinct[leadResponse.Id] != null)
                    {
                        if (postedFilter[leadResponse.Posted] != null)
                            leadResponse.Posted = (distinct[leadResponse.Id] as LeadResponse).Posted;
                        postedFilter[leadResponse.Posted] = 1;
                    }
                    distinct[leadResponse.Id] = leadResponse;
                }
                leadResponseList.Clear();
                foreach (var key in distinct.Keys)
                {
                    var val = distinct[key] as LeadResponse;
                    leadResponseList.Add(val);
                }

                leadResponseList = leadResponseList.OrderBy(a => a.Id).ToList();

                leadResponseModel.LeadResponseList = leadResponseList;

                // Dublicates //

                IList<LeadContentDublicate> leadDuplicateList = this._leadContentDuplicateService.GetLeadContentDublicateBySSN(id, !string.IsNullOrEmpty(lead.Ssn) ? lead.Ssn : "");
                var isSelf = leadDuplicateList.Any(leadDuplicate => leadDuplicate.LeadId == id);

                if (!isSelf && leadDuplicateList != null && leadDuplicateList.Count > 0)
                {
                    LeadMainContent leadMainContent = this._leadMainService.GetLeadsAllById(id);
                    LeadContentDublicate leadContentDublicate = new LeadContentDublicate
                    {
                        LeadId = leadMainContent.LeadId,
                        Created = leadMainContent.Created.Value,
                        AffiliateId = leadMainContent.AffiliateId
                    };

                    Affiliate affiliateById = this._affiliateService.GetAffiliateById(leadMainContent.AffiliateId, true);
                    if (affiliateById != null)
                    {
                        leadContentDublicate.AffiliateName = affiliateById.Name;
                    }


                    leadContentDublicate.RequestedAmount = leadMainContent.RequestedAmount;
                    leadContentDublicate.NetMonthlyIncome = leadMainContent.NetMonthlyIncome;
                    leadContentDublicate.PayFrequency = leadMainContent.PayFrequency;
                    leadContentDublicate.Directdeposit = leadMainContent.Directdeposit;
                    leadContentDublicate.Email = leadMainContent.Email;
                    leadContentDublicate.HomePhone = leadMainContent.HomePhone;
                    leadContentDublicate.Ip = leadMainContent.Ip;

                    leadDuplicateList.Insert(0, leadContentDublicate);
                }

                leadResponseModel.LeadDuplicateList = leadDuplicateList;

                List<string> requestedAmountList = new List<string>();
                List<string> netMonthlyIncomeList = new List<string>();
                List<string> payFrequencyList = new List<string>();
                List<string> directDepositList = new List<string>();
                List<string> emailList = new List<string>();
                List<string> homePhoneList = new List<string>();
                List<string> ipList = new List<string>();

                if (lead.RequestedAmount != null)
                {
                    requestedAmountList.Add(lead.RequestedAmount.ToString());
                }

                if (lead.NetMonthlyIncome != null)
                {
                    netMonthlyIncomeList.Add(lead.NetMonthlyIncome.ToString());
                }

                if (lead.PayFrequency != null)
                {
                    payFrequencyList.Add(lead.PayFrequency.ToString());
                }

                if (lead.Directdeposit != null)
                {
                    directDepositList.Add(lead.Directdeposit.ToString());
                }

                if (!string.IsNullOrEmpty(lead.Email))
                {
                    emailList.Add(lead.Email.ToString());
                }

                if (!string.IsNullOrEmpty(lead.HomePhone))
                {
                    homePhoneList.Add(lead.HomePhone.ToString());
                }

                if (!string.IsNullOrEmpty(lead.Ip))
                {
                    ipList.Add(lead.Ip.ToString());
                }

                foreach (LeadContentDublicate ld in leadDuplicateList)
                {
                    if (ld.RequestedAmount != null && !requestedAmountList.Contains(ld.RequestedAmount.ToString()))
                    {
                        requestedAmountList.Add(ld.RequestedAmount.ToString());
                    }

                    if (ld.NetMonthlyIncome != null && !netMonthlyIncomeList.Contains(ld.NetMonthlyIncome.ToString()))
                    {
                        netMonthlyIncomeList.Add(ld.NetMonthlyIncome.ToString());
                    }

                    if (ld.PayFrequency != null && !payFrequencyList.Contains(ld.PayFrequency.ToString()))
                    {
                        payFrequencyList.Add(ld.PayFrequency.ToString());
                    }

                    if (ld.Directdeposit != null && !directDepositList.Contains(ld.Directdeposit.ToString()))
                    {
                        directDepositList.Add(ld.Directdeposit.ToString());
                    }

                    if (ld.Email != null && !emailList.Contains(ld.Email.ToString()))
                    {
                        emailList.Add(ld.Email.ToString());
                    }

                    if (ld.HomePhone != null && !homePhoneList.Contains(ld.HomePhone.ToString()))
                    {
                        homePhoneList.Add(ld.HomePhone.ToString());
                    }

                    if (ld.Ip != null && !ipList.Contains(ld.Ip.ToString()))
                    {
                        ipList.Add(ld.Ip.ToString());
                    }
                }

                leadResponseModel.RequestedAmountCount = requestedAmountList.Count() > 1 ? (requestedAmountList.Count() - 1).ToString() : "";
                leadResponseModel.NetMonthlyIncomeCount = netMonthlyIncomeList.Count() > 1 ? (netMonthlyIncomeList.Count() - 1).ToString() : "";
                leadResponseModel.PayFrequencyCount = payFrequencyList.Count() > 1 ? (payFrequencyList.Count() - 1).ToString() : "";
                leadResponseModel.DirectDepositCount = directDepositList.Count() > 1 ? (directDepositList.Count() - 1).ToString() : "";
                leadResponseModel.EmailCount = emailList.Count() > 1 ? (emailList.Count() - 1).ToString() : "";
                leadResponseModel.HomePhoneCount = homePhoneList.Count() > 1 ? (homePhoneList.Count() - 1).ToString() : "";
                leadResponseModel.IpCount = ipList.Count() > 1 ? (ipList.Count() - 1).ToString() : "";
            }

            leadResponseModel.GeoData = this._leadMainService.GetLeadGeoData(id);

            leadResponseModel.SensitiveData = this._leadSensitiveDataService.GetLeadSensitiveDataByLeadId(id);
            // Redirect

            leadResponseModel.RedirectUrl = null;
            RedirectUrl rUrl = this._redirectUrlService.GetRedirectUrlByLeadId(id);
            if (rUrl != null)
            {
                leadResponseModel.RedirectUrl = rUrl;
            }

            return leadResponseModel; 
        }

        #region reports

        /// <summary>
        /// Bads the ip clicks report.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[NavigationBreadCrumb(Clear = true, Label = "Bad IP Clicks Report")]
        [Route("reports/badIpClicks")]
        public LeadExtendedModel BadIPClicksReport()
        {
            var leadExtendedModel = CreateLeadExtendedModelInstance();
            return leadExtendedModel;
        }

        /// <summary>
        /// Gets the bad ip clicks report ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [Route("reports/badIpClicks")]
        [ContentManagementAntiForgery(true)]
        public ReportBadIPClicksPagedModel GetBadIPClicksReportAjax([FromBody]LeadFilterModel filterModel, int rowsPerPage = 0)
        {
            var clicksModels = new List<ReportBadIPClicksModel>();

            //var maskEmail = _appContext.AppUser?.MaskEmail ?? true;
            //var actionsJson = filterModel.Actions;

            var page = (filterModel.Page != null) ? int.Parse(filterModel.Page) - 1 : 0;
            var pageSize = (filterModel.PageSize != null)
                ? int.Parse(filterModel.PageSize)
                : (rowsPerPage > 0 ? rowsPerPage : RowsPerPage);

            GetDatesFromFilter(filterModel, out var dateFrom, out var dateTo);

            var leadIp = string.Empty;
            var clickIp = string.Empty;

            var filterLeadId = 0L;
            string filterAffiliateIds = string.Empty;

            if (!string.IsNullOrEmpty(filterModel.LeadId))
            {
                long.TryParse(filterModel.LeadId, out filterLeadId);
            }

            if (filterModel.LeadIP != null)
            {
                leadIp = filterModel.LeadIP;
            }
            if (filterModel.ClickIP != null)
            {
                clickIp = filterModel.ClickIP;
            }

            if (!string.IsNullOrEmpty(filterModel.AffiliateIds))
            {
                filterAffiliateIds = filterModel.AffiliateIds;
            }

            List<ReportBadIPClicks> badIpClicksList = this._leadMainService.GetBadIPClicksReport(dateFrom, dateTo, filterLeadId, filterAffiliateIds, leadIp, clickIp, page * pageSize, pageSize);

            var pagedModel = new ReportBadIPClicksPagedModel
            {
                RecordsStart = 0,
                RecordsTotal = badIpClicksList.Count, // 100,
                RecordsFiltered = 0,
                TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToString(DateFormatString),
            };

            foreach (ReportBadIPClicks badIpClicks in badIpClicksList)
            {
                Affiliate affiliate = this._affiliateService.GetAffiliateById(badIpClicks.AffiliateId, true);
                string affiliateName = affiliate?.Name ?? string.Empty;

                clicksModels.Add(new ReportBadIPClicksModel
                {
                    HasPermission = this._permissionService.Authorize(PermissionProvider.LeadsIinfoView) || _appContext.AppUser.UserType == SharedData.BuyerUserTypeId || _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId,
                    LeadId = badIpClicks.Created,
                    LeadIp = badIpClicks.LeadIp,
                    ClickIp = badIpClicks.ClickIp,
                    AffiliateId = badIpClicks.AffiliateId,
                    AffiliateName = affiliateName
                });
            }

            pagedModel.ReportBadIPClicks = clicksModels;
            return pagedModel;
        }

        /// <summary>
        /// Errors the leads report buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[NavigationBreadCrumb(Clear = true, Label = "Error Leads Report")]
        [Route("reports/buyer")]
        public LeadExtendedModel ErrorLeadsReportBuyer()
        {
            var leadExtendedModel = CreateLeadExtendedModelInstance();

            leadExtendedModel.AllAffiliatesList = _affiliateService.GetAllAffiliates(0);
            leadExtendedModel.AllBuyerChannelsList = _buyerChannelService.GetAllBuyerChannels(0);

            foreach (var stateProvince in _stateProvinceService.GetStateProvinceByCountryId(StateUsaCode))
            {
                leadExtendedModel.ListStates.Add(new SelectItem
                {
                    Text = stateProvince.GetLocalized(x => x.Name),
                    Value = stateProvince.Code
                });
            }

            return leadExtendedModel;
        }

        /// <summary>
        /// Errors the leads report affiliate.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[NavigationBreadCrumb(Clear = true, Label = "Error Leads Report")]
        [Route("reports/affiliate")]
        public LeadExtendedModel ErrorLeadsReportAffiliate()
        {
            var leadExtendedModel = CreateLeadExtendedModelInstance();

            leadExtendedModel.AllAffiliatesList = _affiliateService.GetAllAffiliates(0);
            leadExtendedModel.AllAffiliateChannelsList = _affiliateChannelService.GetAllAffiliateChannels(0);

            foreach (var stateProvince in _stateProvinceService.GetStateProvinceByCountryId(StateUsaCode))
            {
                leadExtendedModel.ListStates.Add(new SelectItem
                {
                    Text = stateProvince.GetLocalized(x => x.Name),
                    Value = stateProvince.Code
                });
            }

            return leadExtendedModel;
        }

        /// <summary>
        /// Validations the error leads.
        /// </summary>
        /// <returns>ActionResult.</returns>
        //[NavigationBreadCrumb(Clear = true, Label = "Validation Error Leads Report")]
        [Route("reports/validation")]
        public LeadExtendedModel ValidationErrorLeads()
        {
            var leadExtendedModel = CreateLeadExtendedModelInstance();

            return leadExtendedModel;
        }

        /// <summary>
        /// Gets the validation error leads ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("reports/validation")]
        [ContentManagementAntiForgery(true)]
        public AffiliateResponsePagedModel GetValidationErrorLeadsAjax([FromBody]LeadFilterModel filterModel)
        {
            DateTime dateFrom = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToShortDateString());
            long filterAffiliate = 0;
            long filterAffiliateChannel = 0;

            if (filterModel.Dates != null)
            {
                var dates = filterModel.Dates.Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (!string.IsNullOrEmpty(filterModel.AffiliateId) && long.TryParse(filterModel.AffiliateId, out filterAffiliate))
            {
                filterAffiliate = long.Parse(filterModel.AffiliateId);
            }

            if (!string.IsNullOrEmpty(filterModel.AffiliateChannelId) && long.TryParse(filterModel.AffiliateChannelId, out filterAffiliateChannel))
            {
                filterAffiliateChannel = long.Parse(filterModel.AffiliateChannelId);
            }

            IList<AffiliateResponse> affiliateResponseList = this._affiliateResponseService.GetAffiliateResponsesByFilters(filterAffiliate, filterAffiliateChannel, dateFrom, dateTo);

            var affiliateResponseModels = new List<AffiliateResponseModel>();
            
            var affiliateResponsePagedModel = new AffiliateResponsePagedModel
            {
                RecordsStart = 0,
                RecordsTotal = affiliateResponseList.Count,
                RecordsFiltered = 0,
                TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser)
                    .ToString(DateFormatString)
            };

            foreach (AffiliateResponse affiliateResponse in affiliateResponseList)
            {
                Affiliate affiliate = this._affiliateService.GetAffiliateById(affiliateResponse.AffiliateId, true);
                AffiliateChannel affiliateChannel = this._affiliateChannelService.GetAffiliateChannelById(affiliateResponse.AffiliateChannelId);

                affiliateResponseModels.Add(new AffiliateResponseModel
                {
                    AffiliateName = affiliate?.Name ?? string.Empty, 
                    AffiliateChannelName = affiliateChannel?.Name ?? string.Empty,
                    Id = affiliateResponse.Id,
                    Created = affiliateResponse.Created,
                    AffiliateId = affiliateResponse.AffiliateId,
                    AffiliateChannelId = affiliateResponse.AffiliateChannelId,
                    Response = WebUtility.HtmlEncode(affiliateResponse.Response),
                    MinPrice = affiliateResponse.MinPrice
                });
            }

            affiliateResponsePagedModel.AffiliateResponses = affiliateResponseModels;

            return affiliateResponsePagedModel; 
        }

        /// <summary>
        /// Gets the error leads report ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("reports/paged")]
        [ContentManagementAntiForgery(true)]
        public LeadsErrorReportPagedModel GetErrorLeadsReportAjax([FromBody]LeadFilterModel filterModel)
        {
            var leadsErrorReportModels = new List<LeadsErrorReportModel>();

            //bool maskEmail = _appContext.AppUser?.MaskEmail ?? true;

            int.TryParse(filterModel.Page, out var page);
            int.TryParse(filterModel.PageSize, out var pageSize);
            if (pageSize == 0) pageSize = RowsPerPage;

            long.TryParse(filterModel.LeadId, out var filterLeadId);
            short.TryParse(filterModel.Status, out var filterStatus);
            short.TryParse(filterModel.ErrorType, out var errorType);
            short.TryParse(filterModel.Validator, out var validator);
            short.TryParse(filterModel.ReportType, out var reportType);
            //long.TryParse(filterModel.AffiliateId, out var filterAffiliateId);
            long.TryParse(filterModel.AffiliateChannelId, out var filterAffiliateChannelId);
            //long.TryParse(filterModel.BuyerId, out var filterBuyerId);
            long.TryParse(filterModel.BuyerChannelId, out var filterBuyerChannelId);
            long.TryParse(filterModel.CampaignId, out var filterCampaignId);
            decimal.TryParse(filterModel.MinPrice, out var filterMinPrice);

            GetDatesFromFilter(filterModel, out var dateFrom, out var dateTo);

            var filter = new LeadFilter
            {
                AffiliateId = 0L,
                BuyerId = 0L,
                State = filterModel.State ?? string.Empty,
                LeadId = filterLeadId,
                Status = filterStatus,
                AffiliateChannelId = filterAffiliateChannelId,
                BuyerChannelId = filterBuyerChannelId,
                CampaignId = filterCampaignId,
                MinPrice = filterMinPrice,
                ErrorType = errorType,
                ReportType = reportType,
                Validator = validator
            };

            List<ReportErrorLeads> reportErrorLeadsList = this._leadMainService.GetErrorLeadsReport(
                filter.ErrorType, 
                filter.Validator, 
                dateFrom, 
                dateTo, 
                filter.LeadId, 
                filter.Status,
                filter.AffiliateId, 
                filter.AffiliateChannelId, 
                filter.BuyerId, 
                filter.BuyerChannelId, 
                filter.CampaignId,
                filter.State,
                filter.MinPrice,
                filter.ReportType, 
                page * pageSize, 
                pageSize);

            var leadsErrorReportPagedModel = new LeadsErrorReportPagedModel
            {
                RecordsStart = 0,
                RecordsTotal = this._leadMainService.GetErrorLeadsReportCount(filter.ErrorType, filter.Validator,
                    dateFrom, dateTo, filter.LeadId, filter.Status, filter.AffiliateId, filter.AffiliateChannelId, 
                    filter.BuyerId, filter.BuyerChannelId, filter.CampaignId, filter.State, filter.MinPrice, filter.ReportType),
                RecordsFiltered = 0,
                TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser)
                    .ToString(DateFormatString)
            };

            foreach (ReportErrorLeads reportErrorLeads in reportErrorLeadsList)
            {
                string errorTypeString = GetErrorTypeUiString(reportErrorLeads.ErrorType);

                //string affName = reportErrorLeads.AffiliateName;

                string response = reportErrorLeads.Message;

                try
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(reportErrorLeads.Response);
                    XmlNodeList nodeList = xmlDocument.DocumentElement.GetElementsByTagName("message");
                    if (nodeList.Count > 0)
                        response = nodeList[0].InnerText;
                }
                catch
                {
                    response = reportErrorLeads.Message;
                }

                leadsErrorReportModels.Add(new LeadsErrorReportModel
                {
                    HasPermission = this._permissionService.Authorize(PermissionProvider.LeadsIinfoView),
                    CreatedTimeZoneDate = _settingService.GetTimeZoneDate(reportErrorLeads.Created),
                    LeadId = reportErrorLeads.LeadId,
                    AffiliateChannelName = reportErrorLeads.AffiliateChannelName,
                    BuyerChannelName = reportErrorLeads.BuyerChannelName,
                    State = reportErrorLeads.State,
                    Response = response,
                    ErrorTypeString = errorTypeString,
                    ErrorType = errorType
                });
            }

            leadsErrorReportPagedModel.LeadsErrorReports = leadsErrorReportModels;

            return leadsErrorReportPagedModel;
        }

        #endregion report actions

        #region notes

        /// <summary>
        /// Gets the lead notes ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        [Route("{id}/notes")]
        [ContentManagementAntiForgery(true)]
        public IList<LeadNoteModel> GetLeadNotesAjax(string id)
        {
            var leadIdResult = long.Parse(id);
            
            var leadNotesList = this._noteTitleService.GetLeadNotes(leadIdResult);

            var leadNotes = (from leadNote in leadNotesList
                let noteTitle = this._noteTitleService.GetNoteTitleById(leadNote.NoteTitleId)
                select new LeadNoteModel
                {
                    Author = leadNote.Author ?? string.Empty,
                    Created = leadNote.Created.ToString(), 
                    Note = leadNote.Note, 
                    Title = noteTitle?.Title ?? MissingNoteTitle,
                }).ToList();

            return leadNotes;
        }

        /// <summary>
        /// Adds the lead note.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("{id}/notes/title/{titleId}/add")]
        [ContentManagementAntiForgery(true)]
        public void AddLeadNote([FromUri]string id, [FromUri]string titleId, [FromBody]LeadNoteModel note)
        {
            var leadNote = new LeadNote
            {
                LeadId = long.Parse(id),
                Note = note.Note,
                Author = note.Author,
                NoteTitleId = short.Parse(titleId),
                Created = DateTime.Now
            };

            this._noteTitleService.InsertLeadNote(leadNote);

            LeadMain leadMain = _leadMainService.GetLeadMainById(leadNote.LeadId);
            if (leadMain != null)
            {
                leadMain.UpdateDate = DateTime.UtcNow;
                _leadMainService.UpdateLeadMain(leadMain);
            }
        }

        /// <summary>
        /// Saves the notes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("notes")]
        [ContentManagementAntiForgery(true)]
        public void SaveNotes([FromBody]string note)
        {
            string[] notes = note.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in notes)
            {
                var items = item.Split(':');
                var noteTitle = this._noteTitleService.GetNoteTitleById(long.Parse(items[0]));
                noteTitle.Title = items[1];

                this._noteTitleService.UpdateNoteTitle(noteTitle);
            }
        }

        #endregion notes

        #region export to csv

        /// <summary>
        /// Generates the excel file ajax.
        /// </summary>
        /// <returns>System.String.</returns>
        //[ValidateInput(false)]
        [HttpPost]
        [Route("generate/xls")]
        [ContentManagementAntiForgery(true)]
        public string GenerateExcelFileAjax([FromBody]LeadFilterModel filterModel)
        {
            var maskEmail = _appContext.AppUser?.MaskEmail ?? true;

            var actionsJson = filterModel.Actions;

            var leadMainContents = GetLeadMainContents(filterModel, out var filter);

            var data = new List<string[]>();

            if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
            {
                string[] headers = { "ID", "Email", "Data", "IP", "State", "Affiliate", "Aff.Ch.Id", "Buyer", "Buyer Ch.Id", "Campaign Id", "Status", "Notes" };
                data.Add(headers);
            }
            else
            {
                string[] headers = { "ID", "Email", "Data", "IP", "State", "Campaign Id", "Status", "Notes" };
                data.Add(headers);
            }

            foreach (LeadMainContent leadMainContent in leadMainContents)
            {
                var affiliateName = string.Empty;
                var buyerName = string.Empty;
                var campaignName = string.Empty;
                var buyerChannelName = string.Empty;
                var affiliateChannelName = string.Empty;

                string statusUiString = GetStatusUiString(leadMainContent.Status);

                string monitorUiString = string.Empty;

                Affiliate affiliate = this._affiliateService.GetAffiliateById(leadMainContent.AffiliateId, true);
                if (affiliate != null)
                {
                    affiliateName = affiliate.Name;
                }

                if (leadMainContent.BuyerId != null)
                {
                    Buyer buyer = this._buyerService.GetBuyerById((long)leadMainContent.BuyerId);
                    if (buyer != null)
                    {
                        buyerName = buyer.Name;
                    }
                }
                if (leadMainContent.BuyerChannelId != 0)
                {
                    BuyerChannel buyerChannel = this._buyerChannelService.GetBuyerChannelById((long)leadMainContent.BuyerChannelId);
                    if (buyerChannel != null)
                    {
                        buyerChannelName = buyerChannel.Name;
                    }
                }

                if (leadMainContent.AffiliateChannelId != 0)
                {
                    AffiliateChannel affiliateChannel = this._affiliateChannelService.GetAffiliateChannelById(leadMainContent.AffiliateChannelId);
                    if (affiliateChannel != null)
                    {
                        affiliateChannelName = affiliateChannel.Name;
                    }
                }

                if (leadMainContent.CampaignId != 0)
                {
                    campaignName = this._campaignService.GetCampaignById(leadMainContent.CampaignId).Name;
                }

                string emailResult = string.Empty;
                if (leadMainContent.Email != null)
                {
                    emailResult = maskEmail ? Regex.Replace(leadMainContent.Email, @"(?<=[\w]{2})[\w-\._\+%]*(?=[\w]{1}@)", m => new string('*', m.Length)) : leadMainContent.Email;
                }

                var leadNoteString = string.Empty;

                LeadNote leadNote = this._noteTitleService.GetLeadNote(leadMainContent.LeadId);
                if (leadNote != null && leadNote.NoteTitleId != 0)
                {
                    NoteTitle nodeTitle = this._noteTitleService.GetNoteTitleById((long)leadNote.NoteTitleId);
                    leadNoteString = $"{nodeTitle.Title} {leadNote.Note}";
                }

                if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
                {
                    string[] names1 = {
                                      leadMainContent.Id.ToString(),
                                      emailResult,
                                      $"{leadMainContent.Created:MM/dd/yyyy HH:mm:ss}",
                                      leadMainContent.Ip,
                                      leadMainContent.State,
                                      affiliateName,
                                      affiliateChannelName,
                                      leadMainContent.Status==1 ? buyerName : string.Empty,
                                      leadMainContent.Status==1 ? buyerChannelName : string.Empty,
                                      campaignName,
                                      statusUiString,
                                      monitorUiString,
                                      leadNoteString
                                };
                    data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      leadMainContent.Id.ToString(),
                                      emailResult,
                                      $"{leadMainContent.Created:MM/dd/yyyy HH:mm:ss}",
                                      leadMainContent.Ip,
                                      leadMainContent.State,
                                      campaignName,
                                      statusUiString,
                                     _appContext.AppUser.UserType == SharedData.BuyerUserTypeId ? leadNoteString : string.Empty
                                };
                    data.Add(names1);
                }
            }

            string retPath = Export2CSV(data);

            return retPath;
        }

        /// <summary>
        /// Generates the CSV file ajax.
        /// </summary>
        /// <returns>System.String.</returns>
        //[ValidateInput(false)]
        [HttpPost]
        [Route("generate/csv")]
        [ContentManagementAntiForgery(true)]
        public string GenerateCSVFileAjax([FromBody]LeadFilterModel filterModel)
        {
            // Check Password
            User user = this._userService.GetUserById(_appContext.AppUser.Id);

            string passwordHash = _encryptionService.CreatePasswordHash(filterModel.Password, user.SaltKey);

            if (user.Password != passwordHash)
            {
                return "0";
            }

            return GenerateExcelFileAjax(filterModel);
        }

        #endregion export to csv

        #region columns filter

        /// <summary>
        /// Saves the columns visibility.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("columns/visibility")]
        [ContentManagementAntiForgery(true)]
        public void SaveColumnsVisibility([FromBody]string columns)
        {
            var cookieName = ColumnsCookieName;

            HttpCookie compareCookie = _httpContext.Request.Cookies.Get(cookieName);

            if (compareCookie == null)
            {
                compareCookie = new HttpCookie(cookieName)
                {
                    HttpOnly = true,
                    Expires = DateTime.MaxValue,
                    Value = columns
                };
                _httpContext.Response.Cookies.Add(compareCookie);
            }
            else
            {
                compareCookie.Value = columns;
                _httpContext.Response.Cookies.Set(compareCookie);
            }
        }

        #endregion columns filter

        #endregion route methods

        #region private methods

        ///// <summary>
        ///// Export2s the CSV.
        ///// </summary>
        ///// <param name="strList">The string list.</param>
        ///// <returns>System.String.</returns>
        //[Route("export/csv")] // TODO remove verb and make private
        private string Export2CSV([FromBody]List<string[]> strList)
        {
            var fileName = $"Leads_{_appContext.AppUser.Id.ToString()}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            var mapPath = HostingEnvironment.MapPath("~/Downloads") ?? string.Empty;
            var path = Path.Combine(mapPath, fileName);

            var fileBody = string.Empty;

            foreach (string[] ss in strList)
            {
                fileBody = ss.Aggregate(fileBody, (current, s) => $"{current}{s};");

                fileBody = $"{fileBody}\r\n";
            }

            File.WriteAllText(path, fileBody);

            return fileName;
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="xmlNodes">The list.</param>
        /// <param name="campaignId">The campaign identifier</param>
        private void GetNodes(XmlNode parent, List<XmlNode> xmlNodes, long campaignId)
        {
            bool b = true;

            XmlAttribute attr = null;

            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    b = false;
                    GetNodes(node, xmlNodes, campaignId);

                    if (node.Attributes["decrypted"] == null)
                    {
                        attr = parent.OwnerDocument.CreateAttribute("decrypted");
                        CampaignField ct = _campaignTemplateService.GetCampaignTemplateBySectionAndName("", node.Name, campaignId);

                        if (ct != null && ct.IsHash.HasValue && ct.IsHash.Value && ct.Validator != 5 && ct.Validator != 6 && ct.Validator != 12 && ct.TemplateField.ToLower() != "ssn" && ct.TemplateField.ToLower() != "dlnumber" && ct.TemplateField.ToLower() != "accountnumber")
                        {
                            attr.Value = Helper.Decrypt(node.InnerText);
                        }
                        node.Attributes.Append(attr);
                    }
                }
            }

            if (b)
            {
                attr = parent.OwnerDocument.CreateAttribute("decrypted");
                if (parent.ParentNode == null)
                {
                    if (!xmlNodes.Contains(parent))
                    {
                        CampaignField ct = _campaignTemplateService.GetCampaignTemplateBySectionAndName("", parent.Name, campaignId);
                        if (ct != null && ct.IsHash.HasValue && ct.IsHash.Value && ct.Validator != 5 && ct.Validator != 6 && ct.Validator != 12 && ct.TemplateField.ToLower() != "ssn" && ct.TemplateField.ToLower() != "dlnumber" && ct.TemplateField.ToLower() != "accountnumber")
                        {
                            attr.Value = Helper.Decrypt(parent.InnerText);
                        }
                        xmlNodes.Add(parent);
                        parent.Attributes.Append(attr);

                        /*BlackListType blackListType = _blackListService.GetBlackListType(parent.Name);
                        if (blackListType != null)
                        {
                        }*/
                    }
                }
                else
                {
                    if (!xmlNodes.Contains(parent.ParentNode))
                    {
                        CampaignField campaignTemplate = _campaignTemplateService
                            .GetCampaignTemplateBySectionAndName(string.Empty, parent.ParentNode.Name, campaignId);
                        if (campaignTemplate != null && campaignTemplate.IsHash.HasValue && campaignTemplate.IsHash.Value)
                        {
                            attr.Value = Helper.Decrypt(parent.ParentNode.InnerText);
                        }
                        xmlNodes.Add(parent.ParentNode);
                        parent.ParentNode.Attributes.Append(attr);
                    }
                }
            }
        }

        private LeadExtendedModel CreateLeadExtendedModelInstance(bool showNotes = false)
        {
            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);

            var leadExtendedModel = new LeadExtendedModel
            {
                TotalRowsCount = TotalRowsCount, // this._leadMainService.GetLeadsCount();
                RowsPerPage = RowsPerPage,
                TimeZoneNowStr = tzDateTime.ToString(DateFormatString),
                TimeZoneNow = tzDateTime,
                ShowNotes = showNotes
            };
            
            leadExtendedModel.PageCount = (int) Math.Ceiling((double) leadExtendedModel.TotalRowsCount / leadExtendedModel.RowsPerPage);

            return leadExtendedModel;
        }

        private void GetDatesFromFilter(LeadFilterModel filterModel, out DateTime dateFrom, out DateTime dateTo)
        {
            DateTime dateNow = this._settingService.GetTimeZoneDate(DateTime.Now, _appContext.AppUser);

            dateFrom = (filterModel.DateFrom != null && filterModel.DateFrom != "0")
                ? Convert.ToDateTime(filterModel.DateFrom)
                : Convert.ToDateTime(dateNow.ToShortDateString());

            dateTo = (filterModel.DateTo != null && filterModel.DateTo != "0")
                ? Convert.ToDateTime(filterModel.DateTo)
                : Convert.ToDateTime(dateNow.ToShortDateString());

            if (filterModel.Dates != null)
            {
                var dates = filterModel.Dates.Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);
        }

        private List<LeadMainContent> GetLeadMainContents(LeadFilterModel filterModel, out LeadFilter filter)
        {
            filter = new LeadFilter
            {
                LeadId = 0L,
                Email = string.Empty,
                AffiliateChannelId = 0L,
                AffiliateChannelSubId = string.Empty,
                BuyerChannelId = 0L,
                CampaignId = 0L,
                FirstName = string.Empty,
                LastName = string.Empty,
                ZipCode = string.Empty,
                BuyerPrice = 0M,
                Status = -1,
                State = string.Empty,
                IP = string.Empty,
                Page = (filterModel.Page != null) ? int.Parse(filterModel.Page) - 1 : 0,
                PageSize = (filterModel.PageSize != null) ? int.Parse(filterModel.PageSize) : 50
            };

            // _leadModel.RowsPerPage > 0 ? _leadModel.RowsPerPage : RowsPerPage;
            GetDatesFromFilter(filterModel, out var dateFrom, out var dateTo);

            if (!string.IsNullOrEmpty(filterModel.LeadId) && long.TryParse(filterModel.LeadId, out var filterLeadId))
            {
                filter.LeadId = filterLeadId;
            }

            if (filterModel.Status != null)
            {
                if (!short.TryParse(filterModel.Status, out var status))
                {
                    status = 1;
                }

                filter.Status = status;
            }

            if (filterModel.State != null)
            {
                filter.State = filterModel.State;
            }

            if (filterModel.IP != null)
            {
                filter.IP = filterModel.IP;
            }

            if (filterModel.Email != null)
            {
                filter.Email = filterModel.Email;
            }
            if (long.TryParse(filterModel.AffiliateId, out var filterAffiliateId))
            {
                filter.AffiliateId = filterAffiliateId; ////filterAffiliate = long.Parse(filterModel.AffiliateId);
            }
            if (!string.IsNullOrEmpty(filterModel.AffiliateChannelId)
                && long.TryParse(filterModel.AffiliateChannelId, out var filterAffiliateChannelId))
            {
                filter.AffiliateChannelId = filterAffiliateChannelId;
            }
            if (!string.IsNullOrEmpty(filterModel.AffiliateChannelSubId))
            {
                filter.AffiliateChannelSubId = filterModel.AffiliateChannelSubId;
            }
            if (long.TryParse(filterModel.BuyerId, out var filterBuyerId))
            {
                filter.BuyerId = filterBuyerId; ////filterBuyer = long.Parse(filterModel.BuyerId);
            }
            if (!string.IsNullOrEmpty(filterModel.BuyerChannelId)
                && long.TryParse(filterModel.BuyerChannelId, out var filterBuyerChannelId))
            {
                filter.BuyerChannelId = filterBuyerChannelId; ////filterBuyerChannel = long.Parse(filterModel.BuyerChannelId);
            }
            if (!string.IsNullOrEmpty(filterModel.CampaignId) && long.TryParse(filterModel.CampaignId, out var filterCampaignId))
            {
                filter.CampaignId = filterCampaignId; ////filterCampaign = long.Parse(filterModel.CampaignId);
            }

            if (filterModel.FirstName != null)
            {
                filter.FirstName = filterModel.FirstName;
            }
            if (filterModel.LastName != null)
            {
                filter.LastName = filterModel.LastName;
            }
            if (filterModel.ZipCode != null)
            {
                filter.ZipCode = filterModel.ZipCode;
            }

            if (!string.IsNullOrEmpty(filterModel.BuyerPrice) && decimal.TryParse(filterModel.BuyerPrice, out var filterBuyerPrice))
            {
                filter.BuyerPrice = filterBuyerPrice; ////filterBPrice = decimal.Parse(filterModel.BuyerPrice);
            }

            filter.Notes = filterModel.Notes ?? string.Empty;

            var leadMainContents = (List<LeadMainContent>)this._leadMainService.GetLeadsAll(
                dateFrom,
                dateTo,
                filter.LeadId,
                filter.Email,
                filter.AffiliateId,
                filter.AffiliateChannelId,
                filter.AffiliateChannelSubId,
                filter.BuyerId,
                filter.BuyerChannelId,
                filter.CampaignId,
                filter.Status,
                filter.IP,
                filter.State,
                filter.FirstName,
                filter.LastName,
                filter.BuyerPrice,
                filter.ZipCode,
                filter.Notes,
                filter.Page * filter.PageSize, 
                filter.PageSize);

            return leadMainContents;
        }

        private LeadExtendedModel GetLeadExtendedInfo(long id, string type, bool allBuyerChannels = true, bool allCampaigns = true)
        {
            var leadExtendedModel = CreateLeadExtendedModelInstance(true);

            if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                type = "b";
                id = _appContext.AppUser.ParentId;

                var buyer = _buyerService.GetBuyerById(id);
                if (buyer != null && buyer.AlwaysSoldOption == 0)
                    leadExtendedModel.ShowNotes = false;
            }

            if (type == "a")
            {
                leadExtendedModel.AffiliateId = id;
                leadExtendedModel.BuyerId = -1L;
                Affiliate affiliate = _affiliateService.GetAffiliateById(id, true);
                if (affiliate != null)
                    leadExtendedModel.AffiliateName = affiliate.Name;
            }
            else if (type == "b")
            {
                leadExtendedModel.BuyerId = id;
                leadExtendedModel.SelectedBuyerId = id;
                leadExtendedModel.AffiliateId = -1L;
                Buyer buyer1 = _buyerService.GetBuyerById(id, true);
                if (buyer1 != null)
                    leadExtendedModel.BuyerName = buyer1.Name;
            }

            leadExtendedModel.Type = type;
            leadExtendedModel.AllLeadNotes = this._noteTitleService.GetAllNoteTitles();

            leadExtendedModel.AllAffiliatesList = this._affiliateService.GetAllAffiliates(0);
            leadExtendedModel.AllAffiliateChannelsList = this._affiliateChannelService.GetAllAffiliateChannels(0);

            leadExtendedModel.AllBuyersList = this._buyerService.GetAllBuyers(0);
            
            leadExtendedModel.AllBuyerChannelsList = allBuyerChannels
                ? this._buyerChannelService.GetAllBuyerChannels(0)
                : GetBuyerChannelList(leadExtendedModel.BuyerId, _appContext.AppUser);
            
            leadExtendedModel.AllCampaignsList = allCampaigns 
                ? this._campaignService.GetAllCampaigns(0)
                : GetCampaignsByGivenBuyerChannelList(leadExtendedModel.AllBuyerChannelsList);

            HttpCookie compareCookie = this._httpContext.Request.Cookies.Get(ColumnsCookieName);
            if (compareCookie != null)
            {
                leadExtendedModel.VisibleColumns = compareCookie.Value;
            }

            return leadExtendedModel;
        }

        private List<Campaign> GetCampaignsByGivenBuyerChannelList(IList<BuyerChannel> buyerChannels)
        {
            var campaignList = new List<Campaign>();

            foreach (var buyerChannel in buyerChannels)
            {
                var campaign = this._campaignService.GetCampaignById(buyerChannel.CampaignId);
                if (!campaignList.Contains(campaign))
                {
                    campaignList.Add(campaign);
                }
            }

            return campaignList;
        }

        /// <summary>
        /// Gets the buyer channel list.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="user">The user.</param>
        /// <returns>List&lt;BuyerChannel&gt;.</returns>
        [NonAction]
        //[Route("buyer/{buyerId}/channels")]
        private List<BuyerChannel> GetBuyerChannelList(long buyerId, [FromBody]User user)
        {
            var buyerChannels = (List<BuyerChannel>)this._buyerChannelService.GetAllBuyerChannelsByBuyerId(buyerId, 0);

            var userBuyerChannels = (List<UserBuyerChannel>)_userService.GetUserBuyerChannels(user.Id);

            return buyerChannels
                .Where(bc => userBuyerChannels.Count <= 0 || userBuyerChannels
                                 .FirstOrDefault(ubc => ubc.BuyerChannelId == bc.Id) != null)
                .ToList();
        }

        private bool GetAllowAffiliateRedirect()
        {
            Setting allowAffiliateRedirectSetting = _settingService.GetSetting("System.AllowAffiliateRedirect");
            bool allowAffiliateRedirect = true;
            if (allowAffiliateRedirectSetting != null)
            {
                short.TryParse(allowAffiliateRedirectSetting.Value, out var allowAffiliateRedirectValue);
                allowAffiliateRedirect = allowAffiliateRedirectValue == 1;
            }

            return allowAffiliateRedirect;
        }

        private string GetLeadNoteUiString(long id, long leadId)
        {
            string leadNoteString = $"<a nohref='nohref' id='l{id}' class='addnotebtn' data-titleid='0'>Pending Contact</a>";

            LeadNote leadNote = this._noteTitleService.GetLeadNote(leadId);
            if (leadNote != null && leadNote.NoteTitleId != 0)
            {
                var noteTitle = this._noteTitleService.GetNoteTitleById((long)leadNote.NoteTitleId);
                leadNoteString = $" <a nohref='nohref' id='l{id}' class='addnotebtn' data-titleid='{leadNote.NoteTitleId}'>{noteTitle.Title}</a>";
            }

            return leadNoteString;
        }
        
        private string GetRedirectUiString(long id, bool isClicked)
        {
            return isClicked
                ? $" <a class='idhref redirect-ico' title='Redirect' id='{id}' nohref='nohref'> <i class='glyphicon glyphicon-share-alt position-left'></i></a>"
                : " <span style='color:#F44336'> <i class='glyphicon glyphicon-share-alt position-left'></i></span>";
        }
        private string GetErrorTypeUiString(short? errorType)
        {
            string errorTypeString = "None";

            if (errorType.HasValue)
            {
                switch (errorType.Value)
                {
                    case 1: errorTypeString = "Unknown"; break;
                    case 2: errorTypeString = "No Data"; break;
                    case 3: errorTypeString = "Invalid Data"; break;
                    case 4: errorTypeString = "Unknown DB Field"; break;
                    case 5: errorTypeString = "Missing Value"; break;
                    case 6: errorTypeString = "Missing Field"; break;
                    case 7: errorTypeString = "NotExisting DB Record"; break;
                    case 8: errorTypeString = "Dropped"; break;
                    case 9: errorTypeString = "Daily Cap Reached"; break;
                    case 10: errorTypeString = "Integration Error"; break;
                    case 11: errorTypeString = "Filter Error"; break;
                    case 12: errorTypeString = "Not Enough Balance"; break;
                    case 13: errorTypeString = "Schedule Cap Limit"; break;
                    case 14: errorTypeString = "Min Price Error"; break;
                }
            }

            return errorTypeString;
        }

        private string GetStatusUiString(short status)
        {
            string statusName = string.Empty;
            switch (status)
            {
                case 1: { statusName = "Sold"; break; }
                case 2: { statusName = "Error"; break; }
                case 3: { statusName = "Reject"; break; }
                case 4:
                case 7: { statusName = "Processing"; break; }
                case 5: { statusName = "Filter"; break; }
                case 6: { statusName = "Min Price"; break; }
                case 0: { statusName = "Test"; break; }
            }
            return statusName;
        }
        private string GetMonitorUiString(long id, int warning)
        {
            var monitor = string.Empty;
            var warningColor = string.Empty;
            if (warning > 0)
            {
                if (warning == 1) warningColor = "#F09922";
                if (warning == 2) warningColor = "#f44336";
                monitor += $"<a nohref='nohref' title='Dublicate Monitor' id='{id}' class='idhref monitor-ico'><span class='badge' style='background-color: {warningColor}'>D</span></a>";
            }

            return monitor;
        }
        #endregion private methods

        #endregion methods
    }
}
