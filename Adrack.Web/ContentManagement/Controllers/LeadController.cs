// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure;
using Adrack.Data;
using Adrack.Service.Accounting;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.Web.ContentManagement.Models.Lead;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using Adrack.Service.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Collections;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class LeadController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    public partial class LeadController : BaseContentManagementController
    {
        #region Fields

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly ILeadMainService _leadMainService;

        /// <summary>
        /// The accounting service
        /// </summary>
        private readonly IAccountingService _accountingService;

        /// <summary>
        /// The lead main respones service
        /// </summary>
        private readonly ILeadMainResponseService _leadMainResponesService;

        /// <summary>
        /// The affiliate response service
        /// </summary>
        private readonly IAffiliateResponseService _affiliateResponseService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The lead content dublicate service
        /// </summary>
        private readonly ILeadContentDublicateService _leadContentDublicateService;

        /// <summary>
        /// The redirect URL service
        /// </summary>
        private readonly IRedirectUrlService _redirectUrlService;

        /// <summary>
        /// The note title service
        /// </summary>
        private readonly INoteTitleService _noteTitleService;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// The lead model
        /// </summary>
        public LeadModel _leadModel;

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// The encryption service
        /// </summary>
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        /// The campaign template service
        /// </summary>
        private readonly ICampaignTemplateService _campaignTemplateService;

        /// <summary>
        /// The black list service
        /// </summary>
        private readonly IBlackListService _blackListService;

        /// <summary>
        /// The lead sensitive data service
        /// </summary>
        private readonly ILeadSensitiveDataService _leadSensitiveDataService;

        /// <summary>
        /// The HTTP context
        /// </summary>
        private readonly HttpContextBase _httpContext;


        /// <summary>
        /// The state province service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="leadMainService">The lead main service.</param>
        /// <param name="accountingService">The accounting service.</param>
        /// <param name="leadMainResponesService">The lead main respones service.</param>
        /// <param name="affiliateResponseService">The affiliate response service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="campaignService">The campaign service.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="leadContentDublicateService">The lead content dublicate service.</param>
        /// <param name="appContext">Application Context</param>
        /// <param name="redirectUrlService">The redirect URL service.</param>
        /// <param name="noteTitleService">The note title service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="encryptionService">The encryption service.</param>
        /// <param name="campaignTemplateService">The campaign template service.</param>
        /// <param name="leadSensitiveDataService">The lead sensitive data service.</param>
        /// <param name="blackListService">The black list service.</param>
        /// <param name="httpContext">The HTTP context.</param>

        public LeadController(
                            ILeadMainService leadMainService,
                            IAccountingService accountingService,
                            ILeadMainResponseService leadMainResponesService,
                            IAffiliateResponseService affiliateResponseService,
                            ISettingService settingService,
                            ICampaignService campaignService,
                            IAffiliateService affiliateService,
                            IAffiliateChannelService affiliateChannelService,
                            IBuyerService buyerService,
                            IBuyerChannelService buyerChannelService,
                            ILeadContentDublicateService leadContentDublicateService,
                            IAppContext appContext,
                            IRedirectUrlService redirectUrlService,
                            INoteTitleService noteTitleService,
                            IUserService userService,
                            IEncryptionService encryptionService,
                            ICampaignTemplateService campaignTemplateService,
                            ILeadSensitiveDataService leadSensitiveDataService,
                            IBlackListService blackListService,
                            IStateProvinceService stateProvinceService,
                            HttpContextBase httpContext
                            )
        {
            this._leadMainService = leadMainService;
            this._accountingService = accountingService;
            this._leadMainResponesService = leadMainResponesService;
            this._affiliateResponseService = affiliateResponseService;
            this._leadModel = new LeadModel();
            this._settingService = settingService;
            this._campaignService = campaignService;
            this._affiliateService = affiliateService;
            this._affiliateChannelService = affiliateChannelService;
            this._buyerService = buyerService;
            this._buyerChannelService = buyerChannelService;
            this._leadContentDublicateService = leadContentDublicateService;
            this._appContext = appContext;
            this._redirectUrlService = redirectUrlService;
            this._noteTitleService = noteTitleService;
            this._userService = userService;
            this._encryptionService = encryptionService;
            this._campaignTemplateService = campaignTemplateService;
            this._leadSensitiveDataService = leadSensitiveDataService;
            this._blackListService = blackListService;
            this._httpContext = httpContext;
            this._stateProvinceService = stateProvinceService;
        }

        #endregion Constructor

        /// <summary>
        /// Gets the buyer channel list.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="user">The user.</param>
        /// <returns>List&lt;BuyerChannel&gt;.</returns>
        [NonAction]
        public List<BuyerChannel> GetBuyerChannelList(long buyerId, User user)
        {
            List<BuyerChannel> list = (List<BuyerChannel>)this._buyerChannelService.GetAllBuyerChannelsByBuyerId(buyerId, 0).OrderBy(x => x.Name).ToList();
            List<BuyerChannel> buyerChannelList = new List<BuyerChannel>();

            List<UserBuyerChannel> userBuyerChannels = (List<UserBuyerChannel>)_userService.GetUserBuyerChannels(user.Id);

            foreach (BuyerChannel bc in list)
            {
                if (userBuyerChannels.Count > 0 && userBuyerChannels.Where(x => x.BuyerChannelId == bc.Id).FirstOrDefault() == null)
                {
                    continue;
                }

                buyerChannelList.Add(bc);
            }

            return buyerChannelList;
        }

        // GET: Leads List

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Logs / Leads")]
        public ActionResult Index()
        {
            this._leadModel.TotalRowsCount = 1000; //  this._leadMainService.GetLeadsCount();
            this._leadModel.RowsPerPage = 100;

            this._leadModel.PageCount = (int)Math.Ceiling((double)this._leadModel.TotalRowsCount / this._leadModel.RowsPerPage);

            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);
            this._leadModel.TimeZoneNowStr = tzDateTime.ToString("MM/dd/yyyy");
            this._leadModel.TimeZoneNow = tzDateTime;

            ViewBag.AllLeadNotes = this._noteTitleService.GetAllNoteTitles();

            ViewBag.AllAffiliatesList = this._affiliateService.GetAllAffiliates(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllBuyersList = this._buyerService.GetAllBuyers(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllBuyerChannelsList = this._buyerChannelService.GetAllBuyerChannels(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllAffiliateChannelsList = this._affiliateChannelService.GetAllAffiliateChannels(0).OrderBy(x => x.Name).ToList();

            Uri uri = new Uri(Helper.GetBaseUrl(Request));
            HttpCookie compareCookie = _httpContext.Request.Cookies.Get("AdrackColumnVisibility-" + uri.Host + "-" + _appContext.AppUser.Id.ToString());
            if (compareCookie != null)
            {
                ViewBag.VisibleColumns = compareCookie.Value;
            }

            Buyer buyer = null;

            this._leadModel.ShowNotes = true;

            return View("IndexPartial", this._leadModel);
        }

        /// <summary>
        /// Affiliates the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Affiliate(long Id)
        {
            return IndexPartial(Id, "a");
        }

        /// <summary>
        /// Buyers the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Buyer(long Id)
        {
            return IndexPartial(Id, "b");
        }

        // GET: Leads List
        /// <summary>
        /// Indexes the partial.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult IndexPartial(long Id, string type)
        {
            this._leadModel.ShowNotes = true;

            Buyer buyer = null;

            this._leadModel.TotalRowsCount = 1000; //  this._leadMainService.GetLeadsCount();
            this._leadModel.RowsPerPage = 100;

            this._leadModel.PageCount = (int)Math.Ceiling((double)this._leadModel.TotalRowsCount / this._leadModel.RowsPerPage);

            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);
            this._leadModel.TimeZoneNowStr = tzDateTime.ToString("MM/dd/yyyy");
            this._leadModel.TimeZoneNow = tzDateTime; ;

            if (type == null && Request["type"] != null)
            {
                type = Request["type"];
            }

            if (type == "a")
            {
                ViewBag.AffiliateId = Id;
                ViewBag.BuyerId = -1;
                Affiliate affiliate = _affiliateService.GetAffiliateById(Id, true);
                if (affiliate != null)
                    ViewBag.AffiliateName = affiliate.Name;
            }
            else if (type == "b")
            {
                ViewBag.BuyerId = Id;
                ViewBag.SelectedBuyerId = Id;
                ViewBag.AffiliateId = -1;
                Buyer buyer1 = _buyerService.GetBuyerById(Id, true);
                if (buyer1 != null)
                    ViewBag.BuyerName = buyer1.Name;
            }

            ViewBag.Type = type;
            ViewBag.AllLeadNotes = this._noteTitleService.GetAllNoteTitles();

            ViewBag.AllAffiliatesList = this._affiliateService.GetAllAffiliates(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllBuyersList = this._buyerService.GetAllBuyers(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllBuyerChannelsList = GetBuyerChannelList(ViewBag.BuyerId, _appContext.AppUser);
            ViewBag.AllAffiliateChannelsList = this._affiliateChannelService.GetAllAffiliateChannels(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllCampaignsList = this._campaignService.GetAllCampaigns(0);

            Uri uri = new Uri(Helper.GetBaseUrl(Request));
            HttpCookie compareCookie = _httpContext.Request.Cookies.Get("AdrackColumnVisibility-" + uri.Host + "-" + _appContext.AppUser.Id.ToString());
            if (compareCookie != null)
            {
                ViewBag.VisibleColumns = compareCookie.Value;
            }

            return PartialView("IndexPartial", this._leadModel);
        }

        /// <summary>
        /// Indexes the partial aff.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult IndexPartialAff(long Id, string type)
        {
            this._leadModel.ShowNotes = true;

            Buyer buyer = null;

            this._leadModel.TotalRowsCount = 1000; //  this._leadMainService.GetLeadsCount();
            this._leadModel.RowsPerPage = 100;

            this._leadModel.PageCount = (int)Math.Ceiling((double)this._leadModel.TotalRowsCount / this._leadModel.RowsPerPage);

            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);
            this._leadModel.TimeZoneNowStr = tzDateTime.ToString("MM/dd/yyyy");
            this._leadModel.TimeZoneNow = tzDateTime;

            if (type == null && Request["type"] != null)
            {
                type = Request["type"];
            }

            if (type == "a")
            {
                ViewBag.AffiliateId = Id;
                ViewBag.BuyerId = -1;
            }
            else if (type == "b")
            {
                ViewBag.BuyerId = Id;
                ViewBag.SelectedBuyerId = Id;
                ViewBag.AffiliateId = -1;
            }

            ViewBag.AllLeadNotes = this._noteTitleService.GetAllNoteTitles();

            ViewBag.AllAffiliatesList = this._affiliateService.GetAllAffiliates(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllBuyersList = this._buyerService.GetAllBuyers(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllBuyerChannelsList = this._buyerChannelService.GetAllBuyerChannels(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllAffiliateChannelsList = this._affiliateChannelService.GetAllAffiliateChannels(0).OrderBy(x => x.Name).ToList();
            ViewBag.AllCampaignsList = this._campaignService.GetAllCampaigns(0).OrderBy(x => x.Name).ToList();

            Uri uri = new Uri(Helper.GetBaseUrl(Request));
            HttpCookie compareCookie = _httpContext.Request.Cookies.Get("AdrackColumnVisibility-" + uri.Host + "-" + _appContext.AppUser.Id.ToString());
            if (compareCookie != null)
            {
                ViewBag.VisibleColumns = compareCookie.Value;
            }

            return PartialView(this._leadModel);
        }

        public ActionResult IndexPartialReport()
        {
            return PartialView();
        }

        /// <summary>
        /// Gets the leads ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetLeadsAjax()
        {
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            bool maskEmail = _appContext.AppUser == null ? true : _appContext.AppUser.MaskEmail;

            bool report = false;
            bool.TryParse(Request["report"], out report);

            var actionsJson = Request["actions"];
            int page = 0;
            int pageSize = 50; // _leadModel.RowsPerPage > 0 ? _leadModel.RowsPerPage : 100;

            DateTime userNow = this._settingService.GetTimeZoneDate(DateTime.Now, _appContext.AppUser);

            DateTime dateFrom = Convert.ToDateTime(userNow.ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(userNow.ToShortDateString());

            short Status = -1;
            string State = "";
            string IP = "";

            long FilterLeadId = 0;
            string FilterEmail = "";
            string FilterAffiliate = Request["affiliate"]; ;
            string FilterAffiliateChannel = Request["affiliatechannel"];
            string FilterAffiliateChannelSubId = "";
            string FilterBuyer = Request["buyer"];
            string FilterBuyerChannel = Request["buyerchannel"];
            string FilterCampaign = Request["campaign"];

            string FilterFirstName = "";
            string FilterLastName = "";
            string FilterZipCode = "";
            decimal FilterBPrice = 0;

            if (Request["page"] != null)
            {
                page = int.Parse(Request["page"]) - 1;
            }

            if (Request["pagesize"] != null)
            {
                pageSize = int.Parse(Request["pagesize"]);
            }

            if (Request["leadid"] != null && Request["leadid"] != "" && long.TryParse(Request["leadid"], out FilterLeadId))
            {
                FilterLeadId = long.Parse(Request["leadid"]);
            }

            if (Request["datefrom"] != null && Request["datefrom"] != "0")
            {
                dateFrom = Convert.ToDateTime(Request["datefrom"]);
            }
            if (Request["dateto"] != null && Request["dateto"] != "0")
            {
                dateTo = Convert.ToDateTime(Request["dateto"]);
            }

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (Request["status"] != null)
            {
                if (!short.TryParse(Request["status"], out Status))
                {
                    Status = 1;
                }
            }

            if (Request["state"] != null)
            {
                State = Request["state"];
            }

            if (Request["ip"] != null)
            {
                IP = Request["ip"];
            }

            if (Request["email"] != null)
            {
                FilterEmail = Request["email"];
            }
            if (string.IsNullOrEmpty(Request["affiliate"]))
            {
                FilterAffiliate = "";
            }
            if (string.IsNullOrEmpty(Request["affiliatechannel"]))
            {
                FilterAffiliateChannel = "";
            }

            if (Request["affiliatesubid"] != null && Request["affiliatesubid"] != "")
            {
                FilterAffiliateChannelSubId = Request["affiliatesubid"];
            }
            if (string.IsNullOrEmpty(Request["buyer"]))
            {
                FilterBuyer = "";
            }
            if (string.IsNullOrEmpty(Request["buyerchannel"]))
            {
                FilterBuyerChannel = "";
            }
            if (string.IsNullOrEmpty(Request["campaign"]))
            {
                FilterCampaign = "";
            }

            if (Request["firstname"] != null)
            {
                FilterFirstName = Request["firstname"];
            }
            if (Request["lastname"] != null)
            {
                FilterLastName = Request["lastname"];
            }
            if (Request["zipcode"] != null)
            {
                FilterZipCode = Request["zipcode"];
            }

            if (Request["bprice"] != null && Request["bprice"] != "" && decimal.TryParse(Request["bprice"], out FilterBPrice))
            {
                FilterBPrice = decimal.Parse(Request["bprice"]);
            }

            Setting allowAffiliateRedirectSetting = _settingService.GetSetting("System.AllowAffiliateRedirect");
            bool allowAffiliateRedirect = true;
            if (allowAffiliateRedirectSetting != null)
            {
                short dm = 1;
                short.TryParse(allowAffiliateRedirectSetting.Value, out dm);
                allowAffiliateRedirect = (dm == 1 ? true : false);
            }

            string Notes = Request["Notes"];

            List<LeadMainContent> leadMainContent = (List<LeadMainContent>)this._leadMainService.GetLeadsAll(
                dateFrom,
                dateTo,
                FilterLeadId,
                FilterEmail,
                FilterAffiliate,
                FilterAffiliateChannel,
                FilterAffiliateChannelSubId,
                FilterBuyer,
                FilterBuyerChannel,
                FilterCampaign,
                Status,
                IP,
                State,
                FilterFirstName,
                FilterLastName,
                FilterBPrice,
                FilterZipCode,
                (string.IsNullOrEmpty(Notes) ? "" : Notes),
                page * pageSize, pageSize);


            JsonData jd = new JsonData();
            jd.draw = page;
            //jd.recordsTotal = (FilterLeadId == 0 ? (int)this._leadMainService.GetLeadsCount(dateFrom, dateTo, FilterEmail, FilterAffiliate, FilterAffiliateChannel, FilterBuyer, FilterBuyerChannel, FilterCampaign, Status, IP, State, (string.IsNullOrEmpty(Notes) ? "" : Notes)) : 1);
            jd.recordsFiltered = jd.recordsTotal;
            jd.TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToString("MM/dd/yyyy");

            string affNmae = "";
            string buyerName = "";
            string campaignName = "";
            string buyerChannelName = "";
            string affiliateChannelName = "";

            foreach (LeadMainContent lm in leadMainContent)
            {
                affNmae = "";
                buyerName = "";
                campaignName = "";
                buyerChannelName = "";
                affiliateChannelName = "";

                string StatusStr = "";
                switch (lm.Status)
                {
                    case 1: { StatusStr = "<div class='status-label label label-success'> Sold </div>"; break; }
                    case 2: { StatusStr = "<div class='status-label label label-error'> Error </div>"; break; }
                    case 3: { StatusStr = "<div class='status-label label label-danger'> Reject </div>"; break; }
                    case 4: { StatusStr = "<div class='status-label label label-info'> Processing </div>"; break; }
                    case 5: { StatusStr = "<div class='status-label label label-warning'> Filter </div>"; break; }
                    case 6: { StatusStr = "<div class='status-label label label-danger'> Min Price </div>"; break; }
                    case 7: { StatusStr = "<div class='status-label label label-info'> Processing </div>"; break; }
                    case 0: { StatusStr = "<div class='status-label label label-default'> Test </div>"; break; }
                }

                string monitor = "";
                string redirect = "";
                string WarningColor = "";
                if (lm.Warning > 0)
                {
                    if (lm.Warning == 1) WarningColor = "#F09922";
                    if (lm.Warning == 2) WarningColor = "#f44336";
                    monitor += "<a nohref='nohref' title='Dublicate Monitor' id='" + lm.Id.ToString() + "' class='idhref monitor-ico'><span class='badge' style='background-color: " + WarningColor + "'>D</span></a>";
                }

                if (lm.Status == 1 && allowAffiliateRedirect)
                {
                    if (lm.Clicked != null && lm.Clicked == true)
                    {
                        redirect = " <a class='idhref redirect-ico' title='Redirect' id='" + lm.Id.ToString() + "' nohref='nohref'>" + " <i class='glyphicon glyphicon-share-alt position-left'></i></a>";
                    }
                    else
                    {
                        redirect = " <span style='color:#F44336'>" + " <i class='glyphicon glyphicon-share-alt position-left'></i></span>";
                    }
                }

                Affiliate aff = this._affiliateService.GetAffiliateById(lm.AffiliateId, true);
                if (aff != null)
                {
                    affNmae = aff.Name;
                }

                if (lm.BuyerId != null)
                {
                    Buyer buyer = this._buyerService.GetBuyerById((long)lm.BuyerId);
                    if (buyer != null)
                    {
                        buyerName = buyer.Name;
                    }
                }
                if (lm.BuyerChannelId != null)
                {
                    BuyerChannel bch = this._buyerChannelService.GetBuyerChannelById((long)lm.BuyerChannelId);
                    if (bch != null)
                    {
                        buyerChannelName = bch.Name;
                    }
                }

                if (lm.AffiliateChannelId != 0)
                {
                    AffiliateChannel ach = this._affiliateChannelService.GetAffiliateChannelById(lm.AffiliateChannelId);
                    if (ach != null)
                    {
                        affiliateChannelName = ach.Name;
                    }
                }

                if (lm.CampaignId != 0)
                {
                    campaignName = this._campaignService.GetCampaignById(lm.CampaignId).Name;
                }

                string EmailResult = "";
                if (lm.Email != null)
                {
                    if (maskEmail)
                        EmailResult = Regex.Replace(lm.Email, @"(?<=[\w]{2})[\w-\._\+%]*(?=[\w]{1}@)", m => new string('*', m.Length));
                    else
                        EmailResult = lm.Email;
                }

                string LeadNote = "<a nohref='nohref' id='l" + lm.Id.ToString() + "' class='addnotebtn' data-titleid='0'>Pending Contact</a>";

                LeadNote ln = this._noteTitleService.GetLeadNote(lm.LeadId);
                NoteTitle noteTitle = null;
                if (ln != null && ln.NoteTitleId != 0)
                {
                    noteTitle = this._noteTitleService.GetNoteTitleById((long)ln.NoteTitleId);
                    LeadNote = " <a nohref='nohref' id='l" + lm.Id.ToString() + "' class='addnotebtn' data-titleid='" + ln.NoteTitleId + "'>" + noteTitle.Title + "</a>";
                }

                DateTime created = _settingService.GetTimeZoneDate(lm.Created.Value);
                DateTime? updated = null;
                if (lm.UpdateDate.HasValue)
                    updated = _settingService.GetTimeZoneDate(lm.UpdateDate.Value);

                if (report)
                {
                    string[] names1 = {
                                      permissionService.Authorize(PermissionProvider.LeadsIinfoView) ? "<a class='idhref' id='" + lm.Id.ToString() + "' nohref='nohref'><b>" + lm.Id.ToString() + "</b></a>" : "<b>" + lm.Id.ToString() + "</b> ",
                                      "<div class='text-ellipsisZZZ text-left'>" + EmailResult + "</div>",
                                      "<span class='text-center'>" + created.ToShortDateString() + "<br>" + created.ToLongTimeString() + "</span>",
                                      "<span class='text-ellipsis text-center'><a href='/Management/Affiliate/Item/" + lm.AffiliateId.ToString() + "' title='" + lm.AffiliateId.ToString() + "# " + affNmae + "'>" + affNmae + "</a></span>",
                                      "<span class='text-ellipsis text-center'><a href='/Management/AffiliateChannel/Item/"+lm.AffiliateChannelId.ToString()+"' title='" + lm.AffiliateChannelId.ToString() + "# " + affiliateChannelName + "'>" + affiliateChannelName + "</a></span>",
                                      "<div style='width:100 %; text-align: left; margin-left:8px;'>" + StatusStr + " " + redirect + "</div>",
                                      "<span class='text-center'>" + Math.Round(lm.BuyerPrice.HasValue ? lm.BuyerPrice.Value : 0, 2).ToString() + "</span>",
                                };
                    jd.data.Add(names1);

                }
                else
                if ((_appContext.AppUser.UserType == UserTypes.Super || _appContext.AppUser.UserType == UserTypes.Super))
                {
                    string[] names1 = {
                                      permissionService.Authorize(PermissionProvider.LeadsIinfoView) ? "<a class='idhref' id='" + lm.Id.ToString() + "' nohref='nohref'><b>" + lm.Id.ToString() + "</b></a>" : "<b>" + lm.Id.ToString() + "</b> ",

                                      "<div class='text-ellipsisZZZ text-left'>" + EmailResult + "</div>",
                                      "<span class='text-center'>" + lm.Firstname + "</span>",
                                      "<span class='text-center'>" + lm.Lastname + "</span>",
                                      "<span class='text-center'>" + created.ToShortDateString() + "<br>" + created.ToLongTimeString() + "</span>",
//                                      "<span class='text-center'>" + (updated.HasValue ? updated.Value.ToShortDateString() + "<br>" + updated.Value.ToLongTimeString() : "") + "</span>",
                                      "<span class='text-center'>" + lm.Ip + "</span>",
                                      "<span class='text-center'>" + lm.Zip + "</span>",
                                      "<span class='text-center'>" + lm.State + "</span>",
                                      "<span class='text-ellipsis text-center'><a href='/Management/Affiliate/Item/" + lm.AffiliateId.ToString() + "' title='" + lm.AffiliateId.ToString() + "# " + affNmae + "'>" + affNmae + "</a></span>",
                                      "<span class='text-ellipsis text-center'><a href='/Management/AffiliateChannel/Item/"+lm.AffiliateChannelId.ToString()+"' title='" + lm.AffiliateChannelId.ToString() + "# " + affiliateChannelName + "'>" + affiliateChannelName + "</a></span>",
                                      //lm.AffiliateSubId,
                                      (lm.Status==1 ? "<p class='text-ellipsis'><a href='/Management/Buyer/Item/"+lm.BuyerId.ToString()+"' title='" + lm.BuyerId.ToString() + "# " + buyerName + "'>" + buyerName + "</a></span>" : ""),
                                      (lm.Status==1 ? "<p class='text-ellipsis'><a href='/Management/BuyerChannel/Item/"+lm.BuyerChannelId.ToString()+"' title='" + lm.BuyerChannelId.ToString() + "# " + buyerChannelName + "'>" + buyerChannelName + "</a></span>" : ""),
                                      "<span class='text-ellipsis text-center'><a href='/Management/Campaign/Item/" + lm.CampaignId.ToString() + "' title='" + lm.CampaignId.ToString() + "# " + campaignName + "'>" + campaignName + "</a></span>",
                                      "<div style='width:100 %; text-align: left; margin-left:8px;'>" + StatusStr + " " + redirect + "</div>",
                                      "<span class='text-center'>" + monitor + "</span>",
                                      "<span class='text-center'>" + lm.ProcessingTime.ToString() + "</span>",
                                      "<span class='text-center'>" + Math.Round(lm.BuyerPrice.HasValue ? lm.BuyerPrice.Value : 0, 2).ToString() + "</span>",
                                      "<span class='text-center'>" + Math.Round(lm.AffiliatePrice.HasValue ? lm.AffiliatePrice.Value : 0, 2).ToString() + "</span>",
                                      "<span class='text-center'>" + Math.Round((lm.BuyerPrice.HasValue ? lm.BuyerPrice.Value : 0) - (lm.AffiliatePrice.HasValue ? lm.AffiliatePrice.Value : 0), 2).ToString() + "</span>",
                                };
                    jd.data.Add(names1);
                }
                else if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                {
                    string[] names1 = {
                                      "<a class='idhrefbuyer' id='" + lm.Id.ToString() + "' nohref='nohref'><b>" + lm.Id.ToString() + "</b></a>",
                                      "<p class='text-ellipsis'><a href='#' title='" + EmailResult + "'>" + EmailResult + "</a></p>",
                                      "<p class='text-center'>" + lm.Firstname + "</p>",
                                      "<p class='text-center'>" + lm.Lastname + "</p>",
                                      "<p class='text-center'>" + created.ToShortDateString() + "<br>" + created.ToLongTimeString() + "</p>",
                                      //"<p class='text-center'>" + (updated.HasValue ? updated.Value.ToShortDateString() + "<br>" + updated.Value.ToLongTimeString() : "") + "</p>",
                                      "<p class='text-center'>" + lm.Ip + "</p>",
                                      "<p class='text-center'>" + lm.Zip + "</p>",
                                      "<p class='text-center'>" + lm.State + "</p>",
                                      "<p class='text-center'>" + campaignName + "</p>",
                                      StatusStr,
                                      "<p class='text-center'>" + Math.Round(lm.AffiliatePrice.HasValue ? lm.AffiliatePrice.Value : 0, 2).ToString() + "</p>",
                                };
                    jd.data.Add(names1);
                }
                else
                {
                    bool noNotes = false;
                    if (_appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
                    {
                        Buyer buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
                        if (buyer != null && buyer.AlwaysSoldOption == 0) noNotes = true;
                    }

                    if (noNotes)
                    {
                        string[] names1 = {
                                      "<a class='idhrefbuyer' id='" + lm.Id.ToString() + "' nohref='nohref'><b>" + lm.Id.ToString() + "</b></a>",
                                      "<p class='text-ellipsis'><a href='#' title='" + EmailResult + "'>" + EmailResult + "</a></p>",
                                      "<p class='text-center'>" + lm.Firstname + "</p>",
                                      "<p class='text-center'>" + lm.Lastname + "</p>",
                                      "<p class='text-center'>" + created.ToShortDateString() + "<br>" + created.ToLongTimeString() + "</p>",
                                      //"<p class='text-center'>" + (updated.HasValue ? updated.Value.ToShortDateString() + "<br>" + updated.Value.ToLongTimeString() : "") + "</p>",
                                      "<p class='text-center'>" + lm.Ip + "</p>",
                                      "<p class='text-center'>" + lm.Zip + "</p>",
                                      "<p class='text-center'>" + lm.State + "</p>",
                                      lm.Status==1 ? "<p class='text-center'>" + buyerChannelName + "</p>" : "",
                                      "<p class='text-center'>" + campaignName + "</p>",
                                      StatusStr,
                                      "<p class='text-center'>" + Math.Round(lm.BuyerPrice.HasValue ? lm.BuyerPrice.Value : 0, 2).ToString() + "</p>"
                                      //_appContext.AppUser.UserTypeId == SharedData.BuyerUserTypeId ? "<p class='text-center'>" + LeadNote + "</p>" : ""
                                };
                        jd.data.Add(names1);
                    }
                    else
                    {
                        string[] names1 = {
                                      "<a class='idhrefbuyer' id='" + lm.Id.ToString() + "' nohref='nohref'><b>" + lm.Id.ToString() + "</b></a>",
                                      "<p class='text-ellipsis'><a href='#' title='" + EmailResult + "'>" + EmailResult + "</a></p>",
                                      "<p class='text-center'>" + lm.Firstname + "</p>",
                                      "<p class='text-center'>" + lm.Lastname + "</p>",
                                      "<p class='text-center'>" + created.ToShortDateString() + "<br>" + created.ToLongTimeString() + "</p>",
                                      //"<p class='text-center'>" + (updated.HasValue ? updated.Value.ToShortDateString() + "<br>" + updated.Value.ToLongTimeString() : "") + "</p>",
                                      "<p class='text-center'>" + lm.Ip + "</p>",
                                      "<p class='text-center'>" + lm.Zip + "</p>",
                                      "<p class='text-center'>" + lm.State + "</p>",
                                      lm.Status==1 ? "<p class='text-center'>" + buyerChannelName + "</p>" : "",
                                      "<p class='text-center'>" + campaignName + "</p>",
                                      StatusStr,
                                      _appContext.AppUser.UserType == SharedData.BuyerUserTypeId ? "<p class='text-center' title='" + (ln != null ? ln.Note : "") + "' alt='" + (ln != null ? ln.Note : "") + "'>" + LeadNote + "</p>" : ""
                                     //_appContext.AppUser.UserTypeId == SharedData.BuyerUserTypeId ? "<p class='text-center'>" + LeadNote + "</p>" : ""
                                };
                        jd.data.Add(names1);
                    }
                }
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Generates the excel file ajax.
        /// </summary>
        /// <returns>System.String.</returns>
        [ContentManagementAntiForgery(true)]
        [ValidateInput(false)]
        public string GenerateExcelFileAjax()
        {
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            bool maskEmail = _appContext.AppUser == null ? true : _appContext.AppUser.MaskEmail;

            var actionsJson = Request["actions"];
            int page = 0;
            int pageSize = 50; // _leadModel.RowsPerPage > 0 ? _leadModel.RowsPerPage : 100;

            DateTime dateFrom = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());
            short Status = -1;
            string State = "";
            string IP = "";

            long FilterLeadId = 0;
            string FilterEmail = "";
            long FilterAffiliate = 0;
            long FilterAffiliateChannel = 0;
            long FilterBuyer = 0;
            long FilterBuyerChannel = 0;
            long FilterCampaign = 0;
            string FilterAffiliateChannelSubId = "";
            string FilterFirstName = "";
            string FilterLastName = "";
            string FilterZipCode = "";
            decimal FilterBPrice = 0;

            if (Request["page"] != null)
            {
                page = int.Parse(Request["page"]) - 1;
            }

            if (Request["pagesize"] != null)
            {
                pageSize = int.Parse(Request["pagesize"]);
            }

            if (Request["leadid"] != null && Request["leadid"] != "" && long.TryParse(Request["leadid"], out FilterLeadId))
            {
                FilterLeadId = long.Parse(Request["leadid"]);
            }

            if (Request["datefrom"] != null && Request["datefrom"] != "0")
            {
                dateFrom = Convert.ToDateTime(Request["datefrom"]);
            }
            if (Request["dateto"] != null && Request["dateto"] != "0")
            {
                dateTo = Convert.ToDateTime(Request["dateto"]);
            }

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (Request["status"] != null)
            {
                Status = short.Parse(Request["status"]);
            }

            if (Request["state"] != null)
            {
                State = Request["state"];
            }

            if (Request["ip"] != null)
            {
                IP = Request["ip"];
            }

            if (Request["email"] != null)
            {
                FilterEmail = Request["email"];
            }
            if (Request["affiliate"] != null && Request["affiliate"] != "" && long.TryParse(Request["affiliate"], out FilterAffiliate))
            {
                FilterAffiliate = long.Parse(Request["affiliate"]);
            }
            if (Request["affiliatechannel"] != null && Request["affiliatechannel"] != "" && long.TryParse(Request["affiliatechannel"], out FilterAffiliateChannel))
            {
                FilterAffiliateChannel = long.Parse(Request["affiliatechannel"]);
            }

            if (Request["affiliatesubid"] != null && Request["affiliatesubid"] != "")
            {
                FilterAffiliateChannelSubId = Request["affiliatesubid"];
            }

            if (Request["buyer"] != null && Request["buyer"] != "" && long.TryParse(Request["buyer"], out FilterBuyer))
            {
                FilterBuyer = long.Parse(Request["buyer"]);
            }
            if (Request["buyerchannel"] != null && Request["buyerchannel"] != "" && long.TryParse(Request["buyerchannel"], out FilterBuyerChannel))
            {
                FilterBuyerChannel = long.Parse(Request["buyerchannel"]);
            }
            if (Request["campaign"] != null && Request["campaign"] != "" && long.TryParse(Request["campaign"], out FilterCampaign))
            {
                FilterCampaign = long.Parse(Request["campaign"]);
            }
            if (Request["firstname"] != null)
            {
                FilterFirstName = Request["firstname"];
            }
            if (Request["lastname"] != null)
            {
                FilterLastName = Request["lastname"];
            }
            if (Request["zipcode"] != null)
            {
                FilterZipCode = Request["zipcode"];
            }
            if (Request["bprice"] != null)
            {
                FilterBPrice = decimal.Parse(Request["bprice"]);
            }

            List<LeadMainContent> leadMainContent = (List<LeadMainContent>)this._leadMainService.GetLeadsAll(
                dateFrom,
                dateTo,
                FilterLeadId,
                FilterEmail,
                FilterAffiliate,
                FilterAffiliateChannel,
                FilterAffiliateChannelSubId,
                FilterBuyer,
                FilterBuyerChannel,
                FilterCampaign,
                Status,
                IP,
                State,
                FilterFirstName,
                FilterLastName,
                FilterBPrice,
                FilterZipCode,
                "",
                page * pageSize, pageSize
                );

            JsonData jd = new JsonData();
            jd.draw = page;
            jd.recordsTotal = (FilterLeadId == 0 ? (int)this._leadMainService.GetLeadsCount(dateFrom, dateTo, FilterEmail, FilterAffiliate, FilterAffiliateChannel, FilterBuyer, FilterBuyerChannel, FilterCampaign, Status, IP, State, "") : 1);
            jd.recordsFiltered = jd.recordsTotal;
            jd.TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToString("MM/dd/yyyy");

            string affNmae = "";
            string buyerName = "";
            string campaignName = "";
            string buyerChannelName = "";
            string affiliateChannelName = "";

            if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
            {
                string[] headers = { "ID", "Email", "Data", "IP", "State", "Affiliate", "Aff.Ch.Id", "Buyer", "Buyer Ch.Id", "Campaign Id", "Status", "Notes" };
                jd.data.Add(headers);
            }
            else
            {
                string[] headers = { "ID", "Email", "Data", "IP", "State", "Campaign Id", "Status", "Notes" };
                jd.data.Add(headers);
            }

            foreach (LeadMainContent lm in leadMainContent)
            {
                affNmae = "";
                buyerName = "";
                campaignName = "";
                buyerChannelName = "";
                affiliateChannelName = "";

                string StatusStr = "";
                switch (lm.Status)
                {
                    case 1: { StatusStr = "Sold"; break; }
                    case 2: { StatusStr = "Error"; break; }
                    case 3: { StatusStr = "Reject"; break; }
                    case 4: { StatusStr = "Processing"; break; }
                    case 5: { StatusStr = "Filter"; break; }
                    case 6: { StatusStr = "Min Price"; break; }
                    case 0: { StatusStr = "Test"; break; }
                }

                string monitor = "";

                Affiliate aff = this._affiliateService.GetAffiliateById(lm.AffiliateId, true);
                if (aff != null)
                {
                    affNmae = aff.Name;
                }

                if (lm.BuyerId != null)
                {
                    Buyer buyer = this._buyerService.GetBuyerById((long)lm.BuyerId);
                    if (buyer != null)
                    {
                        buyerName = buyer.Name;
                    }
                }
                if (lm.BuyerChannelId != 0)
                {
                    BuyerChannel bch = this._buyerChannelService.GetBuyerChannelById((long)lm.BuyerChannelId);
                    if (bch != null)
                    {
                        buyerChannelName = bch.Name;
                    }
                }

                if (lm.AffiliateChannelId != 0)
                {
                    AffiliateChannel ach = this._affiliateChannelService.GetAffiliateChannelById(lm.AffiliateChannelId);
                    if (ach != null)
                    {
                        affiliateChannelName = ach.Name;
                    }
                }

                if (lm.CampaignId != 0)
                {
                    campaignName = this._campaignService.GetCampaignById(lm.CampaignId).Name;
                }

                string EmailResult = "";
                if (lm.Email != null)
                {
                    if (maskEmail)
                        EmailResult = Regex.Replace(lm.Email, @"(?<=[\w]{2})[\w-\._\+%]*(?=[\w]{1}@)", m => new string('*', m.Length));
                    else
                        EmailResult = lm.Email;
                }

                string LeadNote = "";

                LeadNote ln = this._noteTitleService.GetLeadNote(lm.LeadId);
                if (ln != null && ln.NoteTitleId != 0)
                {
                    NoteTitle n = this._noteTitleService.GetNoteTitleById((long)ln.NoteTitleId);
                    LeadNote = n.Title + " " + ln.Note;
                }

                if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
                {
                    string[] names1 = {
                                      lm.Id.ToString(),
                                      EmailResult,
                                      String.Format("{0:MM/dd/yyyy HH:mm:ss}", lm.Created),
                                      lm.Ip,
                                      lm.State,
                                      affNmae,
                                      affiliateChannelName,
                                      lm.Status==1 ? buyerName : "",
                                      lm.Status==1 ? buyerChannelName : "",
                                      campaignName,
                                      StatusStr,
                                      monitor,
                                      LeadNote
                                };
                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      lm.Id.ToString(),
                                      EmailResult,
                                      String.Format("{0:MM/dd/yyyy HH:mm:ss}", lm.Created),
                                      lm.Ip,
                                      lm.State,
                                      campaignName,
                                      StatusStr,
                                     _appContext.AppUser.UserType == SharedData.BuyerUserTypeId ? LeadNote : ""
                                };
                    jd.data.Add(names1);
                }
            }

            string retPath = Export2CSV(jd.data);

            return retPath;
        }

        /// <summary>
        /// Generates the CSV file ajax.
        /// </summary>
        /// <returns>System.String.</returns>
        [ContentManagementAntiForgery(true)]
        [ValidateInput(false)]
        public string GenerateCSVFileAjax()
        {
            // Check Password
            User u = this._userService.GetUserById(_appContext.AppUser.Id);

            string passwordHash = _encryptionService.CreatePasswordHash(Request["pass"], u.SaltKey);

            if (u.Password != passwordHash)
            {
                return "0";
            }

            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            bool maskEmail = _appContext.AppUser == null ? true : _appContext.AppUser.MaskEmail;

            var actionsJson = Request["actions"];
            int page = 0;
            int pageSize = 50;

            DateTime dateFrom = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());
            short Status = -1;
            string State = "";
            string IP = "";

            long FilterLeadId = 0;
            string FilterEmail = "";
            long FilterAffiliate = 0;
            long FilterAffiliateChannel = 0;
            long FilterBuyer = 0;
            long FilterBuyerChannel = 0;
            long FilterCampaign = 0;
            string FilterAffiliateChannelSubId = "";

            string FilterFirstName = "";
            string FilterLastName = "";
            string FilterZipCode = "";
            decimal FilterBPrice = 0;

            if (Request["page"] != null)
            {
                page = int.Parse(Request["page"]) - 1;
            }

            if (Request["pagesize"] != null)
            {
                pageSize = int.Parse(Request["pagesize"]);
            }

            if (Request["leadid"] != null && Request["leadid"] != "" && long.TryParse(Request["leadid"], out FilterLeadId))
            {
                FilterLeadId = long.Parse(Request["leadid"]);
            }

            if (Request["datefrom"] != null && Request["datefrom"] != "0")
            {
                dateFrom = Convert.ToDateTime(Request["datefrom"]);
            }
            if (Request["dateto"] != null && Request["dateto"] != "0")
            {
                dateTo = Convert.ToDateTime(Request["dateto"]);
            }

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (Request["status"] != null)
            {
                Status = short.Parse(Request["status"]);
            }

            if (Request["state"] != null)
            {
                State = Request["state"];
            }

            if (Request["ip"] != null)
            {
                IP = Request["ip"];
            }

            if (Request["email"] != null)
            {
                FilterEmail = Request["email"];
            }
            if (Request["affiliate"] != null && Request["affiliate"] != "" && long.TryParse(Request["affiliate"], out FilterAffiliate))
            {
                FilterAffiliate = long.Parse(Request["affiliate"]);
            }
            if (Request["affiliatechannel"] != null && Request["affiliatechannel"] != "" && long.TryParse(Request["affiliatechannel"], out FilterAffiliateChannel))
            {
                FilterAffiliateChannel = long.Parse(Request["affiliatechannel"]);
            }
            if (Request["affiliatesubid"] != null && Request["affiliatesubid"] != "")
            {
                FilterAffiliateChannelSubId = Request["affiliatesubid"];
            }
            if (Request["buyer"] != null && Request["buyer"] != "" && long.TryParse(Request["buyer"], out FilterBuyer))
            {
                FilterBuyer = long.Parse(Request["buyer"]);
            }
            if (Request["buyerchannel"] != null && Request["buyerchannel"] != "" && long.TryParse(Request["buyerchannel"], out FilterBuyerChannel))
            {
                FilterBuyerChannel = long.Parse(Request["buyerchannel"]);
            }
            if (Request["campaign"] != null && Request["campaign"] != "" && long.TryParse(Request["campaign"], out FilterCampaign))
            {
                FilterCampaign = long.Parse(Request["campaign"]);
            }
            if (Request["firstname"] != null)
            {
                FilterFirstName = Request["firstname"];
            }
            if (Request["lastname"] != null)
            {
                FilterLastName = Request["lastname"];
            }
            if (Request["zipcode"] != null)
            {
                FilterZipCode = Request["zipcode"];
            }
            if (Request["bprice"] != null)
            {
                FilterBPrice = decimal.Parse(Request["bprice"]);
            }

            List<LeadMainContent> leadMainContent = (List<LeadMainContent>)this._leadMainService.GetLeadsAll(
                dateFrom,
                dateTo,
                FilterLeadId,
                FilterEmail,
                FilterAffiliate,
                FilterAffiliateChannel,
                FilterAffiliateChannelSubId,
                FilterBuyer,
                FilterBuyerChannel,
                FilterCampaign,
                Status,
                IP,
                State,
                FilterFirstName,
                FilterLastName,
                FilterBPrice,
                FilterZipCode,
                "",
                page * pageSize, pageSize);

            JsonData jd = new JsonData();
            jd.draw = page;
            jd.recordsTotal = (FilterLeadId == 0 ? (int)this._leadMainService.GetLeadsCount(dateFrom, dateTo, FilterEmail, FilterAffiliate, FilterAffiliateChannel, FilterBuyer, FilterBuyerChannel, FilterCampaign, Status, IP, State, "") : 1); 
            jd.recordsFiltered = jd.recordsTotal;
            jd.TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToString("MM/dd/yyyy");

            string affNmae = "";
            string buyerName = "";
            string campaignName = "";
            string buyerChannelName = "";
            string affiliateChannelName = "";

            if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
            {
                string[] headers = { "LeadID", "Email", "Data", "IP", "State", "Affiliate", "Aff.Ch.Id", "Buyer", "Buyer Ch.Id", "Campaign Id", "Status", "Notes" };
                jd.data.Add(headers);
            }
            else
            {
                string[] headers = { "LeadID", "Email", "Data", "IP", "State", "Campaign Id", "Status", "Notes" };
                jd.data.Add(headers);
            }

            foreach (LeadMainContent lm in leadMainContent)
            {
                affNmae = "";
                buyerName = "";
                campaignName = "";
                buyerChannelName = "";
                affiliateChannelName = "";

                string StatusStr = "";
                switch (lm.Status)
                {
                    case 1: { StatusStr = "Sold"; break; }
                    case 2: { StatusStr = "Error"; break; }
                    case 3: { StatusStr = "Reject"; break; }
                    case 4: { StatusStr = "Processing"; break; }
                    case 5: { StatusStr = "Filter"; break; }
                    case 6: { StatusStr = "Min Price"; break; }
                    case 0: { StatusStr = "Test"; break; }
                }

                string monitor = "";

                Affiliate aff = this._affiliateService.GetAffiliateById(lm.AffiliateId, true);
                if (aff != null)
                {
                    affNmae = aff.Name;
                }

                if (lm.BuyerId != null)
                {
                    Buyer buyer = this._buyerService.GetBuyerById((long)lm.BuyerId);
                    if (buyer != null)
                    {
                        buyerName = buyer.Name;
                    }
                }
                if (lm.BuyerChannelId != null)
                {
                    BuyerChannel bch = this._buyerChannelService.GetBuyerChannelById((long)lm.BuyerChannelId);
                    if (bch != null)
                    {
                        buyerChannelName = bch.Name;
                    }
                }

                if (lm.AffiliateChannelId != 0)
                {
                    AffiliateChannel ach = this._affiliateChannelService.GetAffiliateChannelById(lm.AffiliateChannelId);
                    if (ach != null)
                    {
                        affiliateChannelName = ach.Name;
                    }
                }

                if (lm.CampaignId != 0)
                {
                    campaignName = this._campaignService.GetCampaignById(lm.CampaignId).Name;
                }

                string EmailResult = "";
                if (lm.Email != null)
                {
                    if (maskEmail)
                        EmailResult = Regex.Replace(lm.Email, @"(?<=[\w]{2})[\w-\._\+%]*(?=[\w]{1}@)", m => new string('*', m.Length));
                    else
                        EmailResult = lm.Email;
                }

                string LeadNote = "";

                LeadNote ln = this._noteTitleService.GetLeadNote(lm.LeadId);
                if (ln != null && ln.NoteTitleId != 0)
                {
                    NoteTitle n = this._noteTitleService.GetNoteTitleById((long)ln.NoteTitleId);
                    LeadNote = n.Title + " " + ln.Note;
                }

                if (_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId)
                {
                    string[] names1 = {
                                      lm.Id.ToString(),
                                      EmailResult,
                                      String.Format("{0:MM/dd/yyyy HH:mm:ss}", lm.Created),
                                      lm.Ip,
                                      lm.State,
                                      affNmae,
                                      affiliateChannelName,
                                      lm.Status==1 ? buyerName : "",
                                      lm.Status==1 ? buyerChannelName : "",
                                      campaignName,
                                      StatusStr,
                                      monitor,
                                      LeadNote
                                };
                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      lm.Id.ToString(),
                                      EmailResult,
                                      String.Format("{0:MM/dd/yyyy HH:mm:ss}", lm.Created),
                                      lm.Ip,
                                      lm.State,
                                      campaignName,
                                      StatusStr,
                                     _appContext.AppUser.UserType == SharedData.BuyerUserTypeId ? LeadNote : ""
                                };
                    jd.data.Add(names1);
                }
            }

            string retPath = Export2CSV(jd.data);

            return retPath;
        }

        /// <summary>
        /// Gets the lead by identifier ajax.
        /// </summary>
        /// <returns>System.String.</returns>
        [ContentManagementAntiForgery(true)]
        public string GetLeadByIdAjax()
        {
            if (Request["leadid"] == null)
                return null;
            long leadId = long.Parse(Request["leadid"]);
            LeadMainContent lead = (LeadMainContent)this._leadMainService.GetLeadsAllById(leadId);

            if (lead == null) return "";

            return lead.ReceivedData;
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="list">The list.</param>
        private void GetNodes(XmlNode parent, List<XmlNode> list, long CampaignID)
        {
            bool b = true;

            XmlAttribute attr = null;

            foreach (XmlNode node in parent.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    b = false;
                    GetNodes(node, list, CampaignID);

                    if (node.Attributes["decrypted"] == null)
                    {
                        attr = parent.OwnerDocument.CreateAttribute("decrypted");
                        CampaignField ct = _campaignTemplateService.GetCampaignTemplateBySectionAndName("", node.Name,CampaignID);

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
                    if (!list.Contains(parent))
                    {
                        CampaignField ct = _campaignTemplateService.GetCampaignTemplateBySectionAndName("", parent.Name, CampaignID);
                        if (ct != null && ct.IsHash.HasValue && ct.IsHash.Value && ct.Validator != 5 && ct.Validator != 6 && ct.Validator != 12 && ct.TemplateField.ToLower() != "ssn" && ct.TemplateField.ToLower() != "dlnumber" && ct.TemplateField.ToLower() != "accountnumber")
                        {
                            attr.Value = Helper.Decrypt(parent.InnerText);
                        }
                        list.Add(parent);
                        parent.Attributes.Append(attr);

                        /*BlackListType blackListType = _blackListService.GetBlackListType(parent.Name);
                        if (blackListType != null)
                        {
                        }*/
                    }
                }
                else
                {
                    if (!list.Contains(parent.ParentNode))
                    {
                        CampaignField ct = _campaignTemplateService.GetCampaignTemplateBySectionAndName("", parent.ParentNode.Name, CampaignID);
                        if (ct != null && ct.IsHash.HasValue && ct.IsHash.Value)
                        {
                            attr.Value = Helper.Decrypt(parent.ParentNode.InnerText);
                        }
                        list.Add(parent.ParentNode);
                        parent.ParentNode.Attributes.Append(attr);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the email item.
        /// </summary>
        /// <param name="lead">The lead.</param>
        /// <param name="leadEmailFields">The lead email fields.</param>
        /// <returns>System.String.</returns>
        public string GetEmailItem(LeadMain lead, string leadEmailFields)
        {
            //ViewDataDictionary viewData = new ViewDataDictionary();
            //Helper.RenderViewToString<int>(viewData, this.ControllerContext, "~/Views/Lead/ItemEmail.cshtml", 1);

            List<XmlNode> list = new List<XmlNode>();

            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(lead.ReceivedData);
                GetNodes(xmldoc.DocumentElement, list,lead.CampaignId);
            }
            catch
            {
            }

            using (var sw = new StringWriter())
            {
                ViewDataDictionary viewData = new ViewDataDictionary();
                viewData["nodes"] = list;
                viewData["AllowedNodes"] = this._campaignTemplateService.CampaignTemplateAllowedNames(lead.CampaignId);
                viewData["SensitiveData"] = this._leadSensitiveDataService.GetLeadSensitiveDataByLeadId(lead.Id);
                viewData["LeadEmailFields"] = (!string.IsNullOrEmpty(leadEmailFields) ? leadEmailFields.Split(new char[1] { ',' }) : new string[0]);

                TempDataDictionary tempData = new TempDataDictionary();

                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         "ItemEmail");
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Items the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult Item(long Id)
        {
            if (_appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                LeadMain lm = this._leadMainService.GetLeadMainById(Id);
                if (lm != null)
                {
                    lm.ViewDate = DateTime.UtcNow;
                    this._leadMainService.UpdateLeadMain(lm);
                }
            }

            LeadMainContent lead = (LeadMainContent)this._leadMainService.GetLeadsAllById(Id);

            if (lead != null)
            {
                List<AffiliateResponse> affiliateResponses = (List<AffiliateResponse>)this._affiliateResponseService.GetAffiliateResponsesByLeadId(Id);

                ViewBag.AffiliateResponses = affiliateResponses;
                AffiliateResponse affresp = null; ;

                if (affiliateResponses.Count > 0)
                {
                    affresp = affiliateResponses.Where(x => !string.IsNullOrEmpty(x.Message)).FirstOrDefault();
                }

                ViewBag.AffiliateResponseMessage = (affresp != null ? affresp.Message : "");

                ViewBag.RDate = lead.ReceivedData;

                ViewBag.Lead = lead;
                ViewBag.LeadCreated = _settingService.GetTimeZoneDate(lead.Created.Value);

                if (lead.CampaignId != 0)
                {
                    ViewBag.CampaignName = this._campaignService.GetCampaignById(lead.CampaignId).Name;
                }

                Affiliate aff = this._affiliateService.GetAffiliateById(lead.AffiliateId, true);
                if (aff != null)
                {
                    ViewBag.AffiliateName = aff.Name;
                }
                AffiliateChannel affCh = this._affiliateChannelService.GetAffiliateChannelById(lead.AffiliateChannelId);
                if (affCh != null)
                {
                    ViewBag.AffiliateChannelName = affCh.Name;
                }

                string Status = "";
                switch (lead.Status)
                {
                    case 1: { Status = "<div data-id='1' class='for-popup-header status-label label label-success'> Sold </div>"; break; }
                    case 2: { Status = "<div data-id='2' class='for-popup-header status-label label label-error'> Error </div>"; break; }
                    case 3: { Status = "<div data-id='3' class='for-popup-header status-label label label-danger'> Reject </div>"; break; }
                    case 4: { Status = "<div data-id='4' class='for-popup-header status-label label label-info'> Processing </div>"; break; }
                    case 5: { Status = "<div data-id='5' class='for-popup-header status-label label label-warning'> Filter </div>"; break; }
                    case 6: { Status = "<div data-id='6' class='for-popup-header status-label label label-danger'> Min Price </div>"; break; }
                    case 0: { Status = "<div data-id='0' class='for-popup-header status-label label label-default'> Test </div>"; break; }
                }

                ViewBag.Status = Status;

                List<XmlNode> list = new List<XmlNode>();

                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(lead.ReceivedData);
                    GetNodes(xmldoc.DocumentElement, list, lead.CampaignId);
                }
                catch
                {
                }
                ViewBag.nodes = list;
                ViewBag.AllowedNodes = this._campaignTemplateService.CampaignTemplateAllowedNames(lead.CampaignId);
                //this._campaignTemplateService.IsCampaignTemplateHidden(lead.CampaignId)

                IList<LeadResponse> leadResponseList = (IList<LeadResponse>)this._leadMainResponesService.GetLeadMainResponseByLeadId(Id);
                
                Hashtable distinct = new Hashtable();
                Hashtable postedFilter = new Hashtable();            

                foreach (LeadResponse leadL in leadResponseList)
                {
                    if (distinct[leadL.Id] != null)
                    {
                        if (postedFilter[leadL.Posted]!=null)
                            leadL.Posted = (distinct[leadL.Id] as LeadResponse).Posted;
                        postedFilter[leadL.Posted] = 1;
                    }
                    distinct[leadL.Id] = leadL; 
                }
                leadResponseList.Clear();
                foreach (var key in distinct.Keys)
                {
                    var val = distinct[key] as LeadResponse;                    
                    leadResponseList.Add(val);
                }

                leadResponseList = leadResponseList.OrderBy(a => a.Id).ToList();

                ViewBag.leadResponseList = leadResponseList;

                // Dublicates //

                bool isSelf = false;
                IList<LeadContentDublicate> leadDublicateList = (IList<LeadContentDublicate>)this._leadContentDublicateService.GetLeadContentDublicateBySSN(Id, !string.IsNullOrEmpty(lead.Ssn) ? lead.Ssn : "");
                foreach (LeadContentDublicate l in leadDublicateList)
                {
                    if (l.LeadId == Id)
                    {
                        isSelf = true;
                        break;
                    }
                }
                if (!isSelf && leadDublicateList != null && leadDublicateList.Count > 0)
                {
                    LeadMainContent lmc = this._leadMainService.GetLeadsAllById(Id);
                    LeadContentDublicate lcd = new LeadContentDublicate();
                    lcd.LeadId = lmc.LeadId;
                    lcd.Created = (DateTime)lmc.Created;
                    lcd.AffiliateId = lmc.AffiliateId;
                    
                    Affiliate aff2 = this._affiliateService.GetAffiliateById(lmc.AffiliateId, true);
                    if (aff2 != null)
                    {
                        lcd.AffiliateName = aff2.Name;
                    }
                    

                    lcd.RequestedAmount = lmc.RequestedAmount;
                    lcd.NetMonthlyIncome = lmc.NetMonthlyIncome;
                    lcd.PayFrequency = lmc.PayFrequency;
                    lcd.Directdeposit = lmc.Directdeposit;
                    lcd.Email = lmc.Email;
                    lcd.HomePhone = lmc.HomePhone;
                    lcd.Ip = lmc.Ip;

                    leadDublicateList.Insert(0, lcd);
                }

                ViewBag.leadDublicateList = leadDublicateList;

                List<string> RequestedAmountList = new List<string>();
                List<string> NetMonthlyIncomeList = new List<string>();
                List<string> PayFrequencyList = new List<string>();
                List<string> DirectdepositList = new List<string>();
                List<string> EmailList = new List<string>();
                List<string> HomePhoneList = new List<string>();
                List<string> IpList = new List<string>();

                if (lead.RequestedAmount != null)
                {
                    RequestedAmountList.Add(lead.RequestedAmount.ToString());
                }

                if (lead.NetMonthlyIncome != null)
                {
                    NetMonthlyIncomeList.Add(lead.NetMonthlyIncome.ToString());
                }

                if (lead.PayFrequency != null)
                {
                    PayFrequencyList.Add(lead.PayFrequency.ToString());
                }

                if (lead.Directdeposit != null)
                {
                    DirectdepositList.Add(lead.Directdeposit.ToString());
                }

                if (lead.Email != null && lead.Email != "")
                {
                    EmailList.Add(lead.Email.ToString());
                }

                if (lead.HomePhone != null && lead.HomePhone != "")
                {
                    HomePhoneList.Add(lead.HomePhone.ToString());
                }

                if (lead.Ip != null && lead.Ip != "")
                {
                    IpList.Add(lead.Ip.ToString());
                }

                foreach (LeadContentDublicate ld in leadDublicateList)
                {
                    if (ld.RequestedAmount != null && !RequestedAmountList.Contains(ld.RequestedAmount.ToString()))
                    {
                        RequestedAmountList.Add(ld.RequestedAmount.ToString());
                    }

                    if (ld.NetMonthlyIncome != null && !NetMonthlyIncomeList.Contains(ld.NetMonthlyIncome.ToString()))
                    {
                        NetMonthlyIncomeList.Add(ld.NetMonthlyIncome.ToString());
                    }

                    if (ld.PayFrequency != null && !PayFrequencyList.Contains(ld.PayFrequency.ToString()))
                    {
                        PayFrequencyList.Add(ld.PayFrequency.ToString());
                    }

                    if (ld.Directdeposit != null && !DirectdepositList.Contains(ld.Directdeposit.ToString()))
                    {
                        DirectdepositList.Add(ld.Directdeposit.ToString());
                    }

                    if (ld.Email != null && !EmailList.Contains(ld.Email.ToString()))
                    {
                        EmailList.Add(ld.Email.ToString());
                    }

                    if (ld.HomePhone != null && !HomePhoneList.Contains(ld.HomePhone.ToString()))
                    {
                        HomePhoneList.Add(ld.HomePhone.ToString());
                    }

                    if (ld.Ip != null && !IpList.Contains(ld.Ip.ToString()))
                    {
                        IpList.Add(ld.Ip.ToString());
                    }
                }

                ViewBag.RequestedAmountCount = RequestedAmountList.Count() > 1 ? (RequestedAmountList.Count() - 1).ToString() : "";
                ViewBag.NetMonthlyIncomeCount = NetMonthlyIncomeList.Count() > 1 ? (NetMonthlyIncomeList.Count() - 1).ToString() : "";
                ViewBag.PayFrequencyCount = PayFrequencyList.Count() > 1 ? (PayFrequencyList.Count() - 1).ToString() : "";
                ViewBag.DirectdepositCount = DirectdepositList.Count() > 1 ? (DirectdepositList.Count() - 1).ToString() : "";
                ViewBag.EmailCount = EmailList.Count() > 1 ? (EmailList.Count() - 1).ToString() : "";
                ViewBag.HomePhoneCount = HomePhoneList.Count() > 1 ? (HomePhoneList.Count() - 1).ToString() : "";
                ViewBag.IpCount = IpList.Count() > 1 ? (IpList.Count() - 1).ToString() : "";


                List<LeadJourneyModel> leadJourneyList = new List<LeadJourneyModel>();

                leadJourneyList.Add(new LeadJourneyModel { 
                     Id = lead.AffiliateChannelId,
                     Name = "<b>Affiliate:</b> " + aff.Name + "<br> <b>Channel:</b> " + affCh.Name,
                     DateTime = (lead.Created.HasValue ? lead.Created.Value : DateTime.UtcNow),
                     Action = "<div class='status-label label label-info'>Received</div>",
                     Data = "<b>Min price:</b> " + lead.Minprice.ToString()
                });

                foreach(var lr in leadResponseList)
                {
                    Buyer buyer = _buyerService.GetBuyerById(lr.BuyerId);
                    BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(lr.BuyerChannelId);
                    string data = "";

                    string StatusStr = "";
                    switch (lr.Status)
                    {
                        case 1: { StatusStr = "<div class='status-label label label-success'> Sold </div>"; break; }
                        case 2: { StatusStr = "<div class='status-label label label-error'> Error </div>"; break; }
                        case 3: { StatusStr = "<div class='status-label label label-danger'> Reject </div>"; break; }
                    }

                    leadJourneyList.Add(new LeadJourneyModel
                    {
                        Id = (lead.BuyerChannelId.HasValue ? lead.BuyerChannelId.Value : 0),
                        Name = "<b>Buyer:</b> " + buyer.Name + "<br> <b>Channel:</b> " + buyerChannel.Name,
                        DateTime = lr.Created,
                        Action = "<div class='status-label label label-warning'>Posted</div>",
                        Data = StatusStr
                    });
                }

                foreach(var ar in affiliateResponses)
                {
                    leadJourneyList.Add(new LeadJourneyModel
                    {
                        Id = lead.AffiliateChannelId,
                        Name = "<b>Affiliate:</b> " + aff.Name + "<br> <b>Channel:</b> " + affCh.Name,
                        DateTime = ar.Created,
                        Action = "<div class='status-label label label-success'>Responsed</div>",
                        Data = ar.Message
                    });
                }

                ViewBag.LeadJourneys = leadJourneyList;
            }

            ViewBag.GeoData = this._leadMainService.GetLeadGeoData(Id);

            ViewBag.SensitiveData = this._leadSensitiveDataService.GetLeadSensitiveDataByLeadId(Id);
            // Redirect

            ViewBag.Redirect = null;
            RedirectUrl rurl = this._redirectUrlService.GetRedirectUrlByLeadId(Id);
            if (rurl != null)
            {
                ViewBag.Redirect = rurl;
            }

            return PartialView(this._leadModel);
        }

        /// <summary>
        /// Bads the ip clicks report.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Bad IP Clicks Report")]
        public ActionResult BadIPClicksReport()
        {
            this._leadModel.TotalRowsCount = 1000; //  this._leadMainService.GetLeadsCount();
            this._leadModel.RowsPerPage = 100;

            this._leadModel.PageCount = (int)Math.Ceiling((double)this._leadModel.TotalRowsCount / this._leadModel.RowsPerPage);

            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);
            this._leadModel.TimeZoneNowStr = tzDateTime.ToString("MM/dd/yyyy");
            this._leadModel.TimeZoneNow = tzDateTime;

            return View(this._leadModel);
        }

        /// <summary>
        /// Gets the bad ip clicks report ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetBadIPClicksReportAjax()
        {
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            bool maskEmail = _appContext.AppUser == null ? true : _appContext.AppUser.MaskEmail;

            var actionsJson = Request["actions"];
            int page = 0;
            int pageSize = _leadModel.RowsPerPage > 0 ? _leadModel.RowsPerPage : 100;

            DateTime dateFrom = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString());

            string LeadIP = "";
            string ClickIP = "";

            long FilterLeadId = 0;

            string FilterAffiliates = string.Empty;

            if (Request["page"] != null)
            {
                page = int.Parse(Request["page"]) - 1;
            }

            if (Request["pagesize"] != null)
            {
                pageSize = int.Parse(Request["pagesize"]);
            }

            if (Request["leadid"] != null && Request["leadid"] != "" && long.TryParse(Request["leadid"], out FilterLeadId))
            {
                FilterLeadId = long.Parse(Request["leadid"]);
            }

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (Request["leadip"] != null)
            {
                LeadIP = Request["leadip"];
            }
            if (Request["clickip"] != null)
            {
                ClickIP = Request["clickip"];
            }

            if (Request["affiliate"] != null && Request["affiliate"] != "")
            {
                FilterAffiliates = Request["affiliate"].ToString();
            }

            List<ReportBadIPClicks> bipcList = (List<ReportBadIPClicks>)this._leadMainService.GetBadIPClicksReport(dateFrom, dateTo, FilterLeadId, FilterAffiliates, LeadIP, ClickIP, page * pageSize, pageSize);

            JsonData jd = new JsonData();
            jd.draw = 0;
            jd.recordsTotal = 100;
            jd.recordsFiltered = 0;
            jd.TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToString("MM/dd/yyyy");

            foreach (ReportBadIPClicks bipc in bipcList)
            {
                Affiliate aff = this._affiliateService.GetAffiliateById(bipc.AffiliateId, true);
                string affName = "";
                if (aff != null)
                {
                    affName = aff.Name;
                }
                string[] names1 = {
                                      permissionService.Authorize(PermissionProvider.LeadsIinfoView) || _appContext.AppUser.UserType == SharedData.BuyerUserTypeId || _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId ? "<a class='idhref' id='" + bipc.LeadId.ToString() + "' href='#'><b>" + bipc.LeadId.ToString() + "</b></a>" : "<b>" + bipc.LeadId.ToString() + "</b> ",
                                      "<p style='text-align: center'>"+String.Format("{0:MM/dd/yyyy <br> HH:mm:ss}", bipc.Created) + "</p>",
                                      "<p class='text-center' style='text-align: center'><a href='/Management/Affiliate/Item/" + bipc.AffiliateId.ToString() + "' title='" + bipc.AffiliateId.ToString() + "# " + affName + "'>" + affName + "</a></p>",
                                      "<p style='text-align: center'>"+bipc.LeadIp+"</p>",
                                      "<p style='text-align: center'>"+bipc.ClickIp+"</p>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Errors the leads report buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Error Leads Report")]
        public ActionResult ErrorLeadsReportBuyer()
        {
            if (_appContext.AppUser != null &&
                _appContext.AppUser.UserType != SharedData.BuiltInUserTypeId &&
                _appContext.AppUser.UserType != SharedData.NetowrkUserTypeId)
                return HttpNotFound();

            this._leadModel.TotalRowsCount = 1000; //  this._leadMainService.GetLeadsCount();
            this._leadModel.RowsPerPage = 100;

            this._leadModel.PageCount = (int)Math.Ceiling((double)this._leadModel.TotalRowsCount / this._leadModel.RowsPerPage);

            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);
            this._leadModel.TimeZoneNowStr = tzDateTime.ToString("MM/dd/yyyy");
            this._leadModel.TimeZoneNow = tzDateTime;

            ViewBag.Affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(0);
            ViewBag.BuyerChannels = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannels(0);
            ViewBag.AffiliateChannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannels(0);
            ViewBag.Campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            ViewBag.SelectedStatus = Request["status"];
            ViewBag.SelectedBuyerId = Request["buyerid"];
            ViewBag.SelectedStartDate = Request["startDate"];
            ViewBag.SelectedEndDate = Request["endDate"];
            ViewBag.SelectedErrorType = Request["error"];

            foreach (var value in _stateProvinceService.GetStateProvinceByCountryId(80))
            {
                this._leadModel.ListStates.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Code
                });
            }

            return View(this._leadModel);
        }

        /// <summary>
        /// Errors the leads report affiliate.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Error Leads Report")]
        public ActionResult ErrorLeadsReportAffiliate()
        {
            if (_appContext.AppUser != null &&
                _appContext.AppUser.UserType != SharedData.BuiltInUserTypeId &&
                _appContext.AppUser.UserType != SharedData.NetowrkUserTypeId)
                return HttpNotFound();

            this._leadModel.TotalRowsCount = 1000; //  this._leadMainService.GetLeadsCount();
            this._leadModel.RowsPerPage = 100;

            this._leadModel.PageCount = (int)Math.Ceiling((double)this._leadModel.TotalRowsCount / this._leadModel.RowsPerPage);

            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);
            this._leadModel.TimeZoneNowStr = tzDateTime.ToString("MM/dd/yyyy");
            this._leadModel.TimeZoneNow = tzDateTime;

            ViewBag.Affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(0).OrderBy(x => x.Name).ToList();
            ViewBag.AffiliateChannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannels(0).OrderBy(x => x.Name).ToList();

            foreach (var value in _stateProvinceService.GetStateProvinceByCountryId(80))
            {
                this._leadModel.ListStates.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Code
                });
            }

            return View(this._leadModel);
        }

        /// <summary>
        /// Validations the error leads.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Validation Error Leads Report")]
        public ActionResult ValidationErrorLeads()
        {
            this._leadModel.TotalRowsCount = 1000; //  this._leadMainService.GetLeadsCount();
            this._leadModel.RowsPerPage = 100;

            this._leadModel.PageCount = (int)Math.Ceiling((double)this._leadModel.TotalRowsCount / this._leadModel.RowsPerPage);

            DateTime tzDateTime = this._settingService.GetTimeZoneDate(DateTime.UtcNow);
            this._leadModel.TimeZoneNowStr = tzDateTime.ToString("MM/dd/yyyy");
            this._leadModel.TimeZoneNow = tzDateTime;

            return View(this._leadModel);
        }

        /// <summary>
        /// Gets the validation error leads ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetValidationErrorLeadsAjax()
        {
            DateTime dateFrom = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToShortDateString());
            long FilterAffiliate = 0;
            long FilterAffiliateChannel = 0;

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0]);
                dateTo = Convert.ToDateTime(dates[1]);
            }

            dateFrom = _settingService.GetUTCDate(dateFrom);
            dateTo = _settingService.GetUTCDate(dateTo);

            if (Request["affiliate"] != null && Request["affiliate"] != "" && long.TryParse(Request["affiliate"], out FilterAffiliate))
            {
                FilterAffiliate = long.Parse(Request["affiliate"]);
            }

            if (Request["affiliatechannel"] != null && Request["affiliatechannel"] != "" && long.TryParse(Request["affiliatechannel"], out FilterAffiliateChannel))
            {
                FilterAffiliateChannel = long.Parse(Request["affiliatechannel"]);
            }

            IList<AffiliateResponse> affResponseList = this._affiliateResponseService.GetAffiliateResponsesByFilters(FilterAffiliate, FilterAffiliateChannel, dateFrom, dateTo);

            JsonData jd = new JsonData();
            jd.draw = 0;
            jd.recordsTotal = affResponseList.Count;
            jd.recordsFiltered = 0;
            jd.TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToString("MM/dd/yyyy");

            foreach (AffiliateResponse affresp in affResponseList)
            {
                Affiliate aff = this._affiliateService.GetAffiliateById(affresp.AffiliateId, true);
                AffiliateChannel affChanel = this._affiliateChannelService.GetAffiliateChannelById(affresp.AffiliateChannelId);

                string affName = "";
                string affChName = "";
                if (aff != null)
                {
                    affName = aff.Name;
                }
                if (affChanel != null)
                {
                    affChName = affChanel.Name;
                }
                string[] names1 = {
                                    //(affresp.LeadId.HasValue ? affresp.LeadId.ToString() : "0"),
                                    affresp.Id.ToString(),
                                    "<p style='text-align: center'>"+String.Format("{0:MM/dd/yyyy <br> HH:mm:ss}", affresp.Created) + "</p>",
                                    "<p class='text-left'><a href='/Management/Affiliate/Item/" + affresp.AffiliateId.ToString() + "' title='" + affresp.AffiliateId.ToString() + "# " + affName + "'>" + affresp.AffiliateId.ToString() + "# " + affName + "</a></p>",
                                    "<p class='text-left'><a href='/Management/AffiliateChannel/Item/"+affresp.AffiliateChannelId.ToString()+"' title='" + affresp.AffiliateChannelId.ToString() + "# " + affChName + "'>" + affresp.AffiliateChannelId.ToString() + "# " + affChName + "</a></p>",
                                    "<textarea style='width:95%'>" + System.Net.WebUtility.HtmlEncode(affresp.Response) + "</textarea>",
                                    "<div class=\"no-margin text-bold alignright\">" + String.Format("{0:$###,###.00}", affresp.MinPrice) + "</div>"
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the error leads report ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetErrorLeadsReportAjax()
        {
            var permissionService = AppEngineContext.Current.Resolve<IPermissionService>();
            bool maskEmail = _appContext.AppUser == null ? true : _appContext.AppUser.MaskEmail;

            int page = 0;
            int pageSize = _leadModel.RowsPerPage > 0 ? _leadModel.RowsPerPage : 100;

            DateTime dateFrom = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToShortDateString());
            DateTime dateTo = Convert.ToDateTime(this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToShortDateString());

            //dateFrom = _settingService.GetTimeZoneDate(dateFrom);
            //dateTo = _settingService.GetTimeZoneDate(dateTo);

            long FilterLeadId = 0;
            long FilterAffiliate = 0;
            long FilterAffiliateChannel = 0;

            long FilterBuyer = 0;
            long FilterBuyerChannel = 0;
            long FilterCampaign = 0;

            decimal FilterMinPrice = 0;

            string FilterState = string.IsNullOrEmpty(Request["state"]) ? "" : Request["state"];

            short ErrorType = 0;
            short Validator = 0;
            short reportType = 1;
            short FilterStatus = 0;

            int.TryParse(Request["page"], out page);
            int.TryParse(Request["pagesize"], out pageSize);
            if (pageSize == 0) pageSize = 100;

            long.TryParse(Request["leadid"], out FilterLeadId);
            short.TryParse(Request["error"], out ErrorType);
            short.TryParse(Request["validator"], out Validator);
            short.TryParse(Request["reportType"], out reportType);
            short.TryParse(Request["status"], out FilterStatus);
            //long.TryParse(Request["affiliate"], out FilterAffiliate);
            long.TryParse(Request["affiliatechannel"], out FilterAffiliateChannel);
            long.TryParse(Request["buyer"], out FilterBuyer);
            long.TryParse(Request["buyerchannel"], out FilterBuyerChannel);
            long.TryParse(Request["campaign"], out FilterCampaign);
            decimal.TryParse(Request["minprice"], out FilterMinPrice);

            if (Request["dates"] != null)
            {
                var dates = Request["dates"].Split(':');
                dateFrom = Convert.ToDateTime(dates[0] + " 00:00:00");
                dateTo = Convert.ToDateTime(dates[1] + " 23:59:59");
            }

            //dateFrom = _settingService.GetUTCDate(dateFrom);
            //dateTo = _settingService.GetUTCDate(dateTo);

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            List<ReportErrorLeads> bipcList = (List<ReportErrorLeads>)this._leadMainService.GetErrorLeadsReport(ErrorType, Validator, dateFrom, dateTo, FilterLeadId, FilterStatus, FilterAffiliate, FilterAffiliateChannel, FilterBuyer, FilterBuyerChannel, FilterCampaign, FilterState, FilterMinPrice, reportType, (page - 1) * pageSize, pageSize);

            JsonData jd = new JsonData();
            jd.draw = 0;
            jd.recordsTotal = this._leadMainService.GetErrorLeadsReportCount(ErrorType, Validator, dateFrom, dateTo, FilterLeadId, FilterStatus, FilterAffiliate, FilterAffiliateChannel, FilterBuyer, FilterBuyerChannel, FilterCampaign, FilterState, FilterMinPrice, reportType);
            jd.recordsFiltered = 0;
            jd.TimeZoneNowStr = this._settingService.GetTimeZoneDate(DateTime.UtcNow, _appContext.AppUser).ToString("MM/dd/yyyy");

            foreach (ReportErrorLeads bipc in bipcList)
            {
                string errorType = "None";

                if (bipc.ErrorType.HasValue)
                {
                    switch (bipc.ErrorType.Value)
                    {
                        case 1: errorType = "Unknown"; break;
                        case 2: errorType = "No Data"; break;
                        case 3: errorType = "Invalid Data"; break;
                        case 4: errorType = "Unknown DB Field"; break;
                        case 5: errorType = "Missing Value"; break;
                        case 6: errorType = "Missing Field"; break;
                        case 7: errorType = "NotExisting DB Record"; break;
                        case 8: errorType = "Dropped"; break;
                        case 9: errorType = "Daily Cap Reached"; break;
                        case 10: errorType = "Integration Error"; break;
                        case 11: errorType = "Filter Error"; break;
                        case 12: errorType = "Not Enough Balance"; break;
                        case 13: errorType = "Schedule Cap Limit"; break;
                        case 14: errorType = "Min Price Error"; break;
                        case 17: errorType = "Timeout"; break;
                    }
                }
                string statusStr = "None";

                switch (bipc.Status)
                {
                    case 1: statusStr = "<div class='status-label label label-success'>Sold</div>"; break;
                    case 2: statusStr = "<div class='status-label label label-error'>Error</div>"; break;
                    case 3: statusStr = "<div class='status-label label label-danger'>Reject</div>"; break;
                }

                string affName = bipc.AffiliateName;
                string Response = bipc.Message;

                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(bipc.Response);
                    XmlNodeList nd = xmldoc.DocumentElement.GetElementsByTagName("message");
                    if (nd.Count > 0)
                        Response = nd[0].InnerText;
                }
                catch
                {
                    Response = bipc.Message;
                }

                string affiliateChannelName = bipc.AffiliateChannelName.Length > 20 ? bipc.AffiliateChannelName.Substring(0, 20) + "..." : bipc.AffiliateChannelName;
                string buyerChannelName = bipc.BuyerChannelName.Length > 20 ? bipc.BuyerChannelName.Substring(0, 20) + "..." : bipc.BuyerChannelName;
                string campaignName = bipc.CampaignName.Length > 20 ? bipc.CampaignName.Substring(0, 20) + "..." : bipc.CampaignName;

                if (reportType == 1)
                {
                    string[] names1 = {
                                      permissionService.Authorize(PermissionProvider.LeadsIinfoView) ? "<p style='text-align: center'><a class='idhref' id='" + bipc.LeadId.ToString() + "' href='#'><b>" + bipc.LeadId.ToString() + "</b></a></p>" : "<b>" + bipc.LeadId.ToString() + "</b> ",
                                      "<p style='text-align: center'>" + statusStr + "</p>",
                                      "<p style='text-align: center'>"+String.Format("{0:MM/dd/yyyy <br> HH:mm:ss}", _settingService.GetTimeZoneDate(bipc.Created)) + "</p>",
                                      /*reportType == 2 ? ("<p class='text-center' style='text-align: center'><a href='/Management/Affiliate/Item/" + bipc.AffiliateId.ToString() + "' title='" + bipc.AffiliateId.ToString() + "# " + affName + "'>" + affName + "</a></p>") :
                                                        ("<p class='text-center' style='text-align: center'><a href='/Management/Buyer/Item/" + bipc.BuyerId.ToString() + "' title='" + bipc.BuyerId.ToString() + "# " + bipc.BuyerName + "'>" + bipc.BuyerName + "</a></p>"),*/
                                      "<p style='text-align: center'>" + affiliateChannelName + "</p>",
                                      "<p style='text-align: center'>" + buyerChannelName + "</p>",
                                      "<p style='text-align: center'>" + campaignName + "</p>",
                                      "<p style='text-align: center'>" + bipc.State + "</p>",
                                      "<p style='text-align: center'>" + (Response.Length > 30 ? Response.Substring(0, 30) : Response) +  "</p>",
                                      "<p style='text-align: center'>" + errorType + "</p>",
                                      "<p style='text-align: center'>" + bipc.Minprice + "</p>"
                                };
                    jd.data.Add(names1);
                }
                else
                {
                    string[] names1 = {
                                      permissionService.Authorize(PermissionProvider.LeadsIinfoView) ? "<p style='text-align: center'><a class='idhref' id='" + bipc.LeadId.ToString() + "' href='#'><b>" + bipc.LeadId.ToString() + "</b></a></p>" : "<b>" + bipc.LeadId.ToString() + "</b> ",
                                      "<p style='text-align: center'>"+String.Format("{0:MM/dd/yyyy <br> HH:mm:ss}", _settingService.GetTimeZoneDate(bipc.Created)) + "</p>",
                                      /*reportType == 2 ? ("<p class='text-center' style='text-align: center'><a href='/Management/Affiliate/Item/" + bipc.AffiliateId.ToString() + "' title='" + bipc.AffiliateId.ToString() + "# " + affName + "'>" + affName + "</a></p>") :
                                                        ("<p class='text-center' style='text-align: center'><a href='/Management/Buyer/Item/" + bipc.BuyerId.ToString() + "' title='" + bipc.BuyerId.ToString() + "# " + bipc.BuyerName + "'>" + bipc.BuyerName + "</a></p>"),*/
                                      "<p style='text-align: center'>" + affiliateChannelName + "</p>",
                                      "<p style='text-align: center'>" + bipc.State + "</p>",
                                      "<p style='text-align: center'>" + Response + "</p>",
                                      "<p style='text-align: center'>" + errorType + "</p>"
                                };
                    jd.data.Add(names1);
                }
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the lead notes ajax.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult GetLeadNotesAjax()
        {
            long leadId = long.Parse(Request["leadid"]);
            JsonData jd = new JsonData();

            IList<LeadNote> leadNotesList = this._noteTitleService.GetLeadNotes(leadId);
            foreach (LeadNote ln in leadNotesList)
            {
                NoteTitle nt = this._noteTitleService.GetNoteTitleById(ln.NoteTitleId);
                string[] names1 = {
                                    ln.Created.ToString(),
                                    (nt != null ? nt.Title : "Pending contact"),
                                    ln.Note,
                                    (string.IsNullOrEmpty(ln.Author) ? "" : ln.Author)
                                };
                jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the lead note.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult AddLeadNote()
        {
            LeadNote lNote = new LeadNote();

            lNote.LeadId = long.Parse(Request["leadid"]);
            lNote.Note = Request["note"];
            lNote.Author = Request["author"];
            lNote.NoteTitleId = short.Parse(Request["titleid"]);
            lNote.Created = DateTime.Now;

            this._noteTitleService.InsertLeadNote(lNote);
            /*
                        LeadNote ln = this._noteTitleService.GetLeadNote(long.Parse(Request["leadid"]));
                        if (ln != null)
                        {
                            ln.Note = lNote.Note;
                            ln.NoteTitleId = lNote.NoteTitleId;
                            this._noteTitleService.UpdateLeadNote(ln);
                        }
                        else
                        {
                            this._noteTitleService.InsertLeadNote(lNote);
                        }
            */

            LeadMain leadMain = _leadMainService.GetLeadMainById(lNote.LeadId);
            if (leadMain != null)
            {
                leadMain.UpdateDate = DateTime.UtcNow;
                _leadMainService.UpdateLeadMain(leadMain);
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the notes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        public ActionResult SaveNotes()
        {
            LeadNote lNote = new LeadNote();

            string[] notes = Request["notes"].Split(';');

            NoteTitle nt = new NoteTitle();
            foreach (string s in notes)
            {
                if (s != "")
                {
                    nt = this._noteTitleService.GetNoteTitleById(long.Parse(s.Split(':')[0]));
                    nt.Title = s.Split(':')[1];
                    this._noteTitleService.UpdateNoteTitle(nt);
                }
            }

            return Json(1, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Export2s the CSV.
        /// </summary>
        /// <param name="strList">The string list.</param>
        /// <returns>System.String.</returns>
        public string Export2CSV(List<string[]> strList)
        {
            string fileName = "Leads_" + _appContext.AppUser.Id.ToString() + "_" + String.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now) + ".csv";
            var path = Path.Combine(Server.MapPath("~/Downloads"), fileName);

            int i = 1;

            string fileBody = "";

            foreach (string[] ss in strList)
            {
                foreach (string s in ss)
                {
                    fileBody += s + ";";
                }
                i++;
                fileBody += "\r\n";
            }

            System.IO.File.WriteAllText(path, fileBody);

            return fileName;
        }

        /// <summary>
        /// Saves the columns visibility.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [ContentManagementAntiForgery(true)]
        public ActionResult SaveColumnsVisibility()
        {
            Uri uri = new Uri(Helper.GetBaseUrl(Request));

            HttpCookie compareCookie = _httpContext.Request.Cookies.Get("AdrackColumnVisibility-" + uri.Host + "-" + _appContext.AppUser.Id.ToString());

            if (compareCookie == null)
            {
                compareCookie = new HttpCookie("AdrackColumnVisibility-" + uri.Host + "-" + _appContext.AppUser.Id.ToString());
                compareCookie.HttpOnly = true;
                compareCookie.Expires = DateTime.MaxValue;
                compareCookie.Value = Request["columns"];
                _httpContext.Response.Cookies.Add(compareCookie);
            }
            else
            {
                compareCookie.Value = Request["columns"];
                _httpContext.Response.Cookies.Set(compareCookie);
            }

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
    }
}