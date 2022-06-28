// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ReportController.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Configuration;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Data;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Localization;
using Adrack.Web.ContentManagement.Models.Lead.Reports;
using Adrack.Web.Framework;
using Adrack.Web.Framework.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Adrack.Web.ContentManagement.Controllers
{
    /// <summary>
    /// Class ReportController.
    /// Implements the <see cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// Implements the <see cref="System.Web.Mvc.IController" />
    /// </summary>
    /// <seealso cref="Adrack.Web.ContentManagement.Controllers.BaseContentManagementController" />
    /// <seealso cref="System.Web.Mvc.IController" />
    public partial class ReportController : BaseContentManagementController, IController
    {
        /// <summary>
        /// Class ReportCSVGenerator.
        /// </summary>
        private class ReportCSVGenerator
        {
            /// <summary>
            /// The instance
            /// </summary>
            public static ReportCSVGenerator Instance = new ReportCSVGenerator();

            /// <summary>
            /// Gets or sets the builder.
            /// </summary>
            /// <value>The builder.</value>
            public StringBuilder Builder { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ReportCSVGenerator"/> class.
            /// </summary>
            public ReportCSVGenerator()
            {
                Builder = new StringBuilder();
            }

            /// <summary>
            /// Initializes the specified columns.
            /// </summary>
            /// <param name="columns">The columns.</param>
            public void Init(string columns)
            {
                Builder.Clear();
                Builder.AppendLine(columns);
            }

            /// <summary>
            /// Adds the row.
            /// </summary>
            /// <param name="row">The row.</param>
            public void AddRow(string row)
            {
                Builder.AppendLine(row);
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return Builder.ToString();
            }
        }

        #region Fields

        /// <summary>
        /// The application context
        /// </summary>
        private readonly IAppContext _appContext;

        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly IReportService _reportService;

        /// <summary>
        /// The buyer service
        /// </summary>
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// The campaign service
        /// </summary>
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        /// <summary>
        /// The affiliate channel service
        /// </summary>
        private readonly IAffiliateChannelService _affiliateChannelService;

        /// <summary>
        /// The buyer channel service
        /// </summary>
        private readonly IBuyerChannelService _buyerChannelService;

        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;

        /// <summary>
        /// The lead schedule service
        /// </summary>
        private readonly IBuyerChannelScheduleService _buyerChannelScheduleService;

        /// <summary>
        /// The country province service
        /// </summary>
        private readonly ICountryService _countryService;

        /// <summary>
        /// The state province service
        /// </summary>
        private readonly IStateProvinceService _stateProvinceService;

        /// <summary>
        /// The localized string service
        /// </summary>
        private readonly ILocalizedStringService _localizedStringService;

        /// <summary>
        /// The note title service
        /// </summary>
        private readonly INoteTitleService _noteTitleService;

        private readonly ILeadMainService _leadMainService;



        #endregion Fields

        #region Constructor

        /// <summary>
        /// Directory Controller
        /// </summary>
        /// <param name="appContext">Application Context</param>
        /// <param name="reportService">The report service.</param>
        /// <param name="buyerService">The buyer service.</param>
        /// <param name="campaignSerice">The campaign serice.</param>
        /// <param name="affiliateService">The affiliate service.</param>
        /// <param name="buyerChannelService">The buyer channel service.</param>
        /// <param name="settingService">The setting service.</param>
        /// <param name="buyerChannelScheduleService">The lead schedule service.</param>
        /// <param name="localizedStringService">Localized String Service</param>
        /// <param name="stateProvinceService">State Province Service</param>
        /// <param name="noteTitleService">The note title service.</param>
        /// <param name="callCenterSettingService">The call center setting service.</param>
        /// <param name="affiliateChannelService">The affiliate channel service.</param>

        public ReportController(IAppContext appContext, 
            IReportService reportService, 
            IBuyerService buyerService, 
            ICampaignService campaignSerice, 
            IAffiliateService affiliateService, 
            IBuyerChannelService buyerChannelService, 
            ISettingService settingService, 
            IBuyerChannelScheduleService buyerChannelScheduleService, 
            ILocalizedStringService localizedStringService, 
            IStateProvinceService stateProvinceService, 
            INoteTitleService noteTitleService, 
            IAffiliateChannelService affiliateChannelService, 
            ICountryService countryService,
            ILeadMainService leadMainService)
        {
            this._appContext = appContext;
            this._reportService = reportService;
            this._buyerService = buyerService;
            this._campaignService = campaignSerice;
            this._affiliateService = affiliateService;
            this._settingService = settingService;
            this._buyerChannelScheduleService = buyerChannelScheduleService;
            this._buyerChannelService = buyerChannelService;
            this._localizedStringService = localizedStringService;
            this._stateProvinceService = stateProvinceService;
            this._countryService = countryService;
            this._noteTitleService = noteTitleService;            
            this._affiliateChannelService = affiliateChannelService;
            this._leadMainService = leadMainService;
        }

        #endregion Constructor

        /// <summary>
        /// Prepares the buyer report model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="buyerId">The buyer identifier.</param>
        protected void PrepareBuyerReportModel(BuyerReportModel model, long buyerId)
        {
            model.BaseUrl = Helper.GetBaseUrl(Request);
            Buyer buyer = null;

            if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
            }
            else
            {
                buyer = _buyerService.GetBuyerById(buyerId);
            }

            if (buyer != null)
                model.AlwaysSoldOption = buyer.AlwaysSoldOption;

            List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(0);

            foreach (var b in buyers)
            {
                if (buyerId > 0 && b.Id != buyerId) continue;
                model.ListBuyers.Add(new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            }

            List<BuyerChannel> channels;

            if (buyerId != 0)
                channels = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannelsByBuyerId(buyerId, 0);
            else
                channels = (List<BuyerChannel>)_buyerChannelService.GetAllBuyerChannels(0);

            List<long> campaignsAdded = new List<long>();
            List<long> achanelsAdded = new List<long>();
            List<string> allowedAffiliateChannels = new List<string>();


            foreach (var ch in channels)
            {
                Campaign campaign = _campaignService.GetCampaignById(ch.CampaignId);
                if (campaign != null && !campaignsAdded.Contains(campaign.Id))
                {
                    campaignsAdded.Add(campaign.Id);
                    model.ListCampaigns.Add(new SelectListItem() { Text = campaign.Name, Value = campaign.Id.ToString() });
                }
                allowedAffiliateChannels.Add(ch.AllowedAffiliateChannels);
                model.ListBuyerChannels.Add(new SelectListItem() { Text = ch.Name, Value = ch.Id.ToString() });
            }

            List<AffiliateChannel> achannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannels(0);
            foreach (AffiliateChannel c in achannels)
            {
                bool contains = false;
                foreach(string al in allowedAffiliateChannels)
                {
                    if (!string.IsNullOrEmpty(al) && al.Contains(":" + c.Id + ";"))
                    {
                        contains = true;
                        break;
                    }
                }


                if (!contains)
                {
                    continue;
                }
                model.ListAffiliateChannels.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            foreach (var value in _countryService.GetAllCountries())
            {
                model.ListCountry.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Id.ToString(),
                    Selected = value.Id == 80
                });
            }

            foreach (var value in _stateProvinceService.GetStateProvinceByCountryId(80))
            {
                model.ListStates.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Code
                });
            }

            List<NoteTitle> noteTitleList = (List<NoteTitle>)_noteTitleService.GetAllNoteTitlesSorted();

            foreach (NoteTitle n in noteTitleList)
            {
                if (n.Title.Length == 0) continue;
                model.ListNoteTitles.Add(new SelectListItem() { Text = n.Title, Value = n.Id.ToString() });
            }
        }

        /// <summary>
        /// Prepares the affiliate report model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="affiliateId">The affiliate identifier.</param>
        protected void PrepareAffiliateReportModel(AffiliateReportModel model, long affiliateId)
        {
            model.BaseUrl = Helper.GetBaseUrl(Request);

            List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(0);

            foreach (var b in affiliates)
            {
                if (_appContext.AppUser != null && 
                    _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId && 
                    _appContext.AppUser.ParentId != b.Id)
                {
                    continue;
                }

                model.ListAffiliates.Add(new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            }

            List<AffiliateChannel> achannels = (List<AffiliateChannel>)_affiliateChannelService.GetAllAffiliateChannels(0);
            foreach (AffiliateChannel c in achannels)
            {
                if (_appContext.AppUser != null &&
                    _appContext.AppUser.UserType == SharedData.AffiliateUserTypeId &&
                    _appContext.AppUser.ParentId != c.AffiliateId)
                {
                    continue;
                }
                model.ListAffiliateChannels.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            foreach (var value in _countryService.GetAllCountries())
            {
                model.ListCountry.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Id.ToString(),
                    Selected = value.Id == 80
                });
            }

            foreach (var value in _stateProvinceService.GetStateProvinceByCountryId(80))
            {
                model.ListStates.Add(new SelectListItem
                {
                    Text = value.GetLocalized(x => x.Name),
                    Value = value.Code
                });
            }
        }

        /// <summary>
        /// Reports the buyers.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Buyer reports")]
        public ActionResult ReportBuyers(long buyerId = 0)
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            ViewBag.TimeZoneNow = this._settingService.GetTimeZoneDate(DateTime.UtcNow);

            BuyerReportModel m = new BuyerReportModel();
            PrepareBuyerReportModel(m, buyerId);

            return View(m);
        }

        /// <summary>
        /// Reports the buyers partial.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersPartial(long id = 0)
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            if (_appContext.AppUser != null && _appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                id = _appContext.AppUser.ParentId;
            }

            BuyerReportModel model = new BuyerReportModel();
            model.BuyerId = id;

            PrepareBuyerReportModel(model, id);

            return PartialView("ReportBuyers", model);
        }

        /// <summary>
        /// Reports the affiliates.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Affiliate reports")]
        public ActionResult ReportAffiliates()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            ViewBag.TimeZoneNow = this._settingService.GetTimeZoneDate(DateTime.UtcNow);

            AffiliateReportModel model = new AffiliateReportModel();

            model.AffiliateId = 0;

            PrepareAffiliateReportModel(model, 0);

            return View(model);
        }

        /// <summary>
        /// Reports the affiliates partial.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportAffiliatesPartial(long id = 0)
        {
            AffiliateReportModel model = new AffiliateReportModel();
            model.AffiliateId = id;

            PrepareAffiliateReportModel(model, model.AffiliateId);

            return PartialView("ReportAffiliates", model);
        }

        /// <summary>
        /// Reports the admin.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>ActionResult.</returns>
        [NavigationBreadCrumb(Clear = true, Label = "Admin reports")]
        public ActionResult ReportAdmin(long buyerId = 0)
        {
            AdminReportModel m = new AdminReportModel();
            m.BaseUrl = Helper.GetBaseUrl(Request);

            return View(m);
        }

        /// <summary>
        /// Gets the date ranges from string.
        /// </summary>
        /// <param name="startDateStr">The start date string.</param>
        /// <param name="endDateStr">The end date string.</param>
        /// <param name="convertToUtc">if set to <c>true</c> [convert to UTC].</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        protected void GetDateRangesFromString(string startDateStr, string endDateStr, bool convertToUtc, out DateTime startDate, out DateTime endDate)
        {
            startDate = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            endDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 23, 59, 59);

            try
            {
                startDate = Convert.ToDateTime(startDateStr);
                startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
                endDate = Convert.ToDateTime(endDateStr);
                endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            }
            catch (Exception ex)
            {
                //Arman Handle Exception
                LogException(ex);
            }

            if (convertToUtc)
            {
                startDate = _settingService.GetUTCDate(startDate);
                endDate = _settingService.GetUTCDate(endDate);
            }
        }

        /// <summary>
        /// Buyers the report by buyer channel.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult BuyerReportByBuyerChannel()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();
            PrepareBuyerReportModel(model, 0);

            return PartialView(model);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem> GetBuyerReportByBuyerChannelData(
            DateTime startDate, DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem>();

            List<BuyerReportByBuyerChannel> report = (List<BuyerReportByBuyerChannel>)this._reportService.BuyerReportByBuyerChannels(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            long buyerChannelId = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem totals = new BuyerReportByBuyerChannelModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.BuyerChannelId = 0;
            totals.BuyerChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.LoanedLeads = 0;
            totals.Cost = 0;
            totals.Cap = 0;
            totals.AcceptRate = 0;
            totals.AveragePrice = 0;
            totals.Profit = 0;
            totals.Rank = 0;
            totals.CapHit = false;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem prevItem = null;

            foreach (BuyerReportByBuyerChannel r in report)
            {
                if (r.BuyerChannelId != buyerChannelId)
                {
                    if (prevItem != null)
                    {
                        if (startDate.Year == endDate.Year && startDate.Month == endDate.Month && startDate.Day == endDate.Day)
                        {
                            List<BuyerChannelSchedule> schedule = (List<BuyerChannelSchedule>)_buyerChannelScheduleService.GetBuyerChannelsByBuyerChannelId(buyerChannelId);

                            foreach (BuyerChannelSchedule s in schedule)
                            {
                                if (s.DayValue == ((short)startDate.DayOfWeek + 1))
                                {
                                    //parent.Cap += s.Quantity;
                                    prevItem.Cap += s.Quantity;
                                    totals.Cap += s.Quantity;

                                    if (s.LeadStatus.HasValue)
                                    {
                                        switch (s.LeadStatus.Value)
                                        {
                                            case -1: if (prevItem.Cap < prevItem.TotalLeads) prevItem.CapHit = true; break;
                                            case 1: if (prevItem.Cap < prevItem.SoldLeads) prevItem.CapHit = true; break;
                                            case 3: if (prevItem.Cap < prevItem.RejectedLeads) prevItem.CapHit = true; break;
                                        }
                                    }
                                }
                            }
                        }

                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.BuyerChannelName, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.AcceptRate));
                    }

                    item = new Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem();
                    item.title = r.BuyerChannelName;
                    item.folder = true;
                    item.expanded = true;

                    item.BuyerId = r.BuyerId;
                    item.BuyerName = r.BuyerName;
                    item.BuyerChannelId = r.BuyerChannelId;
                    item.BuyerChannelName = r.BuyerChannelName;

                    item.TotalLeads = 0;
                    item.SoldLeads = 0;
                    item.RejectedLeads = 0;
                    item.LoanedLeads = 0;
                    item.Cost = 0;
                    item.Cap = 0;
                    item.AveragePrice = 0;
                    item.Profit = 0;
                    item.CapHit = false;
                    item.Rank = 0;
                    item.CapHit = false;
                    item.Redirected = 0;
                    item.RedirectedRate = 0;

                    buyers.Add(item);

                    buyerChannelId = r.BuyerChannelId;
                }

                Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem b = item;

                b.title = "<a href='/Management/BuyerChannel/Item/" + r.BuyerChannelId.ToString() + "' target='_blank'>" + r.BuyerChannelId.ToString() + "-" + r.BuyerChannelName + "</a>";
                b.folder = false;
                b.expanded = true;

                b.BuyerId = r.BuyerId;
                b.BuyerName = r.BuyerName;
                b.BuyerChannelId = r.BuyerChannelId;
                b.BuyerChannelName = r.BuyerChannelName;

                b.TotalLeads += r.TotalLeads;
                b.SoldLeads += r.SoldLeads;
                b.RejectedLeads += r.RejectedLeads;
                b.Redirected += r.Redirected;
                b.LoanedLeads += r.LoanedLeads;
                b.Cost += r.Cost;

                b.Rank = r.OrderNum;
                b.Profit += (r.Cost - r.AffiliatePrice);
                b.AveragePrice += r.AveragePrice;

                b.AcceptRate = (b.TotalLeads > 0 ? Math.Round(((decimal)b.SoldLeads / (decimal)b.TotalLeads * (decimal)100.0), 2) : 0);
                b.RedirectedRate = (b.SoldLeads > 0 ? Math.Round(((decimal)b.Redirected / (decimal)b.SoldLeads * (decimal)100.0), 2) : 0);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.LoanedLeads += r.LoanedLeads;
                totals.Cost += r.Cost;

                totals.Profit += (r.Cost - r.AffiliatePrice);
                totals.AveragePrice += r.AveragePrice;

                totals.AcceptRate = (totals.TotalLeads > 0 ? Math.Round(((decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0), 2) : 0);
                totals.RedirectedRate = (totals.SoldLeads > 0 ? Math.Round(((decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0), 2) : 0);

                prevItem = item;
            }

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.BuyerChannelName, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.AcceptRate));
            }

            buyers.Add(totals);

            return buyers;
        }

        /// <summary>
        /// Gets the buyer report by buyer channel.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBuyerReportByBuyerChannel()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Channel,Number of posted leads,Number of sold leads,Money made,Accept rate");

            string buyerIds = Request["buyerIds"];
            string buyerChannelIds = Request["buyerChannelIds"];
            string affiliateIds = Request["affiliateIds"];
            string campaignIds = Request["campaignIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetBuyerReportByBuyerChannelData(startDate, endDate, buyerIds, buyerChannelIds, affiliateIds, campaignIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "BuyerReportByBuyerChannel.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by hour.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByHour()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();

            List<Campaign> campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            foreach (var c in campaigns)
            {
                m.ListCampaigns.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            return View(m);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem> GetReportBuyersByHourData(
            DateTime date1, DateTime date2, DateTime date3, string buyerChannelIds, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem>();

            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem totals = new BuyerReportByHourModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.BuyerChannelId = 0;
            totals.BuyerChannelName = "";
            totals.TotalLeads1 = 0;
            totals.SoldLeads1 = 0;
            totals.LoanedLeads1 = 0;
            totals.Cost1 = 0;
            totals.Cap1 = 0;
            totals.AcceptRate1 = 0;
            totals.TotalLeads2 = 0;
            totals.SoldLeads2 = 0;
            totals.LoanedLeads2 = 0;
            totals.Cost2 = 0;
            totals.Cap2 = 0;
            totals.AcceptRate2 = 0;
            totals.TotalLeads2 = 0;
            totals.SoldLeads2 = 0;
            totals.LoanedLeads2 = 0;
            totals.Cost2 = 0;
            totals.Cap2 = 0;
            totals.AcceptRate2 = 0;
            totals.TotalLeads3 = 0;
            totals.SoldLeads3 = 0;
            totals.LoanedLeads3 = 0;
            totals.Cost3 = 0;
            totals.Cap3 = 0;
            totals.AcceptRate3 = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem prevItem = null;

            Dictionary<int, Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem> hours = new Dictionary<int, Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem>();
            for (int i = 0; i <= 23; i++)
            {
                string t = (i.ToString().Length < 2 ? "0" + i.ToString() : i.ToString()) + ":00";
                item = new BuyerReportByHourModel.TreeItem() { title = t, Hour = i, SoldLeads1 = 0, TotalLeads1 = 0, LoanedLeads1 = 0, SoldLeads2 = 0, TotalLeads2 = 0, LoanedLeads2 = 0, SoldLeads3 = 0, TotalLeads3 = 0, LoanedLeads3 = 0 };
                hours.Add(i, item);
                buyers.Add(item);
            }

            List<BuyerReportByHour> report = (List<BuyerReportByHour>)this._reportService.BuyerReportByHour(date1, (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));
            foreach (BuyerReportByHour r in report)
            {
                Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem b = hours[r.Hour];

                string t = (r.Hour.ToString().Length < 2 ? "0" + r.Hour.ToString() : r.Hour.ToString()) + ":00";

                b.title = t;
                b.folder = false;
                b.expanded = true;

                b.BuyerId = r.BuyerId;
                b.BuyerName = r.BuyerName;
                b.BuyerChannelId = r.BuyerChannelId;
                b.BuyerChannelName = r.BuyerChannelName;
                b.Hour = r.Hour;

                b.TotalLeads1 += r.TotalLeads;
                b.SoldLeads1 += r.SoldLeads;
                b.LoanedLeads1 += r.LoanedLeads;
                b.Cost1 += r.Cost;

                b.AcceptRate1 = (b.TotalLeads1 > 0 ? Math.Round(((decimal)b.SoldLeads1 / (decimal)b.TotalLeads1 * (decimal)100.0), 2) : 0);

                totals.TotalLeads1 += r.TotalLeads;
                totals.SoldLeads1 += r.SoldLeads;
                totals.LoanedLeads1 += r.LoanedLeads;
                totals.Cost1 += r.Cost;

                totals.AcceptRate1 = (totals.TotalLeads1 > 0 ? Math.Round(((decimal)totals.SoldLeads1 / (decimal)totals.TotalLeads1 * (decimal)100.0), 2) : 0);

                List<BuyerChannelSchedule> schedule = (List<BuyerChannelSchedule>)_buyerChannelScheduleService.GetBuyerChannelsByBuyerChannelId(r.BuyerChannelId);

                foreach (BuyerChannelSchedule s in schedule)
                {
                    if (s.DayValue == ((short)date1.DayOfWeek + 1))
                    {
                        //parent.Cap += s.Quantity;
                        b.Cap1 += s.Quantity;
                        totals.Cap1 += s.Quantity;
                    }
                }

                prevItem = item;
            }

            report = (List<BuyerReportByHour>)this._reportService.BuyerReportByHour(date2, (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));
            foreach (BuyerReportByHour r in report)
            {
                Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem b = hours[r.Hour];

                string t = (r.Hour.ToString().Length < 2 ? "0" + r.Hour.ToString() : r.Hour.ToString()) + ":00";

                b.title = t;
                b.folder = false;
                b.expanded = true;

                b.BuyerId = r.BuyerId;
                b.BuyerName = r.BuyerName;
                b.BuyerChannelId = r.BuyerChannelId;
                b.BuyerChannelName = r.BuyerChannelName;
                b.Hour = r.Hour;

                b.TotalLeads2 += r.TotalLeads;
                b.SoldLeads2 += r.SoldLeads;
                b.LoanedLeads2 += r.LoanedLeads;
                b.Cost2 += r.Cost;

                b.AcceptRate2 = (b.TotalLeads2 > 0 ? Math.Round(((decimal)b.SoldLeads2 / (decimal)b.TotalLeads2 * (decimal)100.0), 2) : 0);

                totals.TotalLeads2 += r.TotalLeads;
                totals.SoldLeads2 += r.SoldLeads;
                totals.LoanedLeads2 += r.LoanedLeads;
                totals.Cost2 += r.Cost;

                totals.AcceptRate2 = (totals.TotalLeads2 > 0 ? Math.Round(((decimal)totals.SoldLeads2 / (decimal)totals.TotalLeads2 * (decimal)100.0), 2) : 0);

                List<BuyerChannelSchedule> schedule = (List<BuyerChannelSchedule>)_buyerChannelScheduleService.GetBuyerChannelsByBuyerChannelId(r.BuyerChannelId);

                foreach (BuyerChannelSchedule s in schedule)
                {
                    if (s.DayValue == ((short)date3.DayOfWeek + 1))
                    {
                        //parent.Cap += s.Quantity;
                        b.Cap2 += s.Quantity;
                        totals.Cap2 += s.Quantity;
                    }
                }

                prevItem = item;
            }

            report = (List<BuyerReportByHour>)this._reportService.BuyerReportByHour(date3, (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));
            foreach (BuyerReportByHour r in report)
            {
                Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem b = hours[r.Hour];

                string t = (r.Hour.ToString().Length < 2 ? "0" + r.Hour.ToString() : r.Hour.ToString()) + ":00";

                b.title = t;
                b.folder = false;
                b.expanded = true;

                b.BuyerId = r.BuyerId;
                b.BuyerName = r.BuyerName;
                b.BuyerChannelId = r.BuyerChannelId;
                b.BuyerChannelName = r.BuyerChannelName;
                b.Hour = r.Hour;

                b.TotalLeads3 += r.TotalLeads;
                b.SoldLeads3 += r.SoldLeads;
                b.LoanedLeads3 += r.LoanedLeads;
                b.Cost3 += r.Cost;

                b.AcceptRate3 = (b.TotalLeads3 > 0 ? Math.Round(((decimal)b.SoldLeads3 / (decimal)b.TotalLeads3 * (decimal)100.0), 2) : 0);

                totals.TotalLeads3 += r.TotalLeads;
                totals.SoldLeads3 += r.SoldLeads;
                totals.LoanedLeads3 += r.LoanedLeads;
                totals.Cost3 += r.Cost;

                totals.AcceptRate3 = (totals.TotalLeads3 > 0 ? Math.Round(((decimal)totals.SoldLeads3 / (decimal)totals.TotalLeads3 * (decimal)100.0), 2) : 0);

                List<BuyerChannelSchedule> schedule = (List<BuyerChannelSchedule>)_buyerChannelScheduleService.GetBuyerChannelsByBuyerChannelId(r.BuyerChannelId);

                foreach (BuyerChannelSchedule s in schedule)
                {
                    if (s.DayValue == ((short)date3.DayOfWeek + 1))
                    {
                        //parent.Cap += s.Quantity;
                        b.Cap3 += s.Quantity;
                        totals.Cap3 += s.Quantity;
                    }
                }

                prevItem = item;
            }

            buyers.Add(totals);

            for (int i = 0; i <= 23; i++)
            {
                string t = (i.ToString().Length < 2 ? "0" + i.ToString() : i.ToString()) + ":00";
                item = hours[i];
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6}", t, item.TotalLeads1, item.SoldLeads1, item.TotalLeads2, item.SoldLeads2, item.TotalLeads3, item.SoldLeads3));
            }

            return buyers;
        }

        /// <summary>
        /// Gets the report buyers by hour.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByHour()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByHourModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Hour,Total leads1,Sold leads1,Total leads2,Sold leads2,Total leads3,Sold leads3");

            string buyerChannelIds = Request["buyerChannels"];
            string campaignIds = Request["campaigns"];
            DateTime startDate1;
            DateTime endDate1;
            GetDateRangesFromString(Request["reportDate1"], Request["reportDate1"], false, out startDate1, out endDate1);


            DateTime startDate2;
            DateTime endDate2;
            GetDateRangesFromString(Request["reportDate2"], Request["reportDate2"], false, out startDate2, out endDate2);

            DateTime startDate3;
            DateTime endDate3;
            GetDateRangesFromString(Request["reportDate3"], Request["reportDate3"], false, out startDate3, out endDate3);

            buyers = GetReportBuyersByHourData(startDate1, startDate2, startDate3, buyerChannelIds, campaignIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "BuyerReportByBuyerChannel.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Buyers the report by buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult BuyerReportByBuyer()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();
            PrepareBuyerReportModel(model, 0);

            return PartialView(model);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem> GetBuyerReportByBuyerData(DateTime startDate,
            DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem>();

            List<BuyerReportByBuyerChannel> report = (List<BuyerReportByBuyerChannel>)this._reportService.BuyerReportByBuyerChannels(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            long buyerId = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem totals = new BuyerReportByBuyerChannelModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.BuyerChannelId = 0;
            totals.BuyerChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.LoanedLeads = 0;
            totals.Cost = 0;
            totals.Cap = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;
            totals.Profit = 0;
            totals.AveragePrice = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem prevItem = null;

            foreach (BuyerReportByBuyerChannel r in report)
            {
                DateTime? lastSoldDate = null;

                if (r.LastSoldDate.HasValue)
                    lastSoldDate = _settingService.GetTimeZoneDate(r.LastSoldDate.Value);

                if (r.BuyerId != buyerId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.LastSoldDate));
                    }

                    item = new Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem();
                    item.title = r.BuyerName;
                    item.folder = true;
                    item.expanded = true;

                    item.BuyerId = r.BuyerId;
                    item.BuyerName = r.BuyerName;
                    item.BuyerChannelId = r.BuyerChannelId;
                    item.BuyerChannelName = r.BuyerChannelName;
                    item.LastSoldDate = (lastSoldDate.HasValue ? lastSoldDate.Value.ToShortDateString() + " " + lastSoldDate.Value.ToShortTimeString() : "");

                    item.TotalLeads = 0;
                    item.SoldLeads = 0;
                    item.RejectedLeads = 0;
                    item.LoanedLeads = 0;
                    item.Cost = 0;
                    item.Cap = 0;
                    item.Redirected = 0;

                    buyers.Add(item);

                    buyerId = r.BuyerId;
                }

                Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem b = item;

                b.title = "<a href='/Management/Buyer/Item/" + r.BuyerId.ToString() + "' target='_blank'>" + r.BuyerId.ToString() + "-" + r.BuyerName + "</a>";
                b.folder = false;
                b.expanded = true;

                b.BuyerId = r.BuyerId;
                b.BuyerName = r.BuyerName;
                b.BuyerChannelId = r.BuyerChannelId;
                b.BuyerChannelName = r.BuyerChannelName;
                b.LastSoldDate = (lastSoldDate.HasValue ? lastSoldDate.Value.ToShortDateString() + " " + lastSoldDate.Value.ToShortTimeString() : "");

                b.TotalLeads += r.TotalLeads;
                b.SoldLeads += r.SoldLeads;
                b.RejectedLeads += r.RejectedLeads;
                b.LoanedLeads += r.LoanedLeads;
                b.Cost += r.Cost;
                b.Redirected += r.Redirected;
                b.RedirectedRate = (b.SoldLeads > 0 ? Math.Round(((decimal)b.Redirected / (decimal)b.SoldLeads * (decimal)100.0), 2) : 0);
                b.AcceptRate = (b.TotalLeads > 0 ? Math.Round(((decimal)b.SoldLeads / (decimal)b.TotalLeads * (decimal)100.0), 2) : 0);
                b.Profit += (r.Cost - r.AffiliatePrice);
                b.AveragePrice += r.AveragePrice;

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.LoanedLeads += r.LoanedLeads;
                totals.Cost += r.Cost;
                totals.Redirected += r.Redirected;
                totals.Profit += (r.Cost - r.AffiliatePrice);
                totals.AveragePrice += r.AveragePrice;

                totals.AcceptRate = (totals.TotalLeads > 0 ? Math.Round(((decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0), 2) : 0);
                totals.RedirectedRate = (totals.SoldLeads > 0 ? Math.Round(((decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0), 2) : 0);

                List<BuyerChannelSchedule> schedule = (List<BuyerChannelSchedule>)_buyerChannelScheduleService.GetBuyerChannelsByBuyerChannelId(r.BuyerChannelId);

                foreach (BuyerChannelSchedule s in schedule)
                {
                    if (s.DayValue == ((short)startDate.DayOfWeek + 1))
                    {
                        //parent.Cap += s.Quantity;
                        b.Cap += s.Quantity;
                        totals.Cap += s.Quantity;
                    }
                }

                prevItem = item;
            }

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.LastSoldDate));
            }

            buyers.Add(totals);

            return buyers;
        }

        /// <summary>
        /// Gets the buyer report by buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetBuyerReportByBuyer()
        {
            ReportCSVGenerator.Instance.Init("Buyer,Posted leads,Sold leads,Money made,Accept rate,Redirect rate");

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.BuyerReportByBuyerChannelModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            string buyerIds = Request["buyerIds"];
            string buyerChannelIds = Request["buyerChannelIds"];
            string affiliateIds = Request["affiliateIds"];
            string campaignIds = Request["campaignIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetBuyerReportByBuyerData(startDate, endDate, buyerIds, buyerChannelIds, affiliateIds, campaignIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyerByBuyer.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by campaigns.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByCampaigns()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();

            PrepareBuyerReportModel(model, 0);

            return PartialView(model);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem> GetReportBuyersByCampaignsData(DateTime startDate, 
            DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem>();

            List<ReportBuyersByCampaigns> report = (List<ReportBuyersByCampaigns>)this._reportService.ReportBuyersByCampaigns(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem totals = new ReportBuyersByCampaignsModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.CampaignName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Debit = 0;
            totals.Credit = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;

            foreach (ReportBuyersByCampaigns r in report)
            {
                Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem b = null;
                b = new Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem();
                b.title = r.CampaignName;
                b.folder = false;
                b.expanded = true;

                b.BuyerId = r.BuyerId;
                b.BuyerName = r.BuyerName;
                b.CampaignName = r.CampaignName;

                b.TotalLeads = r.TotalLeads;
                b.SoldLeads = r.SoldLeads;
                b.Debit = r.Debet;
                b.Credit = r.Credit;
                b.Redirected = r.Redirected;
                b.RedirectedRate = (b.SoldLeads > 0 ? Math.Round(((decimal)b.Redirected / (decimal)b.SoldLeads * (decimal)100.0), 2) : 0);


                b.AcceptRate = (b.TotalLeads > 0 ? Math.Round(((decimal)b.SoldLeads / (decimal)b.TotalLeads * (decimal)100.0), 2) : 0);
                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Debit += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.RedirectedRate = (totals.SoldLeads > 0 ? Math.Round(((decimal)totals.Redirected / (decimal)b.SoldLeads * (decimal)100.0), 2) : 0);

                totals.AcceptRate = (b.AcceptRate > 0 ? Math.Round(((decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0), 2) : 0);

                //parent.children.Add(b);
                buyers.Add(b);

                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", b.CampaignName, b.TotalLeads, b.SoldLeads, b.Debit, b.Credit));
            }

            buyers.Add(totals);

            return buyers;
        }

        /// <summary>
        /// Gets the report buyers by campaigns.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByCampaigns()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByCampaignsModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Buyer Name/Campaign name,Total leads,Sold leads,Debit,Credit");

            string buyerIds = Request["buyerIds"];
            string buyerChannelIds = Request["buyerChannelIds"];
            string affiliateIds = Request["affiliateIds"];
            string campaignIds = Request["campaignIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetReportBuyersByCampaignsData(startDate, endDate, buyerIds, buyerChannelIds, affiliateIds, campaignIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersByCampaigns.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by affiliate channels.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByAffiliateChannels()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();

            PrepareBuyerReportModel(model, 0);

            return PartialView(model);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem> GetReportBuyersByAffiliateChannelsData(DateTime startDate,
            DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem>();

            List<ReportBuyersByAffiliateChannels> report = (List<ReportBuyersByAffiliateChannels>)this._reportService.ReportBuyersByAffiliateChannels(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            long affiliateChannelId = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem totals = new ReportBuyersByAffiliateChannelsModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.AffiliateChannelId = 0;
            totals.AffiliateChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.Debet = 0;
            totals.Credit = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;
            totals.Profit = 0;
            totals.AveragePrice = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem prevItem = null;

            foreach (ReportBuyersByAffiliateChannels r in report)
            {
                if (r.AffiliateChannelId != affiliateChannelId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.AffiliateChannelName, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Debet, prevItem.AcceptRate));
                    }

                    affiliateChannelId = r.AffiliateChannelId;
                    item = new Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem();
                    buyers.Add(item);
                }

                item = buyers[buyers.Count - 1];

                item.title = r.AffiliateChannelName;
                item.folder = false;
                item.expanded = true;

                item.BuyerId = r.BuyerId;
                item.BuyerName = r.BuyerName;
                item.AffiliateChannelId = r.AffiliateChannelId;
                item.AffiliateChannelName = r.AffiliateChannelName;

                item.TotalLeads += r.TotalLeads;
                item.SoldLeads += r.SoldLeads;
                item.RejectedLeads += r.RejectedLeads;
                item.Debet += r.Debet;
                item.Credit += r.Credit;
                item.Redirected += r.Redirected;
                item.RedirectedRate = (item.SoldLeads > 0 ? Math.Round(((decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0), 2) : 0);
                item.Profit += (r.Debet - r.Credit);
                item.AveragePrice += r.AveragePrice;

                item.AcceptRate = (item.TotalLeads > 0 ? Math.Round(((decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0), 2) : 0);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.Debet += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected = r.Redirected;
                totals.RedirectedRate = (totals.SoldLeads > 0 ? Math.Round(((decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0), 2) : 0);
                totals.AcceptRate = (totals.TotalLeads > 0 ? Math.Round(((decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0), 2) : 0);
                totals.Profit += (r.Debet - r.Credit);
                totals.AveragePrice += r.AveragePrice;

                prevItem = item;
            }

            buyers.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.AffiliateChannelName, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Debet, prevItem.AcceptRate));
            }

            return buyers;
        }

        /// <summary>
        /// Gets the report buyers by affiliate channels.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByAffiliateChannels()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByAffiliateChannelsModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Buyer/Affiliate Channel,Total leads,Sold leads," + ((_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId) ? "Money Made" : "Amount of money spent") + ",Accept rate");

            string buyerIds = Request["buyerIds"];
            string buyerChannelIds = Request["buyerChannelIds"];
            string affiliateIds = Request["affiliateIds"];

            string campaignIds = Request["campaignIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetReportBuyersByAffiliateChannelsData(startDate, endDate, buyerIds, buyerChannelIds, affiliateIds, campaignIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersByAffiliateChannels.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByStates()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();
            PrepareBuyerReportModel(model, 0);
            return PartialView(model);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem> GetReportBuyersByStatesData(DateTime startDate, DateTime endDate,
            string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds, string stateIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem>();

            List<ReportBuyersByStates> report = (List<ReportBuyersByStates>)this._reportService.ReportBuyersByStates(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds), (string.IsNullOrEmpty(stateIds) || stateIds == "null" ? "''" : stateIds));

            string state = "";

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem totals = new ReportBuyersByStatesModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.State = "";

            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.Debit = 0;
            totals.Credit = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;
            totals.AveragePrice = 0;
            totals.Profit = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem prevItem = null;

            foreach (ReportBuyersByStates r in report)
            {
                if (state != r.State)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Debit, prevItem.AcceptRate));
                    }

                    item = new Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem();
                    item.title = r.State;
                    item.folder = false;
                    item.expanded = true;

                    item.BuyerId = r.BuyerId;
                    item.BuyerName = r.BuyerName;
                    item.State = r.State;

                    item.TotalLeads = r.TotalLeads;
                    item.SoldLeads = r.SoldLeads;
                    item.RejectedLeads = r.RejectedLeads;
                    item.Debit = r.Debet;
                    item.Credit = r.Credit;
                    item.Redirected = r.Redirected;
                    item.RedirectedRate = (item.SoldLeads > 0 ? Math.Round(((decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0), 2) : 0);
                    item.Profit = (r.Debet - r.Credit);
                    item.AveragePrice = r.AveragePrice;

                    buyers.Add(item);
                    state = r.State;
                }
                else
                {
                    item.TotalLeads += r.TotalLeads;
                    item.SoldLeads += r.SoldLeads;
                    item.RejectedLeads += r.RejectedLeads;
                    item.Debit += r.Debet;
                    item.Credit += r.Credit;
                    item.Redirected += r.Redirected;
                    item.RedirectedRate = (item.SoldLeads > 0 ? Math.Round(((decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0), 2) : 0);
                    item.Profit += (r.Debet - r.Credit);
                    item.AveragePrice += r.AveragePrice;
                }

                if (item != null)
                    item.AcceptRate = (item.TotalLeads > 0 ? Math.Round(((decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0), 2) : 0);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.Debit += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit += (r.Debet - r.Credit);
                totals.AveragePrice += r.AveragePrice;

                totals.RedirectedRate = (totals.SoldLeads > 0 ? Math.Round(((decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0), 2) : 0);
                totals.AcceptRate = (totals.TotalLeads > 0 ? Math.Round(((decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0), 2) : 0);

                prevItem = item;
            }

            buyers.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Debit, prevItem.AcceptRate));
            }

            return buyers;
        }

        /// <summary>
        /// Gets the report buyers by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByStates()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByStatesModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("State,Posted leads,Sold leads,Rejected leads," + ((_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId) ? "Money Made" : "Amount of money spent") + ",Accept rate");

            string buyerIds = Request["buyerIds"];
            string buyerChannelIds = Request["buyerChannelIds"];
            string affiliateIds = Request["affiliateIds"];
            string campaignIds = Request["campaignIds"];

            string stateIds = Request["sids"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetReportBuyersByStatesData(startDate, endDate, buyerIds, buyerChannelIds, affiliateIds, campaignIds, stateIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersByAffiliateChannels.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by reaction time.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByReactionTime()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();

            return PartialView(m);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem> GetReportBuyersByReactionTimeData(DateTime startDate, DateTime endDate, long buyerId, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem>();

            List<ReportBuyersByReactionTime> report = (List<ReportBuyersByReactionTime>)this._reportService.ReportBuyersByReactionTime(startDate, endDate, buyerId, (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            string date = "";

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem totals = new ReportBuyersByReactionTimeModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.LeadViews = 0;
            totals.MinElapsed = 0;
            totals.AvgElapsed = 0;
            totals.MaxElapsed = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem prevItem = null;

            foreach (ReportBuyersByReactionTime r in report)
            {
                if (date != r.Created.ToShortDateString())
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", prevItem.title, prevItem.MinElapsed, prevItem.AvgElapsed, prevItem.MaxElapsed));
                    }

                    item = new Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem();
                    item.title = r.Created.ToShortDateString();
                    item.folder = false;
                    item.expanded = true;

                    item.LeadViews = r.LeadViews;
                    item.MinElapsed = r.MinElapsed;
                    item.AvgElapsed = r.AvgElapsed;
                    item.MaxElapsed = r.MaxElapsed;

                    //                    parent.children.Add(b);
                    buyers.Add(item);
                    date = r.Created.ToShortDateString();
                }
                else
                {
                    item.LeadViews += r.LeadViews;
                    item.MinElapsed += r.MinElapsed;
                    item.AvgElapsed += r.AvgElapsed;
                    item.MaxElapsed += r.MaxElapsed;
                }

                /*parent.TotalLeads += r.TotalLeads;
                parent.SoldLeads += r.SoldLeads;
                parent.RejectedLeads += r.RejectedLeads;
                parent.Debit += r.Debet;
                parent.Credit += r.Credit;*/

                totals.LeadViews += r.LeadViews;
                totals.MinElapsed += r.MinElapsed;
                totals.AvgElapsed += r.AvgElapsed;
                totals.MaxElapsed += r.MaxElapsed;

                prevItem = item;
            }

            buyers.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", prevItem.title, prevItem.MinElapsed, prevItem.AvgElapsed, prevItem.MaxElapsed));
            }

            return buyers;
        }

        /// <summary>
        /// Gets the report buyers by reaction time.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByReactionTime()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByReactionTimeModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Date,Lead views,Min elapsed time,Avg elapsed,Max elapsed time");

            string buyerIdStr = Request["buyerid"];
            string campaignIds = Request["campaignIds"];

            long buyerId = 0;
            long.TryParse(buyerIdStr, out buyerId);

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetReportBuyersByReactionTimeData(startDate, endDate, buyerId, campaignIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersByReactionTime.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by dates.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByDates(long buyerId = 0)
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();
            PrepareBuyerReportModel(model, buyerId);

            return PartialView(model);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem> GetReportBuyersByDatesData(DateTime startDate, DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem>();

            List<ReportBuyersByDates> report = (List<ReportBuyersByDates>)this._reportService.ReportBuyersByDates(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            string date = "";

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem totals = new ReportBuyersByDatesModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.Date = "";

            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.LoanedLeads = 0;
            totals.Debit = 0;
            totals.Credit = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.AveragePrice = 0;
            totals.Profit = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem prevItem = null;

            foreach (ReportBuyersByDates r in report)
            {
                if (date != r.Date.ToShortDateString())
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.Debit, prevItem.AcceptRate, prevItem.RedirectedRate));
                    }

                    item = new Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem();
                    item.title = r.Date.ToShortDateString();
                    item.folder = false;
                    item.expanded = true;

                    item.Date = r.Date.ToShortDateString();

                    item.TotalLeads = r.TotalLeads;
                    item.SoldLeads = r.SoldLeads;
                    item.RejectedLeads = r.RejectedLeads;
                    item.LoanedLeads = r.LoanedLeads;
                    item.Debit = r.Debet;
                    item.Credit = r.Credit;
                    item.Redirected = r.Redirected;

                    item.Profit = (r.Debet - r.Credit);
                    item.AveragePrice = r.AveragePrice;

                    //parent.children.Add(b);
                    buyers.Add(item);
                    date = r.Date.ToShortDateString();
                }
                else
                {
                    item.TotalLeads += r.TotalLeads;
                    item.SoldLeads += r.SoldLeads;
                    item.RejectedLeads += r.RejectedLeads;
                    item.LoanedLeads += r.LoanedLeads;
                    item.Debit += r.Debet;
                    item.Credit += r.Credit;
                    item.Redirected += r.Redirected;

                    item.Profit += (r.Debet - r.Credit);
                    item.AveragePrice += r.AveragePrice;
                }

                /*parent.TotalLeads += r.TotalLeads;
                parent.SoldLeads += r.SoldLeads;
                parent.LoanedLeads += r.LoanedLeads;
                parent.Debit += r.Debet;
                parent.Credit += r.Credit;*/

                if (item != null)
                {
                    item.AcceptRate = (item.TotalLeads > 0 ? Math.Round(((decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0), 2) : 0);
                    item.RedirectedRate = (item.SoldLeads > 0 ? Math.Round(((decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0), 2) : 0);
                }

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.LoanedLeads += r.LoanedLeads;
                totals.Debit += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;

                totals.Profit += (r.Debet - r.Credit);
                totals.AveragePrice += r.AveragePrice;

                totals.AcceptRate = (totals.SoldLeads > 0 ? Math.Round(((decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0), 2) : 0);
                totals.RedirectedRate = (totals.SoldLeads > 0 ? Math.Round(((decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0), 2) : 0);

                prevItem = item;
            }

            buyers.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Debit, prevItem.AcceptRate, prevItem.RedirectedRate));
            }

            return buyers;
        }

        /// <summary>
        /// Gets the report buyers by dates.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByDates()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByDatesModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Date,Post leads,Sold leads," + ((_appContext.AppUser.UserType == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserType == SharedData.NetowrkUserTypeId) ? "Money Made" : "Amount of money spent") + ",Accept rate,Redirect rate");

            string buyerIds = Request["buyerIds"];
            string buyerChannelIds = Request["buyerChannelIds"];
            string affiliateIds = Request["affiliateIds"];
            string campaignIds = Request["campaignIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetReportBuyersByDatesData(startDate, endDate, buyerIds, buyerChannelIds, affiliateIds, campaignIds);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersByDates.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by lead notes.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByLeadNotes(long buyerId = 0)
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();
            PrepareBuyerReportModel(model, buyerId);

            return PartialView(model);
        }

        protected List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem> GetReportBuyersByLeadNotesData(DateTime startDate, DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem>();

            List<ReportBuyersByLeadNotes> report = (List<ReportBuyersByLeadNotes>)this._reportService.ReportBuyersByLeadNotes(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            string buyerName = "";
            string buyerChannelName = "";
            string date = "";

            short type = 1;
            if (!short.TryParse(Request["type"], out type)) type = 1;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem totals = new ReportBuyersByLeadNotesModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.Quantity1 = 0;
            totals.Quantity2 = 0;
            totals.Quantity3 = 0;
            totals.Quantity4 = 0;
            totals.Quantity5 = 0;
            totals.Quantity6 = 0;
            totals.Quantity7 = 0;
            totals.Quantity8 = 0;
            totals.Quantity9 = 0;
            totals.Quantity10 = 0;
            totals.Quantity11 = 0;
            totals.Quantity12 = 0;
            totals.Quantity13 = 0;
            totals.Quantity14 = 0;
            totals.Quantity15 = 0;

            List<string> noteTitles = new List<string>();

            List<NoteTitle> noteTitleList = (List<NoteTitle>)_noteTitleService.GetAllNoteTitlesSorted();

            string csvColumns = "";
            foreach (NoteTitle n in noteTitleList)
            {
                if (n.Title.Length == 0) continue;
                noteTitles.Add(n.Title);
                csvColumns += n.Title + ",";
            }

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem b = null;

            foreach (ReportBuyersByLeadNotes r in report)
            {
                int nindex = noteTitles.IndexOf(r.Title);

                if ((date != r.Created.ToShortDateString() && type == 1) ||
                    (buyerName != r.BuyerName && type == 3) ||
                    (buyerChannelName != r.ChannelName && type == 2)
                    )
                {
                    b = new Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem();
                    if (type == 1)
                        b.title = r.Created.ToShortDateString();
                    else
                        if (type == 2)
                        b.title = r.ChannelName;
                    else
                        b.title = r.BuyerName;

                    b.folder = false;
                    b.expanded = true;

                    switch (nindex)
                    {
                        case 0: b.Quantity1 = r.Quantity; break;
                        case 1: b.Quantity2 = r.Quantity; break;
                        case 2: b.Quantity3 = r.Quantity; break;
                        case 3: b.Quantity4 = r.Quantity; break;
                        case 4: b.Quantity5 = r.Quantity; break;
                        case 5: b.Quantity6 = r.Quantity; break;
                        case 6: b.Quantity7 = r.Quantity; break;
                        case 7: b.Quantity8 = r.Quantity; break;
                        case 8: b.Quantity9 = r.Quantity; break;
                        case 9: b.Quantity10 = r.Quantity; break;
                        case 10: b.Quantity11 = r.Quantity; break;
                        case 11: b.Quantity12 = r.Quantity; break;
                        case 12: b.Quantity13 = r.Quantity; break;
                        case 13: b.Quantity14 = r.Quantity; break;
                        case 14: b.Quantity15 = r.Quantity; break;
                    }

                    //parent.children.Add(b);
                    buyers.Add(b);
                    date = r.Created.ToShortDateString();
                    buyerName = r.BuyerName;
                    buyerChannelName = r.ChannelName;
                }
                else
                {
                    switch (nindex)
                    {
                        case 0: b.Quantity1 += r.Quantity; break;
                        case 1: b.Quantity2 += r.Quantity; break;
                        case 2: b.Quantity3 += r.Quantity; break;
                        case 3: b.Quantity4 += r.Quantity; break;
                        case 4: b.Quantity5 += r.Quantity; break;
                        case 5: b.Quantity6 += r.Quantity; break;
                        case 6: b.Quantity7 += r.Quantity; break;
                        case 7: b.Quantity8 += r.Quantity; break;
                        case 8: b.Quantity9 += r.Quantity; break;
                        case 9: b.Quantity10 += r.Quantity; break;
                        case 10: b.Quantity11 += r.Quantity; break;
                        case 11: b.Quantity12 += r.Quantity; break;
                        case 12: b.Quantity13 += r.Quantity; break;
                        case 13: b.Quantity14 += r.Quantity; break;
                        case 14: b.Quantity15 += r.Quantity; break;
                    }
                }

                switch (nindex)
                {
                    case 0: totals.Quantity1 += r.Quantity; break;
                    case 1: totals.Quantity2 += r.Quantity; break;
                    case 2: totals.Quantity3 += r.Quantity; break;
                    case 3: totals.Quantity4 += r.Quantity; break;
                    case 4: totals.Quantity5 += r.Quantity; break;
                    case 5: totals.Quantity6 += r.Quantity; break;
                    case 6: totals.Quantity7 += r.Quantity; break;
                    case 7: totals.Quantity8 += r.Quantity; break;
                    case 8: totals.Quantity9 += r.Quantity; break;
                    case 9: totals.Quantity10 += r.Quantity; break;
                    case 10: totals.Quantity11 += r.Quantity; break;
                    case 11: totals.Quantity12 += r.Quantity; break;
                    case 12: totals.Quantity13 += r.Quantity; break;
                    case 13: totals.Quantity14 += r.Quantity; break;
                    case 14: totals.Quantity15 += r.Quantity; break;
                }
            }

            buyers.Add(totals);

            return buyers;
        }

        /// <summary>
        /// Gets the report buyers by lead notes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByLeadNotes()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByLeadNotesModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            string buyerIds = Request["buyerIds"];
            string buyerChannelIds = Request["buyerChannelIds"];
            string campaignIds = Request["campaignIds"];
            string affiliateIds = Request["affiliateIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            buyers = GetReportBuyersByLeadNotesData(startDate, endDate, buyerIds, buyerChannelIds, affiliateIds, campaignIds);

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers comparison.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersComparison()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();

            PrepareBuyerReportModel(model, 0);

            return View(model);
        }

        /// <summary>
        /// Reports the buyers comparison partial.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersComparisonPartial()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();

            return PartialView(m);
        }

        public List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersComparisonModel.TreeItem> GetReportBuyersComparisonData(DateTime date1, DateTime date2, DateTime date3, long[] buyerIdsLong, long[] campaignIds, long buyerChannelId, bool byBuyers = true)
        {
            DateTime[] dates = new DateTime[3];
            dates[0] = date1;
            dates[1] = date2;
            dates[2] = date3;

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersComparisonModel.TreeItem> buyerItems = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersComparisonModel.TreeItem>();

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersComparisonModel.TreeItem totalItem = new ReportBuyersComparisonModel.TreeItem();
            totalItem.title = "<b>Total</b>";
            totalItem.Date1Buyers = new ReportBuyersComparisonModel.TreeItem() { title = "Total" };
            totalItem.Date2Buyers = new ReportBuyersComparisonModel.TreeItem() { title = "Total" };
            totalItem.Date3Buyers = new ReportBuyersComparisonModel.TreeItem() { title = "Total" };

            if (campaignIds.Length == 0 || (campaignIds.Length == 1 && campaignIds[0] == 0))
            {
                for (int i = 0; i < buyerIdsLong.Length; i++)
                {
                    if (buyerIdsLong[i] == 0) continue;

                    string buyerName = "";

                    if (byBuyers)
                    {
                        Buyer buyer2 = _buyerService.GetBuyerById(buyerIdsLong[i]);
                        buyerName = buyer2.Name;
                    }
                    else
                    {
                        BuyerChannel buyer2 = _buyerChannelService.GetBuyerChannelById(buyerIdsLong[i]);
                        buyerName = buyer2.Name;
                    }

                    Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersComparisonModel.TreeItem buyerItem = new ReportBuyersComparisonModel.TreeItem();
                    buyerItem.title = buyerName;
                    buyerItem.folder = true;
                    buyerItem.expanded = false;
                    buyerItems.Add(buyerItem);
                    bool buyerItemRowsFound = false;

                    buyerItem.Date1Buyers = new ReportBuyersComparisonModel.TreeItem() { title = buyerName };
                    buyerItem.Date2Buyers = new ReportBuyersComparisonModel.TreeItem() { title = buyerName };
                    buyerItem.Date3Buyers = new ReportBuyersComparisonModel.TreeItem() { title = buyerName };

                    List<ReportBuyersComparison> reportRows = null;

                    if (byBuyers)
                        reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparison(buyerIdsLong[i], buyerChannelId, 0, date1, date2, date3);
                    else
                        reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparisonBuyerChannels(buyerIdsLong[i], 0, date1, date2, date3);

                    foreach (ReportBuyersComparison row in reportRows)
                    {
                        buyerItemRowsFound = true;
                        int index = 0;
                        /*for (int j = 0; j < buyerIdsLong.Length; j++)
                        {
                            if (buyerIdsLong[j] == row.BuyerId)
                            {
                                index = j;
                                break;
                            }
                        }*/

                        if (row.Created.Day == date1.Day && row.Created.Month == date1.Month && row.Created.Year == date1.Year)
                        {
                            buyerItem.Date1Buyers.title = row.BuyerName;
                            buyerItem.Date1Buyers.Posted += row.Posted;
                            buyerItem.Date1Buyers.Sold += row.Sold;
                            buyerItem.Date1Buyers.Rejected += row.Rejected;
                            buyerItem.Date1Buyers.Redirected += row.Redirected;
                            buyerItem.Date1Buyers.Revenue += row.Revenue;
                        }

                        if (row.Created.Day == date2.Day && row.Created.Month == date2.Month && row.Created.Year == date2.Year)
                        {
                            buyerItem.Date2Buyers.title = row.BuyerName;
                            buyerItem.Date2Buyers.Posted += row.Posted;
                            buyerItem.Date2Buyers.Sold += row.Sold;
                            buyerItem.Date2Buyers.Rejected += row.Rejected;
                            buyerItem.Date2Buyers.Redirected += row.Redirected;
                            buyerItem.Date2Buyers.Revenue += row.Revenue;
                        }

                        if (row.Created.Day == date3.Day && row.Created.Month == date3.Month && row.Created.Year == date3.Year)
                        {
                            buyerItem.Date3Buyers.title = row.BuyerName;
                            buyerItem.Date3Buyers.Posted += row.Posted;
                            buyerItem.Date3Buyers.Sold += row.Sold;
                            buyerItem.Date3Buyers.Rejected += row.Rejected;
                            buyerItem.Date3Buyers.Redirected += row.Redirected;
                            buyerItem.Date3Buyers.Revenue += row.Revenue;
                        }
                    }

                    if (!buyerItemRowsFound)
                    {
                        buyerItems.Remove(buyerItem);
                    }
                    else
                    {
                        totalItem.Date1Buyers.Posted += buyerItem.Date1Buyers.Posted;
                        totalItem.Date1Buyers.Sold += buyerItem.Date1Buyers.Sold;
                        totalItem.Date1Buyers.Rejected += buyerItem.Date1Buyers.Rejected;
                        totalItem.Date1Buyers.Redirected += buyerItem.Date1Buyers.Redirected;
                        totalItem.Date1Buyers.Revenue += buyerItem.Date1Buyers.Revenue;

                        totalItem.Date2Buyers.Posted += buyerItem.Date2Buyers.Posted;
                        totalItem.Date2Buyers.Sold += buyerItem.Date2Buyers.Sold;
                        totalItem.Date2Buyers.Rejected += buyerItem.Date2Buyers.Rejected;
                        totalItem.Date2Buyers.Redirected += buyerItem.Date2Buyers.Redirected;
                        totalItem.Date2Buyers.Revenue += buyerItem.Date2Buyers.Revenue;

                        totalItem.Date3Buyers.Posted += buyerItem.Date3Buyers.Posted;
                        totalItem.Date3Buyers.Sold += buyerItem.Date3Buyers.Sold;
                        totalItem.Date3Buyers.Rejected += buyerItem.Date3Buyers.Rejected;
                        totalItem.Date3Buyers.Redirected += buyerItem.Date3Buyers.Redirected;
                        totalItem.Date3Buyers.Revenue += buyerItem.Date3Buyers.Revenue;
                    }
                }
            }
            else
            {
                for (int i = 0; i < campaignIds.Length; i++)
                {
                    if (campaignIds[i] == 0) continue;

                    string campaignName = "";

                    Campaign campaign = _campaignService.GetCampaignById(campaignIds[i]);
                    campaignName = campaign.Name;

                    Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersComparisonModel.TreeItem buyerItem = new ReportBuyersComparisonModel.TreeItem();
                    buyerItem.title = campaignName;
                    buyerItem.folder = true;
                    buyerItem.expanded = false;
                    buyerItems.Add(buyerItem);

                    buyerItem.Date1Buyers = new ReportBuyersComparisonModel.TreeItem() { title = campaignName };
                    buyerItem.Date2Buyers = new ReportBuyersComparisonModel.TreeItem() { title = campaignName };
                    buyerItem.Date3Buyers = new ReportBuyersComparisonModel.TreeItem() { title = campaignName };

                    List<ReportBuyersComparison> reportRows = null;

                    if (byBuyers)
                        reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparison(0, 0, campaignIds[i], date1, date2, date3);
                    else
                        reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparisonBuyerChannels(0, campaignIds[i], date1, date2, date3);

                    foreach (ReportBuyersComparison row in reportRows)
                    {
                        int index = 0;

                        if (row.Created.Day == date1.Day && row.Created.Month == date1.Month && row.Created.Year == date1.Year)
                        {
                            buyerItem.Date1Buyers.title = row.BuyerName;
                            buyerItem.Date1Buyers.Posted += row.Posted;
                            buyerItem.Date1Buyers.Sold += row.Sold;
                            buyerItem.Date1Buyers.Rejected += row.Rejected;
                            buyerItem.Date1Buyers.Redirected += row.Redirected;
                            buyerItem.Date1Buyers.Revenue += row.Revenue;

                            totalItem.Date1Buyers.Posted += row.Posted;
                            totalItem.Date1Buyers.Sold += row.Sold;
                            totalItem.Date1Buyers.Rejected += row.Rejected;
                            totalItem.Date1Buyers.Redirected += row.Redirected;
                            totalItem.Date1Buyers.Revenue += row.Revenue;
                        }

                        if (row.Created.Day == date2.Day && row.Created.Month == date2.Month && row.Created.Year == date2.Year)
                        {
                            buyerItem.Date2Buyers.title = row.BuyerName;
                            buyerItem.Date2Buyers.Posted += row.Posted;
                            buyerItem.Date2Buyers.Sold += row.Sold;
                            buyerItem.Date2Buyers.Rejected += row.Rejected;
                            buyerItem.Date2Buyers.Redirected += row.Redirected;
                            buyerItem.Date1Buyers.Revenue += row.Revenue;

                            totalItem.Date2Buyers.Posted += row.Posted;
                            totalItem.Date2Buyers.Sold += row.Sold;
                            totalItem.Date2Buyers.Rejected += row.Rejected;
                            totalItem.Date2Buyers.Redirected += row.Redirected;
                            totalItem.Date1Buyers.Revenue += row.Revenue;
                        }

                        if (row.Created.Day == date3.Day && row.Created.Month == date3.Month && row.Created.Year == date3.Year)
                        {
                            buyerItem.Date3Buyers.title = row.BuyerName;
                            buyerItem.Date3Buyers.Posted += row.Posted;
                            buyerItem.Date3Buyers.Sold += row.Sold;
                            buyerItem.Date3Buyers.Rejected += row.Rejected;
                            buyerItem.Date3Buyers.Redirected += row.Redirected;
                            buyerItem.Date1Buyers.Revenue += row.Revenue;

                            totalItem.Date3Buyers.Posted += row.Posted;
                            totalItem.Date3Buyers.Sold += row.Sold;
                            totalItem.Date3Buyers.Rejected += row.Rejected;
                            totalItem.Date3Buyers.Redirected += row.Redirected;
                            totalItem.Date1Buyers.Revenue += row.Revenue;
                        }
                    }
                }
            }

            buyerItems.Add(totalItem);

            return buyerItems;
        }

        /// <summary>
        /// Gets the report buyers comparison.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersComparison()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(new { }, JsonRequestBehavior.AllowGet);

            bool byBuyers = true;
            if (!bool.TryParse(Request["bybuyers"], out byBuyers)) byBuyers = true;

            string buyerIdsStr = Request["ids"];
            string campaignIdsStr = Request["campaignIds"];

            if (string.IsNullOrEmpty(buyerIdsStr)) return Json(new { name = "GetReportBuyersComparison" }, JsonRequestBehavior.AllowGet);

            string[] buyerIds = buyerIdsStr.Split(new char[1] { ',' });

            string date1str = Request["date1"];
            string date2str = Request["date2"];
            string date3str = Request["date3"];

            long[] buyerIdsLong = new long[buyerIds.Length];

            for (int i = 1; i < buyerIds.Length; i++)
            {
                if (i - 1 >= buyerIdsLong.Length) continue;
                buyerIdsLong[i - 1] = 0;
                long.TryParse(buyerIds[i], out buyerIdsLong[i - 1]);
            }

            string[] campaignIds = campaignIdsStr.Split(new char[1] { ',' });
            long[] campaignIdsLong = new long[campaignIds.Length];

            for (int i = 0; i < campaignIds.Length; i++)
            {
                campaignIdsLong[i] = 0;
                long.TryParse(campaignIds[i], out campaignIdsLong[i]);
            }


            long buyerChannelId = 0;
            long.TryParse(Request["BuyerChannelId"], out buyerChannelId);

            DateTime date1 = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            date1 = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);
            DateTime date2 = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            date2 = new DateTime(date2.Year, date2.Month, date2.Day, 0, 0, 0);
            DateTime date3 = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            date3 = new DateTime(date3.Year, date3.Month, date3.Day, 0, 0, 0);

            try
            {
                date1 = Convert.ToDateTime(date1str);
                date1 = new DateTime(date1.Year, date1.Month, date1.Day, 0, 0, 0);
                date2 = Convert.ToDateTime(date2str);
                date2 = new DateTime(date2.Year, date2.Month, date2.Day, 0, 0, 0);
                date3 = Convert.ToDateTime(date3str);
                date3 = new DateTime(date3.Year, date3.Month, date3.Day, 0, 0, 0);
            }
            catch (Exception ex)
            {
                //Arman Handle Exception
                LogException(ex);
            }

            date1 = _settingService.GetUTCDate(date1);
            date2 = _settingService.GetUTCDate(date2);
            date3 = _settingService.GetUTCDate(date3);

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersComparisonModel.TreeItem> buyerItems = GetReportBuyersComparisonData(date1, date2, date3, buyerIdsLong, campaignIdsLong, buyerChannelId, byBuyers);

            return Json(buyerItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by traffic estimator.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByTrafficEstimator()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();

            List<Campaign> campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            foreach (var c in campaigns)
            {
                m.ListCampaigns.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            m.BaseUrl = Helper.GetBaseUrl(Request);

            return View(m);
        }

        /// <summary>
        /// Gets the report buyers by traffic estimator.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByTrafficEstimator()
        {
            ReportCSVGenerator.Instance.Init("Buyer,Total leads,Unique leads");

            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            DateTime end = new DateTime(start.Year, start.Month, start.Day, 23, 59, 59);

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out start, out end);

            int price1 = 0;
            int price2 = 0;
            int.TryParse(Request["price1"], out price1);
            int.TryParse(Request["price2"], out price2);

            Dictionary<long, Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByTrafficEstimatorModel.TreeItem> buyers = new Dictionary<long, Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByTrafficEstimatorModel.TreeItem>();

            dynamic data = JsonConvert.DeserializeObject(Request["data"]);

            List<List<ReportBuyersByTrafficEstimate>> reports = new List<List<ReportBuyersByTrafficEstimate>>();

            List<ReportBuyersByTrafficEstimate> report = null;

            List<string> fields = new List<string>();
            List<string> values = new List<string>();
            List<short> valueTypes = new List<short>();
            List<short> conditions = new List<short>();
            List<bool> exludes = new List<bool>();

            for (int i = 0; i < data.Count; i++)
            {
                report = null;

                string field = data[i]["field"].ToString();
                string value1 = data[i]["value1"].ToString();
                string value2 = data[i]["value2"].ToString();
                string exclude = data[i]["exclude"].ToString();
                string validator = data[i]["validator"].ToString();
                string condition = data[i]["condition"].ToString();

                fields.Add(field);
                values.Add(value1 + "," + value2);

                short nValidator = 0;
                short.TryParse(validator, out nValidator);
                valueTypes.Add(nValidator);

                short nCondition = 1;
                short.TryParse(condition, out nCondition);
                conditions.Add(nCondition);

                bool bExclude = false;
                bool.TryParse(exclude, out bExclude);
                exludes.Add(bExclude);
            }

            report = (List<ReportBuyersByTrafficEstimate>)this._reportService.ReportBuyersByTrfficEstimate(start, end, Request["buyerChannels"], Request["campaigns"], fields, values, valueTypes, conditions, exludes);

            ReportBuyersByTrafficEstimatorModel.TreeItem item = null;
            ReportBuyersByTrafficEstimatorModel.TreeItem prevItem = null;
            ReportBuyersByTrafficEstimatorModel.TreeItem total = new ReportBuyersByTrafficEstimatorModel.TreeItem() { title = "Total", folder = true, Quantity = 0, UQuantity = 0 };

            foreach (var r in report)
            {
                item = new ReportBuyersByTrafficEstimatorModel.TreeItem();

                if (buyers.ContainsKey(r.BuyerChannelId)) item = buyers[r.BuyerChannelId];
                else
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2}", prevItem.BuyerChannelName, prevItem.Quantity, prevItem.UQuantity));
                    }
                    buyers.Add(r.BuyerChannelId, item);
                }

                item.title = r.BuyerChannelName;
                item.BuyerChannelId = r.BuyerChannelId;
                item.BuyerId = r.BuyerId;
                item.BuyerChannelName = r.BuyerChannelName;
                item.BuyerName = r.BuyerName;
                item.Created = r.Created;
                item.Quantity += r.Quantity;
                item.UQuantity += r.UQuantity;
                total.Quantity += r.Quantity;
                total.UQuantity += r.UQuantity;

                prevItem = item;
            }

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2}", prevItem.BuyerChannelName, prevItem.Quantity, prevItem.UQuantity));
            }

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersByTrafficEstimator.csv");

            var items = buyers.Select(d => d.Value).ToList();
            items.Add(total);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the buyers by prices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersByPrices()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();

            List<Campaign> campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            foreach (var c in campaigns)
            {
                m.ListCampaigns.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            return View(m);
        }

        /// <summary>
        /// Gets the report buyers by prices.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersByPrices()
        {
            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            DateTime end = new DateTime(start.Year, start.Month, start.Day, 23, 59, 59);

            ReportCSVGenerator.Instance.Init("Channel,Price,Total leads,Sold leads,Unique leads");

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out start, out end);

            int price1 = 0;
            int price2 = 0;
            int.TryParse(Request["price1"], out price1);
            int.TryParse(Request["price2"], out price2);

            List<ReportBuyersByPrices> reportRows = (List<ReportBuyersByPrices>)this._reportService.ReportBuyersByPrices(start, end, "", Request["buyerChannels"], Request["campaigns"], price1, price2);
            Dictionary<string, Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByPricesModel.TreeItem> buyers = new Dictionary<string, Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersByPricesModel.TreeItem>();

            ReportBuyersByPricesModel.TreeItem item = null;
            ReportBuyersByPricesModel.TreeItem prevItem = null;

            foreach (ReportBuyersByPrices reportRow in reportRows)
            {
                item = new ReportBuyersByPricesModel.TreeItem();

                if (buyers.ContainsKey(reportRow.BuyerChannelId.ToString() + "-" + reportRow.BuyerPrice.ToString())) item = buyers[reportRow.BuyerChannelId.ToString() + "-" + reportRow.BuyerPrice.ToString()];
                else
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", prevItem.title, prevItem.BuyerPrice, prevItem.Quantity, prevItem.SoldQuantity, prevItem.UQuantity));
                    }
                    item.title = reportRow.BuyerChannelName;
                    item.BuyerPrice = Math.Round(reportRow.BuyerPrice, 1);
                    item.Quantity = item.UQuantity = item.SoldQuantity = 0;
                    buyers.Add(reportRow.BuyerChannelId.ToString() + "-" + reportRow.BuyerPrice.ToString(), item);
                }

                if (reportRow.Status == 1)
                    item.SoldQuantity += reportRow.Quantity;
                item.Quantity += reportRow.Quantity;
                item.UQuantity += reportRow.UQuantity;

                prevItem = item;
            }

            if (prevItem != null)
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", prevItem.title, prevItem.BuyerPrice, prevItem.Quantity, prevItem.SoldQuantity, prevItem.UQuantity));

            var items = buyers.Select(d => d.Value).ToList();

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersByPrices.csv");

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Win rate report.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersWinRateReport()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();

            List<Campaign> campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            foreach (var c in campaigns)
            {
                m.ListCampaigns.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            return View(m);
        }

        /// <summary>
        /// Gets the report buyers by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersWinRateReport()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersWinRateReportModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersWinRateReportModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Buyer channel,Sold leads,Rejected leads,Min price error leads");

            string buyerChannelIds = Request["buyerChannelIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            List<ReportBuyersWinRateReport> report = (List<ReportBuyersWinRateReport>)this._reportService.ReportBuyersWinRateReport(startDate, endDate, (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds));

            long buyerChannelId = 0;
            decimal buyerPrice = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersWinRateReportModel.TreeItem totals = new ReportBuyersWinRateReportModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.BuyerChannelId = 0;
            totals.BuyerChannelName = "";

            totals.AffiliatePrice = 0;
            totals.BuyerPrice = 0;
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.MinPriceErrorLeads = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersWinRateReportModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersWinRateReportModel.TreeItem prevItem = null;

            foreach (ReportBuyersWinRateReport r in report)
            {
                if (buyerPrice != r.BuyerPrice)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3}", prevItem.title, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.MinPriceErrorLeads));
                    }

                    item = new Models.Lead.Reports.ReportBuyersWinRateReportModel.TreeItem();
                    item.title = "$" + Math.Round(r.BuyerPrice, 2).ToString();
                    item.folder = false;
                    item.expanded = true;

                    item.BuyerChannelId = r.BuyerChannelId;
                    item.BuyerChannelName = r.BuyerChannelName;

                    item.TotalLeads = r.TotalLeads;
                    item.SoldLeads = r.SoldLeads;
                    item.RejectedLeads = r.RejectedLeads;
                    item.MinPriceErrorLeads = r.MinPriceErrorLeads;
                    item.AffiliatePrice = 0;
                    item.BuyerPrice = r.BuyerPrice;
                    item.OtherBuyerPrice = "0";

                    /*List<decimal> prices = (List<decimal>)_reportService.GetPricePoints(startDate, endDate, r.BuyerChannelId.ToString(), true);
                    if (prices.Count > 0)
                    {
                        item.OtherBuyerPrice = "";
                    }
                    foreach(decimal price in prices)
                    {
                        item.OtherBuyerPrice += Math.Round(price, 2) + ",";
                    }

                    if (prices.Count > 0)
                        item.OtherBuyerPrice = item.OtherBuyerPrice.Remove(item.OtherBuyerPrice.Length - 1);*/

                    buyers.Add(item);

                    buyerPrice = r.BuyerPrice;
                }
                else
                {
                    item.TotalLeads += r.TotalLeads;
                    item.SoldLeads += r.SoldLeads;
                    item.RejectedLeads += r.RejectedLeads;
                    item.MinPriceErrorLeads += r.MinPriceErrorLeads;
                    item.BuyerPrice += r.BuyerPrice;
                }

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.MinPriceErrorLeads += r.MinPriceErrorLeads;
                totals.BuyerPrice += r.BuyerPrice;

                prevItem = item;
            }

            buyers.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3}", prevItem.title, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.MinPriceErrorLeads));
            }

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportBuyersWinRateReport.csv");

            return Json(buyers, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Conversion Analysys.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersConversionAnalysys()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();

            List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(0).OrderBy(x => x.Name).ToList();

            foreach (var c in buyers)
            {
                m.ListBuyers.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            return View(m);
        }

        /// <summary>
        /// Gets the report buyers by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportBuyersConversionAnalysys(HttpPostedFileBase file1, HttpPostedFileBase file2)
        {
            List<string> idsOrEmails1 = new List<string>();
            bool takeFromId1 = false;
            if (file1 != null && file1.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file1.FileName);
                string dir = Server.MapPath("~/App_Data/Uploads");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(dir, DateTime.Now.Ticks.ToString() + "_" + fileName);
                file1.SaveAs(path);

                StreamReader sr = new StreamReader(path);
                string line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    if (row.Length > 0)
                    {
                        long leadId = 0;
                        if (!takeFromId1 && long.TryParse(row[0], out leadId))
                        {
                            takeFromId1 = true;
                        }

                        idsOrEmails1.Add(row[0]);
                    }
                }
                sr.Close();

                System.IO.File.Delete(path);
            }

            List<string> idsOrEmails2 = new List<string>();
            bool takeFromId2 = false;

            if (file2 != null && file2.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file2.FileName);
                string dir = Server.MapPath("~/App_Data/Uploads");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(dir, DateTime.Now.Ticks.ToString() + "_" + fileName);
                file1.SaveAs(path);

                StreamReader sr = new StreamReader(path);
                string line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');
                    if (row.Length > 0)
                    {
                        long leadId = 0;
                        if (!takeFromId1 && long.TryParse(row[0], out leadId))
                        {
                            takeFromId2 = true;
                        }

                        idsOrEmails2.Add(row[0]);
                    }
                }
                sr.Close();

                System.IO.File.Delete(path);
            }

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersConversionAnalysysModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersConversionAnalysysModel.TreeItem>();
            Dictionary<string, Adrack.Web.ContentManagement.Models.Lead.Reports.ReportBuyersConversionAnalysysModel.TreeItem> data = new Dictionary<string, ReportBuyersConversionAnalysysModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            ReportCSVGenerator.Instance.Init("Name,Sold,Loaned,Conversion rate,Defaulted");

            string buyerChannelIds = Request["buyerChannelIds"];

            string analyzeBy = Request["analyzeBy"];
            if (string.IsNullOrEmpty(analyzeBy)) analyzeBy = "channel";

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            List<LeadMainContent> rows = (List<LeadMainContent>)this._leadMainService.GetLeadsAll(
                startDate,
                endDate,
                0,
                "",
                "",
                "",
                "",
                "",
                buyerChannelIds,
                "",
                1,
                "",
                "",
                "",
                "",
                0,
                "",
                "",
                1, 1000);

            foreach (var row in rows)
            {
                string name = "";
                LeadMain leadMain = null;

                switch (analyzeBy.ToLower())
                {
                    case "channel":
                        AffiliateChannel affiliateChannel = _affiliateChannelService.GetAffiliateChannelById(row.AffiliateChannelId, true);
                        name = affiliateChannel.Name;
                        break;
                    case "state":
                        name = row.State.ToUpper();
                        break;
                    case "payfrequency":
                        name = row.PayFrequency;
                        break;
                    case "age":
                        if (row.Dob.HasValue)
                        {
                            TimeSpan ts = DateTime.UtcNow - row.Dob.Value;
                            name = ((int)(ts.TotalDays / 365)).ToString();
                        }
                        break;
                    case "dayoftheweek":
                        if (row.Created.HasValue)
                        {
                            name = string.Format("{0}", row.Created.Value.DayOfWeek);
                        }
                        break;
                    case "rla":
                        name = row.RequestedAmount.ToString();
                        break;
                    case "netmonthlyincome":
                        name = row.NetMonthlyIncome.ToString();
                        break;
                    case "incometype":
                        name = row.IncomeType.ToString();
                        break;
                    case "banktime":
                        leadMain = _leadMainService.GetLeadMainById(row.LeadId);
                        if (leadMain != null)
                        {
                            try
                            {
                                XmlReader xmlReader = XmlReader.Create(new StringReader(leadMain.ReceivedData));
                                if (xmlReader.ReadToDescendant("BANKMONTHS"))
                                {
                                    xmlReader.Read();//this moves reader to next node which is text 
                                    name = xmlReader.Value; //this might give value than 
                                }
                            }
                            catch
                            {

                            }
                        }
                        break;
                    case "housingtype":
                        leadMain = _leadMainService.GetLeadMainById(row.LeadId);
                        if (leadMain != null)
                        {
                            try
                            {
                                XmlReader xmlReader = XmlReader.Create(new StringReader(leadMain.ReceivedData));
                                if (xmlReader.ReadToDescendant("RENTOROWN"))
                                {
                                    xmlReader.Read();//this moves reader to next node which is text 
                                    name = xmlReader.Value; //this might give value than 
                                }
                            }
                            catch
                            {

                            }
                        }
                        break;
                    case "emptime":
                        name = row.Emptime.ToString();
                        break;
                    case "addresstime":
                        name = row.AddressMonth.ToString();
                        break;
                }

                if (string.IsNullOrEmpty(name)) continue;

                if (!data.ContainsKey(name))
                {
                    data[name] = new ReportBuyersConversionAnalysysModel.TreeItem() { title = name, LoanedLeads = 0, SoldLeads = 0 };
                }

                data[name].SoldLeads++;

                if (takeFromId1)
                {
                    if (idsOrEmails1.Contains(row.LeadId.ToString()))
                        data[name].LoanedLeads++;
                }
                else
                {
                    if (idsOrEmails1.Contains(row.Email.ToString()))
                        data[name].LoanedLeads++;
                }

                if (takeFromId2)
                {
                    if (idsOrEmails2.Contains(row.LeadId.ToString()))
                        data[name].DefaultedLeads++;
                }
                else
                {
                    if (idsOrEmails2.Contains(row.Email.ToString()))
                        data[name].DefaultedLeads++;
                }
            }
            var dataList = data.Select(x => x.Value).ToList().OrderBy(x => x.title).ToList();

            foreach (var item in dataList)
            {
                item.ConversionRate = Math.Round((double)item.LoanedLeads / (double)item.SoldLeads * (double)100, 2);

                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", item.title, item.SoldLeads, item.LoanedLeads, item.ConversionRate, item.DefaultedLeads));
            }

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "BuyersConversionAnalysys.csv");

            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportSendingTime()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel m = new BuyerReportModel();
            List<Campaign> campaigns = (List<Campaign>)_campaignService.GetAllCampaigns(0);

            foreach (var c in campaigns)
            {
                m.ListCampaigns.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });
            }

            return View(m);
        }

        /// <summary>
        /// Gets the report buyers by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportSendingTime()
        {
            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportSendingTimeModel.TreeItem> buyers = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportSendingTimeModel.TreeItem>();
            Dictionary<string, Adrack.Web.ContentManagement.Models.Lead.Reports.ReportSendingTimeModel.TreeItem> data = new Dictionary<string, ReportSendingTimeModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return Json(buyers, JsonRequestBehavior.AllowGet);

            string campaignIds = Request["campaignIds"];

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            var total = new ReportSendingTimeModel.TreeItem()
            {
                title = "Total",
                BuyerId = 0,
                AfterRejectAvg = 0,
                AfterRejectMax = 0,
                AfterRejectMin = 0,
                AfterRejectQuantity = 0,
                AfterSoldAvg = 0,
                AfterSoldMax = 0,
                AfterSoldMin = 0,
                AfterSoldQuantity = 0,
                BeforeRejectAvg = 0,
                BeforeRejectMax = 0,
                BeforeRejectMin = 0,
                BeforeRejectQuantity = 0,
                BeforeSoldAvg = 0,
                BeforeSoldMax = 0,
                BeforeSoldMin = 0,
                BeforeSoldQuantity = 0,
                BeforePostedQuantity = 0,
                AfterPostedQuantity = 0
            };

            List<ReportSendingTime> rows = (List<ReportSendingTime>)this._reportService.ReportSendingTime(startDate, endDate, campaignIds);

            foreach (var row in rows)
            {
                if (!data.ContainsKey(row.Name))
                {
                    data[row.Name] = new ReportSendingTimeModel.TreeItem() {
                        title = row.Name,
                        BuyerId = row.Id,
                        AfterRejectAvg = 0,
                        AfterRejectMax = 0,
                        AfterRejectMin = 0,
                        AfterRejectQuantity = 0,
                        AfterSoldAvg = 0,
                        AfterSoldMax = 0,
                        AfterSoldMin = 0,
                        AfterSoldQuantity = 0,
                        BeforeRejectAvg = 0,
                        BeforeRejectMax = 0,
                        BeforeRejectMin = 0,
                        BeforeRejectQuantity = 0,
                        BeforeSoldAvg = 0,
                        BeforeSoldMax = 0,
                        BeforeSoldMin = 0,
                        BeforeSoldQuantity = 0,
                        BeforePostedQuantity = 0,
                        AfterPostedQuantity = 0,
                        TotalPaused = 0
                    };
                }

                switch(row.TimeoutType.ToLower())
                {
                    case "before":
                        if (row.StatusType == "Sold")
                        {
                            data[row.Name].BeforeSoldAvg += Math.Round(row.AvgSec, 1);
                            data[row.Name].BeforeSoldMax += Math.Round(row.MaxSec, 1);
                            data[row.Name].BeforeSoldMin += Math.Round(row.MinSec, 1);
                            data[row.Name].BeforeSoldQuantity += (int)row.Quantity;

                            if (data[row.Name].BeforeSoldMax > total.BeforeSoldMax)
                            {
                                total.BeforeSoldMax = data[row.Name].BeforeSoldMax;
                            }
                            if (data[row.Name].BeforeSoldMin < total.BeforeSoldMin)
                            {
                                total.BeforeSoldMin = data[row.Name].BeforeSoldMin;
                            }
                            total.BeforeSoldQuantity += (int)row.Quantity;
                            total.BeforeSoldAvg += Math.Round(row.AvgSec, 1);

                        }
                        else if (row.StatusType == "Reject")
                        {
                            data[row.Name].BeforeRejectAvg += Math.Round(row.AvgSec, 1);
                            data[row.Name].BeforeRejectMax += Math.Round(row.MaxSec, 1);
                            data[row.Name].BeforeRejectMin += Math.Round(row.MinSec, 1);
                            data[row.Name].BeforeRejectQuantity += (int)row.Quantity;

                            if (data[row.Name].BeforeRejectMax > total.BeforeRejectMax)
                            {
                                total.BeforeRejectMax = data[row.Name].BeforeRejectMax;
                            }
                            if (data[row.Name].BeforeRejectMin < total.BeforeRejectMin)
                            {
                                total.BeforeRejectMin = data[row.Name].BeforeRejectMin;
                            }
                            total.BeforeRejectQuantity += (int)row.Quantity;
                            total.BeforeRejectAvg += Math.Round(row.AvgSec, 1);
                        }
                        data[row.Name].BeforePostedQuantity += (int)row.Quantity;
                        total.BeforePostedQuantity += (int)row.Quantity;
                        break;
                    case "after":
                        if (row.StatusType == "Sold")
                        {
                            data[row.Name].AfterSoldAvg += Math.Round(row.AvgSec, 1);
                            data[row.Name].AfterSoldMax += Math.Round(row.MaxSec, 1);
                            data[row.Name].AfterSoldMin += Math.Round(row.MinSec, 1);
                            data[row.Name].AfterSoldQuantity += (int)row.Quantity;

                            if (data[row.Name].AfterSoldMax > total.AfterSoldMax)
                            {
                                total.AfterSoldMax = data[row.Name].AfterSoldMax;
                            }
                            if (data[row.Name].AfterSoldMin < total.AfterSoldMin)
                            {
                                total.AfterSoldMin = data[row.Name].AfterSoldMin;
                            }
                            total.AfterSoldQuantity += (int)row.Quantity;
                            total.AfterSoldAvg += Math.Round(row.AvgSec, 1);

                        }
                        else if (row.StatusType == "Reject")
                        {
                            data[row.Name].AfterRejectAvg += Math.Round(row.AvgSec, 1);
                            data[row.Name].AfterRejectMax += Math.Round(row.MaxSec, 1);
                            data[row.Name].AfterRejectMin += Math.Round(row.MinSec, 1);
                            data[row.Name].AfterRejectQuantity += (int)row.Quantity;

                            if (data[row.Name].AfterRejectMax > total.AfterRejectMax)
                            {
                                total.AfterRejectMax = data[row.Name].AfterRejectMax;
                            }
                            if (data[row.Name].AfterRejectMin < total.AfterRejectMin)
                            {
                                total.AfterRejectMin = data[row.Name].AfterRejectMin;
                            }
                            total.AfterRejectQuantity += (int)row.Quantity;
                            total.AfterRejectAvg += Math.Round(row.AvgSec, 1);
                        }
                        data[row.Name].AfterPostedQuantity += (int)row.Quantity;
                        total.AfterPostedQuantity += row.Quantity;
                        break;
                    case "Paused":
                        data[row.Name].TotalPaused += (int)row.Quantity;
                        total.TotalPaused += row.Quantity;
                        break;
                }
            }
            var dataList = data.Select(x => x.Value).ToList();

            total.AfterRejectAvg = Math.Round(total.AfterRejectAvg / dataList.Count(), 1);
            total.AfterSoldAvg = Math.Round(total.AfterSoldAvg / dataList.Count(), 1);
            total.BeforeRejectAvg = Math.Round(total.BeforeRejectAvg / dataList.Count(), 1);
            total.BeforeSoldAvg = Math.Round(total.BeforeSoldAvg / dataList.Count(), 1);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportSendingTime.csv");

            return Json(new { list = dataList, total = total}, JsonRequestBehavior.AllowGet);
        }

        //************************** Affiliate reports *************************************

        /// <summary>
        /// Reports the affiliates by epl.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportAffiliatesByEpl()
        {
            return PartialView();
        }

        /// <summary>
        /// Gets the report affiliates by epl.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportAffiliatesByEpl()
        {
            ReportCSVGenerator.Instance.Init("Affiliate name/Affiliate channel,Accept rate %,EPL,Redirect %,EPA");

            string affiliateIds = Request["affiliateIds"];

            if (string.IsNullOrEmpty(affiliateIds))
            {
                affiliateIds = "";

                List<Affiliate> affiliateList = (List<Affiliate>)_affiliateService.GetAllAffiliates();

                for (int i = 0; i < affiliateList.Count; i++)
                {
                    affiliateIds += affiliateList[i].Id.ToString();

                    if (i < affiliateList.Count - 1) affiliateIds += ",";
                }
            }

            string aid = Request["affiliateid"];
            if (!string.IsNullOrEmpty(aid) && aid != "0") affiliateIds = aid;

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            List<ReportAffiliatesByEpl> report = (List<ReportAffiliatesByEpl>)this._reportService.ReportAffiliatesByEpl(startDate, endDate, (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(Request["affiliateChannelIds"]) || Request["affiliateChannelIds"] == "null" ? "" : Request["affiliateChannelIds"]));

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem> affiliates = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem>();

            long AffiliateId = 0;
            long affiliateChannelId = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem totals = new ReportAffiliatesByEplModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.AffiliateChannelId = 0;
            totals.AffiliateChannelName = "";
            totals.Total = 0;
            totals.Sold = 0;
            totals.Redirects = 0;
            totals.Profit = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem item2 = null;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem prevItem = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem prevItem2 = null;

            foreach (ReportAffiliatesByEpl r in report)
            {
                if (r.AffiliateId != AffiliateId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", prevItem.AffiliateName, prevItem.AcceptRate, prevItem.EPL, prevItem.RedirectedRate, prevItem.EPA));
                    }

                    item = new Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem();
                    item.title = r.AffiliateName;
                    item.folder = true;
                    item.expanded = true;

                    item.AffiliateId = r.AffiliateId;
                    item.AffiliateName = r.AffiliateName;
                    item.AffiliateChannelId = r.AffiliateChannelId;
                    item.AffiliateChannelName = r.AffiliateChannelName;

                    item.Total = 0;
                    item.Sold = 0;
                    item.Redirects = 0;
                    item.Profit = 0;
                    item.AcceptRate = 0;
                    item.RedirectedRate = 0;
                    item.EPL = 0;
                    item.EPA = 0;

                    affiliates.Add(item);

                    AffiliateId = r.AffiliateId;
                }

                Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByEplModel.TreeItem parent = affiliates[affiliates.Count - 1];

                prevItem2 = item2;

                if (affiliateChannelId != r.AffiliateChannelId)
                {
                    if (prevItem2 != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", "-" + prevItem2.AffiliateChannelName, prevItem2.AcceptRate, prevItem2.EPL, prevItem2.RedirectedRate, prevItem2.EPA));
                    }
                    item2 = new ReportAffiliatesByEplModel.TreeItem();
                    parent.children.Add(item2);
                    affiliateChannelId = r.AffiliateChannelId;
                }

                item2 = parent.children[parent.children.Count - 1];
                item2.title = "<a href='/Management/AffiliateChannel/Item/" + r.AffiliateChannelId.ToString() + "' target='_blank'>" + r.AffiliateChannelId.ToString() + "-" + r.AffiliateChannelName + "</a>";
                item2.folder = false;
                item2.expanded = true;

                item2.AffiliateId = r.AffiliateId;
                item2.AffiliateName = r.AffiliateName;
                item2.AffiliateChannelId = r.AffiliateChannelId;
                item2.AffiliateChannelName = r.AffiliateChannelName;

                item2.Total += r.Total;
                item2.Sold += r.Sold;
                item2.Redirects += r.Redirects;
                item2.Profit += r.Profit;

                item2.AcceptRate = Math.Round(item2.Total > 0 ? (decimal)item2.Sold / (decimal)item2.Total * (decimal)100.0 : 0, 2);
                item2.RedirectedRate = Math.Round(item2.Sold > 0 ? (decimal)item2.Redirects / (decimal)item2.Sold * (decimal)100.0 : 0, 2);
                item2.EPL = Math.Round(item2.Total > 0 ? (decimal)item2.Profit / (decimal)item2.Total : 0, 2);
                item2.EPA = Math.Round(item2.Sold > 0 ? (decimal)item2.Profit / (decimal)item2.Sold : 0, 2);

                parent.Total += r.Total;
                parent.Sold += r.Sold;
                parent.Redirects += r.Redirects;
                parent.Profit += r.Profit;

                parent.AcceptRate = Math.Round(parent.Total > 0 ? (decimal)parent.Sold / (decimal)parent.Total * (decimal)100.0 : 0, 2);
                parent.RedirectedRate = Math.Round(parent.Sold > 0 ? (decimal)parent.Redirects / (decimal)parent.Sold * (decimal)100.0 : 0, 2);
                parent.EPL = Math.Round(parent.Total > 0 ? (decimal)parent.Profit / (decimal)parent.Total : 0, 2);
                parent.EPA = Math.Round(parent.Sold > 0 ? (decimal)parent.Profit / (decimal)parent.Sold : 0, 2);

                totals.Total += r.Total;
                totals.Sold += r.Sold;
                totals.Redirects += r.Redirects;
                totals.Profit += r.Profit;

                totals.AcceptRate = Math.Round(totals.Total > 0 ? (decimal)totals.Sold / (decimal)totals.Total * (decimal)100.0 : 0, 2);
                totals.RedirectedRate = Math.Round(totals.Sold > 0 ? (decimal)totals.Redirects / (decimal)totals.Sold * (decimal)100.0 : 0, 2);
                totals.EPL = Math.Round(totals.Total > 0 ? (decimal)totals.Profit / (decimal)totals.Total : 0, 2);
                totals.EPA = Math.Round(totals.Sold > 0 ? (decimal)totals.Profit / (decimal)totals.Sold : 0, 2);

                prevItem = item;
            }

            affiliates.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", prevItem.title, prevItem.AcceptRate, prevItem.EPL, prevItem.RedirectedRate, prevItem.EPA));
            }

            if (prevItem2 != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4}", "-" + prevItem2.title, prevItem2.AcceptRate, prevItem2.EPL, prevItem2.RedirectedRate, prevItem2.EPA));
            }

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportAffiliatesByEPL.csv");

            return Json(affiliates, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the affiliates by affiliate channels.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportAffiliatesByAffiliateChannels()
        {
            AffiliateReportModel m = new AffiliateReportModel();

            List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates();

            foreach (var b in affiliates)
            {
                m.ListAffiliates.Add(new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            }

            return PartialView(m);
        }

        /// <summary>
        /// Gets the report affiliates by affiliate channels.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportAffiliatesByAffiliateChannels()
        {
            ReportCSVGenerator.Instance.Init("Affiliate/Affiliate Channel,Total leads,Sold leads,Accept rate %,Redirect %,Affiliate Profit,EPL,EPA");

            string affiliateIds = Request["affiliateIds"];

            if (string.IsNullOrEmpty(affiliateIds))
            {
                affiliateIds = "";

                List<Affiliate> affiliateList = (List<Affiliate>)_affiliateService.GetAllAffiliates();

                for (int i = 0; i < affiliateList.Count; i++)
                {
                    affiliateIds += affiliateList[i].Id.ToString();

                    if (i < affiliateList.Count - 1) affiliateIds += ",";
                }
            }

            string aid = Request["affiliateid"];
            if (!string.IsNullOrEmpty(aid) && aid != "0") affiliateIds = aid;

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            List<ReportAffiliatesByAffiliateChannels> report = (List<ReportAffiliatesByAffiliateChannels>)this._reportService.ReportAffiliatesByAffiliateChannels(startDate, endDate, (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(Request["affiliateChannelIds"]) || Request["affiliateChannelIds"] == "null" ? "" : Request["affiliateChannelIds"]));

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem> affiliates = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem>();

            long AffiliateId = 0;
            long affiliateChannelId = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem totals = new ReportAffiliatesByAffiliateChannelsModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.AffiliateChannelId = 0;
            totals.AffiliateChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Debet = 0;
            totals.Credit = 0;
            totals.Redirected = 0;
            totals.Profit = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem item2 = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem prevItem = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem prevItem2 = null;

            foreach (ReportAffiliatesByAffiliateChannels r in report)
            {
                if (r.AffiliateId != AffiliateId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
                    }

                    item = new Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem();
                    item.title = r.AffiliateName;
                    item.folder = true;
                    item.expanded = true;

                    item.AffiliateId = r.AffiliateId;
                    item.AffiliateName = r.AffiliateName;
                    item.AffiliateChannelId = r.AffiliateChannelId;
                    item.AffiliateChannelName = r.AffiliateChannelName;

                    item.TotalLeads = 0;
                    item.SoldLeads = 0;
                    item.Debet = 0;
                    item.Credit = 0;
                    item.Redirected = 0;
                    item.Profit = 0;

                    affiliates.Add(item);

                    AffiliateId = r.AffiliateId;
                }

                Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByAffiliateChannelsModel.TreeItem parent = affiliates[affiliates.Count - 1];
                prevItem2 = item2;

                if (affiliateChannelId != r.AffiliateChannelId)
                {
                    if (prevItem2 != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", "-" + prevItem2.title, prevItem2.TotalLeads, prevItem2.SoldLeads, prevItem2.AcceptRate, prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA));
                    }
                    item2 = new ReportAffiliatesByAffiliateChannelsModel.TreeItem();
                    item2.title = r.AffiliateChannelName;
                    parent.children.Add(item2);
                    affiliateChannelId = r.AffiliateChannelId;
                }

                if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

                item2 = parent.children[parent.children.Count - 1];
                item2.title = r.AffiliateChannelName;
                item2.folder = false;
                item2.expanded = true;

                item2.AffiliateId = r.AffiliateId;
                item2.AffiliateName = r.AffiliateName;
                item2.AffiliateChannelId = r.AffiliateChannelId;
                item2.AffiliateChannelName = r.AffiliateChannelName;

                item2.TotalLeads += r.TotalLeads;
                item2.SoldLeads += r.SoldLeads;
                item2.Debet += r.Debet;
                item2.Credit += r.Credit;
                item2.Redirected += r.Redirected;
                item2.Profit = item2.Debet - item2.Credit;

                item2.AcceptRate = Math.Round(item2.TotalLeads > 0 ? (decimal)item2.SoldLeads / (decimal)item2.TotalLeads * (decimal)100.0 : 0, 2);
                item2.RedirectedRate = Math.Round(item2.SoldLeads > 0 ? (decimal)item2.Redirected / (decimal)item2.SoldLeads * (decimal)100.0 : 0, 2);
                item2.EPL = Math.Round(item2.TotalLeads > 0 ? (decimal)item2.Profit / (decimal)item2.TotalLeads : 0, 2);
                item2.EPA = Math.Round(item2.SoldLeads > 0 ? (decimal)item2.Profit / (decimal)item2.SoldLeads : 0, 2);

                parent.TotalLeads += r.TotalLeads;
                parent.SoldLeads += r.SoldLeads;
                parent.Debet += r.Debet;
                parent.Credit += r.Credit;
                parent.Redirected += r.Redirected;
                parent.Profit = parent.Debet - parent.Credit;

                parent.AcceptRate = Math.Round(parent.TotalLeads > 0 ? (decimal)parent.SoldLeads / (decimal)parent.TotalLeads * (decimal)100.0 : 0, 2);
                parent.RedirectedRate = Math.Round(parent.SoldLeads > 0 ? (decimal)parent.Redirected / (decimal)parent.SoldLeads * (decimal)100.0 : 0, 2);
                parent.EPL = Math.Round(parent.TotalLeads > 0 ? (decimal)parent.Profit / (decimal)parent.TotalLeads : 0, 2);
                parent.EPA = Math.Round(parent.SoldLeads > 0 ? (decimal)parent.Profit / (decimal)parent.SoldLeads : 0, 2);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Debet += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit = totals.Debet - totals.Credit;

                totals.AcceptRate = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0 : 0, 2);
                totals.RedirectedRate = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0 : 0, 2);
                totals.EPL = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
                totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0, 2);

                prevItem = item;
            }

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
            }

            if (prevItem2 != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", "-" + prevItem2.title, prevItem2.TotalLeads, prevItem2.SoldLeads, prevItem2.AcceptRate, prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA));
            }

            affiliates.Add(totals);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportAffiliatesByAffiliateChannels.csv");

            return Json(affiliates, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Reports the affiliates by affiliate channels.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportClickMain()
        {
            AffiliateReportModel m = new AffiliateReportModel();

            List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates();

            foreach (var b in affiliates)
            {
                m.ListAffiliates.Add(new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            }

            return PartialView(m);
        }

        /// <summary>
        /// Gets the report affiliates by affiliate channels.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportClickMain()
        {
            ReportCSVGenerator.Instance.Init("Affiliate/Affiliate Channel,Hits,Unique clicks,Total leads,CTA,Sold leads,Accept rate %,Redirect %,Affiliate Profit,EPL,EPA,EPC");

            string affiliateIds = Request["affiliateIds"];

            if (string.IsNullOrEmpty(affiliateIds))
            {
                affiliateIds = "";

                List<Affiliate> affiliateList = (List<Affiliate>)_affiliateService.GetAllAffiliates();

                for (int i = 0; i < affiliateList.Count; i++)
                {
                    affiliateIds += affiliateList[i].Id.ToString();

                    if (i < affiliateList.Count - 1) affiliateIds += ",";
                }
            }

            string aid = Request["affiliateid"];
            if (!string.IsNullOrEmpty(aid) && aid != "0") affiliateIds = aid;

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            List<ReportClickMain> report = (List<ReportClickMain>)this._reportService.ReportClickMain(startDate, endDate, (string.IsNullOrEmpty(Request["affiliateChannelIds"]) || Request["affiliateChannelIds"] == "null" ? "" : Request["affiliateChannelIds"]));

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem> affiliates = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem>();

            long AffiliateId = 0;
            long affiliateChannelId = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem totals = new ReportClickMainModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.AffiliateChannelId = 0;
            totals.AffiliateChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Debet = 0;
            totals.Credit = 0;
            totals.Redirected = 0;
            totals.Profit = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem item2 = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem prevItem = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem prevItem2 = null;

            foreach (ReportClickMain r in report)
            {
                if (r.AffiliateId != AffiliateId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", prevItem.title, prevItem.Hits, prevItem.UniqueClicks, prevItem.TotalLeads, prevItem.CTA, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA, prevItem.EPC));
                    }

                    item = new Models.Lead.Reports.ReportClickMainModel.TreeItem();
                    item.title = r.AffiliateName;
                    item.folder = true;
                    item.expanded = true;

                    item.AffiliateId = r.AffiliateId;
                    item.AffiliateName = r.AffiliateName;
                    item.AffiliateChannelId = r.AffiliateChannelId;
                    item.AffiliateChannelName = r.AffiliateChannelName;

                    item.TotalLeads = 0;
                    item.SoldLeads = 0;
                    item.Debet = 0;
                    item.Credit = 0;
                    item.Redirected = 0;
                    item.Profit = 0;
                    item.TotalProfit = 0;
                    item.Hits = 0;
                    item.UniqueClicks = 0;
                    item.ClickProfit = 0;
                    item.CTA = 0;
                    item.EPC = 0;

                    affiliates.Add(item);

                    AffiliateId = r.AffiliateId;
                }

                Adrack.Web.ContentManagement.Models.Lead.Reports.ReportClickMainModel.TreeItem parent = affiliates[affiliates.Count - 1];
                prevItem2 = item2;

                if (affiliateChannelId != r.AffiliateChannelId)
                {
                    if (prevItem2 != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "-" + prevItem2.title, prevItem2.Hits, prevItem2.UniqueClicks, prevItem2.TotalLeads, prevItem2.CTA, prevItem2.SoldLeads, prevItem2.AcceptRate, prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA, prevItem2.EPC));
                    }
                    item2 = new ReportClickMainModel.TreeItem();
                    item2.title = r.AffiliateChannelName;
                    parent.children.Add(item2);
                    affiliateChannelId = r.AffiliateChannelId;
                }

                if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

                item2 = parent.children[parent.children.Count - 1];
                item2.title = r.AffiliateChannelName;
                item2.folder = false;
                item2.expanded = true;

                item2.AffiliateId = r.AffiliateId;
                item2.AffiliateName = r.AffiliateName;
                item2.AffiliateChannelId = r.AffiliateChannelId;
                item2.AffiliateChannelName = r.AffiliateChannelName;

                item2.TotalLeads += r.TotalLeads;
                item2.SoldLeads += r.SoldLeads;
                item2.Debet += r.Debet;
                item2.Credit += r.Credit;
                item2.Redirected += r.Redirected;
                item2.Profit = item2.Debet - item2.Credit;
                item2.Hits += r.Hits;
                item2.UniqueClicks += r.UniqueClicks;
                item2.ClickProfit += r.ClickProfit;
                item2.TotalProfit = item2.ClickProfit + item2.Profit;


                item2.AcceptRate = Math.Round(item2.TotalLeads > 0 ? (decimal)item2.SoldLeads / (decimal)item2.TotalLeads * (decimal)100.0 : 0, 2);
                item2.RedirectedRate = Math.Round(item2.SoldLeads > 0 ? (decimal)item2.Redirected / (decimal)item2.SoldLeads * (decimal)100.0 : 0, 2);
                item2.EPL = Math.Round(item2.TotalLeads > 0 ? (decimal)item2.Profit / (decimal)item2.TotalLeads : 0, 2);
                item2.EPA = Math.Round(item2.SoldLeads > 0 ? (decimal)item2.Profit / (decimal)item2.SoldLeads : 0, 2);
                item2.CTA = Math.Round(item2.UniqueClicks > 0 ? (decimal)item2.TotalLeads / (decimal)item2.UniqueClicks * 100 : 0, 2);
                item2.EPC = Math.Round(item2.UniqueClicks > 0 ? (decimal)item2.Profit / (decimal)item2.UniqueClicks : 0, 2);

                parent.TotalLeads += r.TotalLeads;
                parent.SoldLeads += r.SoldLeads;
                parent.Debet += r.Debet;
                parent.Credit += r.Credit;
                parent.Redirected += r.Redirected;
                parent.Profit = parent.Debet - parent.Credit;
                parent.Hits += r.Hits;
                parent.UniqueClicks += r.UniqueClicks;
                parent.ClickProfit += r.ClickProfit;
                parent.TotalProfit = parent.ClickProfit + parent.Profit;

                parent.AcceptRate = Math.Round(parent.TotalLeads > 0 ? (decimal)parent.SoldLeads / (decimal)parent.TotalLeads * (decimal)100.0 : 0, 2);
                parent.RedirectedRate = Math.Round(parent.SoldLeads > 0 ? (decimal)parent.Redirected / (decimal)parent.SoldLeads * (decimal)100.0 : 0, 2);
                parent.EPL = Math.Round(parent.TotalLeads > 0 ? (decimal)parent.Profit / (decimal)parent.TotalLeads : 0, 2);
                parent.EPA = Math.Round(parent.SoldLeads > 0 ? (decimal)parent.Profit / (decimal)parent.SoldLeads : 0, 2);
                parent.CTA = Math.Round(parent.UniqueClicks > 0 ? (decimal)parent.TotalLeads / (decimal)parent.UniqueClicks * 100 : 0, 2);
                parent.EPC = Math.Round(parent.UniqueClicks > 0 ? (decimal)parent.Profit / (decimal)parent.UniqueClicks : 0, 2);


                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Debet += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit = totals.Debet - totals.Credit;
                totals.Hits += r.Hits;
                totals.UniqueClicks += r.UniqueClicks;
                totals.ClickProfit += r.ClickProfit;
                totals.TotalProfit = totals.ClickProfit + totals.Profit;

                totals.AcceptRate = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0 : 0, 2);
                totals.RedirectedRate = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0 : 0, 2);
                totals.EPL = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
                totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0, 2);
                totals.CTA = Math.Round(totals.UniqueClicks > 0 ? (decimal)totals.TotalLeads / (decimal)totals.UniqueClicks * 100 : 0, 2);
                totals.EPC = Math.Round(totals.UniqueClicks > 0 ? (decimal)totals.Profit / (decimal)totals.UniqueClicks : 0, 2);
                 
                prevItem = item;
            }

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "-" + prevItem.title, prevItem.Hits, prevItem.UniqueClicks, prevItem.TotalLeads, prevItem.CTA, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA, prevItem.EPC));
            }

            if (prevItem2 != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "-" + prevItem2.title, prevItem2.Hits, prevItem2.UniqueClicks, prevItem2.TotalLeads, prevItem2.CTA, prevItem2.SoldLeads, prevItem2.AcceptRate, prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA, prevItem2.EPC));
            }

            affiliates.Add(totals);

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "Clicks.csv");

            return Json(affiliates, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Reports the affiliates by campaigns.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportAffiliatesByCampaigns()
        {
            AffiliateReportModel m = new AffiliateReportModel();

            List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates();

            foreach (var b in affiliates)
            {
                m.ListAffiliates.Add(new SelectListItem() { Text = b.Name, Value = b.Id.ToString() });
            }

            return PartialView(m);
        }

        /// <summary>
        /// Gets the report affiliates by campaigns.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportAffiliatesByCampaigns()
        {
            ReportCSVGenerator.Instance.Init("Affiliate Name/Campaign name,Total leads,Sold leads,Accept rate %,Redirect %,Affiliate profit,EPL,EPA");

            string affiliateIds = Request["affiliateIds"];

            if (string.IsNullOrEmpty(affiliateIds))
            {
                affiliateIds = "";

                List<Affiliate> affiliateList = (List<Affiliate>)_affiliateService.GetAllAffiliates();

                for (int i = 0; i < affiliateList.Count; i++)
                {
                    affiliateIds += affiliateList[i].Id.ToString();

                    if (i < affiliateList.Count - 1) affiliateIds += ",";
                }
            }

            long affiliateid = 0;

            long.TryParse(Request["affiliateid"], out affiliateid);

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            List<ReportAffiliatesByCampaigns> report = (List<ReportAffiliatesByCampaigns>)this._reportService.ReportAffiliatesByCampaigns(startDate, endDate, (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(Request["affiliateChannelIds"]) || Request["affiliateChannelIds"] == "null" ? "" : Request["affiliateChannelIds"]));

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByCampaignsModel.TreeItem> affiliates = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByCampaignsModel.TreeItem>();

            string campaignName = "";

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByCampaignsModel.TreeItem totals = new ReportAffiliatesByCampaignsModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.CampaignName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Debit = 0;
            totals.Credit = 0;
            totals.Redirected = 0;
            totals.Profit = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByCampaignsModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByCampaignsModel.TreeItem prevItem = null;

            foreach (ReportAffiliatesByCampaigns r in report)
            {
                if (r.CampaignName != campaignName)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
                    }
                    campaignName = r.CampaignName;
                    item = new Models.Lead.Reports.ReportAffiliatesByCampaignsModel.TreeItem();
                    affiliates.Add(item);
                }

                if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

                item = affiliates[affiliates.Count - 1];
                item.title = r.CampaignName;
                item.folder = false;
                item.expanded = true;

                item.AffiliateId = r.AffiliateId;
                item.AffiliateName = r.AffiliateName;
                item.CampaignName = r.CampaignName;

                item.TotalLeads += r.TotalLeads;
                item.SoldLeads += r.SoldLeads;
                item.Debit += r.Debet;
                item.Credit += r.Credit;
                item.Redirected += r.Redirected;
                item.Profit = item.Debit - item.Credit;

                item.AcceptRate = Math.Round(item.TotalLeads > 0 ? (decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0 : 0, 2);
                item.RedirectedRate = Math.Round(item.SoldLeads > 0 ? (decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0 : 0, 2);
                item.EPL = Math.Round(item.TotalLeads > 0 ? (decimal)item.Profit / (decimal)item.TotalLeads : 0, 2);
                item.EPA = Math.Round(item.SoldLeads > 0 ? (decimal)item.Profit / (decimal)item.SoldLeads : 0, 2);

                //parent.TotalLeads += r.TotalLeads;
                //parent.SoldLeads += r.SoldLeads;
                //parent.Debit += r.Debet;
                //parent.Credit += r.Credit;

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Debit += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit = totals.Debit - totals.Credit;

                totals.AcceptRate = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0 : 0, 2);
                totals.RedirectedRate = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0 : 0, 2);
                totals.EPL = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
                totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0, 2);

                prevItem = item;

                //parent.children.Add(b);
            }

            affiliates.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
            }

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportAffiliatesByCampaigns.csv");

            return Json(affiliates, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the affiliates by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportAffiliatesByStates()
        {
            AffiliateReportModel m = new AffiliateReportModel();

            PrepareAffiliateReportModel(m, 0);

            return PartialView(m);
        }

        /// <summary>
        /// Gets the report affiliates by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportAffiliatesByStates()
        {
            ReportCSVGenerator.Instance.Init("Affiliate Name/Campaign name,Total leads,Sold leads,Accept rate %,Redirect %,Affiliate profit,EPL,EPA");

            string affiliateIds = Request["ids"];
            if (string.IsNullOrEmpty(affiliateIds))
            {
                affiliateIds = "";

                List<Affiliate> affiliateList = (List<Affiliate>)_affiliateService.GetAllAffiliates();

                for (int i = 0; i < affiliateList.Count; i++)
                {
                    affiliateIds += affiliateList[i].Id.ToString();

                    if (i < affiliateList.Count - 1) affiliateIds += ",";
                }
            }

            string aid = Request["affiliateid"];
            string stateIds = Request["sids"];
            //if (!string.IsNullOrEmpty(aid)) affiliatesIds = aid;

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(Request["startDate"], Request["endDate"], false, out startDate, out endDate);

            List<ReportAffiliatesByStates> report = (List<ReportAffiliatesByStates>)this._reportService.ReportAffiliatesByStates(startDate, endDate, (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(Request["affiliateChannelIds"]) || Request["affiliateChannelIds"] == "null" ? "" : Request["affiliateChannelIds"]), stateIds);

            List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByStatesModel.TreeItem> affiliates = new List<Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByStatesModel.TreeItem>();

            string state = "null";

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByStatesModel.TreeItem totals = new ReportAffiliatesByStatesModel.TreeItem();
            totals.title = "Total";
            totals.folder = true;
            totals.expanded = false;

            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.State = "";

            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Debit = 0;
            totals.Credit = 0;
            totals.Redirected = 0;
            totals.Profit = 0;

            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByStatesModel.TreeItem item = null;
            Adrack.Web.ContentManagement.Models.Lead.Reports.ReportAffiliatesByStatesModel.TreeItem prevItem = null;

            foreach (ReportAffiliatesByStates r in report)
            {
                if (state != r.State)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
                    }
                    state = r.State;
                    item = new Models.Lead.Reports.ReportAffiliatesByStatesModel.TreeItem();
                    affiliates.Add(item);
                }

                if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

                item = affiliates[affiliates.Count - 1];
                item.title = r.State;
                item.folder = false;
                item.expanded = true;

                item.AffiliateId = r.AffiliateId;
                item.AffiliateName = r.AffiliateName;
                item.State = r.State;

                item.TotalLeads += r.TotalLeads;
                item.SoldLeads += r.SoldLeads;
                item.Debit += r.Debet;
                item.Credit += r.Credit;
                item.Redirected += r.Redirected;
                item.Profit = item.Debit - item.Credit;

                item.AcceptRate = Math.Round(item.TotalLeads > 0 ? (decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0 : 0, 2);
                item.RedirectedRate = Math.Round(item.SoldLeads > 0 ? (decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0 : 0, 2);
                item.EPL = Math.Round(item.TotalLeads > 0 ? (decimal)item.Profit / (decimal)item.TotalLeads : 0, 2);
                item.EPA = Math.Round(item.SoldLeads > 0 ? (decimal)item.Profit / (decimal)item.SoldLeads : 0, 2);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Debit += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit = totals.Debit - totals.Credit;

                totals.AcceptRate = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0 : 0, 2);
                totals.RedirectedRate = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0 : 0, 2);
                totals.EPL = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
                totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0, 2);

                prevItem = item;
            }

            affiliates.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
            }

            bool downloadCsv = false;
            if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
                return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportAffiliatesByStates.csv");

            return Json(affiliates, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reports the by days.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="showDonuts">if set to <c>true</c> [show donuts].</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportByDays(long id = 0, bool showDonuts = true)
        {
            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            ReportByDaysModel model = new ReportByDaysModel();

            model.ParentId = id;

            model.UserType = _appContext.AppUser != null ? _appContext.AppUser.UserType : 0;
            ViewBag.BuyerType = -1;
            if (model.UserType == SharedData.BuyerUserTypeId)
            {
                Buyer buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);
                if (buyer != null)
                {
                    ViewBag.BuyerType = buyer.AlwaysSoldOption;
                }
            }

            model.Report = new List<ReportByDays>();// (List<ReportByDays>)_reportService.ReportByDays(start, id);

            ViewBag.BaseUrl = Helper.GetBaseUrl(Request);
            ViewBag.TimeZoneNow = this._settingService.GetTimeZoneDate(DateTime.UtcNow).ToShortDateString();

            ViewBag.showDonuts = showDonuts;
            ViewBag.AllCampaignsList = (List<Campaign>)this._campaignService.GetAllCampaigns(0);

            return PartialView(model);
        }

        /// <summary>
        /// Gets the report by days.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportByDays()
        {
            short timeZone = 1;
            short.TryParse(Request["tz"], out timeZone);

            long parentid = 0;

            try
            {
                parentid = long.Parse(Request["parentid"]);
            }
            catch
            {
            }

            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow, (timeZone == 2 ? _appContext.AppUser : null));

            DateTime startDate = start.AddMonths(-1);
            DateTime endDate = start;

            if (Request["dates"] != null && Request["mode"] != null && int.Parse(Request["mode"]) == 1)
            {
                var datesArr = Request["dates"].Split('-');

                if (datesArr.Length > 1)
                {
                    startDate = Convert.ToDateTime(datesArr[0].Trim());
                    start = startDate;
                    endDate = Convert.ToDateTime(datesArr[1].Trim());
                }
            }

            long CampaingnId = 0;
            if (Request["campaingnid"] != null)
            {
                CampaingnId = long.Parse(Request["campaingnid"]);
            }

            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            DateTime tzStart = startDate;
            DateTime tzEnd = endDate;

            DateTime dateIter = startDate;
            DateTime dateIterEnd = endDate;

            startDate = _settingService.GetUTCDate(startDate, (timeZone == 2 ? _appContext.AppUser : null));
            endDate = _settingService.GetUTCDate(endDate, (timeZone == 2 ? _appContext.AppUser : null));

            Setting tzSetting = _settingService.GetSetting("TimeZone");

            List<ReportByDays> Report = (List<ReportByDays>)_reportService.ReportByDays(tzStart, tzEnd, CampaingnId, parentid);

            List<object> data = new List<object>();
            List<object> dates = new List<object>();

            object soldsum = new object();
            object receivedsum = new object();
            object postedsum = new object();
            object revenuesum = new object();
            object profitsum = new object();

            long SoldSum = 0;
            long ReceivedSum = 0;
            long PostedSum = 0;
            long LoanedSum = 0;
            decimal BPriceSum = 0;
            decimal ProfitSum = 0;
            decimal APriceSum = 0;

            List<ReportTotalsByDate> totalsByDate = (List<ReportTotalsByDate>)_reportService.ReportTotalsByDate(tzStart, tzEnd, CampaingnId, parentid);
            if (totalsByDate.Count > 0)
            {
                PostedSum = totalsByDate[0].posted;
                SoldSum = totalsByDate[0].sold;
                BPriceSum = totalsByDate[0].bprice;
                ProfitSum = totalsByDate[0].profit;
                APriceSum = totalsByDate[0].aprice;
                ReceivedSum = totalsByDate[0].received;
                LoanedSum = totalsByDate[0].loaned;
            }

            List<string> MonthStr = new List<string>();
            do
            {
                MonthStr.Add(dateIter.ToShortDateString());
                dateIter = dateIter.AddDays(1);
            }
            while (dateIter <= dateIterEnd);

            bool inArray = false;

            foreach (string mStr in MonthStr)
            {
                inArray = false;
                foreach (ReportByDays r in Report)
                {
                    //DateTime dt = r.Created;
                    //dt = _settingService.GetTimeZoneDate(dt, (timeZone == 2 ? _appContext.AppUser : null), tzSetting);

                    if (mStr == r.Created.ToShortDateString())
                    {
                        dates.Add(mStr);
                        //dates.Add(r.Created.ToShortDateString());
                        //data.Add(new { year = r.Created.Year, month = r.Created.Month, day = r.Created.Day, posted = r.Total, sold = r.Sold, received = r.Received, revenue = r.AffiliatePrice, profit = r.Profit });
                        data.Add(new { posted = r.Total, sold = r.Sold, received = r.Received, bprice = r.BuyerPrice, aprice = r.AffiliatePrice, profit = r.Profit });

                        inArray = true;
                        break;
                    }
                }

                if (inArray == false)
                {
                    dates.Add(mStr);
                    data.Add(new { posted = 0, sold = 0, received = 0, bprice = 0, profit = 0, aprice = 0 });
                }
            }

            long[] StatusesArr = GetReportByStatuses(startDate, endDate);
            string StatusesStr = StatusesStr = string.Join(",", StatusesArr);
            long StatusesTotals = 0;

            foreach (long ll in StatusesArr)
            {
                StatusesTotals += ll;
            }

            string topStatesStr = GetReportTopStates(tzStart, tzEnd, parentid > 0 ? parentid : 0, parentid < 0 ? -parentid : 0, CampaingnId);

            return Json(new { data = data, dates = dates, soldsum = SoldSum, receivedsum = ReceivedSum, loanedsum = LoanedSum, postedsum = PostedSum, bpricesum = BPriceSum, profitsum = ProfitSum, apricesum = APriceSum, statuses = StatusesStr, statusestotals = StatusesTotals, topstates = topStatesStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the report by date rang.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportByDateRang()
        {
            short timeZone = 1;
            short.TryParse(Request["tz"], out timeZone);

            long parentid = 0;

            try
            {
                parentid = long.Parse(Request["parentid"]);
            }
            catch
            {
            }

            DateTime now = _settingService.GetTimeZoneDate(DateTime.UtcNow, (timeZone == 2 ? _appContext.AppUser : null));

            try
            {
                if (Request["date"]!=null && !Request["date"].ToString().Contains("-")) //do not check ranges
                    now = DateTime.Parse(Request["date"]);
            }
            catch
            {
            }

            long campaignid = 0;

            try
            {
                if (Request["campaignid"]!=null)
                    campaignid = long.Parse(Request["campaignid"]);
            }
            catch
            {
            }

            if (_appContext.AppUser.UserType == SharedData.BuyerUserTypeId)
            {
                parentid = _appContext.AppUser.ParentId;
            }

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
            {
                parentid = -_appContext.AppUser.ParentId;
            }

            /*if (this._appContext.AppUser.UserTypeId == SharedData.NetowrkUserTypeId || this._appContext.AppUser.UserTypeId == SharedData.BuiltInUserTypeId)
            {
                uList.Clear();

                UsersIdName.Add(new KeyValuePair<long, string>(this._appContext.AppUser.Id, this._appContext.AppUser.GetFullName()));

                List<Affiliate> affiliates = (List<Affiliate>)_affiliateService.GetAllAffiliates(this._appContext.AppUser, 0);

                foreach (Affiliate aff in affiliates)
                {
                    User user = _userService.GetUserByParentId(aff.Id, SharedData.AffiliateUserTypeId);
                    if (user != null)
                        UsersIdName.Add(new KeyValuePair<long, string>(user.Id, user.GetFullName()));
                }

                List<Buyer> buyers = (List<Buyer>)_buyerService.GetAllBuyers(this._appContext.AppUser, 0);

                foreach (Buyer buyer in buyers)
                {
                    User user = _userService.GetUserByParentId(buyer.Id, SharedData.BuyerUserTypeId);
                    if (user != null)
                        UsersIdName.Add(new KeyValuePair<long, string>(user.Id, user.GetFullName()));
                }
            }*/

            DateTime start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            DateTime end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            var datesArr = Request["date"].Split('-');

            if (datesArr.Length > 1)
            {
                start = Convert.ToDateTime(datesArr[0].Trim() + " 00:00:00");
                end = Convert.ToDateTime(datesArr[1].Trim() + " 23:59:59");
            }

            DateTime tzStart = start;
            DateTime tzEnd = end;

            start = _settingService.GetUTCDate(start, (timeZone == 2 ? _appContext.AppUser : null));
            end = _settingService.GetUTCDate(end, (timeZone == 2 ? _appContext.AppUser : null));

            int delta = 10;

            List<ReportByMinutes> ReportSold = (List<ReportByMinutes>)_reportService.ReportByDate("sold", start, end, delta, campaignid, parentid);
            List<ReportByMinutes> ReportReceived = (List<ReportByMinutes>)_reportService.ReportByDate("received", start, end, delta, campaignid, parentid);
            List<ReportByMinutes> ReportPosted = (List<ReportByMinutes>)_reportService.ReportByDate("posted", start, end, delta, campaignid, parentid);

            List<object> dates = new List<object>();
            List<object> sold = new List<object>();
            List<object> received = new List<object>();
            List<object> posted = new List<object>();
            List<object> bprice = new List<object>();
            List<object> profit = new List<object>();
            List<object> aprice = new List<object>();

            object soldsum = new object();
            object receivedsum = new object();
            object postedsum = new object();
            object bpricesum = new object();
            object profitsum = new object();
            object apricesum = new object();

            long SoldSum = 0;
            long ReceivedSum = 0;
            long PostedSum = 0;
            long LoanedSum = 0;
            decimal BPriceSum = 0;
            decimal ProfitSum = 0;
            decimal APriceSum = 0;

            DateTime dateIter = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0);  //start;
            DateTime dateEndIter = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 23, 59, 59);

            List<DateTime> minutesStr = new List<DateTime>();

            do
            {
                minutesStr.Add(dateIter);
                dates.Add(String.Format("{0:HH:mm}", dateIter) /*dateIter.ToShortTimeString() */);
                dateIter = dateIter.AddMinutes(delta);
            }
            while (dateIter <= dateEndIter);

            DateTime prev = minutesStr[0];
            Setting tzSetting = _settingService.GetSetting("TimeZone");

            ReportByMinutes values = new ReportByMinutes();

            foreach (DateTime mStr in minutesStr)
            {
                values.Leads = 0;
                foreach (ReportByMinutes r in ReportReceived)
                {
                    DateTime dt = new DateTime(r.Yr, r.Mt, r.Dy, r.Hr, r.Mn, 0);
                    dt = _settingService.GetTimeZoneDate(dt, (timeZone == 2 ? _appContext.AppUser : null), tzSetting);
                    //if (mStr == dt.ToShortTimeString())
                    if (dt.Hour == mStr.Hour && dt.Minute >= mStr.Minute && dt.Minute < mStr.Minute + delta)
                    {
                        values.Leads = r.Leads;
                        break;
                    }
                }
                received.Add(values.Leads);

                values.Leads = 0;
                foreach (ReportByMinutes r in ReportPosted)
                {
                    DateTime dt = new DateTime(r.Yr, r.Mt, r.Dy, r.Hr, r.Mn, 0);
                    dt = _settingService.GetTimeZoneDate(dt, (timeZone == 2 ? _appContext.AppUser : null), tzSetting);

                    //if (mStr == dt.ToShortTimeString())
                    if (dt.Hour == mStr.Hour && dt.Minute >= mStr.Minute && dt.Minute < mStr.Minute + delta)
                    {
                        values.Leads = r.Leads;
                        break;
                    }
                }
                posted.Add(values.Leads);

                values.Leads = 0;
                values.Profit = 0;
                values.BuyerPrice = 0;
                values.AffiliatePrice = 0;

                foreach (ReportByMinutes r in ReportSold)
                {
                    DateTime dt = new DateTime(r.Yr, r.Mt, r.Dy, r.Hr, r.Mn, 0);
                    dt = _settingService.GetTimeZoneDate(dt, (timeZone == 2 ? _appContext.AppUser : null), tzSetting);

                    //if (mStr == dt.ToShortTimeString())
                    if (dt.Hour == mStr.Hour && dt.Minute >= mStr.Minute && dt.Minute < mStr.Minute + delta)
                    {
                        values.Leads = r.Leads;
                        values.BuyerPrice = r.BuyerPrice;
                        values.Profit = r.Profit;
                        values.AffiliatePrice = r.BuyerPrice - r.Profit;
                        break;
                    }
                }
                sold.Add(values.Leads);
                bprice.Add(values.BuyerPrice);
                aprice.Add(values.AffiliatePrice);
                profit.Add(values.Profit);

                prev = mStr;
            }

            List<ReportTotalsByDate> totalsByDate = (List<ReportTotalsByDate>)_reportService.ReportTotalsByDate(tzStart, tzEnd, campaignid, parentid);
            if (totalsByDate.Count > 0)
            {
                PostedSum = totalsByDate[0].posted;
                SoldSum = totalsByDate[0].sold;
                BPriceSum = totalsByDate[0].bprice;
                ProfitSum = totalsByDate[0].profit;
                APriceSum = totalsByDate[0].aprice;
                ReceivedSum = totalsByDate[0].received;
                LoanedSum = totalsByDate[0].loaned;
            }

            long[] StatusesArr = GetReportByStatuses(start, end);
            string StatusesStr = StatusesStr = string.Join(",", StatusesArr);
            long StatusesTotals = 0;

            foreach (long ll in StatusesArr)
            {
                StatusesTotals += ll;
            }

            string topStatesStr = GetReportTopStates(tzStart, tzEnd, parentid > 0 ? parentid : 0, parentid < 0 ? -parentid : 0, campaignid);

            return Json(new { dates = dates, sold = sold, received = received, posted = posted, bprice = bprice, aprice = aprice, profit = profit, dc = dates.Count, sc = sold.Count, rc = received.Count, pc = posted.Count, soldsum = SoldSum, receivedsum = ReceivedSum, postedsum = PostedSum, loanedsum = LoanedSum, bpricesum = BPriceSum, profitsum = ProfitSum, apricesum = APriceSum, statuses = StatusesStr, statusestotals = StatusesTotals, topstates = topStatesStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the report by minutes.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportByMinutes()
        {
            short timeZone = 1;
            short.TryParse(Request["tz"], out timeZone);

            long parentid = 0;

            try
            {
                parentid = long.Parse(Request["parentid"]);
            }
            catch
            {
            }

            Setting tzSetting = _settingService.GetSetting("TimeZone");
            DateTime now = _settingService.GetTimeZoneDate(DateTime.UtcNow, (timeZone == 2 ? _appContext.AppUser : null), tzSetting);

            try
            {
                now = DateTime.Parse(Request["date"]);
            }
            catch
            {
            }

            DateTime start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            DateTime end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            DateTime tzStart = start;
            DateTime tzEnd = end;

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            List<ReportByMinutes> Report = (List<ReportByMinutes>)_reportService.ReportByMinutes(start, end, parentid);

            List<object> dates = new List<object>();
            List<object> sold = new List<object>();
            List<object> received = new List<object>();
            List<object> posted = new List<object>();
            List<object> bprice = new List<object>();
            List<object> profit = new List<object>();
            List<object> aprice = new List<object>();

            object soldsum = new object();
            object receivedsum = new object();
            object postedsum = new object();
            object bpricesum = new object();
            object profitsum = new object();
            object apricesum = new object();

            long SoldSum = 0;
            long ReceivedSum = 0;
            long PostedSum = 0;
            decimal BPriceSum = 0;
            decimal ProfitSum = 0;
            decimal APriceSum = 0;

            DateTime dateIter = start;

            List<string> minutesStr = new List<string>();
            do
            {
                minutesStr.Add(dateIter.ToShortTimeString());
                dates.Add(String.Format("{0:HH:mm}", dateIter) /*dateIter.ToShortTimeString() */);
                dateIter = dateIter.AddMinutes(10);
            }
            while (dateIter <= end);

            bool inArray = false;

            int rowTypeCount = 0;

            foreach (string mStr in minutesStr)
            {
                inArray = false;
                rowTypeCount = 0;
                foreach (ReportByMinutes r in Report)
                {
                    DateTime dt = new DateTime(now.Year, now.Month, now.Day, r.Hr, r.Mn, 0);
                    if (mStr == dt.ToShortTimeString())
                    {
                        if (rowTypeCount == 0 && r.Activity != "sold")
                        {
                            sold.Add(0);
                            bprice.Add(0);
                            aprice.Add(0);
                            profit.Add(0);
                            rowTypeCount++;
                            inArray = true;
                            continue;
                        }

                        switch (r.Activity)
                        {
                            case "received":
                                {
                                    received.Add(r.Leads);
                                    ReceivedSum += r.Leads;
                                    rowTypeCount++;
                                    break;
                                }

                            case "posted":
                                {
                                    posted.Add(r.Leads);
                                    PostedSum += r.Leads;
                                    rowTypeCount++;
                                    break;
                                }

                            case "sold":
                                {
                                    sold.Add(r.Leads);
                                    bprice.Add(r.BuyerPrice);
                                    aprice.Add(r.AffiliatePrice);
                                    profit.Add(r.Profit);
                                    SoldSum += r.Leads;
                                    BPriceSum += r.BuyerPrice;
                                    APriceSum += r.AffiliatePrice;
                                    ProfitSum += r.Profit;
                                    rowTypeCount++;
                                    break;
                                }
                        }

                        inArray = true;
                    }
                }

                if (inArray == false)
                {
                    sold.Add(0);
                    received.Add(0);
                    posted.Add(0);
                    bprice.Add(0);
                    aprice.Add(0);
                    profit.Add(0);
                }
                else
                {
                    if (rowTypeCount < 3)
                    {
                        sold.Add(0);
                    }
                }
            }

            long[] StatusesArr = GetReportByStatuses(start, end);
            string StatusesStr = StatusesStr = string.Join(",", StatusesArr);
            long StatusesTotals = 0;

            foreach (long ll in StatusesArr)
            {
                StatusesTotals += ll;
            }

            string topStatesStr = GetReportTopStates(tzStart, tzEnd, parentid > 0 ? parentid : 0, parentid < 0 ? -parentid : 0, 0);

            return Json(new { dates = dates, sold = sold, received = received, posted = posted, bprice = bprice, aprice = aprice, profit = profit, dc = dates.Count, sc = sold.Count, rc = received.Count, pc = posted.Count, soldsum = SoldSum, receivedsum = ReceivedSum, postedsum = PostedSum, loanedsum = 0, bpricesum = BPriceSum, profitsum = ProfitSum, apricesum = APriceSum, statuses = StatusesStr, statusestotals = StatusesTotals, topstates = topStatesStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the report by hour.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportByHour()
        {
            long parentid = 0;

            try
            {
                parentid = long.Parse(Request["parentid"]);
            }
            catch
            {
            }

            short timeZone = 1;
            short.TryParse(Request["tz"], out timeZone);

            Setting tzSetting = _settingService.GetSetting("TimeZone");
            DateTime now = _settingService.GetTimeZoneDate(DateTime.UtcNow, (timeZone == 2 ? _appContext.AppUser : null), tzSetting);

            DateTime start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            DateTime end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            DateTime tzStart = start;
            DateTime tzEnd = end;

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            List<ReportByHour> Report = (List<ReportByHour>)_reportService.ReportByHour(start, end, parentid);

            List<object> dates = new List<object>();
            List<object> sold = new List<object>();
            List<object> received = new List<object>();
            List<object> posted = new List<object>();

            sold.Add(new List<object>() { now.Year, now.Month, now.Day, 0, 0, 0 });
            received.Add(new List<object>() { now.Year, now.Month, now.Day, 0, 0, 0 });
            posted.Add(new List<object>() { now.Year, now.Month, now.Day, 0, 0, 0 });

            foreach (ReportByHour r in Report)
            {
                if (r.Activity == "sold")
                    sold.Add(new List<object>() { r.Yr, r.Mt, r.Dy, r.Hr, r.Leads });
                if (r.Activity == "received")
                    received.Add(new List<object>() { r.Yr, r.Mt, r.Dy, r.Hr, r.Leads });
                if (r.Activity == "posted")
                    posted.Add(new List<object>() { r.Yr, r.Mt, r.Dy, r.Hr, r.Leads });
            }

            sold.Add(new List<object>() { now.Year, now.Month, now.Day, 23, 59, 0 });
            received.Add(new List<object>() { now.Year, now.Month, now.Day, 23, 59, 0 });
            posted.Add(new List<object>() { now.Year, now.Month, now.Day, 23, 59, 0 });

            long[] StatusesArr = GetReportByStatuses(start, end);
            string StatusesStr = StatusesStr = string.Join(",", StatusesArr);
            long StatusesTotals = 0;

            foreach (long ll in StatusesArr)
            {
                StatusesTotals += ll;
            }
            string topStatesStr = GetReportTopStates(tzStart, tzEnd, parentid > 0 ? parentid : 0, parentid < 0 ? -parentid : 0, 0);

            return Json(new { dates = dates, sold = sold, received = received, posted = posted, dc = dates.Count, sc = sold.Count, rc = received.Count, pc = posted.Count, statuses = StatusesStr, statusestotals = StatusesTotals, topstates = topStatesStr }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the report by year.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportByYear()
        {
            short timeZone = 1;
            short.TryParse(Request["tz"], out timeZone);

            DateTime now2 = DateTime.Now;
            long parentid = 0;
            long campaingnid = 0;

            try
            {
                parentid = long.Parse(Request["parentid"]);
            }
            catch
            {
            }

            if (Request["campaingnid"] != null)
            {
                campaingnid = long.Parse(Request["campaingnid"]);
            }

            DateTime now = _settingService.GetTimeZoneDate(DateTime.UtcNow, (timeZone == 2 ? _appContext.AppUser : null));

            DateTime start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            start = start.AddMonths(-12);
            DateTime end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            DateTime tzStart = start;
            DateTime tzEnd = end;

            start = _settingService.GetUTCDate(start, (timeZone == 2 ? _appContext.AppUser : null));
            end = _settingService.GetUTCDate(end, (timeZone == 2 ? _appContext.AppUser : null));

            List<ReportByYear> Report = (List<ReportByYear>)_reportService.ReportByYear(start, end, parentid, campaingnid);

            List<object> dates = new List<object>();
            List<object> sold = new List<object>();
            List<object> received = new List<object>();
            List<object> posted = new List<object>();

            object soldsum = new object();
            object receivedsum = new object();
            object postedsum = new object();
            object revenuesum = new object();
            object profitsum = new object();

            long SoldSum = 0;
            long ReceivedSum = 0;
            long PostedSum = 0;
            long LoanedSum = 0;
            decimal BPriceSum = 0;
            decimal ProfitSum = 0;
            decimal APriceSum = 0;

            double t0 = (DateTime.Now - now2).TotalMilliseconds;

            List<ReportTotalsByDate> totalsByDate = (List<ReportTotalsByDate>)_reportService.ReportTotalsByDate(tzStart, tzEnd, campaingnid, parentid);
            if (totalsByDate.Count > 0)
            {
                PostedSum = totalsByDate[0].posted;
                SoldSum = totalsByDate[0].sold;
                BPriceSum = totalsByDate[0].bprice;
                ProfitSum = totalsByDate[0].profit;
                APriceSum = totalsByDate[0].aprice;
                ReceivedSum = totalsByDate[0].received;
                LoanedSum = totalsByDate[0].loaned;
            }

            double t1 = (DateTime.Now - now2).TotalMilliseconds;

            int[] soldArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] receivedArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] postedArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            decimal[] bPriceArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            decimal[] aPriceArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            decimal[] profitArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            DateTime dateIter = start;
            int kk = 0;
            do
            {
                dateIter = dateIter.AddMonths(1);
                dates.Add(new List<object>() { dateIter.Year + "/" + dateIter.Month });

                foreach (ReportByYear r in Report)
                {
                    if (dateIter.Year == r.Yr && dateIter.Month == r.Mt)
                    {
                        if (r.Activity == "sold")
                        {
                            soldArr[kk] = r.Leads;
                            bPriceArr[kk] = r.BuyerPrice;
                            profitArr[kk] = r.Profit;
                            aPriceArr[kk] = r.BuyerPrice - r.Profit;
                        }
                        if (r.Activity == "received")
                        {
                            receivedArr[kk] = r.Leads;
                            //ReceivedSum += r.Leads;
                        }
                        if (r.Activity == "posted")
                        {
                            postedArr[kk] = r.Leads;
                        }
                    }
                }
                kk++;
            }
            while (dateIter <= end);

            double t2 = (DateTime.Now - now2).TotalMilliseconds;

            long[] StatusesArr = GetReportByStatuses(start, end);

            double t3 = (DateTime.Now - now2).TotalMilliseconds;

            string StatusesStr = StatusesStr = string.Join(",", StatusesArr);
            long StatusesTotals = 0;

            foreach (long ll in StatusesArr)
            {
                StatusesTotals += ll;
            }

            string topStatesStr = GetReportTopStates(tzStart, tzEnd, parentid > 0 ? parentid : 0, parentid < 0 ? -parentid : 0, campaingnid);

            double t4 = (DateTime.Now - now2).TotalMilliseconds;

            return Json(new { dates = dates, sold = soldArr, received = receivedArr, posted = postedArr, aprice = aPriceArr, bprice = bPriceArr, profit = profitArr, dc = dates.Count, sc = sold.Count, rc = received.Count, pc = posted.Count, soldsum = SoldSum, receivedsum = ReceivedSum, loanedsum = LoanedSum, postedsum = PostedSum, bpricesum = BPriceSum, apricesum = APriceSum, profitsum = ProfitSum, statuses = StatusesStr, statusestotals = StatusesTotals, topstates = topStatesStr, t0 = t0, t1 = t1, t2 = t2, t3 = t3, t4 = t4 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the report by statuses.
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns>System.Int64[].</returns>
        public long[] GetReportByStatuses(DateTime dateFrom, DateTime dateTo)
        {
            IList<ReportByStatuses> statusesList = this._reportService.ReportByStatuses(dateFrom, dateTo);

            long[] retVal = new long[10];

            int j = 0;
            bool inArray = false;
            for (int k = 0; k <= 5; k++)
            {
                inArray = false;
                for (j = 0; j < statusesList.Count; j++)
                {
                    if (k == statusesList[j].Status)
                    {
                        inArray = true;
                        break;
                    }
                }

                retVal[k] = inArray ? statusesList[j].Counts : 0;
            }

            return retVal;
        }

        /// <summary>
        /// Gets the report top states.
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="campaingnid">The campaingnid.</param>
        /// <returns>System.String.</returns>
        public string GetReportTopStates(DateTime dateFrom, DateTime dateTo, long BuyerId, long AffiliateId, long campaingnid)
        {
            List<ReportTopStates> report = (List<ReportTopStates>)this._reportService.ReportByTopStates(dateFrom, dateTo, 5, BuyerId, AffiliateId, campaingnid);

            Dictionary<string, string> statesList = new Dictionary<string, string>();

            foreach (ReportTopStates ai in report)
            {
                statesList.Add(ai.State != null ? ai.State : "Unknown", String.Format("{0:###,###,###}", ai.Counts));
            }

            return JsonConvert.SerializeObject(statesList);
        }

        /// <summary>
        /// Reports the totals.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportTotals(long id = 0)
        {
            ViewBag.BaseUrl = Helper.GetBaseUrl(Request);

            return PartialView(id);
        }

        /// <summary>
        /// Reports the totals buyer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportTotalsBuyer(long id = 0)
        {
            ViewBag.BaseUrl = Helper.GetBaseUrl(Request);

            ViewBag.BuyerId = _appContext.AppUser.ParentId;

            return PartialView(id);
        }

        /// <summary>
        /// Gets the report totals.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportTotals()
        {
            long parentid = 0;

            try
            {
                parentid = long.Parse(Request["parentid"]);
            }
            catch
            {
            } 

            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            DateTime end = _settingService.GetTimeZoneDate(DateTime.UtcNow);

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59); 

            List<ReportTotals> report = (List<ReportTotals>)this._reportService.ReportTotals(start, end, parentid);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = report.Count;
            jd.recordsFiltered = report.Count;
            jd.TimeZoneNowStr = start.ToString() + "-" + end.ToString();

            string change = "";
            int k = 0;
            foreach (ReportTotals ai in report)
            {
                if (ai.receivedp > 0)
                {
                    change = "<span class='text-success-600'><i class='icon-stats-growth2 position-left'></i> " + ai.receivedp.ToString() + "</span>";
                }
                else
                {
                    change = "<span class='text-danger'><i class='icon-stats-decline2 position-left'></i> " + Math.Abs(ai.receivedp).ToString() + "</span>";
                }

                if (ai.name != "Today")
                {
                    if (ai.received > 0 && ai.receivedp == 0)
                        ai.receivedp = 100;

                    if (ai.total > 0 && ai.totalp == 0)
                        ai.totalp = 100;

                    if (ai.sold > 0 && ai.soldp == 0)
                        ai.soldp = 100;

                    if (ai.debit > 0 && ai.debitp == 0)
                        ai.debitp = 100;

                    if (ai.profit > 0 && ai.profitp == 0)
                        ai.profitp = 100;

                    if (ai.redirected > 0 && ai.redirectedp == 0)
                        ai.redirectedp = 100;
                }

                string[] names1 = {
                                      "<div style='text-align: left' id='NameType" + k.ToString() + "'>" + ai.name + "</div>",
                                      ai.received.ToString() + "<br>" + (ai.receivedp == 0 ? " " : ( ai.receivedp > 0 ? "<span class='text-danger'><i class='icon-stats-decline2 position-left'></i> " + Math.Round(ai.receivedp, 2).ToString() + "%</span>" : "<span class='text-success-600'><i class='icon-stats-growth2 position-left'></i> " + Math.Round(Math.Abs(ai.receivedp), 2).ToString() + "%</span>")),
                                      ai.total.ToString() + "<br>" + (ai.totalp == 0 ? " " : ( ai.totalp > 0 ? "<span class='text-danger'><i class='icon-stats-decline2 position-left'></i> " +  Math.Round(ai.totalp, 2).ToString() + "%</span>" : "<span class='text-success-600'><i class='icon-stats-growth2 position-left'></i> " + Math.Round(Math.Abs(ai.totalp), 2).ToString() + "%</span>")),
                                      ai.redirected.ToString() + "<br>" + (ai.redirectedp == 0 ? " " : ( ai.redirectedp > 0 ? "<span class='text-danger'><i class='icon-stats-decline2 position-left'></i> " +  Math.Round(ai.redirectedp, 2).ToString() + "%</span>" : "<span class='text-success-600'><i class='icon-stats-growth2 position-left'></i> " + Math.Round(Math.Abs(ai.redirectedp), 2).ToString() + "%</span>")),
                                      String.Format("{0:$###,###,###.00}", ai.sold) + "<br>" + (ai.soldp == 0 ? " " : ( ai.soldp > 0 ? "<span class='text-danger'><i class='icon-stats-decline2 position-left'></i> " +  Math.Round(ai.soldp, 2).ToString() + "%</span>" : "<span class='text-success-600'><i class='icon-stats-growth2 position-left'></i> " + Math.Round(Math.Abs(ai.soldp), 2).ToString() + "%</span>")),
                                      String.Format("{0:$###,###,###.00}", ai.debit) + "<br>" + (ai.debitp == 0 ? " " : ( ai.debitp > 0 ? "<span class='text-danger'><i class='icon-stats-decline2 position-left'></i> " +  Math.Round(ai.debitp, 2).ToString() + "%</span>" : "<span class='text-success-600'><i class='icon-stats-growth2 position-left'></i> " + Math.Round(Math.Abs(ai.debitp), 2).ToString() + "%</span>")),
                                      String.Format("{0:$###,###,###.00}", ai.profit) + "<br>" + (ai.profitp == 0 ? " " : ( ai.profitp > 0 ? "<span class='text-danger'><i class='icon-stats-decline2 position-left'></i> " +  Math.Round(ai.profitp, 2).ToString() + "%</span>" : "<span class='text-success-600'><i class='icon-stats-growth2 position-left'></i> " + Math.Round(Math.Abs(ai.profitp), 2).ToString() + "%</span>")),
                                      ai.received != 0 ? String.Format("{0:$###,###,###.00}", (ai.debit / ai.received)) : "",
                                      ai.sold != 0 ? String.Format("{0:$###,###,###.00}", (ai.debit / ai.sold)) : ""
                                };
                jd.data.Add(names1);
            }
            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the report totals buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [ContentManagementAntiForgery(true)]
        [AppHttpsRequirement(SslRequirement.No)]
        public ActionResult GetReportTotalsBuyer()
        {
            long parentid = 0;

            try
            {
                parentid = long.Parse(Request["parentid"]);
            }
            catch
            {
            }

            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            DateTime end = _settingService.GetTimeZoneDate(DateTime.UtcNow);

            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            List<ReportTotalsBuyer> report = (List<ReportTotalsBuyer>)this._reportService.ReportTotalsBuyer(start, end, parentid);

            JsonData jd = new JsonData();
            jd.draw = 1;
            jd.recordsTotal = report.Count;
            jd.recordsFiltered = report.Count;
            foreach (ReportTotalsBuyer ai in report)
            {
                    string[] names1 = {
                                      "<b>" + ai.name + "</b>",
                                      "<p class='text-center'>" + String.Format("{0:$###,###,###.00}", ai.debit) + "</p>",
                                      "<p class='text-center'>" + String.Format("{0:###,###,###}", ai.received) + "</p>",
                                      "<p class='text-center'>" + String.Format("{0:###,###,###}", ai.sold) + "</p>",
                                      "<p class='text-center'>" + String.Format("{0:###,###,###}", ai.rejected) + "</p>"
                                };
                    jd.data.Add(names1);
            }

            return Json(jd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Class ReportRow.
        /// </summary>
        public class ReportRow
        {
            /// <summary>
            /// The buyer identifier
            /// </summary>
            public long buyerId;

            /// <summary>
            /// The store identifier
            /// </summary>
            public long storeId;

            /// <summary>
            /// The total
            /// </summary>
            public int total;

            /// <summary>
            /// The initial
            /// </summary>
            public int initial;

            /// <summary>
            /// The reminder1
            /// </summary>
            public int reminder1;

            /// <summary>
            /// The reminder2
            /// </summary>
            public int reminder2;

            /// <summary>
            /// The client busy
            /// </summary>
            public int clientBusy = 0;

            /// <summary>
            /// The redirected
            /// </summary>
            public int redirected = 0;

            /// <summary>
            /// The store pressed one
            /// </summary>
            public int storePressedOne = 0;

            /// <summary>
            /// The total duration
            /// </summary>
            public double totalDuration;

            /// <summary>
            /// The no action
            /// </summary>
            public int noAction;

            /// <summary>
            /// The average duration
            /// </summary>
            public double avgDuration;

            /// <summary>
            /// The p initial
            /// </summary>
            public double pInitial;

            /// <summary>
            /// The p reminder1
            /// </summary>
            public double pReminder1;

            /// <summary>
            /// The p reminder2
            /// </summary>
            public double pReminder2;
        }

        /// <summary>
        /// Subs the identifier converter.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult SubIdConverter()
        {
            return View();
        }

        /// <summary>
        /// Decrypts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult Decrypt()
        {
            string value = "Not valid hashed value";
            try
            {
                value = Helper.Decrypt(Request["value"]);
            }
            catch { }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Subs the identifier converter.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult SubIdConverter(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                string dir = Server.MapPath("~/App_Data/Uploads");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(dir, DateTime.Now.Ticks.ToString() + "_" + fileName);
                file.SaveAs(path);

                StreamReader sr = new StreamReader(path);
                int index = -1;
                int.TryParse(Request["index"], out index);
                string line = "";
                int lineNumber = 1;

                List<string[]> rows = new List<string[]>();

                while ((line = sr.ReadLine()) != null)
                {
                    string[] row = line.Split(',');

                    if (index > row.Length)
                    {
                        rows.Add(row);
                        lineNumber++;
                        break;
                    }

                    if (lineNumber > 1)
                        row[index - 1] = Helper.Decrypt(row[index - 1]);

                    rows.Add(row);
                    lineNumber++;
                }
                sr.Close();

                StringBuilder sb = new StringBuilder();
                foreach (string[] row in rows)
                {
                    sb.AppendLine(string.Join(",", row));
                }

                System.IO.File.Delete(path);
                return File(new System.Text.UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "convertedSubIds.csv");
            }
            // redirect back to the index action to show the form once again
            return View();
        }

        public class PdfReportData
        {
            public string Name { get; set; }
            public int PrevValue { get; set; }

            public int CurValue { get; set; }
        }

        /// <summary>
        /// Add New Page in PDF document
        /// </summary>
        /// <returns></returns>
        /// ColumnsWidths = { 68f, 20f, 12f, 20f };
        /// newPage = true|false - Report on New Page
        private void AddReportPage(iTextSharp.text.Document doc, string Title, string[] ColumnTitles, string[][] CellsArray, float[] ColumnsWidths, bool newPage = true)
        {
            iTextSharp.text.Font fontH1 = FontFactory.GetFont("arial", 16); fontH1.SetStyle("Bold");
            iTextSharp.text.Font fontH2 = FontFactory.GetFont("arial", 14); fontH2.SetStyle("Bold");
            iTextSharp.text.Font fontText = FontFactory.GetFont("arial", 10);
            iTextSharp.text.BaseColor color1 = new iTextSharp.text.BaseColor(213, 231, 250);

            if (newPage)
            {
                doc.NewPage();
            }

            PdfPTable table = new PdfPTable(ColumnTitles.Length);

            table.SetTotalWidth(ColumnsWidths);

            Paragraph pTitle = new Paragraph(Title + "\n\n", fontH1);
            pTitle.Alignment = Element.ALIGN_CENTER;

            doc.Add(pTitle);


            PdfPCell[] cells = new PdfPCell[ColumnTitles.Length];

            for (int i = 0; i < ColumnTitles.Length; i++)
            {
                cells[i] = new PdfPCell(new Phrase(ColumnTitles[i] + "\n", fontH2)) { VerticalAlignment = Element.ALIGN_CENTER, BackgroundColor = color1 };
            }

            PdfPRow row = new PdfPRow(cells);
            table.Rows.Add(row);

            foreach (string[] TableCells in CellsArray)
            {
                cells = new PdfPCell[TableCells.Length];
                for (int i = 0; i < TableCells.Length; i++)
                {
                    cells[i] = new PdfPCell(new Phrase(TableCells[i])) { VerticalAlignment = Element.ALIGN_CENTER };
                }
                table.Rows.Add(new PdfPRow(cells));
            }

            doc.Add(table);
        }

        protected List<PdfReportData> GetSalesReportData(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2, string type)
        {
            Dictionary<string, PdfReportData> data = new Dictionary<string, PdfReportData>();

            List<ChartData> report1 = (List<ChartData>)_reportService.SalesReport(startDate1, endDate1, type);
            List<ChartData> report2 = (List<ChartData>)_reportService.SalesReport(startDate2, endDate2, type);

            for (int i = 0; i < report1.Count; i++)
            {
                PdfReportData cd = null;
                if (!data.ContainsKey(report1[i].Name.ToString()))
                {
                    cd = new PdfReportData() { Name = report1[i].Name, CurValue = 0, PrevValue = 0 };
                    data.Add(cd.Name, cd);
                }
                else
                    cd = data[report1[i].Name.ToString()];

                cd.PrevValue += report1[i].Value;

                //rows[i] = new string[] { report[i].Name.ToString(), report[i].Value.ToString()};
            }

            for (int i = 0; i < report2.Count; i++)
            {
                PdfReportData cd = null;
                if (!data.ContainsKey(report2[i].Name.ToString()))
                {
                    cd = new PdfReportData() { Name = report2[i].Name, CurValue = 0, PrevValue = 0 };
                    data.Add(cd.Name, cd);
                }
                else
                    cd = data[report2[i].Name.ToString()];

                cd.CurValue += report2[i].Value;

                
            }

            if (type != "state")
                return data.Select(d => d.Value).OrderBy(d => decimal.Parse(d.Name)).ToList();
            else
                return data.Select(d => d.Value).OrderBy(d => d.Name).ToList();
        }


        /// <summary>
        /// Buyers the report by buyer channel.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult ReportBuyersStatement()
        {
            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return HttpNotFound();

            BuyerReportModel model = new BuyerReportModel();
            PrepareBuyerReportModel(model, 0);

            return View(model);
        }

        // REPORT PDF //////////////////////// 
        /// <summary>
        /// PDFs the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult CreateReportPdf(long id)
        {
            /*  AZ?
            if (this._appContext.AppUser.UserTypeId == SharedData.AffiliateUserTypeId)
            {
                return null;
            }
            */
            string CompanyName = this._settingService.GetSetting("Settings.CompanyName").Value;
            string CompanyAddress = this._settingService.GetSetting("Settings.CompanyAddress").Value;
            string CompanyBank = this._settingService.GetSetting("Settings.CompanyBank").Value;
            string CompanyEmail = this._settingService.GetSetting("Settings.CompanyEmail").Value;
            string CompanyLogoPath = this._settingService.GetSetting("Settings.CompanyLogoPath").Value;

            Buyer buyer = _buyerService.GetBuyerById(id);
            string buyerName = "";

            if (buyer != null)
            {
                buyerName = buyer.Name;
            }

            var doc = new iTextSharp.text.Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            //doc.SetMargins(0, 0, 0, 0);

            long ticks = DateTime.Now.Ticks;

            PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/Report-" + id.ToString() + ticks.ToString() + ".pdf", FileMode.Create));
            doc.Open();

            iTextSharp.text.Image bg = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/arrowshade-bg.jpg");
            bg.Alignment = iTextSharp.text.Image.UNDERLYING;
            bg.SetAbsolutePosition(0, 0);
            doc.Add(bg);

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/" + CompanyLogoPath);
            jpg.ScalePercent(20f);

            iTextSharp.text.Font fontHeader = FontFactory.GetFont("arial", 22); fontHeader.SetStyle("Bold");
            iTextSharp.text.Font fontH1 = FontFactory.GetFont("arial", 16); fontH1.SetStyle("Bold");
            iTextSharp.text.Font fontH2 = FontFactory.GetFont("arial", 14); fontH2.SetStyle("Bold");
            iTextSharp.text.Font fontText = FontFactory.GetFont("arial", 10);

            iTextSharp.text.BaseColor color1 = new iTextSharp.text.BaseColor(220, 220, 250);

            Paragraph pHeader = new Paragraph("MONTHLY STATEMENT\n\n", fontHeader);
            pHeader.Alignment = Element.ALIGN_CENTER;

            doc.Add(pHeader);

            DateTime endDate2 = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month));
            DateTime startDate2 = new DateTime(endDate2.Year, endDate2.Month, 1);

            DateTime endDate1 = new DateTime(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.UtcNow.AddMonths(-1).Year, DateTime.UtcNow.AddMonths(-1).Month));
            DateTime startDate1 = new DateTime(endDate1.Year, endDate1.Month, 1);

            string prevMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(startDate1.Month);
            string curMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(startDate2.Month);


            doc.Add(new Paragraph(CompanyName + "\n" +
                                    "Email:" + CompanyEmail + "\n\n", fontH1));

            doc.Add(new Paragraph("Buyer: " + buyerName + "\nYear: " + DateTime.UtcNow.Year.ToString() + " \n" +
                                                                "Current month: " + curMonthName + "\n" +
                                                                "Previous month: " + prevMonthName
                                                                , fontH1));

            //Buyer buyer = _buyerService.GetBuyerById(_appContext.AppUser.ParentId);

            string buyerIds = buyer.Id.ToString();
            string buyerChannelIds = "0";
            string campaignIds = "0";
            string affiliateIds = "0";
            string states = "0";

            foreach (var value in _stateProvinceService.GetStateProvinceByCountryId(80))
            {
                states += "," + value.Code;
            }

            foreach (BuyerChannel bc in _buyerChannelService.GetAllBuyerChannelsByBuyerId(buyer.Id))
            {
                buyerChannelIds += "," + bc.Id.ToString();
                campaignIds += "," + bc.CampaignId.ToString();

                foreach(AffiliateChannel ac in _affiliateChannelService.GetAllAffiliateChannelsByCampaignId(bc.CampaignId))
                {
                    affiliateIds += "," + ac.Id.ToString();
                }
            }


            var report = GetSalesReportData(startDate1, endDate1, startDate2, endDate2, "price");
            string[][] rows = new string[report.Count][];

            for(int i = 0; i < report.Count; i++)
            {
                rows[i] = new string[] { report[i].Name.ToString(), report[i].CurValue.ToString(), report[i].PrevValue.ToString() };
            }

            AddReportPage(doc, "Sales statistics by sale price",
                ("Price, " + curMonthName + ", " + prevMonthName).Split(new char[1] { ',' }),
                rows,
                new float[] { 80f, 80f, 80f }
                );

            report = GetSalesReportData(startDate1, endDate1, startDate2, endDate2, "dayofweek");
            rows = new string[report.Count][];

            for (int i = 0; i < report.Count; i++)
            {
                switch(report[i].Name)
                {
                    case "1":
                        report[i].Name = "Monday"; break;
                    case "2":
                        report[i].Name = "Tuesday"; break;
                    case "3":
                        report[i].Name = "Wednesday"; break;
                    case "4":
                        report[i].Name = "Thursday"; break;
                    case "5":
                        report[i].Name = "Friday"; break;
                    case "6":
                        report[i].Name = "Saturday"; break;
                    case "7":
                        report[i].Name = "Sunday"; break;
                }

                rows[i] = new string[] { report[i].Name.ToString(), report[i].CurValue.ToString(), report[i].PrevValue.ToString() };
            }

            AddReportPage(doc, "Sales statistics by day of week",
                ("Day of week, " + curMonthName + ", " + prevMonthName).Split(new char[1] { ',' }),
                rows,
                new float[] { 80f, 80f, 80f }
                );



            report = GetSalesReportData(startDate1, endDate1, startDate2, endDate2, "hour");

            Dictionary<int, PdfReportData> hoursReport = new Dictionary<int, PdfReportData>();

            for (int i = 0; i <= 23; i++)
            {
                string t = (i.ToString().Length < 2 ? "0" + i.ToString() : i.ToString()) + ":00";
                PdfReportData item = new PdfReportData() { Name = t, CurValue = 0, PrevValue = 0 };
                hoursReport.Add(i, item);

            }

            for (int i = 0; i < report.Count; i++)
            {
                PdfReportData item = hoursReport[int.Parse(report[i].Name)];
                if (item != null)
                { 
                    item.CurValue += report[i].CurValue;
                    item.PrevValue += report[i].PrevValue;
                }
            }

            var hoursList = hoursReport.Select(d => d.Value).ToList();
            rows = new string[hoursList.Count][];

            for (int i = 0; i < hoursList.Count; i++)
            {
                rows[i] = new string[] { hoursList[i].Name.ToString(), hoursList[i].CurValue.ToString(), hoursList[i].PrevValue.ToString() };
            }

            AddReportPage(doc, "Sales statistics by hour",
                ("Hour, " + curMonthName + ", " + prevMonthName).Split(new char[1] { ',' }),
                rows,
                new float[] { 80f, 80f, 80f }
                );



            report = GetSalesReportData(startDate1, endDate1, startDate2, endDate2, "state");
            rows = new string[report.Count][];

            for (int i = 0; i < report.Count; i++)
            {
                rows[i] = new string[] { report[i].Name.ToUpper(), report[i].CurValue.ToString(), report[i].PrevValue.ToString() };
            }

            AddReportPage(doc, "Sales statistics by state",
                ("State, " + curMonthName + ", " + prevMonthName).Split(new char[1] { ',' }),
                rows,
                new float[] { 80f, 80f, 80f }
                );


            doc.Add(new Paragraph("\nThank you for using Leads.\n", fontText));

            doc.Close();

            return File(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/Report-" + id.ToString() + ticks.ToString() + ".pdf", "application/pdf", "Report-" + id.ToString() + ticks.ToString() + ".pdf");

            return PartialView();
        }
    }
}