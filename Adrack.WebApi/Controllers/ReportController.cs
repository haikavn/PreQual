using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml;
using Adrack.Core;
using Adrack.Core.Domain.CustomReports;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using Adrack.Core.Domain.Security;
using Adrack.Data;
using Adrack.PlanManagement;
using Adrack.Service.Configuration;
using Adrack.Service.Directory;
using Adrack.Service.Helpers;
using Adrack.Service.Lead;
using Adrack.Service.Membership;
using Adrack.WebApi.Controllers.ReportModels;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Models.Reports;
using Adrack.WebApi.Models.Reports.AffilliateReports;
using Adrack.WebApi.Models.Reports.BuyerReports;
using Adrack.WebApi.Models.Reports.CustomReports;
using Adrack.WebApi.Models.Reports.LeadReports;
using Adrack.WebApi.Models.Reports.NetworkReports;
using Adrack.WebApi.PdfBuilder.HighCharts;
using Adrack.WebApi.PdfBuilder.PdfReportCreators;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using PdfSharp.Drawing;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using WebGrease.Extensions;

namespace Adrack.WebApi.Controllers
{
    [RoutePrefix("api/report")]
    [AllowAnonymous]
    public class ReportController : BaseApiController
    {
        public class PdfReportData
        {
            public string Name { get; set; }
            public int PrevValue { get; set; }

            public int CurValue { get; set; }
        }

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

        #region Methods

        /// <summary>
        /// Get Report Sending Time.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("getReportSendingTime")]
        public IHttpActionResult GetReportSendingTime(ReportSendingInModel inpObj)
        {
            List<ReportSendingTimeModel.TreeItem> buyers = new List<ReportSendingTimeModel.TreeItem>();
            Dictionary<string, ReportSendingTimeModel.TreeItem> data = new Dictionary<string, ReportSendingTimeModel.TreeItem>();

            if (_appContext.AppUser.UserType == SharedData.AffiliateUserTypeId)
                return NotFound();

            string campaignIds = string.Join(",", inpObj.CampaignIds);  
            string buyerIds = string.Join(",", inpObj.BuyerIds); 
            string buyerChannelIds = string.Join(",", inpObj.BuyerChannelIds); 

            DateTime startDate;
            DateTime endDate;
            
            GetDateRangesFromString(inpObj.StartDate.ToString(CultureInfo.InvariantCulture), inpObj.EndDate.ToString(CultureInfo.InvariantCulture), false, out startDate, out endDate);

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

            List<ReportSendingTime> rows = (List<ReportSendingTime>)this._reportService.ReportSendingTimeByFilter(startDate, endDate, campaignIds, buyerIds, buyerChannelIds);

            foreach (var row in rows)
            {
                if (!data.ContainsKey(row.Name))
                {
                    data[row.Name] = new ReportSendingTimeModel.TreeItem()
                    {
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

                switch (row.TimeoutType.ToLower())
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

            total.AfterRejectAvg = 0;
            total.AfterSoldAvg = 0;
            total.BeforeRejectAvg = 0;
            total.BeforeSoldAvg = 0;


            if (dataList.Count() != 0)
            {
                total.AfterRejectAvg = Math.Round(total.AfterRejectAvg / dataList.Count(), 1);
                total.AfterSoldAvg = Math.Round(total.AfterSoldAvg / dataList.Count(), 1);
                total.BeforeRejectAvg = Math.Round(total.BeforeRejectAvg / dataList.Count(), 1);
                total.BeforeSoldAvg = Math.Round(total.BeforeSoldAvg / dataList.Count(), 1);
            }
            // bool downloadCsv = false;
            //if (bool.TryParse(Request["csv"], out downloadCsv) && downloadCsv)
            //    return File(new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString()), "text/csv", "ReportSendingTime.csv");

            return Ok(new { list = dataList, total = total });
        }

        /// <summary>
        /// Get Report Click Main.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("getReportClickMain")]
        public IHttpActionResult GetReportClickMain(AffiliatesReportClicksMainInModel affiliatesReport)
        {
            ReportCSVGenerator.Instance.Init("Affiliate/Affiliate Channel,Hits,Unique clicks,Total leads,CTA,Sold leads,Accept rate %,Redirect %,Affiliate Profit,EPL,EPA,EPC");

            GetDateRangesFromString(affiliatesReport.DateFrom.ToString(CultureInfo.InvariantCulture), affiliatesReport.DateTo.ToString(CultureInfo.InvariantCulture), false, out var startDate, out var endDate);
            List<ReportClickMain> report = (List<ReportClickMain>)this._reportService.ReportClickMain(startDate, endDate, string.Join(",", affiliatesReport.AffiliateChannelIds));

            List<ReportClickMainModel> affiliates = new List<ReportClickMainModel>();

            long affiliateId = 0;
            long affiliateChannelId = 0;

            ReportClickMainModel totals = new ReportClickMainModel();
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

            ReportClickMainModel item = null;
            ReportClickMainModel item2 = null;
            ReportClickMainModel prevItem = null;
            ReportClickMainModel prevItem2 = null;

            foreach (ReportClickMain r in report)
            {
                if (r.AffiliateId != affiliateId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", prevItem.title, prevItem.Hits, prevItem.UniqueClicks, prevItem.TotalLeads, prevItem.CTA, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA, prevItem.EPC));
                    }

                    item = new ReportClickMainModel();
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

                    affiliateId = r.AffiliateId;
                }

                ReportClickMainModel parent = affiliates[affiliates.Count - 1];
                prevItem2 = item2;

                if (affiliateChannelId != r.AffiliateChannelId)
                {
                    if (prevItem2 != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "-" + prevItem2.title, prevItem2.Hits, prevItem2.UniqueClicks, prevItem2.TotalLeads, prevItem2.CTA, prevItem2.SoldLeads, prevItem2.AcceptRate, prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA, prevItem2.EPC));
                    }
                    item2 = new ReportClickMainModel();
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

            if (affiliatesReport.IsCsv)
            {
                return PerformCsvDownload("Affiliates-Report-Clicks-Main.csv", "text/csv");
            }

            return Ok(affiliates);
        }






        /// <summary>
        /// Gets the report buyers by dates.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("buyers/byDate")]
        public IHttpActionResult GetReportBuyersByDates(BuyersReportRequestModel buyerReportModel)
        {
            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(buyerReportModel.StartDate.Date.ToString(),
                buyerReportModel.EndDate.Date.ToString(), false, out startDate, out endDate);

            //ReportCSVGenerator.Instance.Init("Date,Post leads,Sold leads," + ((_appContext.AppUser.UserTypeId == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserTypeId == SharedData.NetowrkUserTypeId) ? "Money Made" : "Amount of money spent") + ",Accept rate,Redirect rate");
            //ReportCSVGenerator.Instance.Init("Date,Post leads,Sold leads," + "Amount of money spent" +
            //                                 ",Accept rate,Redirect rate");

            ReportCSVGenerator.Instance.Init("Date,Posted Leads,Sold Leads,Rejected Leads,Cost,Profit,Average Price,Accept Rate,Redirect Rate");

            buyerReportModel.AffiliateChannelIds = new List<long>();

            _reportService.ValidateFilters(FilterBuyerIds: buyerReportModel.BuyerIds, FilterBuyerChannelIds: buyerReportModel.BuyerChannelIds, FilterAffiliateChannelIds: buyerReportModel.AffiliateChannelIds, FilterCampaignIds: buyerReportModel.CampaignIds);

            var buyers = GetReportBuyersByDatesData(startDate, endDate, string.Join(",", buyerReportModel.BuyerIds),
               string.Join(",", buyerReportModel.BuyerChannelIds), string.Join(",", buyerReportModel.AffiliateChannelIds), string.Join(",", buyerReportModel.CampaignIds));

            if (buyerReportModel.IsCsv)
            {
                return PerformCsvDownload("ReportBuyersByDates.csv", "text/csv");
            }

            return Ok(buyers);
        }

        [HttpPost]
        [Route("dashboard/leadsreporttotals")]
        public IHttpActionResult DashboardGetLeadsReportTotals()
        {
            return Ok(_reportService.LeadDashboardTotals());
        }

        //[HttpGet]
        [HttpPost]
        [Route("dashboard/leadsreport")]
        public IHttpActionResult DashboardGetLeadsReportByPeriod(DashboardLeadsReportFilterModel model)
        {
            LeadsReport report = new LeadsReport();

            model.StartDate = new DateTime(model.StartDate.Year, model.StartDate.Month, model.StartDate.Day, 0, 0, 0);
            model.EndDate = new DateTime(model.EndDate.Year, model.EndDate.Month, model.EndDate.Day, 23, 59, 59);

            double totalDays = (model.EndDate - model.StartDate).TotalDays;

            //model.StartDate = _settingService.GetUTCDate(model.StartDate);
            //model.EndDate = _settingService.GetUTCDate(model.EndDate);

            _reportService.ValidateFilters(FilterCampaignIds: model.CampaignIds);

            if (totalDays <= 1) 
            {
                var resultHours = _reportService.LeadDashboardReportByHour(model.StartDate, model.EndDate, model.CampaignIds);

                DateTime StartDate = new DateTime(2020, 1, 1);

                for(int i = 0; i <= 23; i++)
                {
                    var items = resultHours.Where(x => x.Hour == i).ToList();

                    var item = new LeadsReportByHour()
                    {
                        Cost = 0,
                        Date = new DateTime(model.StartDate.Year, model.StartDate.Month, model.StartDate.Day, i, 0, 0),
                        Hour = i,
                        Posted = 0,
                        Profit = 0,
                        Received = 0,
                        Revenue = 0,
                        Sold = 0,
                        Pings = 0
                    };

                    foreach(var e in items)
                    {
                        item.Cost += e.Cost;
                        item.Posted += e.Posted;
                        item.Profit += e.Profit;
                        item.Received += e.Received;
                        item.Revenue += e.Revenue;
                        item.Sold += e.Sold;
                        item.Pings += e.Received + e.Posted;
                    }

                    var lead = new LeadInformationRecord();
                    lead.DateStart = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.Hour, 0, 0);
                    lead.DateEnd = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.Hour, 0, 0);
                    lead.Label = $"{item.Hour}:00";
                    lead.Profit = item.Profit.HasValue ? (double)item.Profit.Value : 0.0;
                    lead.Cost = item.Cost.HasValue ? (double)item.Cost.Value : 0.0;
                    lead.Posted = item.Posted.HasValue ? (double)item.Posted.Value : 0.0;
                    lead.Pings = item.Pings.HasValue ? (double)item.Pings.Value : 0.0;

                    lead.Sold = item.Sold.HasValue ? (double)item.Sold.Value : 0.0;
                    lead.Revenue = item.Revenue.HasValue ? (double)item.Revenue.Value : 0.0;
                    lead.Recieved = item.Received.HasValue ? (double)item.Received.Value : 0.0;
                    report.Leads.Add(lead);

                }

                /*foreach (var item in resultHours)
                {
                    var lead = new LeadInformationRecord();
                    lead.DateStart = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.Hour, 0, 0);
                    lead.DateEnd = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, item.Hour, 0, 0);
                    lead.Label = $"{item.Hour}:00";
                    lead.Profit = item.Profit.HasValue ? (double)item.Profit.Value : 0.0;
                    lead.Cost = item.Cost.HasValue ? (double)item.Cost.Value : 0.0;
                    lead.Posted = item.Posted.HasValue ? (double)item.Posted.Value : 0.0;
                    lead.Sold = item.Sold.HasValue ? (double)item.Sold.Value : 0.0;
                    lead.Revenue = item.Revenue.HasValue ? (double)item.Revenue.Value : 0.0;
                    lead.Recieved = item.Received.HasValue ? (double)item.Received.Value : 0.0;
                    report.Leads.Add(lead);
                }*/
            }
            else
            {
                var resultDays = _reportService.LeadDashboardReportByDays(model.StartDate, model.EndDate, model.CampaignIds);

                DateTime StartDate = new DateTime(2020, 1, 1);

                DateTime curDate = model.StartDate;

                while (curDate.Year != model.EndDate.Year || 
                       curDate.Month != model.EndDate.Month || 
                       curDate.Day != model.EndDate.Day)
                {
                    var items = resultDays.Where(x => x.Created.Year == curDate.Year && x.Created.Month == curDate.Month && x.Created.Day == curDate.Day).ToList();
                    
                    var item = new LeadsReportByDay()
                    {
                        Cost = 0,
                        Created = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0),
                        Posted = 0,
                        Profit = 0,
                        Received = 0,
                        Revenue = 0,
                        Sold = 0,
                        Pings = 0
                    };

                    foreach (var e in items)
                    {
                        item.Cost += e.Cost;
                        item.Posted += e.Posted;
                        item.Profit += e.Profit;
                        item.Received += e.Received;
                        item.Revenue += e.Revenue;
                        item.Sold += e.Sold;
                        item.Pings += e.Posted + e.Received;
                    }

                    var lead = new LeadInformationRecord();
                    lead.DateStart = new DateTime(item.Created.Year, item.Created.Month, item.Created.Day, 0, 0, 0);
                    lead.DateEnd = new DateTime(item.Created.Year, item.Created.Month, item.Created.Day, 0, 0, 0);
                    lead.Label = $"{item.Created.Month}/{item.Created.Day}";
                    lead.Profit = item.Profit.HasValue ? (double)item.Profit.Value : 0.0;
                    lead.Cost = item.Cost.HasValue ? (double)item.Cost.Value : 0.0;
                    lead.Posted = item.Posted.HasValue ? (double)item.Posted.Value : 0.0;
                    lead.Pings = item.Pings.HasValue ? (double)item.Pings.Value : 0.0;
                    lead.Sold = item.Sold.HasValue ? (double)item.Sold.Value : 0.0;
                    lead.Revenue = item.Revenue.HasValue ? (double)item.Revenue.Value : 0.0;
                    lead.Recieved = item.Received.HasValue ? (double)item.Received.Value : 0.0;
                    report.Leads.Add(lead);

                    curDate = curDate.AddDays(1);
                }

                /*foreach (var item in resultDays)
                {
                    var lead = new LeadInformationRecord();
                    lead.DateStart = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0);
                    lead.DateEnd = new DateTime(item.Date.Year, item.Date.Month, item.Date.Day, 0, 0, 0);
                    lead.Label = $"{item.Date.Month}/{item.Date.Day}";
                    lead.Profit = item.Profit.HasValue ? (double)item.Profit.Value : 0.0;
                    lead.Cost = item.Cost.HasValue ? (double)item.Cost.Value : 0.0;
                    lead.Posted = item.Posted.HasValue ? (double)item.Posted.Value : 0.0;
                    lead.Sold = item.Sold.HasValue ? (double)item.Sold.Value : 0.0;
                    lead.Revenue = item.Revenue.HasValue ? (double)item.Revenue.Value : 0.0;
                    lead.Recieved = item.Received.HasValue ? (double)item.Received.Value : 0.0;
                    report.Leads.Add(lead);
                }*/
            }

            report.CalculateSummary();
            report.GetSeries();

            return Ok(report);
        }

        [HttpGet]
        [Route("pdf/getBuyerMontlyStatementsList")]
        public IHttpActionResult GetBuyerMontlyStatementsList(long buyerId)
        {
            // string pdf_folder = System.AppDomain.CurrentDomain.BaseDirectory + "Uploads";

            buyerId = _reportService.ValidateFilter(buyerId: buyerId);

            string pdf_folder = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Uploads\\Pdf";

            string[] dirs = Directory.GetFiles(pdf_folder, buyerId.ToString() + "_MontlyStatement_*");

            List<string> filesList = new List<string>();
            foreach (string s in dirs)
            {
                filesList.Add(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Content/Uploads/Pdf/" + Path.GetFileName(s));
            }
       
            return Ok(filesList);
        }


        [HttpGet]
        [Route("pdf/test")]
        public async Task<IHttpActionResult> PdfGeneratorTest(int height = 600)
        {
            PdfReportCreator creator = new PdfReportBuyerMontlyStatement(0,_reportService, _buyerService);
            string fileName = "testOutput.pdf";
            await creator.GenerateReport(fileName);
            return Ok(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Uploads/" + fileName);
        }

        [HttpGet]
        [Route("pdf/invoice")]
        public async Task<IHttpActionResult> PdfGeneratorTestInvoice(int height = 600)
        {
            
            PdfReportCreator creator = new PdfReportInvoice(_planService,_reportService, _buyerService);
            string fileName = "testInvoice.pdf";
            await creator.GenerateReport(fileName);
            return Ok(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Content/Uploads/Pdf/" + fileName);

        }

        [HttpGet]
        [Route("pdf/invoicebuyer")]
        public async Task<IHttpActionResult> PdfGeneratorBuyerTestInvoice(long id = 17)
        {
            id = _reportService.ValidateFilter(buyerId: id);

            PdfReportBuyerInvoice creator = new PdfReportBuyerInvoice(id,_reportService,                
                _buyerService, _leadMainResponseService, _buyerChannelService);
            string fileName = "testBuyerInvoice.pdf";

            List<BuyerChannelSourceData> sourceReport=new List<BuyerChannelSourceData>();
            List<BuyerChannelSourceData> sourceCustomAdjustment=new List<BuyerChannelSourceData>();

            for (var i = 1; i < 10; i++)
            {
                sourceReport.Add(new BuyerChannelSourceData() { Description = i.ToString(), Amount = 2, Count = 3, Price = 4 });
            }

            for (var i = 1; i < 10; i++)
            {
                sourceCustomAdjustment.Add(new BuyerChannelSourceData() { Description = i.ToString(), Amount = 2, Count = 3, Price = 4 });
            }

            creator.SetSourceData(id, sourceReport, sourceCustomAdjustment);

            await creator.GenerateReport(fileName);
            return Ok(Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/Content/Uploads/Pdf/" + fileName);
        }

        [HttpPost]
        [Route("dashboard/topstates")]
        public IHttpActionResult DashboardGedTopStates([FromBody] CustomReportModel model)
        {
            _reportService.ValidateFilters(FilterCampaignIds: model.CampaignIds);

            List<PieChartItemsModel> items = new List<PieChartItemsModel>();
            TopRecordsReport report = new TopRecordsReport();
            var top = 5;
            //simulation
            #region Recieved
            var Recieved = _reportService.ReportTopStateRecieved(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Recieved != null && Recieved.Any())
            {
                Recieved.ForEach(x =>
                {
                    if (x.Counts > 0)
                    {
                        items.Add(new PieChartItemsModel()
                        {
                            ChartCategory = "Recieved",
                            State = x.State,
                            Value = x.Counts,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate
                        });
                    }
                });
            }
            else
            {
                items.Add(new PieChartItemsModel()
                {
                    ChartCategory = "Recieved",
                    State = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region Posted
            var Posted = _reportService.ReportTopStatePosted(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Posted != null && Posted.Any())
            {
                Posted.ForEach(x =>
                {
                    if (x.Counts > 0)
                    {
                        items.Add(new PieChartItemsModel()
                        {
                            ChartCategory = "Posted",
                            State = x.State,
                            Value = x.Counts,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate
                        });
                    }
                });
            }
            else
            {
                items.Add(new PieChartItemsModel()
                {
                    ChartCategory = "Posted",
                    State = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region Revenue
            var Revenue = _reportService.ReportTopStateRevenue(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Revenue != null && Revenue.Any())
            {
                Revenue.ForEach(x =>
                {
                    if (x.Counts > 0)
                    {
                        items.Add(new PieChartItemsModel()
                        {
                            ChartCategory = "Revenue",
                            State = x.State,
                            Value = x.Counts,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate
                        });
                    }
                });
            }
            else
            {
                items.Add(new PieChartItemsModel()
                {
                    ChartCategory = "Revenue",
                    State = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region Sold
            var Sold = _reportService.ReportTopStateSold(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Sold != null && Sold.Any())
            {
                Sold.ForEach(x =>
                {
                    if (x.Counts > 0)
                    {
                        items.Add(new PieChartItemsModel()
                        {
                            ChartCategory = "Sold",
                            State = x.State,
                            Value = x.Counts,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate

                        });
                    }
                });
            }
            else
            {
                items.Add(new PieChartItemsModel()
                {
                    ChartCategory = "Sold",
                    State = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region Cost
            var Cost = _reportService.ReportTopStateCost(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Cost != null && Cost.Any())
            {
                Cost.ForEach(x =>
                {
                    if (x.Counts > 0)
                    {
                        items.Add(new PieChartItemsModel()
                        {
                            ChartCategory = "Cost",
                            State = x.State,
                            Value = x.Counts,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate

                        });
                    }
                });
            }
            else
            {
                items.Add(new PieChartItemsModel()
                {
                    ChartCategory = "Cost",
                    State = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region Profit
            var Profit = _reportService.ReportTopStateProfit(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Profit != null && Profit.Any())
            {
                Profit.ForEach(x =>
                {
                    if (x.Counts > 0)
                    {
                        items.Add(new PieChartItemsModel()
                        {
                            ChartCategory = "Profit",
                            State = x.State,
                            Value = x.Counts,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate

                        });
                    }
                });
            }
            else
            {
                items.Add(new PieChartItemsModel()
                {
                    ChartCategory = "Profit",
                    State = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            var grouped = items.GroupBy(x => x.State).Select(x => new { Key = x.Key, Items = x }).ToList();
            for (var i = 0; i < grouped.Count; i++)
            {
                var lead = new LeadInformationRecord();

                lead.DateStart = grouped[i].Items.Min(x=>x.StartDate);
                lead.DateEnd = grouped[i].Items.Max(x => x.StartDate);
                lead.Label = grouped[i].Key;
                var cost = grouped[i].Items.Where(x => x.ChartCategory == "Cost").FirstOrDefault();
                lead.Cost = cost!=null? (double)cost.Value:0.0;

                var posted = grouped[i].Items.Where(x => x.ChartCategory == "Posted").FirstOrDefault();
                lead.Posted = posted!=null? (double)posted.Value: 0.0;

                var recieved = grouped[i].Items.Where(x => x.ChartCategory == "Recieved").FirstOrDefault();
                lead.Recieved = recieved!=null?(double)recieved.Value:0.0 ;

                var revenue = grouped[i].Items.Where(x => x.ChartCategory == "Revenue").FirstOrDefault();
                lead.Revenue = revenue!=null?(double)revenue.Value:0.0;

                var sold = grouped[i].Items.Where(x => x.ChartCategory == "Sold").FirstOrDefault();
                lead.Sold = sold!=null?(double)sold.Value:0.0;

                var profit = grouped[i].Items.Where(x => x.ChartCategory == "Profit").FirstOrDefault();
                lead.Profit = profit!=null?(double)profit.Value:0.0;

                report.Records.Add(lead);
            }


            //DateTime StartDate = new DateTime(2020, 1, 1);
            //string[] SimulatedStates = new string[] { "Washington", "California"};
            //for (var i = 0; i < 2; i++)
            //{
            //    var lead = new LeadInformationRecord();

            //    lead.DateStart = StartDate;
            //    StartDate = StartDate.AddHours(1);
            //    lead.DateEnd = StartDate;
            //    var val = (i + 1) * 1000;
            //    lead.Label = SimulatedStates[i];
            //    lead.Cost = val;
            //    lead.Posted = val + 1;
            //    lead.Recieved = val + 2;
            //    lead.Revenue = val + 3;
            //    lead.Sold = val + 4;
            //    report.Records.Add(lead);
            //}

            report.CalculateSummary();
            report.GetSeries();

            return Ok(report);
        }

        [HttpPost]
        [Route("dashboard/barCharts")]
        public IHttpActionResult DashboardGedTopBarCharts([FromBody] CustomReportModel model)
        {
            _reportService.ValidateFilters(FilterCampaignIds: model.CampaignIds);

            List<BarChartItemModel> items = new List<BarChartItemModel>();
            TopBarChartsModel report = new TopBarChartsModel();
            var result = new List<BarchartInformationRecord>();
            var top = 5;
            //simulation
            #region Buyers
            var Buyers = _reportService.ReportTopBuyersRecieved(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Buyers != null && Buyers.Any())
            {
                Buyers.ForEach(x =>
                {
                    items.Add(new BarChartItemModel()
                    {
                        ChartCategory = "Buyers",
                        Name = x.Name,
                        Value = x.Counts,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    });
                });
            }
            else
            {
                items.Add(new BarChartItemModel()
                {
                    ChartCategory = "Buyers",
                    Name = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region Affiliates
            var Affiliates = _reportService.ReportTopAffiliatesRecieved(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (Affiliates != null && Affiliates.Any())
            {
                Affiliates.ForEach(x =>
                {
                    items.Add(new BarChartItemModel()
                    {
                        ChartCategory = "Affiliates",
                        Name = x.Name,
                        Value = x.Counts,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    });
                });
            }
            else
            {
                items.Add(new BarChartItemModel()
                {
                    ChartCategory = "Affiliates",
                    Name = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region BuyerChanels
            var BuyerChanels = _reportService.ReportTopBuyerChannelsRecieved(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (BuyerChanels != null && BuyerChanels.Any())
            {
                BuyerChanels.ForEach(x =>
                {
                    items.Add(new BarChartItemModel()
                    {
                        ChartCategory = "BuyerChanels",
                        Name = x.Name,
                        Value = x.Counts,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    });
                });
            }
            else
            {
                items.Add(new BarChartItemModel()
                {
                    ChartCategory = "BuyerChanels",
                    Name = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion

            #region AffiliateChannels
            var AffiliateChannels = _reportService.ReportTopAffiliateChannelsRecieved(model.StartDate, model.EndDate, model.CampaignIds, top);
            if (AffiliateChannels != null && AffiliateChannels.Any())
            {
                AffiliateChannels.ForEach(x =>
                {
                    items.Add(new BarChartItemModel()
                    {
                        ChartCategory = "AffiliateChannels",
                        Name = x.Name,
                        Value = x.Counts,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    });
                });
            }
            else
            {
                items.Add(new BarChartItemModel()
                {
                    ChartCategory = "AffiliateChannels",
                    Name = "Unknown",
                    Value = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow
                });
            }
            #endregion


            var grouped = items.GroupBy(x => x.ChartCategory).Select(x => new { Key = x.Key, Items = x }).ToList();
            for (var i = 0; i < grouped.Count; i++)
            {
                var lead = new BarchartInformationRecord();

                lead.dateStart = grouped[i].Items.Min(x => x.StartDate);
                lead.dateEnd = grouped[i].Items.Max(x => x.StartDate);
                lead.label = grouped[i].Key;
                lead.type = "auto";
                lead.color = "auto";
                foreach (var item in grouped[i].Items)
                {
                    lead.values.Add(new BarchartItem()
                    {
                        name = item.Name,
                        value = item.Value
                    });
                }

                result.Add(lead);

               

                report.Records.Add(lead);
            }


           

            return Ok(result);
        }

        [HttpPost]
        [Route("dashboard/topcountries")]
        public IHttpActionResult DashboardGedTopCountries(CustomReportModel model)
        {
            TopRecordsReport report = new TopRecordsReport();

            //simulation

            DateTime StartDate = new DateTime(2020, 1, 1);
            string[] SimulatedCountries = new string[] { "US", "UK", "Germany", "France", "Others" };
            for (var i = 0; i < 5; i++)
            {
                var lead = new LeadInformationRecord();

                lead.DateStart = StartDate;
                StartDate = StartDate.AddHours(1);
                lead.DateEnd = StartDate;
                var val = (i + 1) * 1000;
                lead.Label = SimulatedCountries[i];
                lead.Cost = val;
                lead.Posted = val + 1;
                lead.Recieved = val + 2;
                lead.Revenue = val + 3;
                lead.Sold = val + 4;
                report.Records.Add(lead);
            }

            report.CalculateSummary();
            report.GetSeries();

            return Ok(report);
        }

        [HttpPost]
        [Route("dashboard/comparison")]
        public IHttpActionResult DashboardGetComparison()
        {
            long parentid = 0;

            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == UserTypes.Affiliate)
                {
                    var affiliate = _affiliateService.GetAllAffiliates().Take(1).FirstOrDefault();
                    if (affiliate != null)
                        parentid = -affiliate.Id;
                }
                else
                if (_appContext.AppUser.UserType == UserTypes.Buyer)
                {
                    var buyer = _buyerService.GetAllBuyers().Take(1).FirstOrDefault();
                    if (buyer != null)
                        parentid = buyer.Id;
                }
            }

            DateTime tzDate = _settingService.GetTimeZoneDate(DateTime.UtcNow);

            DateTime start = new DateTime(tzDate.Year, tzDate.Month, tzDate.Day, 0, 0, 0);
            DateTime end = new DateTime(tzDate.Year, tzDate.Month, tzDate.Day, 23, 59, 59);

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            List<ReportTotals> report = (List<ReportTotals>)this._reportService.ReportTotals(start, end, parentid);

            List<object> results = new List<object>();

            string change = "";
            int k = 0;
            foreach (ReportTotals ai in report)
            {
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

                results.Add(new
                {
                    name = ai.name,
                    received = ai.received,
                    receivedPercent = Math.Abs(ai.receivedp),
                    receivedChange = ai.receivedp == 0 ? "none" : (ai.receivedp > 0 ? "down" : "up"),
                    total = ai.total,
                    totalPercent = Math.Abs(ai.totalp),
                    totalChange = ai.totalp == 0 ? "none" : (ai.totalp > 0 ? "down" : "up"),
                    redirected = ai.redirected,
                    redirectedPercent = Math.Abs(ai.redirectedp),
                    redirectedChange = ai.redirectedp == 0 ? "none" : (ai.redirectedp > 0 ? "down" : "up"),
                    sold = ai.sold,
                    soldPercent = Math.Abs(ai.soldp),
                    soldChange = ai.soldp == 0 ? "none" : (ai.soldp > 0 ? "down" : "up"),
                    debit = ai.debit,
                    debitPercent = Math.Abs(ai.debitp),
                    debitChange = ai.debitp == 0 ? "none" : (ai.debitp > 0 ? "down" : "up"),
                    profit = ai.profit,
                    profitPercent = Math.Abs(ai.profitp),
                    profitChange = ai.profitp == 0 ? "none" : (ai.profitp > 0 ? "down" : "up"),
                    epl = ai.received > 0 ? Math.Round((ai.debit / ai.received), 2) : 0,
                    epa = ai.sold > 0 ? Math.Round((ai.debit / ai.sold), 2) : 0,
                });
            }
            return Ok(results);
        }

        [HttpPost]
        [Route("dashboard/comparisonByCammpaign")]
        public IHttpActionResult DashboardGetComparisonByCampaign()
        {
            long parentid = 0;

            if (_appContext.AppUser != null)
            {
                if (_appContext.AppUser.UserType == UserTypes.Affiliate)
                {
                    var affiliate = _affiliateService.GetAllAffiliates().Take(1).FirstOrDefault();
                    if (affiliate != null)
                        parentid = -affiliate.Id;
                }
                else
                if (_appContext.AppUser.UserType == UserTypes.Buyer)
                {
                    var buyer = _buyerService.GetAllBuyers().Take(1).FirstOrDefault();
                    if (buyer != null)
                        parentid = buyer.Id;
                }
            }

            DateTime start = _settingService.GetTimeZoneDate(DateTime.UtcNow);
            DateTime end = _settingService.GetTimeZoneDate(DateTime.UtcNow);

            start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

            start = _settingService.GetUTCDate(start);
            end = _settingService.GetUTCDate(end);

            var campaigns = _campaignService.GetAllCampaigns();

            List<object> campaignResults = new List<object>();


            foreach (var campaign in campaigns)
            {
                List<ReportTotals> report = (List<ReportTotals>)this._reportService.ReportTotalsByCampaign(start, end, campaign.Id, parentid);

                List<object> results = new List<object>();

                string change = "";
                int k = 0;
                foreach (ReportTotals ai in report)
                {
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

                    results.Add(new
                    {
                        name = ai.name,
                        received = ai.received,
                        receivedPercent = ai.receivedp,
                        receivedChange = ai.receivedp == 0 ? "none" : (ai.receivedp > 0 ? "down" : "up"),
                        total = ai.total,
                        totalPercent = ai.totalp,
                        totalChange = ai.totalp == 0 ? "none" : (ai.totalp > 0 ? "down" : "up"),
                        redirected = ai.redirected,
                        redirectedPercent = ai.redirectedp,
                        redirectedChange = ai.redirectedp == 0 ? "none" : (ai.redirectedp > 0 ? "down" : "up"),
                        sold = ai.sold,
                        soldPercent = ai.soldp,
                        soldChange = ai.soldp == 0 ? "none" : (ai.soldp > 0 ? "down" : "up"),
                        debit = ai.debit,
                        debitPercent = ai.debitp,
                        debitChange = ai.debitp == 0 ? "none" : (ai.debitp > 0 ? "down" : "up"),
                        profit = ai.profit,
                        profitPercent = ai.profitp,
                        profitChange = ai.profitp == 0 ? "none" : (ai.profitp > 0 ? "down" : "up"),
                        epl = ai.received > 0 ? Math.Round((ai.debit / ai.received), 2) : 0,
                        epa = ai.sold > 0 ? Math.Round((ai.debit / ai.sold), 2) : 0,
                    });
                }

                campaignResults.Add(new
                {
                    id = campaign.Id,
                    name = campaign.Name,
                    rows = results
                });
            }
            return Ok(campaignResults);
        }

        /// <summary>
        /// Gets the report buyers by ByBuyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("buyers/byBuyer")]
        public IHttpActionResult GetBuyerReportByBuyer(BuyersReportRequestModel buyerReportModel)
        {
            _reportService.ValidateFilters(FilterBuyerIds: buyerReportModel.BuyerIds,
                FilterBuyerChannelIds: buyerReportModel.BuyerChannelIds,
                FilterAffiliateIds: buyerReportModel.AffiliateIds, FilterCampaignIds: buyerReportModel.CampaignIds);

            List<BuyerChanneModelResponse> buyers = new List<BuyerChanneModelResponse>();
            ReportCSVGenerator.Instance.Init("Buyer,Posted Leads,Sold Leads,Rejected Leads,Cost,Profit,Average Price,Accept Rate,Redirect Rate,Last Sold Date");

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(buyerReportModel.StartDate.Date.ToString(),
                buyerReportModel.EndDate.Date.ToString(), false, out startDate, out endDate);

            buyers = GetBuyerReportByBuyerData(startDate, endDate, string.Join(",", buyerReportModel.BuyerIds),
            string.Join(",", buyerReportModel.BuyerChannelIds), string.Join(",", buyerReportModel.AffiliateIds), string.Join(",", buyerReportModel.CampaignIds));

            _reportService.InsertReportsViewed("buyers_report");

            if (buyerReportModel.IsCsv)
            {
                return PerformCsvDownload("ReportBuyerByBuyer.csv", "text/csv");
            }

            return Ok(buyers);
        }

        /// <summary>
        /// Gets the report buyers by ByBuyerChannel.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("buyers/byBuyerChannel")]
        public IHttpActionResult GetBuyerReportByBuyerChannel(BuyersReportRequestModel buyerReportModel)
        {
            _reportService.ValidateFilters(FilterBuyerIds: buyerReportModel.BuyerIds,
                FilterBuyerChannelIds: buyerReportModel.BuyerChannelIds,
                FilterAffiliateIds: buyerReportModel.AffiliateIds, FilterCampaignIds: buyerReportModel.CampaignIds);

            List<BuyerChanneModelResponse> buyers = new List<BuyerChanneModelResponse>();
            ReportCSVGenerator.Instance.Init("Buyer,Cap Reached,Posted Leads,Sold Leads,Rejected Leads,Cost,Profit,Average Price,Accept Rate,Redirect Rate");

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(buyerReportModel.StartDate.Date.ToString(),
                buyerReportModel.EndDate.Date.ToString(), false, out startDate, out endDate);

            buyers = GetBuyerReportByBuyerChannelData(startDate, endDate, string.Join(",", buyerReportModel.BuyerIds),
                                                    string.Join(",", buyerReportModel.BuyerChannelIds),
                                                    string.Join(",", buyerReportModel.AffiliateIds),
                                                    string.Join(",", buyerReportModel.CampaignIds));

            if (buyerReportModel.IsCsv)
            {
                return PerformCsvDownload("BuyerReportByBuyerChannel.csv", "text/csv");
            }


            return Ok(buyers);
        }

        [HttpPost]
        [Route("buyers/byAffiliateChannels")]
        public IHttpActionResult GetReportBuyersByAffiliateChannels(BuyersReportRequestModel buyerReportModel)
        {
            List<AffiliateChannelModelResponse> buyers = new List<AffiliateChannelModelResponse>();
            //ReportCSVGenerator.Instance.Init("Buyer/Affiliate Channel,Total leads,Sold leads," + ((_appContext.AppUser.UserTypeId == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserTypeId == SharedData.NetowrkUserTypeId) ? "Money Made" : "Amount of money spent") + ",Accept rate");
            //ReportCSVGenerator.Instance.Init("Buyer/Affiliate Channel,Total leads,Sold leads," +
            //                                 "Amount of money spent" + ",Accept rate");

            _reportService.ValidateFilters(FilterBuyerIds: buyerReportModel.BuyerIds,
    FilterBuyerChannelIds: buyerReportModel.BuyerChannelIds,
    FilterAffiliateIds: buyerReportModel.AffiliateIds, FilterCampaignIds: buyerReportModel.CampaignIds);

            ReportCSVGenerator.Instance.Init("Buyer Name/Affiliate Channel Name,Posted Leads,Sold Leads,Rejected Leads,Cost,Profit,Average Price,Accept Rate,Redirect Rate");

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(buyerReportModel.StartDate.Date.ToString(),
                buyerReportModel.EndDate.Date.ToString(), false, out startDate, out endDate);

            buyers = GetReportBuyersByAffiliateChannelsData(startDate, endDate, string.Join(",", buyerReportModel.BuyerChannelIds),
                                                            string.Join(",", buyerReportModel.BuyerChannelIds),
                                                            string.Join(",", buyerReportModel.AffiliateIds),
                                                            string.Join(",", buyerReportModel.CampaignIds));
            if (buyerReportModel.IsCsv)
            {
                return PerformCsvDownload("ReportBuyersByAffiliateChannels.csv", "text/csv");
            }

            return Ok(buyers);
        }

        [HttpPost]
        [Route("buyers/byStates")]
        public IHttpActionResult GetReportBuyersByStates(BuyersReportRequestModel buyerReportModel)
        {
            _reportService.ValidateFilters(FilterBuyerIds: buyerReportModel.BuyerIds,
                FilterBuyerChannelIds: buyerReportModel.BuyerChannelIds,
                FilterAffiliateIds: buyerReportModel.AffiliateIds, FilterCampaignIds: buyerReportModel.CampaignIds);

            List<BuyersReportByStatesResponseModel> buyers = new List<BuyersReportByStatesResponseModel>();
            //ReportCSVGenerator.Instance.Init("State,Posted leads,Sold leads,Rejected leads," + ((_appContext.AppUser.UserTypeId == SharedData.BuiltInUserTypeId || _appContext.AppUser.UserTypeId == SharedData.NetowrkUserTypeId) ? "Money Made" : "Amount of money spent") + ",Accept rate");
            ReportCSVGenerator.Instance.Init("Country State,Sold Leads,Rejected Leads,Buyer Payed Sum,Accept Rate,Redirect Rate");

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(buyerReportModel.StartDate.Date.ToString(),
                buyerReportModel.EndDate.Date.ToString(), false, out startDate, out endDate);

            List<string> states = new List<string>();

            for (int i = 0; i < buyerReportModel.States.Count; i++)
            {
                var entity = _stateProvinceService.GetStateProvinceById(buyerReportModel.States[i]);
                if (entity != null)
                {
                    states.Add("'" + entity.Code + "'");
                }
                //requestModel.States[i] = $"'{requestModel.States[i]}'";
            }

            buyers = GetReportBuyersByStatesData(startDate, endDate,
                                                string.Join(",", buyerReportModel.BuyerIds),
                                                string.Join(",", buyerReportModel.BuyerChannelIds),
                                                string.Join(",", buyerReportModel.AffiliateIds),
                                                string.Join(",", buyerReportModel.CampaignIds),
                                                string.Join(",", states));

            _reportService.InsertReportsViewed("buyers_statement_report");

            if (buyerReportModel.IsCsv)
            {
                return PerformCsvDownload("ReportBuyersByStates.csv", "text/csv");
            }

            return Ok(buyers);
        }

        [HttpPost]
        [Route("buyers/byPrices")]
        public IHttpActionResult GetReportBuyersByPrices(BuyersReportByPricesFilterModel model)
        {

            ReportCSVGenerator.Instance.Init("Channel,Price,Total Leads,Unique leads");

            var buyers = GetReportBuyersByPricesData(model);

            _reportService.InsertReportsViewed("buyers_by_price_report");

            if (model.IsCsv)
            {
                return PerformCsvDownload("ReportBuyersByPrices.csv", "text/csv");
            }

            return Ok(buyers);
        }


        [HttpPost]
        [Route("buyers/byHour")]
        public IHttpActionResult GetReportBuyersByHour([FromBody]BuyersReportByHourFilterModel model)
        {
            _reportService.ValidateFilters(FilterBuyerChannelIds: model.BuyerChannelIds, FilterCampaignIds: model.CampaignIds);

            ReportCSVGenerator.Instance.Init(
                $"{string.Empty}," +
                $"{model.ReportDate1}, {string.Empty}," +
                $"{model.ReportDate2}, {string.Empty}," +
                $"{model.ReportDate3}, {string.Empty},");

            ReportCSVGenerator.Instance.AddRow("Hour,Total leads1,Sold leads1,Total leads2,Sold leads2,Total leads3,Sold leads3");

            var buyers = GetReportBuyersByHourData(model.ReportDate1, model.ReportDate2, model.ReportDate3, string.Join(",", model.BuyerChannelIds), string.Join(",", model.CampaignIds));

            _reportService.InsertReportsViewed("buyers_by_hour_report");

            if (model.IsCsv)
            {
                return PerformCsvDownload("ReportBuyersByHours.csv", "text/csv");
            }
            return Ok(buyers);
        }

        [HttpPost]
        [Route("affiliates/byChannels")]
        public IHttpActionResult GetReportAffiliatesByAffiliateChannels([FromBody]AffiliatesReportRequestModel requestModel)
        {
            _reportService.ValidateFilters(FilterAffiliateIds: requestModel.AffiliateIds, FilterAffiliateChannelIds: requestModel.AffiliateChannelIds);

            ReportCSVGenerator.Instance.Init(
                "Affiliate Name/Channel Name,Posted Leads,Sold Leads,Accept Rate %,Redirect Rate %,Profit,EPL,EPA");

            string affiliateIds = string.Join(",", requestModel.AffiliateIds);

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(requestModel.StartDate.Date.ToString(), requestModel.EndDate.Date.ToString(), false,
                out startDate, out endDate);

            List<ReportAffiliatesByAffiliateChannels> report =
                (List<ReportAffiliatesByAffiliateChannels>)this._reportService.ReportAffiliatesByAffiliateChannels(
                    startDate, endDate,
                    (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds),
                    (!requestModel.AffiliateChannelIds.Any() ? "" : string.Join(",", requestModel.AffiliateChannelIds)));

            List<AffiliateReportByAffiliateChannelModel>
                affiliates = new List<AffiliateReportByAffiliateChannelModel>();

            long AffiliateId = 0;
            long affiliateChannelId = 0;

            AffiliateReportByAffiliateChannelModel totals = new AffiliateReportByAffiliateChannelModel();
            totals.title = "Total";
            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.AffiliateChannelId = 0;
            totals.AffiliateChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Cost = 0;
            totals.Credit = 0;
            totals.Redirected = 0;
            totals.Profit = 0;

            AffiliateReportByAffiliateChannelModel item = null;
            AffiliateReportByAffiliateChannelModel item2 = null;
            AffiliateReportByAffiliateChannelModel prevItem = null;
            AffiliateReportByAffiliateChannelModel prevItem2 = null;

            foreach (ReportAffiliatesByAffiliateChannels r in report)
            {
                if (r.AffiliateId != AffiliateId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            prevItem.title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate,
                            prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
                    }

                    item = new AffiliateReportByAffiliateChannelModel();
                    item.title = r.AffiliateName;
                    item.AffiliateId = r.AffiliateId;
                    item.AffiliateName = r.AffiliateName;
                    item.AffiliateChannelId = r.AffiliateChannelId;
                    item.AffiliateChannelName = r.AffiliateChannelName;

                    item.TotalLeads = 0;
                    item.SoldLeads = 0;
                    item.Cost = 0;
                    item.Credit = 0;
                    item.Redirected = 0;
                    item.Profit = 0;

                    affiliates.Add(item);

                    AffiliateId = r.AffiliateId;
                }

                AffiliateReportByAffiliateChannelModel parent = affiliates[affiliates.Count - 1];
                prevItem2 = item2;

                if (affiliateChannelId != r.AffiliateChannelId)
                {
                    if (prevItem2 != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            "-" + prevItem2.title, prevItem2.TotalLeads, prevItem2.SoldLeads, prevItem2.AcceptRate,
                            prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA));
                    }

                    item2 = new AffiliateReportByAffiliateChannelModel();
                    item2.title = r.AffiliateChannelName;
                    parent.children.Add(item2);
                    affiliateChannelId = r.AffiliateChannelId;
                }

                if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

                item2 = parent.children[parent.children.Count - 1];

                item2.AffiliateId = r.AffiliateId;
                item2.title = r.AffiliateChannelName;
                item2.AffiliateName = r.AffiliateName;
                item2.AffiliateChannelId = r.AffiliateChannelId;
                item2.AffiliateChannelName = r.AffiliateChannelName;

                item2.TotalLeads += r.TotalLeads;
                item2.SoldLeads += r.SoldLeads;
                item2.Cost += r.Debet;
                item2.Credit += r.Credit;
                item2.Redirected += r.Redirected;
                item2.Profit = item2.Cost - item2.Credit;

                item2.AcceptRate =
                    Math.Round(
                        item2.TotalLeads > 0
                            ? (decimal)item2.SoldLeads / (decimal)item2.TotalLeads * (decimal)100.0
                            : 0, 2);
                item2.RedirectedRate =
                    Math.Round(
                        item2.SoldLeads > 0
                            ? (decimal)item2.Redirected / (decimal)item2.SoldLeads * (decimal)100.0
                            : 0, 2);
                item2.EPL = Math.Round(item2.TotalLeads > 0 ? (decimal)item2.Profit / (decimal)item2.TotalLeads : 0,
                    2);
                item2.EPA = Math.Round(item2.SoldLeads > 0 ? (decimal)item2.Profit / (decimal)item2.SoldLeads : 0, 2);

                parent.TotalLeads += r.TotalLeads;
                parent.SoldLeads += r.SoldLeads;
                parent.Cost += r.Debet;
                parent.Credit += r.Credit;
                parent.Redirected += r.Redirected;
                parent.Profit = parent.Cost - parent.Credit;

                parent.AcceptRate =
                    Math.Round(
                        parent.TotalLeads > 0
                            ? (decimal)parent.SoldLeads / (decimal)parent.TotalLeads * (decimal)100.0
                            : 0, 2);
                parent.RedirectedRate =
                    Math.Round(
                        parent.SoldLeads > 0
                            ? (decimal)parent.Redirected / (decimal)parent.SoldLeads * (decimal)100.0
                            : 0, 2);
                parent.EPL =
                    Math.Round(parent.TotalLeads > 0 ? (decimal)parent.Profit / (decimal)parent.TotalLeads : 0, 2);
                parent.EPA = Math.Round(parent.SoldLeads > 0 ? (decimal)parent.Profit / (decimal)parent.SoldLeads : 0,
                    2);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Cost += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit = totals.Cost - totals.Credit;

                totals.AcceptRate =
                    Math.Round(
                        totals.TotalLeads > 0
                            ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0
                            : 0, 2);
                totals.RedirectedRate =
                    Math.Round(
                        totals.SoldLeads > 0
                            ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0
                            : 0, 2);
                totals.EPL =
                    Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
                totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0,
                    2);

                prevItem = item;
            }

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.title,
                    prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate,
                    prevItem.Profit, prevItem.EPL, prevItem.EPA));
            }

            if (prevItem2 != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                    "-" + prevItem2.title, prevItem2.TotalLeads, prevItem2.SoldLeads, prevItem2.AcceptRate,
                    prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA));
            }

            affiliates.Add(totals);

            _reportService.InsertReportsViewed("affiliates_report");

            if (requestModel.IsCsv)
            {
                return PerformCsvDownload("ReportAffiliatesByAffiliateChannels.csv", "text/csv");
            }

            return Ok(affiliates);
        }

        /// <summary>
        /// Gets the report affiliates by campaigns.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("affiliates/byCampaigns")]
        public IHttpActionResult GetReportAffiliatesByCampaigns([FromBody]AffiliatesReportRequestModel requestModel)
        {
            _reportService.ValidateFilters(FilterAffiliateIds: requestModel.AffiliateIds, FilterAffiliateChannelIds: requestModel.AffiliateChannelIds);

            ReportCSVGenerator.Instance.Init(
                "Affiliate Name/Campaign Name,Posted Leads,Sold Leads,Accept Rate %,Redirect Rate %,Profit,EPL,EPA");

            string affiliateIds = string.Join(",", requestModel.AffiliateIds);

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(requestModel.StartDate.Date.ToString(), requestModel.EndDate.Date.ToString(), false,
                out startDate, out endDate);

            List<ReportAffiliatesByCampaigns> report =
                (List<ReportAffiliatesByCampaigns>)this._reportService.ReportAffiliatesByCampaigns(startDate, endDate,
                    (!requestModel.AffiliateIds.Any() ? "" : affiliateIds),
                    (!requestModel.AffiliateChannelIds.Any() ? "" : String.Join(",", requestModel.AffiliateChannelIds)));

            List<AffiliateReportByCampaignModel> affiliates = new List<AffiliateReportByCampaignModel>();

            string campaignName = "";

            AffiliateReportByCampaignModel totals = new AffiliateReportByCampaignModel();
            totals.Title = "Total";
            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.CampaignName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Cost = 0;
            totals.Credit = 0;
            totals.Redirected = 0;
            totals.Profit = 0;

            AffiliateReportByCampaignModel item = null;
            AffiliateReportByCampaignModel prevItem = null;

            foreach (ReportAffiliatesByCampaigns r in report)
            {
                if (r.CampaignName != campaignName)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            prevItem.Title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate,
                            prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
                    }

                    campaignName = r.CampaignName;
                    item = new AffiliateReportByCampaignModel();
                    affiliates.Add(item);
                }

                if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

                item = affiliates[affiliates.Count - 1];
                item.Title = r.CampaignName;
                item.AffiliateId = r.AffiliateId;
                item.AffiliateName = r.AffiliateName;
                item.CampaignName = r.CampaignName;

                item.TotalLeads += r.TotalLeads;
                item.SoldLeads += r.SoldLeads;
                item.Cost += r.Debet;
                item.Credit += r.Credit;
                item.Redirected += r.Redirected;
                item.Profit = item.Cost - item.Credit;

                item.AcceptRate =
                    Math.Round(
                        item.TotalLeads > 0
                            ? (decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0
                            : 0, 2);
                item.RedirectedRate =
                    Math.Round(
                        item.SoldLeads > 0 ? (decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0 : 0,
                        2);
                item.EPL = Math.Round(item.TotalLeads > 0 ? (decimal)item.Profit / (decimal)item.TotalLeads : 0, 2);
                item.EPA = Math.Round(item.SoldLeads > 0 ? (decimal)item.Profit / (decimal)item.SoldLeads : 0, 2);

                //parent.TotalLeads += r.TotalLeads;
                //parent.SoldLeads += r.SoldLeads;
                //parent.Debit += r.Debet;
                //parent.Credit += r.Credit;

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Cost += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit = totals.Cost - totals.Credit;

                totals.AcceptRate =
                    Math.Round(
                        totals.TotalLeads > 0
                            ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0
                            : 0, 2);
                totals.RedirectedRate =
                    Math.Round(
                        totals.SoldLeads > 0
                            ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0
                            : 0, 2);
                totals.EPL =
                    Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
                totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0,
                    2);

                prevItem = item;

                //parent.children.Add(b);
            }

            affiliates.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.Title,
                    prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate,
                    prevItem.Profit, prevItem.EPL, prevItem.EPA));
            }

            if (requestModel.IsCsv)
            {
                return PerformCsvDownload("ReportAffiliatesByCampaigns.csv", "text/csv");
            }

            return Ok(affiliates);
        }

        /// <summary>
        /// Gets the report affiliates by states.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("affiliates/byStates")]
        public IHttpActionResult GetReportAffiliatesByStates([FromBody]AffiliatesReportRequestModel requestModel)
        {
            _reportService.ValidateFilters(FilterAffiliateIds: requestModel.AffiliateIds, FilterAffiliateChannelIds: requestModel.AffiliateChannelIds);

            ReportCSVGenerator.Instance.Init(
                "Affiliate Name/Country/State,Posted Leads,Sold Leads,Accept Rate %,Redirect Rate %,Profit,EPL,EPA");

            List<string> states = new List<string>();

            for (int i = 0; i < requestModel.States.Count; i++)
            {
                var entity = _stateProvinceService.GetStateProvinceById(requestModel.States[i]);
                if (entity != null)
                {
                    states.Add("'" + entity.Code + "'");
                }
                //requestModel.States[i] = $"'{requestModel.States[i]}'";
            }

            string affiliateIds = String.Join(",", requestModel.AffiliateIds);

            string statesStr = String.Join(",", states);
            //if (!string.IsNullOrEmpty(aid)) affiliatesIds = aid;

            DateTime startDate;
            DateTime endDate;
            GetDateRangesFromString(requestModel.StartDate.Date.ToString(), requestModel.EndDate.Date.ToString(), false,
                out startDate, out endDate);

            List<ReportAffiliatesByStates> report =
                (List<ReportAffiliatesByStates>)this._reportService.ReportAffiliatesByStates(startDate, endDate,
                    (!affiliateIds.Any() ? "" : affiliateIds),
                    (!requestModel.AffiliateChannelIds.Any() ? ""
                        : string.Join(",", requestModel.AffiliateChannelIds)), statesStr);

            List<AffiliateReportByStatesModel> affiliates = new List<AffiliateReportByStatesModel>();

            string state = "null";

            AffiliateReportByStatesModel totals = new AffiliateReportByStatesModel();
            totals.Title = "Total";
            totals.AffiliateId = 0;
            totals.AffiliateName = "";
            totals.State = "";

            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.Cost = 0;
            totals.Credit = 0;
            totals.Redirected = 0;
            totals.Profit = 0;

            AffiliateReportByStatesModel item = null;
            AffiliateReportByStatesModel prevItem = null;

            foreach (ReportAffiliatesByStates r in report)
            {
                if (state != r.State)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            prevItem.Title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate,
                            prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA));
                    }

                    state = r.State;
                    item = new AffiliateReportByStatesModel();
                    affiliates.Add(item);
                }

                if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

                item = affiliates[affiliates.Count - 1];

                item.AffiliateId = r.AffiliateId;
                item.AffiliateName = r.AffiliateName;
                item.State = r.State;
                item.Title = r.State;
                item.TotalLeads += r.TotalLeads;
                item.SoldLeads += r.SoldLeads;
                item.Cost += r.Debet;
                item.Credit += r.Credit;
                item.Redirected += r.Redirected;
                item.Profit = item.Cost - item.Credit;

                item.AcceptRate =
                    Math.Round(
                        item.TotalLeads > 0
                            ? (decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0
                            : 0, 2);
                item.RedirectedRate =
                    Math.Round(
                        item.SoldLeads > 0 ? (decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0 : 0,
                        2);
                item.EPL = Math.Round(item.TotalLeads > 0 ? (decimal)item.Profit / (decimal)item.TotalLeads : 0, 2);
                item.EPA = Math.Round(item.SoldLeads > 0 ? (decimal)item.Profit / (decimal)item.SoldLeads : 0, 2);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.Cost += r.Debet;
                totals.Credit += r.Credit;
                totals.Redirected += r.Redirected;
                totals.Profit = totals.Cost - totals.Credit;

                totals.AcceptRate =
                    Math.Round(
                        totals.TotalLeads > 0
                            ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0
                            : 0, 2);
                totals.RedirectedRate =
                    Math.Round(
                        totals.SoldLeads > 0
                            ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0
                            : 0, 2);
                totals.EPL =
                    Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
                totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0,
                    2);

                prevItem = item;
            }

            affiliates.Add(totals);

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", prevItem.Title,
                    prevItem.TotalLeads, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate,
                    prevItem.Profit, prevItem.EPL, prevItem.EPA));
            }

            if (requestModel.IsCsv)
            {
                return PerformCsvDownload("ReportAffiliatesByStates.csv", "text/csv");
            }

            return Ok(affiliates);
        }

        //[HttpPost]
        //[Route("affiliates/clicks")]
        //public IHttpActionResult GetReportClickMain([FromBody] AffiliatesReportClicksFilterModel model)
        //{
        //    ReportCSVGenerator.Instance.Init("Affiliate/Affiliate Channel,Hits,Unique clicks,Total leads,CTA,Sold leads,Accept rate %,Redirect %,Affiliate Profit,EPL,EPA,EPC");

        //    if (!model.AffiliateIds.Any())
        //    {
        //        var affiliateList = _affiliateService.GetAllAffiliates();

        //        for (int i = 0; i < affiliateList.Count; i++)
        //        {
        //            model.AffiliateIds.Add(affiliateList[i].Id);
        //        }
        //    }

        //    var report = (List<ReportClickMain>)this._reportService.ReportClickMain(model.DateFrom, 
        //                                                                              model.DateTo,
        //                                                                              (model.AffiliateChannelIds.Any() ? string.Join(",",model.AffiliateChannelIds): ""));

        //    var affiliates = new List<AffiliateReportClicksModel>();

        //    long AffiliateId = 0;
        //    long affiliateChannelId = 0;

        //    var totals = new AffiliateReportClicksModel();
        //    totals.Title = "Total";
        //    totals.Folder = true;
        //    totals.Expanded = false;

        //    totals.AffiliateId = 0;
        //    totals.AffiliateName = "";
        //    totals.AffiliateChannelId = 0;
        //    totals.AffiliateChannelName = "";
        //    totals.TotalLeads = 0;
        //    totals.SoldLeads = 0;
        //    totals.Debet = 0;
        //    totals.Credit = 0;
        //    totals.Redirected = 0;
        //    totals.Profit = 0;

        //    AffiliateReportClicksModel item = null;
        //    AffiliateReportClicksModel item2 = null;
        //    AffiliateReportClicksModel prevItem = null;
        //    AffiliateReportClicksModel prevItem2 = null;

        //    foreach (ReportClickMain r in report)
        //    {
        //        if (r.AffiliateId != AffiliateId)
        //        {
        //            if (prevItem != null)
        //            {
        //                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", prevItem.Title, prevItem.Hits, prevItem.UniqueClicks, prevItem.TotalLeads, prevItem.CTA, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA, prevItem.EPC));
        //            }

        //            item = new AffiliateReportClicksModel
        //            {
        //                Title = r.AffiliateName,
        //                Folder = true,
        //                Expanded = true,
        //                AffiliateId = r.AffiliateId,
        //                AffiliateName = r.AffiliateName,
        //                AffiliateChannelId = r.AffiliateChannelId,
        //                AffiliateChannelName = r.AffiliateChannelName,
        //                TotalLeads = 0,
        //                SoldLeads = 0,
        //                Debet = 0,
        //                Credit = 0,
        //                Redirected = 0,
        //                Profit = 0,
        //                TotalProfit = 0,
        //                Hits = 0,
        //                UniqueClicks = 0,
        //                ClickProfit = 0,
        //                CTA = 0,
        //                EPC = 0
        //            };
        //            affiliates.Add(item);

        //            AffiliateId = r.AffiliateId;
        //        }

        //        var parent = affiliates[affiliates.Count - 1];
        //        prevItem2 = item2;

        //        if (affiliateChannelId != r.AffiliateChannelId)
        //        {
        //            if (prevItem2 != null)
        //            {
        //                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "-" + prevItem2.Title, prevItem2.Hits, prevItem2.UniqueClicks, prevItem2.TotalLeads, prevItem2.CTA, prevItem2.SoldLeads, prevItem2.AcceptRate, prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA, prevItem2.EPC));
        //            }

        //            item2 = new AffiliateReportClicksModel {Title = r.AffiliateChannelName};
        //            parent.Children.Add(item2);
        //            affiliateChannelId = r.AffiliateChannelId;
        //        }

        //        if (r.Redirected > r.SoldLeads) r.Redirected = r.SoldLeads;

        //        item2 = parent.Children[parent.Children.Count - 1];
        //        item2.Title = r.AffiliateChannelName;
        //        item2.Folder = false;
        //        item2.Expanded = true;

        //        item2.AffiliateId = r.AffiliateId;
        //        item2.AffiliateName = r.AffiliateName;
        //        item2.AffiliateChannelId = r.AffiliateChannelId;
        //        item2.AffiliateChannelName = r.AffiliateChannelName;

        //        item2.TotalLeads += r.TotalLeads;
        //        item2.SoldLeads += r.SoldLeads;
        //        item2.Debet += r.Debet;
        //        item2.Credit += r.Credit;
        //        item2.Redirected += r.Redirected;
        //        item2.Profit = item2.Debet - item2.Credit;
        //        item2.Hits += r.Hits;
        //        item2.UniqueClicks += r.UniqueClicks;
        //        item2.ClickProfit += r.ClickProfit;
        //        item2.TotalProfit = item2.ClickProfit + item2.Profit;


        //        item2.AcceptRate = Math.Round(item2.TotalLeads > 0 ? (decimal)item2.SoldLeads / (decimal)item2.TotalLeads * (decimal)100.0 : 0, 2);
        //        item2.RedirectedRate = Math.Round(item2.SoldLeads > 0 ? (decimal)item2.Redirected / (decimal)item2.SoldLeads * (decimal)100.0 : 0, 2);
        //        item2.EPL = Math.Round(item2.TotalLeads > 0 ? (decimal)item2.Profit / (decimal)item2.TotalLeads : 0, 2);
        //        item2.EPA = Math.Round(item2.SoldLeads > 0 ? (decimal)item2.Profit / (decimal)item2.SoldLeads : 0, 2);
        //        item2.CTA = Math.Round(item2.UniqueClicks > 0 ? (decimal)item2.TotalLeads / (decimal)item2.UniqueClicks * 100 : 0, 2);
        //        item2.EPC = Math.Round(item2.UniqueClicks > 0 ? (decimal)item2.Profit / (decimal)item2.UniqueClicks : 0, 2);

        //        parent.TotalLeads += r.TotalLeads;
        //        parent.SoldLeads += r.SoldLeads;
        //        parent.Debet += r.Debet;
        //        parent.Credit += r.Credit;
        //        parent.Redirected += r.Redirected;
        //        parent.Profit = parent.Debet - parent.Credit;
        //        parent.Hits += r.Hits;
        //        parent.UniqueClicks += r.UniqueClicks;
        //        parent.ClickProfit += r.ClickProfit;
        //        parent.TotalProfit = parent.ClickProfit + parent.Profit;

        //        parent.AcceptRate = Math.Round(parent.TotalLeads > 0 ? (decimal)parent.SoldLeads / (decimal)parent.TotalLeads * (decimal)100.0 : 0, 2);
        //        parent.RedirectedRate = Math.Round(parent.SoldLeads > 0 ? (decimal)parent.Redirected / (decimal)parent.SoldLeads * (decimal)100.0 : 0, 2);
        //        parent.EPL = Math.Round(parent.TotalLeads > 0 ? (decimal)parent.Profit / (decimal)parent.TotalLeads : 0, 2);
        //        parent.EPA = Math.Round(parent.SoldLeads > 0 ? (decimal)parent.Profit / (decimal)parent.SoldLeads : 0, 2);
        //        parent.CTA = Math.Round(parent.UniqueClicks > 0 ? (decimal)parent.TotalLeads / (decimal)parent.UniqueClicks * 100 : 0, 2);
        //        parent.EPC = Math.Round(parent.UniqueClicks > 0 ? (decimal)parent.Profit / (decimal)parent.UniqueClicks : 0, 2);


        //        totals.TotalLeads += r.TotalLeads;
        //        totals.SoldLeads += r.SoldLeads;
        //        totals.Debet += r.Debet;
        //        totals.Credit += r.Credit;
        //        totals.Redirected += r.Redirected;
        //        totals.Profit = totals.Debet - totals.Credit;
        //        totals.Hits += r.Hits;
        //        totals.UniqueClicks += r.UniqueClicks;
        //        totals.ClickProfit += r.ClickProfit;
        //        totals.TotalProfit = totals.ClickProfit + totals.Profit;

        //        totals.AcceptRate = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.SoldLeads / (decimal)totals.TotalLeads * (decimal)100.0 : 0, 2);
        //        totals.RedirectedRate = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Redirected / (decimal)totals.SoldLeads * (decimal)100.0 : 0, 2);
        //        totals.EPL = Math.Round(totals.TotalLeads > 0 ? (decimal)totals.Profit / (decimal)totals.TotalLeads : 0, 2);
        //        totals.EPA = Math.Round(totals.SoldLeads > 0 ? (decimal)totals.Profit / (decimal)totals.SoldLeads : 0, 2);
        //        totals.CTA = Math.Round(totals.UniqueClicks > 0 ? (decimal)totals.TotalLeads / (decimal)totals.UniqueClicks * 100 : 0, 2);
        //        totals.EPC = Math.Round(totals.UniqueClicks > 0 ? (decimal)totals.Profit / (decimal)totals.UniqueClicks : 0, 2);

        //        prevItem = item;
        //    }

        //    if (prevItem != null)
        //    {
        //        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "-" + prevItem.Title, prevItem.Hits, prevItem.UniqueClicks, prevItem.TotalLeads, prevItem.CTA, prevItem.SoldLeads, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.Profit, prevItem.EPL, prevItem.EPA, prevItem.EPC));
        //    }

        //    if (prevItem2 != null)
        //    {
        //        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", "-" + prevItem2.Title, prevItem2.Hits, prevItem2.UniqueClicks, prevItem2.TotalLeads, prevItem2.CTA, prevItem2.SoldLeads, prevItem2.AcceptRate, prevItem2.RedirectedRate, prevItem2.Profit, prevItem2.EPL, prevItem2.EPA, prevItem2.EPC));
        //    }

        //    affiliates.Add(totals);

        //    if (model.IsCsv)
        //        return PerformCsvDownload("ReportAffiliatesClicks", "text/csv");

        //    return Ok(affiliates);
        //} 


        [HttpPost]
        [Route("reportBuyerComparison")]
        public IHttpActionResult GetReportBuyersComparison([FromBody] ReportBuyersComparisonFilterModel reportBuyersComparisonFilterModel)
        {
            _reportService.ValidateFilters(FilterBuyerIds: reportBuyersComparisonFilterModel.Ids, FilterCampaignIds: reportBuyersComparisonFilterModel.CampaignIds);

            DateTime date1 = _settingService.GetUTCDate(reportBuyersComparisonFilterModel.Date1);
            DateTime date2 = _settingService.GetUTCDate(reportBuyersComparisonFilterModel.Date2);
            DateTime date3 = _settingService.GetUTCDate(reportBuyersComparisonFilterModel.Date3);

            var buyers = GetReportBuyersComparisonData(date1, date2, date3,
                    reportBuyersComparisonFilterModel.Ids.ToArray(),
                    reportBuyersComparisonFilterModel.CampaignIds.ToArray(),
                    reportBuyersComparisonFilterModel.ShowBy);

            _reportService.InsertReportsViewed("buyers_comparison_report");

            if (reportBuyersComparisonFilterModel.IsCsv)
            {
                return PerformCsvDownload("ReportBuyersComparison.csv", "text/csv");
            }

            return Ok(buyers);
        }

        [HttpPost]
        [Route("reportAffiliateComparison")]
        public IHttpActionResult GetReportAffiliateComparison([FromBody] ReportBuyersComparisonFilterModel reportBuyersComparisonFilterModel)
        {
            _reportService.ValidateFilters(FilterBuyerIds: reportBuyersComparisonFilterModel.Ids, FilterCampaignIds: reportBuyersComparisonFilterModel.CampaignIds);

            DateTime date1 = _settingService.GetUTCDate(reportBuyersComparisonFilterModel.Date1);
            DateTime date2 = _settingService.GetUTCDate(reportBuyersComparisonFilterModel.Date2);
            DateTime date3 = _settingService.GetUTCDate(reportBuyersComparisonFilterModel.Date3);

            var buyers = GetReportBuyersComparisonData(date1, date2, date3,
                    reportBuyersComparisonFilterModel.Ids.ToArray(),
                    reportBuyersComparisonFilterModel.CampaignIds.ToArray(),
                    reportBuyersComparisonFilterModel.ShowBy, false);

            _reportService.InsertReportsViewed("buyers_comparison_report");

            if (reportBuyersComparisonFilterModel.IsCsv)
            {
                return PerformCsvDownload("ReportBuyersComparison.csv", "text/csv");
            }

            return Ok(buyers);
        }

        [HttpPost]
        [Route("reportTrafficEstimator")]
        public IHttpActionResult GetReportTrafficEstimator([FromBody] ReportBuyersTrafficEstimatorFilterModel model)
        {
            _reportService.ValidateFilters(FilterBuyerChannelIds: model.BuyerChannelIds, FilterCampaignIds: model.CampaignIds);

            ReportCSVGenerator.Instance.Init("Buyer Name,Total Leads,Unique Leads");

            DateTime date1 = _settingService.GetUTCDate(model.StartDate);
            DateTime date2 = _settingService.GetUTCDate(model.EndDate);

            var report = GetReportBuyersByTrafficEstimatorData(date1, date2, model.BuyerChannelIds, model.CampaignIds, model.Fields);

            _reportService.InsertReportsViewed("buyers_by_traffic_report");

            if (model.IsCsv)
            {
                return PerformCsvDownload("ReportTrafficEstimator.csv", "text/csv");
            }

            return Ok(report);
        }

        [HttpPost]
        [Route("reportBadIpClicks")]
        public IHttpActionResult GetBadIpClicksReport([FromBody] ReportBadIpClickFilterModel model)
        {
            try
            {
                ReportCSVGenerator.Instance.Init("Date,Lead ID,Affiliate,Lead IP,Click IP");

                _reportService.InsertReportsViewed("bad_ip_clicks_report");

                var res = GetBadIpClicksReportData(model);

                if (model.IsCsv)
                {
                    return PerformCsvDownload("ReportBadIpClicks.csv", "text/csv");
                }

                return Ok(res);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Errors the leads report buyer.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("buyer/reportErrorLeads")]
        public IHttpActionResult ErrorLeadsReportBuyer([FromBody] ErrorLeadsReportBuyerFilterModel model)
        {
            try
            {
                _reportService.ValidateFilters(FilterBuyerChannelIds: model.BuyerChannelIds, FilterAffiliateChannelIds: model.AffiliateChannelIds, FilterCampaignIds: model.CampaignIds);

                ReportCSVGenerator.Instance.Init("Date,Campaign,Buyer,Buyer Channel,Affiliate Channel,Status,By Country/State,Reason,Description");

                int page = model.Page > 0 ? model.Page : 1;
                int count = model.Count > 0 ? model.Count : 100;

                _reportService.InsertReportsViewed("buyers_error_leads_report");

                var rep = GetErrorLeadsReportData(model.StartDate,
                                                        model.EndDate,
                                                        model.AffiliateChannelIds,
                                                        model.BuyerChannelIds,
                                                        model.CampaignIds,
                                                        model.LeadId,
                                                        model.StatusId,
                                                        model.Description,
                                                        model.ErrorTypeId,
                                                        model.States,
                                                        (short)Core.ReportType.Buyer, page, count);

                if (model.IsCsv)
                {
                    return PerformCsvDownload("ReportBuyersErrorLeads.csv", "text/csv");
                }

                return Ok(rep);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("affiliate/reportErrorLeads")]
        public IHttpActionResult ErrorLeadsReportAffiliate([FromBody] ErrorLeadsReportAffiliateFilterModel model)
        {
            try
            {
                _reportService.ValidateFilters(FilterAffiliateChannelIds: model.AffiliateChannelIds);

                ReportCSVGenerator.Instance.Init("Date,Affiliate,Affiliate Channel,State,Reason,Description");

                int page = model.Page > 0 ? model.Page : 1;
                int count = model.Count > 0 ? model.Count : 100;

                _reportService.InsertReportsViewed("affiliates_error_leads_report");

                var rep = GetErrorLeadsReportData(model.StartDate,
                                                        model.EndDate,
                                                        model.AffiliateChannelIds,
                                                        new List<long>(),
                                                        new List<long>(),
                                                        model.LeadId,
                                                        0,
                                                        model.Description,
                                                        model.ErrorTypeId,
                                                        model.States,
                                                        (short)Core.ReportType.Affiliate, page, count);
                if (model.IsCsv)
                {
                    return PerformCsvDownload("ReportAffiliatesErrorLeads.csv", "text/csv");
                }

                return Ok(rep);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets the report buyers win rate.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("getReportBuyersWinRateReport")]
        public IHttpActionResult GetReportBuyersWinRateReport([FromBody]BuyersReportWinRateFilterModel model)
        {
            _reportService.ValidateFilters(FilterBuyerChannelIds: model.BuyerChannelIds);

            var buyersReportWinRates = new List<BuyersReportWinRateModel>();
            ReportCSVGenerator.Instance.Init("Price,Sold Leads,Rejected Leads,Rejected by min price");

            var report = _reportService.ReportBuyersWinRateReport(model.StartDate, model.EndDate, string.Join(",", model.BuyerChannelIds));

            decimal buyerPrice = 0;

            var totals = new BuyersReportWinRateModel
            {
                Title = "Total",
                Folder = true,
                Expanded = false,
                BuyerChannelId = 0,
                BuyerChannelName = "",
                AffiliatePrice = 0,
                BuyerPrice = 0,
                TotalLeads = 0,
                SoldLeads = 0,
                RejectedLeads = 0,
                MinPriceErrorLeads = 0
            };

            BuyersReportWinRateModel item = null;
            BuyersReportWinRateModel prevItem = null;

            foreach (var r in report)
            {
                if (buyerPrice != r.BuyerPrice)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow($"{prevItem.Title},{prevItem.SoldLeads},{prevItem.RejectedLeads},{prevItem.MinPriceErrorLeads}");
                    }

                    item = new BuyersReportWinRateModel
                    {
                        Title = "$" + Math.Round(r.BuyerPrice, 2).ToString(CultureInfo.InvariantCulture),
                        Folder = false,
                        Expanded = true,
                        BuyerChannelId = r.BuyerChannelId,
                        BuyerChannelName = r.BuyerChannelName,
                        TotalLeads = r.TotalLeads,
                        SoldLeads = r.SoldLeads,
                        RejectedLeads = r.RejectedLeads,
                        MinPriceErrorLeads = r.MinPriceErrorLeads,
                        AffiliatePrice = 0,
                        BuyerPrice = r.BuyerPrice,
                        OtherBuyerPrice = 0
                    };

                    buyersReportWinRates.Add(item);
                    buyerPrice = r.BuyerPrice;
                }
                else
                {
                    if (item != null)
                    {
                        item.TotalLeads += r.TotalLeads;
                        item.SoldLeads += r.SoldLeads;
                        item.RejectedLeads += r.RejectedLeads;
                        item.MinPriceErrorLeads += r.MinPriceErrorLeads;
                        item.BuyerPrice += r.BuyerPrice;
                    }
                }

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.MinPriceErrorLeads += r.MinPriceErrorLeads;
                totals.BuyerPrice += r.BuyerPrice;

                prevItem = item;
            }

            buyersReportWinRates.Add(totals);

            _reportService.InsertReportsViewed("win_rate_report");

            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow($"{prevItem.Title},{prevItem.SoldLeads},{prevItem.RejectedLeads},{prevItem.MinPriceErrorLeads}");
            }

            if (model.IsCsv)
            {
                return PerformCsvDownload("WinRateReport.csv", "text/csv");
            }
            return Ok(buyersReportWinRates);
        }


        [HttpPost]
        [Route("buyers/statementReport")]
        public async Task<IHttpActionResult> BuyerStatementReport([FromBody] BuyerReportStatementFilterModel buyerReportStatementFilterModel)
        {
            //AZ!!!
            PdfReportCreator creator = new PdfReportBuyerMontlyStatement(0,_reportService, _buyerService);
            await creator.GenerateReport("testOutput.pdf");
            return Ok(1);
            //AZ!!!


            long ticks = DateTime.Now.Ticks;

            ReportCSVGenerator.Instance.AddRow("Row");

            return PerformPDFDownload($"Report-{buyerReportStatementFilterModel.BuyerId}-{ticks}.pdf", "text/csv");


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

            Buyer buyer = _buyerService.GetBuyerById(buyerReportStatementFilterModel.BuyerId);
            string buyerName = "";

            if (buyer != null)
            {
                buyerName = buyer.Name;
            }

            var doc = new iTextSharp.text.Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

            //doc.SetMargins(0, 0, 0, 0);

            PdfWriter.GetInstance(doc, new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/Report-" + buyerReportStatementFilterModel.BuyerId.ToString() + ticks.ToString() + ".pdf", FileMode.Create));
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

                foreach (AffiliateChannel ac in _affiliateChannelService.GetAllAffiliateChannelsByCampaignId(bc.CampaignId))
                {
                    affiliateIds += "," + ac.Id.ToString();
                }
            }


            var report = GetSalesReportData(startDate1, endDate1, startDate2, endDate2, "price");
            string[][] rows = new string[report.Count][];

            for (int i = 0; i < report.Count; i++)
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
                switch (report[i].Name)
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

            return PerformPDFDownload($"Report-{buyerReportStatementFilterModel.BuyerId}-{ticks}.pdf", "text/csv");

            //return File(System.AppDomain.CurrentDomain.BaseDirectory + "/ContentManagement/Uploads/Report-" + id.ToString() + ticks.ToString() + ".pdf", "application/pdf", "Report-" + id.ToString() + ticks.ToString() + ".pdf");
        }

        [HttpPost]
        [Route("executeCustomReport")]
        public IHttpActionResult ExecuteCustomReport([FromBody]CustomReportModel model)
        {
            model = GetAffiliateAndBuyerChannelsFromModel(model);
            var report = _reportService.GetCustomReportById(model.ReportTypeId);
            var reportSetting = _reportService.GetAllReportSettingByReportType(model.ReportTypeId).FirstOrDefault();
            //var reportVariables = _reportService.GetReportVariables(model.ReportTypeId);
            var reportVariables = _reportService.GetCustomReportVariables(model.ReportTypeId);

            var reportQueryString = string.Empty;
            var dateFrom = string.Empty;
            var dateTo = string.Empty;

            var orderSetting = reportSetting.OrderVariableId.HasValue ? reportSetting.OrderVariableId.Value : 0;
            var orderVariable = string.Empty;

            var affiliateAndBuyerChannels = GetAffiliateAndBuyerChannelsReportSetting(reportSetting);

            if (!model.StartDate.HasValue || model.StartDate == DateTime.MinValue || model.StartDate == DateTime.MaxValue)
            {
                var reportDateRange = GenerateDateRangeByReportPeriodType(reportSetting, 0);
                model.StartDate = reportDateRange.Item1;
            }
            if (!model.EndDate.HasValue || model.EndDate == DateTime.MinValue || model.EndDate == DateTime.MaxValue)
            {
                var reportDateRange = GenerateDateRangeByReportPeriodType(reportSetting, 0);
                model.EndDate = reportDateRange.Item2;
            }

            if (model.StartDate != DateTime.MinValue)
            {
                dateFrom = $"'{model.StartDate.Value.Year}-{model.StartDate.Value.Month}-{model.StartDate.Value.Day} {model.StartDate.Value.Hour}:{model.EndDate.Value.Minute}" +
                    $":{model.StartDate.Value.Second}'";
            }
            if (model.EndDate != DateTime.MinValue)
            {
                dateTo = $"'{model.EndDate.Value.Year}-{model.EndDate.Value.Month}-{model.EndDate.Value.Day} {model.EndDate.Value.Hour}:{model.EndDate.Value.Minute}" +
                    $":{model.EndDate.Value.Second}'";
            }

            DateTime startDate = model.StartDate.HasValue ? model.StartDate.Value : DateTime.UtcNow;
            DateTime endDate = model.EndDate.HasValue ? model.EndDate.Value : DateTime.UtcNow;
            
            string buyerChannelIds = string.Join(",", model.BuyerChannelIds);
            string affiliateChannelIds = string.Join(",", model.AffiliateChannelIds);

            var records = _reportService.GetGlobalReport(startDate, endDate, buyerChannelIds, affiliateChannelIds);

            //if (affiliateAndBuyerChannels.AffiliateChannelIds != null && affiliateAndBuyerChannels.AffiliateChannelIds.Any())
            //    model.AffiliateChannelIds =
            //       model.AffiliateChannelIds.Intersect(affiliateAndBuyerChannels.AffiliateChannelIds).ToList();

            //if (affiliateAndBuyerChannels.BuyerChannelIds != null && affiliateAndBuyerChannels.BuyerChannelIds.Any())
            //    model.BuyerChannelIds =
            //        model.BuyerChannelIds.Intersect(affiliateAndBuyerChannels.BuyerChannelIds).ToList();

            if (report != null)
            {
                _reportService.InsertReportsViewed(report.Name, (int)model.ReportTypeId);

                GlobalReport globalReportTotal = new GlobalReport();
                Dictionary<string, object> totalResult = new Dictionary<string, object>();

                List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
                foreach(var record in records)
                {
                    globalReportTotal.Paid += record.Paid;
                    globalReportTotal.PostedLeads += record.PostedLeads;
                    globalReportTotal.Profit += record.Profit;
                    globalReportTotal.RejectedLeads += record.RejectedLeads;
                    globalReportTotal.Revenue += record.Revenue;
                    globalReportTotal.SoldLeads += record.SoldLeads;

                    Dictionary<string, object> result = null;

                    List<Dictionary<string, object>> selectedList = null;
                    bool isExistingResult = false;
                    bool groupByFieldsExist = reportVariables.Where(x => x.IsGroupBy.HasValue && x.IsGroupBy==true).FirstOrDefault() == null ? false : true;

                    foreach (var reportVariable in reportVariables.Where(x => x.IsGroupBy.HasValue && x.IsGroupBy == true).ToList())
                    {
                        if (selectedList == null)
                            selectedList = results.Where(x => x.ContainsKey(reportVariable.Name)).ToList();
                        else
                            selectedList = selectedList.Where(x => x.ContainsKey(reportVariable.Name)).ToList();

                        if (selectedList.Count == 1)
                        {
                            result = selectedList[0];
                            isExistingResult = true;
                        }
                    }

                    if (!groupByFieldsExist && results.Count > 0)
                    {
                        result = results[results.Count - 1];
                    }

                    if (result == null)
                        result = new Dictionary<string, object>();

                    foreach (var reportVariable in reportVariables)
                    {
                        var pi = record.GetType().GetProperty(reportVariable.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        object value = pi.GetValue(record);
                        if (!isExistingResult)
                        {
                            result[reportVariable.Name] = value;
                        }
                        else
                        {
                            if (value != null)
                            {
                                if (value.GetType() == typeof(int))
                                    result[reportVariable.Name] = ((int)result[reportVariable.Name]) + ((int)value);
                                else
                                if (value.GetType() == typeof(decimal))
                                    result[reportVariable.Name] = ((decimal)result[reportVariable.Name]) + ((decimal)value);
                            }
                        }

                        var totalPi = globalReportTotal.GetType().GetProperty(reportVariable.FieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        value = totalPi.GetValue(globalReportTotal);
                        totalResult[reportVariable.Name] = value;
                    }
                    if (!isExistingResult && groupByFieldsExist)
                        results.Add(result);
                }
                if (records.Count > 0)
                    results.Add(totalResult);

                return Ok(results);
                /*var reportTypeVariables = _reportService.GetCustomReportVariables(report.Id);
                if (reportTypeVariables != null && reportTypeVariables.Any())
                    reportQueryString = reportTypeVariables.First().SqlQuery.TrimEnd();
                foreach (var item in reportTypeVariables.Skip(1).Take(reportTypeVariables.Count - 1))
                {
                    var reportTypeVariable = reportVariables.Where(x => x.ReportVariableTypeId == item.Id).FirstOrDefault();

                    if (reportTypeVariable != null)
                    {
                        if (orderSetting == reportTypeVariable.ReportVariableTypeId)
                        {
                            orderVariable = " order by " + item.SqlQuery + (reportSetting.IsAscending.Value ? " asc " : " desc ");
                        }
                        reportQueryString = reportQueryString.Replace("{" + reportTypeVariable.VariableOrder + "}", item.SqlQuery);
                    }
                }

                reportQueryString = reportQueryString.Replace("{orderby}", orderVariable);

                if (((model.AffiliateChannelIds == null || !model.AffiliateChannelIds.Any())
                    && reportQueryString.Contains("@AffiliateChannels"))
                    && ((model.BuyerChannelIds == null || !model.BuyerChannelIds.Any())
                    && reportQueryString.Contains("@BuyerChannels")))
                {
                    _reportService.InsertReportsViewed("custom_report", (int)model.ReportTypeId);
                    return Ok(JsonConvert.DeserializeObject("[]"));
                }
                else if (((model.AffiliateChannelIds == null || !model.AffiliateChannelIds.Any())
                    && reportQueryString.Contains("@AffiliateChannels"))
                    && !reportQueryString.Contains("@BuyerChannels"))
                {
                    _reportService.InsertReportsViewed("custom_report", (int)model.ReportTypeId);
                    return Ok(JsonConvert.DeserializeObject("[]"));
                }
                if (!reportQueryString.Contains("@AffiliateChannels")
                   && ((model.BuyerChannelIds == null || !model.BuyerChannelIds.Any())
                   && reportQueryString.Contains("@BuyerChannels")))
                {
                    _reportService.InsertReportsViewed("custom_report", (int)model.ReportTypeId);
                    return Ok(JsonConvert.DeserializeObject("[]"));
                }

                reportQueryString = reportQueryString.Replace("@End", dateTo).Replace("@Start", dateFrom).Replace("@AffiliateChannels", string.Join(",", model.AffiliateChannelIds) == string.Empty ? "select Id from Affiliatechannel where 1=1" : string.Join(",", model.AffiliateChannelIds)).Replace("@BuyerChannels", string.Join(",", model.BuyerChannelIds) == string.Empty ? "select Id from Buyerchannel where 1=1" : string.Join(",", model.BuyerChannelIds));
                var result = _reportService.ExecuteCustomReportQuery(reportQueryString);
                var dataTable = jsonStringToTable(string.Join("", result));
                if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0 && model.ExportCSV)
                {
                    generateCSV(dataTable);
                    return PerformCsvDownload($"{report.Name}.csv", "text/csv");
                }

                _reportService.InsertReportsViewed("custom_report", (int)model.ReportTypeId);

                string stringResult = string.Join("", result);
                if (string.IsNullOrEmpty(stringResult))
                    stringResult = "[]";

                return Ok(JsonConvert.DeserializeObject(stringResult));*/
            }
            else
            {
                return HttpBadRequest("The provided report doesn't exist");
            }
        }



        [HttpPut]
        [Route("updateCustomReportComment")]
        public IHttpActionResult UpdateCustomReportComment([FromBody]UpdateCustomReportCommentModel model)
        {
            try
            {
                var reportType = _reportService.GetCustomReportById(model.ReportId);
                if (reportType == null)
                {
                    return HttpBadRequest("Provided report doesn't exist");
                }

                reportType.Comment = model.Comment;
                _reportService.UpdateReportType(reportType);

            }
            catch (Exception ex)
            {

                return HttpBadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut]
        [Route("updateCustomReportName")]
        public IHttpActionResult UpdateCustomReportName([FromBody]UpdateCustomReportNameModel model)
        {
            try
            {
                var reportType = _reportService.GetCustomReportById(model.ReportId);
                if (reportType == null)
                {
                    return HttpBadRequest("Provided report doesn't exist");
                }

                reportType.Name = model.Name;
                _reportService.UpdateReportType(reportType);
            }
            catch (Exception ex)
            {

                return HttpBadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPut]
        [Route("updateCustomReport/{reportId}")]
        public IHttpActionResult UpdateCustomReport(int reportId, [FromBody] UpdateCustomReportModel model)
        {
            var reportType = _reportService.GetCustomReportById(reportId);

            if (reportType == null)
            {
                return HttpBadRequest("Provided report doesn't exist");
            }

            var reportSetting = _reportService.GetReportSettingByReportId(reportId);

            if (reportSetting == null)
            {
                return HttpBadRequest("Provided report setting doesn't exist");
            }

            try
            {

                reportType.Name = model.ReportTypeModel.Name;
                reportType.Comment = model.ReportTypeModel.Comment;
                reportType.Created = DateTime.UtcNow;

                if (model.SettingModel.ReportEntityIds1 != null && model.SettingModel.ReportEntityIds1.Any())
                {
                    reportSetting.ReportEntityIds1 = string.Join(",", model.SettingModel.ReportEntityIds1);
                    reportSetting.ReportEntityType1 = model.SettingModel.ReportEntityType1;
                    reportSetting.ReportPeriodType = model.SettingModel.ReportPeriodType;
                    reportSetting.IsAscending = model.SettingModel.IsAscending;
                    reportSetting.OrderVariableId = model.SettingModel.OrderVariableId;

                    if (model.SettingModel.ReportEntityIds2 != null && model.SettingModel.ReportEntityIds2.Any())
                    {
                        reportSetting.ReportEntityType2 = model.SettingModel.ReportEntityType2;
                        reportSetting.ReportEntityIds2 = string.Join(",", model.SettingModel.ReportEntityIds2);
                    }

                    _reportService.UpdateReportSetting(reportSetting);
                }

                _reportService.ClearReportVariables(reportId);
                int order = 1;

                model.ReportTypeModel.VariableIds.ForEach(variable =>
                {
                    var newReportTypeVariable = _reportService.AttachReportVariableType(new ReportVariable()
                    {
                        ReportTypeId = reportType.Id,
                        ReportVariableTypeId = variable,
                        VariableOrder = order
                    });
                    order++;
                });

                return Ok(reportType);
            }
            catch (Exception ex)
            {
                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getCustomReportById/{id}")]
        public IHttpActionResult getCustomreportById(long id)
        {
            var reportType = new Core.Domain.CustomReports.ReportType();
            try
            {
                reportType = _reportService.GetCustomReportById(id);
                if (reportType == null)
                {
                    return HttpBadRequest("Provided report doesn't exist");
                }

                var reportSetting = _reportService.GetReportSettingByReportId(id);

                
                CustomReportSettingModel settingModel = new CustomReportSettingModel();
                if (reportSetting != null)
                {
                    settingModel.ReportEntityIds1 = !string.IsNullOrEmpty(reportSetting.ReportEntityIds1) ? reportSetting.ReportEntityIds1.Split(new char[1] { ',' }).Select(Int64.Parse).ToList() : new List<long>();
                    settingModel.ReportEntityIds2 = !string.IsNullOrEmpty(reportSetting.ReportEntityIds2) ? reportSetting.ReportEntityIds2.Split(new char[1] { ',' }).Select(Int64.Parse).ToList() : new List<long>();
                    settingModel.ReportEntityType1 = reportSetting.ReportEntityType1;
                    settingModel.ReportEntityType2 = reportSetting.ReportEntityType2;
                    settingModel.StartDate = reportSetting.StartDate;
                    settingModel.EndDate = reportSetting.EndDate;
                    settingModel.IsAscending = reportSetting.IsAscending;
                    settingModel.OrderVariableId = reportSetting.OrderVariableId;
                }
                ReportTypeModel reportTypeModel = new ReportTypeModel();
                reportTypeModel.Id = reportType.Id;
                reportTypeModel.Name = reportType.Name;
                reportTypeModel.Comment = reportType.Comment;
                reportTypeModel.VariableIds = new List<long>();

                var userReport = _reportService.GetUserAllReports(_appContext.AppUser.Id).Where(x => x.ReportId == reportType.Id).FirstOrDefault();
                if (userReport != null)
                {
                    reportTypeModel.IsOwner = userReport.IsOwner;
                }
                else
                    reportTypeModel.IsOwner = false;

                var reportVariables = _reportService.GetReportVariables(id);
                foreach (var reportVariable in reportVariables)
                {
                    reportTypeModel.VariableIds.Add(reportVariable.ReportVariableTypeId);
                }

                return Ok(new { reportTypeModel = reportTypeModel, settingModel = settingModel });
            }
            catch (Exception ex)
            {

                return HttpBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getReportVariableTypes")]
        public IHttpActionResult GetReportVariableTypes()
        {
            List<object> list = new List<object>();

            try
            {
                var variables = _reportService.GetReportVariableTypes();
                foreach (var v in variables)
                {
                    list.Add(new { id = v.Id, name = v.Name, GroupName = v.GroupName });
                }

            }
            catch (Exception ex)
            {

                return HttpBadRequest(ex.Message);
            }

            return Ok(list);
        }

        [HttpPost]
        [Route("createCustomReport")]
        public IHttpActionResult CreateCustomReport([FromBody]CreateCustomReportViewModel model)
        {
            var reportType = new Core.Domain.CustomReports.ReportType();
            var reportSetting = new ReportFilterSetting();
            var user = _appContext.AppUser;
            if (model.ReportTypeModel != null)
            {
                reportType = _reportService.AddReportType(new Core.Domain.CustomReports.ReportType()
                {
                    Comment = model.ReportTypeModel.Comment,
                    Name = model.ReportTypeModel.Name,
                    Created = DateTime.UtcNow
                });
                if (reportType != null && reportType.Id != default(int))
                {
                    _reportService.AttachUserReport(new UserReport()
                    {
                        IsOwner = true,
                        ReportId = reportType.Id,
                        UserId = user.Id
                    });
                }
            }
            var order = 1;
            if (model.ReportTypeModel.VariableIds.Any())
            {
                model.ReportTypeModel.VariableIds.ForEach(variable =>
                {
                    /*var newVariableType = _reportService.AddReportVariableType(new ReportVariableType()
                    {
                        Comment = variable.Comment,
                        Name = variable.Name,
                        SqlQuery = variable.SQLQuery
                    });*/
                    var newReportTypeVariable = _reportService.AttachReportVariableType(new ReportVariable()
                    {
                        ReportTypeId = reportType.Id,
                        ReportVariableTypeId = variable,
                        VariableOrder = order
                    });
                    order++;
                });
            }
            if (model.SettingModel != null)
            {
                model.SettingModel = ValidateReportSetting(model.SettingModel);
                reportType = _reportService.GetCustomReportById(reportType.Id);
                if (reportType == null)
                {
                    return HttpBadRequest("Provided report type doesn't exist");
                }
                if (model.SettingModel.ReportEntityIds1 != null && model.SettingModel.ReportEntityIds1.Any())
                {
                    reportSetting = new ReportFilterSetting()
                    {
                        ReportEntityIds1 = string.Join(",", model.SettingModel.ReportEntityIds1),
                        ReportEntityType1 = model.SettingModel.ReportEntityType1,
                        ReportTypeId = reportType.Id,
                        ReportPeriodType = model.SettingModel.ReportPeriodType,
                        ReportEntityIds2 = string.Join(",", model.SettingModel.ReportEntityIds2),
                        ReportEntityType2 = model.SettingModel.ReportEntityType2,
                        IsAscending = model.SettingModel.IsAscending,
                        OrderVariableId = model.SettingModel.OrderVariableId,
                        StartDate = model.SettingModel.StartDate,
                        EndDate = model.SettingModel.EndDate
                    };

                    _reportService.AddReportSetting(reportSetting);
                }
                else
                {
                    return HttpBadRequest("Report setting entity ids can not be empty");
                }

                return Ok(reportType);
            }
            else
            {
                return HttpBadRequest("Report setting can not be null");
            }
        }

        [HttpPost]
        [Route("cloneCustomReport")]
        public IHttpActionResult CloneCustomReport([FromBody]ReportCloneModel model)
        {
            var reportType = new Core.Domain.CustomReports.ReportType();
            var reportSetting = new ReportFilterSetting();
            var reportVariables = new List<long>();
            var user = _appContext.AppUser;

            if (model.SettingModel != null)
            {
                //model.SettingModel = ValidateReportSetting(model.SettingModel);
                reportType = _reportService.GetCustomReportById(model.ReportId);

                if (reportType == null)
                {
                    return HttpBadRequest("Provided report type doesn't exist");
                }

                var curReportSetting = _reportService.GetReportSettingByReportId(reportType.Id);

                if (curReportSetting == null)
                {
                    return HttpBadRequest("Provided report setting doesn't exist");
                }


                if (model.SettingModel.ReportEntityIds1 != null && model.SettingModel.ReportEntityIds1.Any())
                {
                    var clonedReport = _reportService.AddReportType(new Core.Domain.CustomReports.ReportType()
                    {
                        Comment = reportType.Comment,
                        Created = DateTime.UtcNow,
                        Name = model.Name
                    });
                    var reportVariableTypes = _reportService.GetCustomReportVariables(model.ReportId);

                    if (reportVariableTypes != null && reportVariableTypes.Any())
                    {
                        var order = 1;
                        reportVariables.ForEach(variable =>
                        {
                            var newReportTypeVariable = _reportService.AttachReportVariableType(new ReportVariable()
                            {
                                ReportTypeId = clonedReport.Id,
                                ReportVariableTypeId = variable,
                                VariableOrder = order
                            });
                            order++;
                        });
                    }

                    reportSetting = new ReportFilterSetting()
                    {
                        ReportEntityIds1 = string.Join(",", model.SettingModel.ReportEntityIds1),
                        ReportEntityType1 = model.SettingModel.ReportEntityType1,
                        ReportTypeId = clonedReport.Id,
                        ReportPeriodType = model.SettingModel.ReportPeriodType,
                        ReportEntityIds2 = string.Join(",", model.SettingModel.ReportEntityIds2),
                        ReportEntityType2 = model.SettingModel.ReportEntityType2,
                        IsAscending = curReportSetting.IsAscending,
                        OrderVariableId = curReportSetting.OrderVariableId,
                        StartDate = model.SettingModel.StartDate,
                        EndDate = model.SettingModel.EndDate
                    };

                    _reportService.AddReportSetting(reportSetting);

                    _reportService.AttachUserReport(new UserReport()
                    {
                        IsOwner = true,
                        ReportId = clonedReport.Id,
                        UserId = user.Id
                    });

                    return Ok(clonedReport);
                }
                else
                {
                    return HttpBadRequest("Report setting entity ids can not be empty");
                }
            }
            else
            {
                return HttpBadRequest("Report setting can not be null");
            }
        }

        [HttpPost]
        [Route("share")]
        public IHttpActionResult ShareCustomReport(ShareCustomReportModel shareModel)
        {
            var userReports = _reportService.GetUserAllReports(_appContext.AppUser.Id);
            if (userReports != null && userReports.Any())
            {
                var userReportsId = userReports.Where(x => x.IsOwner).Select(x => x.ReportId);
                if (userReportsId.Contains(shareModel.ReportId))
                {
                    if (shareModel.UserIds.Any())
                    {
                        shareModel.UserIds.ForEach(x =>
                        {
                            try
                            {
                                _reportService.AttachUserReport(new UserReport()
                                {
                                    IsOwner = false,
                                    ReportId = shareModel.ReportId,
                                    UserId = x
                                });
                            }
                            catch (Exception ex)
                            { }

                        });
                    }
                    else
                    {
                        return HttpBadRequest("There are no provided users");
                    }
                }
                else
                {
                    return HttpBadRequest("You are not owned to perform this action");
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("unshare")]
        public IHttpActionResult UnShareCustomReport(ShareCustomReportModel shareModel)
        {
            var userReports = _reportService.GetUserAllReports(_appContext.AppUser.Id);
            if (userReports != null && userReports.Any())
            {
                var userReportsId = userReports.Where(x => x.IsOwner).Select(x => x.ReportId);
                if (userReportsId.Contains(shareModel.ReportId))
                {
                    if (shareModel.UserIds.Any())
                    {
                        shareModel.UserIds.ForEach(x =>
                        {
                            try
                            {
                                _reportService.DetachUserReport(new UserReport()
                                {
                                    IsOwner = false,
                                    ReportId = shareModel.ReportId,
                                    UserId = x
                                });
                            }
                            catch (Exception ex)
                            { }

                        });
                    }
                    else
                    {
                        return HttpBadRequest("There are no provided users");
                    }
                }
                else
                {
                    return HttpBadRequest("You are not owned to perform this action");
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("getCustomReports")]
        public IHttpActionResult GetCustomReports()
        {
            var result = new List<UserReportModel>();
            var customReports = _reportService.GetAllCustomReports();
            var userReports = _reportService.GetUserAllReports(_appContext.AppUser.Id);
            if (userReports != null && userReports.Any())
            {
                foreach(var userReport in userReports)
                {
                    var report = _reportService.GetCustomReportById(userReport.ReportId);
                    if (report != null)
                    {
                        string owner = "";

                        if (userReport.IsOwner)
                        {
                            var user = _userService.GetUserById(userReport.UserId);
                            if (user != null)
                            {
                                owner = user.Email;
                            }
                        }
                        else
                        {
                            var ownerReport = _reportService.GetUserAllReports(userReport.ReportId, true).FirstOrDefault();
                            if (ownerReport != null)
                            {
                                var user = _userService.GetUserById(ownerReport.UserId);
                                if (user != null)
                                {
                                    owner = user.Email;
                                }
                            }
                        }

                        result.Add(new UserReportModel()
                        {
                            Id = report.Id,
                            Comment = report.Comment,
                            IsOwner = userReport.IsOwner,
                            Name = report.Name,
                            CreatedAt = report.Created,
                            Owner = owner
                        });
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("getAllCustomReports")]
        public IHttpActionResult getAllCustomReports()
        {
            var customReports = _reportService.GetAllCustomReports();

            return Ok(customReports);
        }


        [HttpPost]
        [Route("createCustomReportSetting")]
        public IHttpActionResult CreateCustomReportSetting([FromBody]CustomReportSettingModel model)
        {
            var result = (long)0;
            if (model != null)
            {
                model = ValidateReportSetting(model);
                var reportType = _reportService.GetCustomReportById(model.ReportTypeId);

                if (reportType == null)
                {
                    return HttpBadRequest("Provided report type doesn't exist");
                }
                else if (_reportService.GetUserAllReports(_appContext.AppUser.Id).FirstOrDefault(x => x.IsOwner && x.ReportId == reportType.Id) == null)
                {
                    return HttpBadRequest("You are not authorized to add setting for this report");
                }
                if (model.ReportEntityIds1 != null && model.ReportEntityIds1.Any())
                {
                    var reportSetting = new ReportFilterSetting()
                    {
                        ReportEntityIds1 = string.Join(",", model.ReportEntityIds1),
                        ReportEntityType1 = model.ReportEntityType1,
                        ReportTypeId = model.ReportTypeId,
                        ReportPeriodType = model.ReportPeriodType,
                        IsAscending = model.IsAscending,
                        OrderVariableId = model.OrderVariableId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate
                    };
                    if (model.ReportEntityIds2 != null && model.ReportEntityIds2.Any())
                    {
                        reportSetting.ReportEntityType2 = model.ReportEntityType2;
                        reportSetting.ReportEntityIds2 = string.Join(",", model.ReportEntityIds2);
                    }

                    result = _reportService.AddReportSetting(reportSetting);
                }
                else
                {
                    return HttpBadRequest("Report setting entity ids can not be empty");
                }

                return Ok(result);
            }
            else
            {
                return HttpBadRequest("Report setting can not be null");
            }
        }

        [HttpPut]
        [Route("updateCustomReportSetting")]
        public IHttpActionResult UpdateCustomReportSetting([FromBody]CustomReportSettingModel model)
        {
            if (model != null)
            {
                model = ValidateReportSetting(model);
                var reportType = _reportService.GetCustomReportById(model.ReportTypeId);
                if (reportType == null)
                {
                    return HttpBadRequest("Provided report type doesn't exist");
                }
                else if (_reportService.GetUserAllReports(_appContext.AppUser.Id).FirstOrDefault(x => x.IsOwner && x.ReportId == reportType.Id) == null)
                {
                    return HttpBadRequest("You are not authorized to update setting for this report");
                }
                var reportSetting = _reportService.GetReportSettingById(model.SettingId);
                if (reportSetting == null)
                {
                    return HttpBadRequest("Provided report setting doesn't exist");
                }
                if (model.ReportEntityIds1 != null && model.ReportEntityIds1.Any())
                {
                    reportSetting.ReportEntityIds1 = string.Join(",", model.ReportEntityIds1);
                    reportSetting.ReportEntityType1 = model.ReportEntityType1;
                    reportSetting.ReportTypeId = model.ReportTypeId;
                    reportSetting.ReportPeriodType = model.ReportPeriodType;
                    reportSetting.IsAscending = model.IsAscending;
                    reportSetting.OrderVariableId = model.OrderVariableId;

                    if (model.ReportEntityIds2 != null && model.ReportEntityIds2.Any())
                    {
                        reportSetting.ReportEntityType2 = model.ReportEntityType2;
                        reportSetting.ReportEntityIds2 = string.Join(",", model.ReportEntityIds2);
                    }

                    _reportService.UpdateReportSetting(reportSetting);
                }
                else
                {
                    return HttpBadRequest("Report setting entity ids can not be empty");
                }

                return Ok();
            }
            else
            {
                return HttpBadRequest("Report setting can not be null");
            }
        }

        [HttpPut]
        [Route("updateCustomReportVariables/{reportId}")]
        public IHttpActionResult UpdateCustomReportVariables(int reportId, [FromBody] List<long> variableIds)
        {
            var reportType = _reportService.GetCustomReportById(reportId);
            if (reportType == null)
            {
                return HttpBadRequest("Provided report type doesn't exist");
            }

            _reportService.ClearReportVariables(reportId);
            int order = 1;

            variableIds.ForEach(variable =>
            {
                var newReportTypeVariable = _reportService.AttachReportVariableType(new ReportVariable()
                {
                    ReportTypeId = reportType.Id,
                    ReportVariableTypeId = variable,
                    VariableOrder = order
                });
                order++;
            });

            return Ok();
        }

        [HttpDelete]
        [Route("deleteCustomReportSetting/{reportSettingId}")]
        public IHttpActionResult DeleteCustomReportSetting(long reportSettingId)
        {
            if (reportSettingId != 0)
            {
                var reportSetting = _reportService.GetReportSettingById(reportSettingId);
                if (reportSetting == null)
                {
                    return HttpBadRequest("Provided report setting doesn't exist");
                }
                _reportService.DeleteReportSettingById(reportSettingId);

                return Ok();
            }
            else
            {
                return HttpBadRequest("Report setting can not be null");
            }
        }


        [HttpDelete]
        [Route("deleteCustomReport/{reportId}")]
        public IHttpActionResult DeleteCustomReport(long reportId)
        {
            var customReport = _reportService.GetCustomReportById(reportId);
            if (customReport == null)
            {
                return HttpBadRequest("Provided report doesn't exist");
            }

            _reportService.DeleteCustomReport(reportId);

            return Ok();
        }

        [HttpGet]
        [Route("getAllCustomReportSettings")]
        public IHttpActionResult GetAllCustomReportSettings()
        {
            var result = new List<CustomReportSettingModel>();
            var reportSettings = _reportService.GetAllReportSettings();
            if (reportSettings != null && reportSettings.Any())
            {
                reportSettings.ForEach(rs =>
                {
                    var entityIds = rs.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();
                    result.Add(new CustomReportSettingModel()
                    {
                        ReportEntityIds1 = entityIds,
                        ReportEntityType1 = rs.ReportEntityType1,
                        SettingId = rs.Id,
                        ReportTypeId = rs.ReportTypeId
                    });
                });
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("getCustomReportSetting/{id}")]
        public IHttpActionResult GetCustomReportSetting(long id)
        {
            var result = new CustomReportSettingModel();
            var reportSetting = _reportService.GetReportSettingByReportId(id);
            if (reportSetting == null)
            {
                return HttpBadRequest("Provided report setting doesn't exist");
            }
            List<long> entityIds1 = new List<long>();
            if (!string.IsNullOrEmpty(reportSetting.ReportEntityIds1))
                entityIds1 = reportSetting.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();
            List<long> entityIds2 = new List<long>();
            if (!string.IsNullOrEmpty(reportSetting.ReportEntityIds2))
                entityIds2 = reportSetting.ReportEntityIds2.Split(',').Select(x => Convert.ToInt64(x)).ToList();

            result = new CustomReportSettingModel()
            {
                ReportEntityIds1 = entityIds1,
                ReportEntityType1 = reportSetting.ReportEntityType1,
                ReportEntityIds2 = entityIds2,
                ReportEntityType2 = reportSetting.ReportEntityType2.HasValue ? reportSetting.ReportEntityType2.Value : ReportEntityType.None,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                IsAscending = reportSetting.IsAscending,
                OrderVariableId = reportSetting.OrderVariableId,
                ReportPeriodType = reportSetting.ReportPeriodType,
                SettingId = reportSetting.Id,
                ReportTypeId = reportSetting.ReportTypeId
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("getCustomReportSettingsByTypeId/{reportTypeId}")]
        public IHttpActionResult GetCustomReportByTypeIdSettings(long reportTypeId)
        {
            var reportType = _reportService.GetCustomReportById(reportTypeId);
            if (reportType == null)
            {
                return HttpBadRequest("Provided report doesn't exist");
            }
            var result = new List<CustomReportSettingModel>();
            var reportSettings = _reportService.GetAllReportSettingByReportType(reportTypeId);
            if (reportSettings != null && reportSettings.Any())
            {
                reportSettings.ForEach(rs =>
                {
                    var entityIds = rs.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();
                    var entityIds2 = !string.IsNullOrEmpty(rs.ReportEntityIds2) ? rs.ReportEntityIds2.Split(',').Select(x => Convert.ToInt64(x)).ToList() : null;
                    var dateRange = GenerateDateRangeByReportPeriodType(rs, 0);
                    result.Add(new CustomReportSettingModel()
                    {
                        ReportEntityIds1 = entityIds,
                        ReportEntityType1 = rs.ReportEntityType1,
                        SettingId = rs.Id,
                        ReportTypeId = rs.ReportTypeId,
                        IsAscending = rs.IsAscending,
                        OrderVariableId = rs.OrderVariableId,
                        ReportEntityIds2 = entityIds2,
                        ReportPeriodType = rs.ReportPeriodType,
                        ReportEntityType2 = rs.ReportEntityType2.HasValue ? rs.ReportEntityType2 : null,
                        StartDate = dateRange.Item1,
                        EndDate = dateRange.Item2
                    });
                });
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("createCustomReportType")]
        public IHttpActionResult CreateCustomReportType([FromBody]ReportVariableTypeModel model)
        {
            var result = new ReportVariableType();
            if (model != null)
            {
                var reportVariableType = new ReportVariableType()
                {
                    Comment = model.Comment,
                    Name = model.Name
                };

                result = _reportService.AddReportVariableType(reportVariableType);

                return Ok(result);
            }
            else
            {
                return HttpBadRequest("Report setting can not be null");
            }
        }

        [HttpPut]
        [Route("updateCustomReporVariableType")]
        public IHttpActionResult UpdateCustomReporVariableType([FromBody]ReportVariableTypeModel model)
        {
            try
            {
                var reportType = _reportService.GetReportVariableType(model.Id);
                if (reportType == null)
                {
                    return HttpBadRequest("Provided report doesn't exist");
                }

                reportType.Comment = model.Comment;
                reportType.Name = model.Name;

                _reportService.UpdateReportVariableType(reportType);

            }
            catch (Exception ex)
            {

                return HttpBadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpDelete]
        [Route("deleteCustomReportVariableType/{id}")]
        public IHttpActionResult deleteCustomReporVariableType(long id)
        {
            try
            {
                var reportType = _reportService.GetReportVariableType(id);
                if (reportType == null)
                {
                    return HttpBadRequest("Provided report doesn't exist");
                }

                _reportService.DeleteReportVariableType(reportType.Id);

            }
            catch (Exception ex)
            {

                return HttpBadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("attachDetachReportVariableType")]
        public IHttpActionResult AttachDetachReportVariableType([FromBody]ReportVariableModel model)
        {
            var result = new ReportVariable();
            if (model != null)
            {
                if (model.isAttached)
                {
                    var reportVariable = new ReportVariable()
                    {
                        ReportTypeId = model.ReportTypeId,
                        ReportVariableTypeId = model.ReportVariableTypeId,
                        VariableOrder = 0,
                    };

                    result = _reportService.AttachReportVariableType(reportVariable);
                    return Ok(result);
                }
                else if (!model.isAttached)
                {
                    var reportVariable = new ReportVariable()
                    {
                        Id = model.Id,
                        ReportTypeId = model.ReportTypeId,
                        ReportVariableTypeId = model.ReportVariableTypeId,
                        VariableOrder = 0,
                    };
                    _reportService.DetachReportVariableType(reportVariable);

                }
                return Ok();
            }
            else
            {
                return HttpBadRequest("Report setting can not be null");
            }
        }

        /*
        [HttpGet]
        [Route("getReportsViewed")]
        public IHttpActionResult GetReportsViewed()
        {
            try
            {
                var reportsViewed = _reportService.GetReportsViewed();

                return Ok(reportsViewed);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }
        */

        [HttpGet]
        [Route("getRecentlyViewedReports")]
        public IHttpActionResult GetRecentlyViewedReports()
        {
            try
            {
                List<object> list = new List<object>();

                var userId = _appContext.AppUser.Id;
                var reportsViewed = _reportService.GetReportsViewedByUserId(userId).Where(x => !x.CustomReportTypeId.HasValue).ToList(); ;

                if (reportsViewed != null)
                {
                    foreach (var item in reportsViewed)
                        list.Add(new { reportName = item.ReportName, reportDate = item.ViewDate });
                }

                return Ok(list);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("getRecentlyViewedCustomReports")]
        public IHttpActionResult GetRecentlyViewedCustomReports()
        {
            try
            {
                List<object> list = new List<object>();

                var userId = _appContext.AppUser.Id;
                var reportsViewed = _reportService.GetReportsViewedByUserId(userId).Where(x => x.CustomReportTypeId.HasValue).ToList();

                if (reportsViewed != null)
                {
                    foreach (var item in reportsViewed)
                    {
                        var customReport = _reportService.GetCustomReportById(item.CustomReportTypeId.Value);
                        if (item.CustomReportTypeId.HasValue && customReport == null) continue;

                        if (_reportService.GetReportSettingByReportId(customReport.Id) == null) continue;

                        list.Add(new { reportId = item.CustomReportTypeId.Value, reportName = item.ReportName, reportDate = item.ViewDate });
                        
                    }
                }
                return Ok(list);
            }
            catch (Exception exception)
            {
                return HttpBadRequest(exception.Message);
            }
        }

        #endregion

        #region PrivateMethods

        private DataTable jsonStringToTable(string jsonContent)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            return dt;
        }

        private void generateCSV(DataTable data)
        {
            var result = string.Empty;
            var columns = string.Empty;
            var rows = string.Empty;

            int colNum = 0;
            int rowNum = 0;
            using (var dt = data)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    columns += column.ColumnName;
                    if ((colNum + 1) < dt.Columns.Count)
                    {
                        colNum++;
                        columns += ",";
                    }
                }

                ReportCSVGenerator.Instance.Init(columns);
                foreach (DataRow row in dt.Rows)
                {
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {

                        rows += row[i].ToString().Replace(",", " ");
                        if ((rowNum + 1) < dt.Columns.Count)
                        {
                            rowNum++;
                            rows += ",";
                        }
                    }

                    ReportCSVGenerator.Instance.AddRow(rows);
                    rows = string.Empty;
                    rowNum = 0;
                }

            }

        }

        //private string jsonToCSV(string jsonContent, string delimiter)
        //{
        //    StringWriter csvString = new StringWriter();
        //    using (var csv = new CsvWriter(csvString))
        //    {
        //        csv.Configuration.SkipEmptyRecords = true;
        //        csv.Configuration.WillThrowOnMissingField = false;
        //        csv.Configuration.Delimiter = delimiter;

        //        using (var dt = jsonStringToTable(jsonContent))
        //        {
        //            foreach (DataColumn column in dt.Columns)
        //            {
        //                csv.WriteField(column.ColumnName);
        //            }
        //            csv.NextRecord();

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                for (var i = 0; i < dt.Columns.Count; i++)
        //                {
        //                    csv.WriteField(row[i]);
        //                }
        //                csv.NextRecord();
        //            }
        //        }
        //    }
        //    return csvString.ToString();
        //}

        private Tuple<DateTime, DateTime> GenerateDateRangeByReportPeriodType(ReportFilterSetting setting, byte timeZone)
        {
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var result = new Tuple<DateTime, DateTime>(startDate, endDate);

            switch (setting.ReportPeriodType)
            {
                case ReportPeriodType.Custom:
                    startDate = new DateTime(setting.StartDate.Year, setting.StartDate.Month, setting.StartDate.Day, 0, 0, 0);
                    endDate = new DateTime(setting.EndDate.Year, setting.EndDate.Month, setting.EndDate.Day, 23, 59, 59);
                    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                    break;
                case ReportPeriodType.Recurring:
                    startDate = new DateTime(setting.StartDate.Year, setting.StartDate.Month, setting.StartDate.Day, 0, 0, 0);
                    if (setting.EndDate < DateTime.Now)
                    {
                        endDate = new DateTime(DateTime.Now.Year + 1, setting.EndDate.Month, setting.EndDate.Day, 23, 59, 59);
                    }
                    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                    break;
                //case ReportPeriodType.LastWeek:
                //    DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever
                //    DateTime startingDate = DateTime.Today;

                //    while (startingDate.DayOfWeek != weekStart)
                //        startingDate = startingDate.AddDays(-1);

                //    startDate = startingDate.AddDays(-7);
                //    endDate = startingDate.AddDays(-1);
                //    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                //    break;
                //case ReportPeriodType.PreviosMonth:
                //    var today = DateTime.Today;
                //    var month = new DateTime(today.Year, today.Month, 1);
                //    startDate = month.AddMonths(-1);
                //    endDate = month.AddDays(-1);
                //    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                //    break;
                //case ReportPeriodType.PreviousYear:
                //    var curYear = DateTime.Now.Year;
                //    startDate = new DateTime(curYear - 1, 1, 1, 0, 0, 0);
                //    endDate = new DateTime(curYear - 1, 12, 31, 0, 0, 0);
                //    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                //    break;
                //case ReportPeriodType.TwoYearsLater:
                //    var currentYear = DateTime.Now.Year;
                //    startDate = new DateTime(currentYear - 2, 1, 1, 0, 0, 0);
                //    endDate = new DateTime(currentYear - 2, 12, 31, 0, 0, 0);
                //    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                //    break;
                //case ReportPeriodType.ThreeYearsLater:
                //    var thisYear = DateTime.Now.Year;
                //    startDate = new DateTime(thisYear - 3, 1, 1, 0, 0, 0);
                //    endDate = new DateTime(thisYear - 3, 12, 31, 0, 0, 0);
                //    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                //    break;
                //case ReportPeriodType.TwoYearsLaterTillNow:
                //    var year = DateTime.Now.Year;
                //    startDate = new DateTime(year - 2, 1, 1, 0, 0, 0);
                //    endDate = new DateTime(year, 12, 31, 0, 0, 0);
                //    result = new Tuple<DateTime, DateTime>(startDate, endDate);
                //    break;
                default:
                    break;
            }
            return result;
        }
        private CustomReportSettingModel ValidateReportSetting(CustomReportSettingModel model)
        {
            if (model.ReportEntityIds1 != null && model.ReportEntityIds1.Any())
            {
                #region ReportEntityType1
                switch (model.ReportEntityType1)
                {
                    case ReportEntityType.Campaign:
                        var campaignIds = _campaignService.GetAllCampaigns().Select(x => x.Id);
                        model.ReportEntityIds1 = campaignIds.Intersect(model.ReportEntityIds1).ToList();
                        break;
                    case ReportEntityType.Affiliate:
                        var affiliateIds = _affiliateService.GetAllAffiliates().Select(x => x.Id);
                        model.ReportEntityIds1 = affiliateIds.Intersect(model.ReportEntityIds1).ToList();
                        break;
                    case ReportEntityType.Buyer:
                        var buyerIds = _buyerService.GetAllBuyers().Select(x => x.Id);
                        model.ReportEntityIds1 = buyerIds.Intersect(model.ReportEntityIds1).ToList();
                        break;
                    case ReportEntityType.BuyerChannel:
                        var buyerChannelIds = _buyerChannelService.GetAllBuyerChannels().Select(x => x.Id);
                        model.ReportEntityIds1 = buyerChannelIds.Intersect(model.ReportEntityIds1).ToList();
                        break;
                    case ReportEntityType.AffiliateChannel:
                        var affiliateChannelIds = _affiliateChannelService.GetAllAffiliateChannels().Select(x => x.Id);
                        model.ReportEntityIds1 = affiliateChannelIds.Intersect(model.ReportEntityIds1).ToList();
                        break;
                    default:
                        break;
                }
                #endregion
                #region ReportEntityType2
                switch (model.ReportEntityType2)
                {
                    case ReportEntityType.Campaign:
                        var campaignIds = _campaignService.GetAllCampaigns().Select(x => x.Id);
                        model.ReportEntityIds2 = campaignIds.Intersect(model.ReportEntityIds2).ToList();
                        break;
                    case ReportEntityType.Affiliate:
                        var affiliateIds = _affiliateService.GetAllAffiliates().Select(x => x.Id);
                        model.ReportEntityIds2 = affiliateIds.Intersect(model.ReportEntityIds2).ToList();
                        break;
                    case ReportEntityType.Buyer:
                        var buyerIds = _buyerService.GetAllBuyers().Select(x => x.Id);
                        model.ReportEntityIds2 = buyerIds.Intersect(model.ReportEntityIds2).ToList();
                        break;
                    case ReportEntityType.BuyerChannel:
                        var buyerChannelIds = _buyerChannelService.GetAllBuyerChannels().Select(x => x.Id);
                        model.ReportEntityIds2 = buyerChannelIds.Intersect(model.ReportEntityIds2).ToList();
                        break;
                    case ReportEntityType.AffiliateChannel:
                        var affiliateChannelIds = _affiliateChannelService.GetAllAffiliateChannels().Select(x => x.Id);
                        model.ReportEntityIds2 = affiliateChannelIds.Intersect(model.ReportEntityIds2).ToList();
                        break;
                    default:
                        break;
                }
                #endregion
                if (model.ReportEntityIds1.Count() == 0 && model.ReportEntityIds2.Count() == 0)
                {
                    throw new Exception("Report setting entity ids are invalid");
                }
            }
            return model;
        }

        private CustomReportModel GetAffiliateAndBuyerChannelsReportSetting(ReportFilterSetting model)
        {
            var result = new CustomReportModel();
            if (model.ReportEntityIds1 != null && model.ReportEntityIds1.Any())
            {
                var ids = new List<long>();
                #region ReportEntityType1
                switch (model.ReportEntityType1)
                {
                    case ReportEntityType.Campaign:
                        var campaignIds = _campaignService.GetAllCampaigns().Select(x => x.Id);

                        ids = model.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByMultipleCampaignId(ids).Select(x => x.Id).ToList();
                        if (affiliateChannels != null && affiliateChannels.Any())
                        {
                            result.AffiliateChannelIds = affiliateChannels;
                        }
                        break;
                    case ReportEntityType.Affiliate:
                        var affiliateIds = _affiliateService.GetAllAffiliates().Select(x => x.Id);
                        ids = model.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        var affiliatech = _affiliateChannelService.GetAllAffiliateChannelsByMultipleAffiliateIds(ids).Select(x => x.Id).ToList();
                        if (affiliatech != null && affiliatech.Any())
                        {
                            result.AffiliateChannelIds = affiliatech;
                        }
                        break;
                    case ReportEntityType.Buyer:
                        var buyerIds = _buyerService.GetAllBuyers().Select(x => x.Id);
                        ids = model.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        var buyerCh = _buyerChannelService.GetAllBuyerChannelsByMultipleBuyerId(ids).Select(x => x.Id).ToList();
                        if (buyerCh != null && buyerCh.Any())
                        {
                            result.BuyerChannelIds = buyerCh;
                        }
                        break;
                    case ReportEntityType.BuyerChannel:
                        var buyerChannelIds = _buyerChannelService.GetAllBuyerChannels().Select(x => x.Id);
                        // ids = model.ReportEntityIds.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        result.BuyerChannelIds = model.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();
                        break;
                    case ReportEntityType.AffiliateChannel:
                        var affiliateChannelIds = _affiliateChannelService.GetAllAffiliateChannels().Select(x => x.Id);
                        //model.ReportEntityIds = affiliateChannelIds.Intersect(model.ReportEntityIds).ToList();

                        result.AffiliateChannelIds = model.ReportEntityIds1.Split(',').Select(x => Convert.ToInt64(x)).ToList();
                        break;
                    default:
                        break;
                }
                #endregion

                #region ReportEntityType2
                switch (model.ReportEntityType2)
                {
                    case ReportEntityType.Campaign:
                        var campaignIds = _campaignService.GetAllCampaigns().Select(x => x.Id);

                        ids = model.ReportEntityIds2.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByMultipleCampaignId(ids).Select(x => x.Id).ToList();
                        if (affiliateChannels != null && affiliateChannels.Any())
                        {
                            result.AffiliateChannelIds = affiliateChannels;
                        }
                        break;
                    case ReportEntityType.Affiliate:
                        var affiliateIds = _affiliateService.GetAllAffiliates().Select(x => x.Id);
                        ids = model.ReportEntityIds2.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        var affiliatech = _affiliateChannelService.GetAllAffiliateChannelsByMultipleAffiliateIds(ids).Select(x => x.Id).ToList();
                        if (affiliatech != null && affiliatech.Any())
                        {
                            result.AffiliateChannelIds = affiliatech;
                        }
                        break;
                    case ReportEntityType.Buyer:
                        var buyerIds = _buyerService.GetAllBuyers().Select(x => x.Id);
                        ids = model.ReportEntityIds2.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        var buyerCh = _buyerChannelService.GetAllBuyerChannelsByMultipleBuyerId(ids).Select(x => x.Id).ToList();
                        if (buyerCh != null && buyerCh.Any())
                        {
                            result.BuyerChannelIds = buyerCh;
                        }
                        break;
                    case ReportEntityType.BuyerChannel:
                        var buyerChannelIds = _buyerChannelService.GetAllBuyerChannels().Select(x => x.Id);
                        // ids = model.ReportEntityIds.Split(',').Select(x => Convert.ToInt64(x)).ToList();

                        result.BuyerChannelIds = model.ReportEntityIds2.Split(',').Select(x => Convert.ToInt64(x)).ToList();
                        break;
                    case ReportEntityType.AffiliateChannel:
                        var affiliateChannelIds = _affiliateChannelService.GetAllAffiliateChannels().Select(x => x.Id);
                        //model.ReportEntityIds = affiliateChannelIds.Intersect(model.ReportEntityIds).ToList();

                        result.AffiliateChannelIds = model.ReportEntityIds2.Split(',').Select(x => Convert.ToInt64(x)).ToList();
                        break;
                    default:
                        break;
                }
                #endregion

                if (model.ReportEntityIds1.Count() == 0 && model.ReportEntityIds2.Count() == 0)
                {
                    throw new Exception("Report setting entity ids are invalid");
                }
            }
            return result;
        }

        private CustomReportModel GetAffiliateAndBuyerChannelsFromModel(CustomReportModel model)
        {

            if (model.BuyerIds != null && model.BuyerIds.Any())
            {
                var buyerIds = model.BuyerIds;
                var buyerChannels = _buyerChannelService.GetAllBuyerChannelsByMultipleBuyerId(buyerIds).Select(x => x.Id).ToList();
                if (buyerChannels != null && buyerChannels.Any())
                {
                    model.BuyerChannelIds.AddRange(buyerChannels);
                }
            }
            if (model.CampaignIds != null && model.CampaignIds.Any())
            {
                var campaigns = model.CampaignIds;
                var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByMultipleCampaignId(campaigns).Select(x => x.Id).ToList();
                if (affiliateChannels != null && affiliateChannels.Any())
                {
                    model.AffiliateChannelIds.AddRange(affiliateChannels);
                }
            }
            if (model.AffiliateIds != null && model.AffiliateIds.Any())
            {
                var affiliates = model.AffiliateIds;
                var affiliateChannels = _affiliateChannelService.GetAllAffiliateChannelsByMultipleAffiliateIds(affiliates).Select(x => x.Id).ToList();
                if (affiliateChannels != null && affiliateChannels.Any())
                {
                    model.AffiliateChannelIds.AddRange(affiliateChannels);
                }
            }
            return model;
        }

        private List<BuyersReportByPricesModel> GetReportBuyersByPricesData(BuyersReportByPricesFilterModel model)
        {
            var dateFrom = _settingService.GetUTCDate(model.StartDate);
            var dateTo = _settingService.GetUTCDate(model.EndDate);
            var reportByPrices = _reportService.ReportBuyersByPrices(dateFrom,
                                                                                        dateTo,
                                                                                        "",
                                                                                        string.Join(",", model.BuyerChannelIds),
                                                                                        string.Join(",", model.CampaignIds),
                                                                                        model.Price1,
                                                                                        model.Price2);
            var buyers = new Dictionary<string, BuyersReportByPricesModel>();

            BuyersReportByPricesModel item = null;
            BuyersReportByPricesModel prevItem = null;

            foreach (var reportRow in reportByPrices)
            {
                item = new BuyersReportByPricesModel();

                if (buyers.ContainsKey(reportRow.BuyerChannelId + "-" + reportRow.BuyerPrice)) item = buyers[reportRow.BuyerChannelId + "-" + reportRow.BuyerPrice];
                else
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(
                            $"{prevItem.Title},{prevItem.BuyerPrice},{prevItem.TotalLeads},{prevItem.UniqueLeads}");
                    }
                    item.Title = reportRow.BuyerChannelName;
                    item.BuyerPrice = Math.Round(reportRow.BuyerPrice, 1);
                    item.TotalLeads = item.UniqueLeads = item.SoldLeads = 0;
                    buyers.Add(reportRow.BuyerChannelId + "-" + reportRow.BuyerPrice, item);
                }

                if (reportRow.Status == 1)
                    item.SoldLeads += reportRow.Quantity;
                item.TotalLeads += reportRow.Quantity;
                item.UniqueLeads += reportRow.UQuantity;

                prevItem = item;
            }

            if (prevItem != null)
                ReportCSVGenerator.Instance.AddRow(
                    $"{prevItem.Title},{prevItem.BuyerPrice},{prevItem.TotalLeads},{prevItem.UniqueLeads}");

            var items = buyers.Select(d => d.Value).ToList();

            return items;
        }

        protected List<BuyersReportByHourModel> GetReportBuyersByHourData(DateTime date1, DateTime date2, DateTime date3, string buyerChannelIds, string campaignIds)
        {
            var buyers = new List<BuyersReportByHourModel>();

            var totals = new BuyersReportByHourModel
            {
                Title = "Total",
                TotalLeads1 = 0,
                SoldLeads1 = 0,
                TotalLeads2 = 0,
                SoldLeads2 = 0,
                TotalLeads3 = 0
            };

            BuyersReportByHourModel item = null;
            BuyersReportByHourModel prevItem = null;
            var hours = new Dictionary<int, BuyersReportByHourModel>();
            for (int i = 0; i <= 23; i++)
            {
                string t = (i.ToString().Length < 2 ? "0" + i.ToString() : i.ToString()) + ":00";
                item = new BuyersReportByHourModel() { Title = t, SoldLeads1 = 0, TotalLeads1 = 0, SoldLeads2 = 0, TotalLeads2 = 0, SoldLeads3 = 0, TotalLeads3 = 0 };
                hours.Add(i, item);
                buyers.Add(item);
            }

            var report = _reportService.BuyerReportByHour(date1, (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            foreach (var r in report)
            {
                var b = hours[r.Hour];

                var t = (r.Hour.ToString().Length < 2 ? "0" + r.Hour.ToString() : r.Hour.ToString()) + ":00";

                b.Title = t;

                b.TotalLeads1 += r.TotalLeads;
                b.SoldLeads1 += r.SoldLeads;

                totals.TotalLeads1 += r.TotalLeads;
                totals.SoldLeads1 += r.SoldLeads;

                prevItem = item;
            }

            report = _reportService.BuyerReportByHour(date2, (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));
            foreach (BuyerReportByHour r in report)
            {
                var b = hours[r.Hour];

                var t = (r.Hour.ToString().Length < 2 ? "0" + r.Hour.ToString() : r.Hour.ToString()) + ":00";

                b.Title = t;
                b.TotalLeads2 += r.TotalLeads;
                b.SoldLeads2 += r.SoldLeads;

                totals.TotalLeads2 += r.TotalLeads;
                totals.SoldLeads2 += r.SoldLeads;

                prevItem = item;
            }

            report = _reportService.BuyerReportByHour(date3, (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));
            foreach (var r in report)
            {
                var b = hours[r.Hour];

                var t = (r.Hour.ToString().Length < 2 ? "0" + r.Hour.ToString() : r.Hour.ToString()) + ":00";

                b.Title = t;
                b.TotalLeads3 += r.TotalLeads;
                b.SoldLeads3 += r.SoldLeads;

                totals.TotalLeads3 += r.TotalLeads;
                totals.SoldLeads3 += r.SoldLeads;

                prevItem = item;
            }

            buyers.Add(totals);

            for (int i = 0; i <= 23; i++)
            {
                string t = (i.ToString().Length < 2 ? "0" + i.ToString() : i.ToString()) + ":00";
                item = hours[i];
                ReportCSVGenerator.Instance.AddRow($"{t},{item.TotalLeads1},{item.SoldLeads1},{item.TotalLeads2},{item.SoldLeads2},{item.TotalLeads3},{item.SoldLeads3}");
            }

            return buyers;
        }

        private List<ErrorLeadsReportResponseModel> GetErrorLeadsReportData(DateTime dateFrom,
           DateTime dateTo,
           List<long> affiliateChannelIds,
           List<long> buyerChannelIds,
           List<long> campaignIds,
           long leadId,
           short statusId,
           string description,
           short errorTypeId,
           List<string> state,
           short reportType,
           int page,
           int count)
        {
            var errorLeadsModels = new List<ErrorLeadsReportResponseModel>();

            var errorLeads = _leadMainService.GetErrorLeadsReport(errorTypeId,
                0,
                dateFrom,
                dateTo,
                leadId,
                statusId,
                new List<long>(),
                affiliateChannelIds,
                new List<long>(),
                buyerChannelIds,
                campaignIds,
                state,
                0,
                reportType,
                (page - 1) * count,
                count);

            foreach (var error in errorLeads)
            {

                var affiliateName = error.AffiliateName;
                var response = error.Message;

                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(error.Response);
                    XmlNodeList nd = xmldoc.DocumentElement.GetElementsByTagName("message");
                    if (nd.Count > 0)
                        response = nd[0].InnerText;
                }
                catch
                {
                    response = error.Message;
                }

                var affiliateChannelName = error.AffiliateChannelName.Length > 20 ? error.AffiliateChannelName.Substring(0, 20) + "..." : error.AffiliateChannelName;
                var buyerChannelName = error.BuyerChannelName.Length > 20 ? error.BuyerChannelName.Substring(0, 20) + "..." : error.BuyerChannelName;
                var campaignName = error.CampaignName.Length > 20 ? error.CampaignName.Substring(0, 20) + "..." : error.CampaignName;
                var buyerName = error.BuyerName.Length > 20 ? error.BuyerName.Substring(0, 20) + "..." : error.BuyerName;


                if (reportType == 1)
                {

                    errorLeadsModels.Add(new ErrorLeadsReportResponseModel
                    {
                        LeadId = error.LeadId,
                        Status = error.Status,
                        Date = error.Created,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerChannelName = buyerChannelName,
                        BuyerName = buyerName,
                        AffiliateName = affiliateName,
                        CampaignName = campaignName,
                        State = error.State,
                        Response = (response.Length > 30 ? response.Substring(0, 30) : response),
                        ErrorType = error.ErrorType,
                        MinPrice = error.Minprice.HasValue ? error.Minprice.Value : 0
                    });

                    ReportCSVGenerator.Instance.AddRow(
                        $"{errorLeadsModels.Last().Date}," +
                        $"{errorLeadsModels.Last().CampaignName}," +
                        $"{errorLeadsModels.Last().BuyerName}," +
                        $"{errorLeadsModels.Last().BuyerChannelName}," +
                        $"{errorLeadsModels.Last().AffiliateChannelName}," +
                        $"{errorLeadsModels.Last().Status}," +
                        $"{errorLeadsModels.Last().State}," +
                        $"{errorLeadsModels.Last().ErrorType}," +
                        $"{errorLeadsModels.Last().Response}");
                }
                else
                {
                    errorLeadsModels.Add(new ErrorLeadsReportResponseModel
                    {
                        LeadId = error.LeadId,
                        Status = error.Status,
                        Date = error.Created,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerName = buyerName,
                        AffiliateName = affiliateName,
                        State = error.State,
                        Response = (response.Length > 30 ? response.Substring(0, 30) : response),
                        ErrorType = error.ErrorType
                    });

                    ReportCSVGenerator.Instance.AddRow(
                        $"{errorLeadsModels.Last().Date}," +
                        $"{errorLeadsModels.Last().AffiliateName}," +
                        $"{errorLeadsModels.Last().AffiliateChannelName}," +
                        $"{errorLeadsModels.Last().State}," +
                        $"{errorLeadsModels.Last().ErrorType}," +
                        $"{errorLeadsModels.Last().Response}");
                }
            }
            return errorLeadsModels;
        }

        private List<ErrorLeadsReportResponseModel> GetErrorLeadsReportData(DateTime dateFrom,
            DateTime dateTo,
            long affiliateChannelId,
            long buyerChannelId,
            long campaignId,
            long leadId,
            short statusId,
            string description,
            short errorTypeId,
            string state,
            short reportType,
            int page,
            int count)
        {
            var errorLeadsModels = new List<ErrorLeadsReportResponseModel>();
            var errorLeads = _leadMainService.GetErrorLeadsReport(errorTypeId,
                0,
                dateFrom,
                dateTo,
                leadId,
                statusId,
                0,
                affiliateChannelId,
                0,
                buyerChannelId,
                campaignId,
                state,
                0,
                reportType,
                (page - 1) * count,
                count);
            foreach (var error in errorLeads)
            {

                var affiliateName = error.AffiliateName;
                var response = error.Message;

                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(error.Response);
                    XmlNodeList nd = xmldoc.DocumentElement.GetElementsByTagName("message");
                    if (nd.Count > 0)
                        response = nd[0].InnerText;
                }
                catch
                {
                    response = error.Message;
                }

                var affiliateChannelName = error.AffiliateChannelName.Length > 20 ? error.AffiliateChannelName.Substring(0, 20) + "..." : error.AffiliateChannelName;
                var buyerChannelName = error.BuyerChannelName.Length > 20 ? error.BuyerChannelName.Substring(0, 20) + "..." : error.BuyerChannelName;
                var campaignName = error.CampaignName.Length > 20 ? error.CampaignName.Substring(0, 20) + "..." : error.CampaignName;

                if (reportType == 1)
                {

                    errorLeadsModels.Add(new ErrorLeadsReportResponseModel
                    {
                        LeadId = error.LeadId,
                        Status = error.Status,
                        Date = error.Created,
                        AffiliateChannelName = affiliateChannelName,
                        BuyerChannelName = buyerChannelName,
                        CampaignName = campaignName,
                        State = error.State,
                        Response = (response.Length > 30 ? response.Substring(0, 30) : response),
                        ErrorType = error.ErrorType,
                        MinPrice = error.Minprice.HasValue ? error.Minprice.Value : 0
                    });
                }
                else
                {
                    errorLeadsModels.Add(new ErrorLeadsReportResponseModel
                    {
                        LeadId = error.LeadId,
                        Status = error.Status,
                        Date = error.Created,
                        AffiliateChannelName = affiliateChannelName,
                        State = error.State,
                        Response = (response.Length > 30 ? response.Substring(0, 30) : response),
                        ErrorType = error.ErrorType
                    });
                }
            }
            return errorLeadsModels;
        }

        private List<ReportBadIpClickModel> GetBadIpClicksReportData(ReportBadIpClickFilterModel model)
        {
            var dateFrom = _settingService.GetUTCDate(model.StartDate);
            var dateTo = _settingService.GetUTCDate(model.EndDate);

            var filterAffiliates = string.Empty;
            if (model.AffiliateIds.Count > 0)
                filterAffiliates = String.Join(",", model.AffiliateIds);

            var badIpClickModelList = new List<ReportBadIpClickModel>();
            var badIpClickList = _leadMainService.GetBadIPClicksReport(dateFrom,
                dateTo,
                model.LeadId,
                filterAffiliates,
                model.LeadIp,
                model.ClickIp,
                0,
                int.MaxValue);

            foreach (var badIpClick in badIpClickList)
            {
                var affiliate = _affiliateService.GetAffiliateById(badIpClick.AffiliateId, true);
                badIpClickModelList.Add(new ReportBadIpClickModel
                {
                    LeadId = badIpClick.LeadId,
                    Date = badIpClick.Created,
                    AffiliateId = badIpClick.AffiliateId,
                    AffiliateName = affiliate.Name,
                    LeadIp = badIpClick.LeadIp,
                    ClickIp = badIpClick.ClickIp
                });

                ReportCSVGenerator.Instance.AddRow(
                    $"{badIpClickModelList.Last().Date}," +
                    $"{badIpClickModelList.Last().LeadId}," +
                    $"{badIpClickModelList.Last().AffiliateName}," +
                    $"{badIpClickModelList.Last().LeadIp}," +
                    $"{badIpClickModelList.Last().ClickIp}");
            }
            return badIpClickModelList;
        }

        protected List<BuyersReportByStatesResponseModel> GetReportBuyersByStatesData(DateTime startDate, DateTime endDate,
        string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds, string states)
        {
            List<BuyersReportByStatesResponseModel> buyers = new List<BuyersReportByStatesResponseModel>();

            List<ReportBuyersByStates> report = (List<ReportBuyersByStates>)this._reportService.ReportBuyersByStates(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds), states);

            string state = null;

            BuyersReportByStatesResponseModel totals = new BuyersReportByStatesResponseModel();
            totals.Title = "Total";
            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.State = "";

            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.Cost = 0;
            totals.Credit = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;
            totals.AveragePrice = 0;
            totals.Profit = 0;

            BuyersReportByStatesResponseModel item = null;
            BuyersReportByStatesResponseModel prevItem = null;

            foreach (ReportBuyersByStates r in report)
            {
                if (state != r.State)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.State, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
                    }

                    item = new BuyersReportByStatesResponseModel();
                    item.Title = !string.IsNullOrEmpty(r.State) ? r.State : "Unknown";
                    item.BuyerId = r.BuyerId;
                    item.BuyerName = r.BuyerName;
                    item.State = r.State;

                    item.TotalLeads = r.TotalLeads;
                    item.SoldLeads = r.SoldLeads;
                    item.RejectedLeads = r.RejectedLeads;
                    item.Cost = r.Debet;
                    item.Credit = r.Credit;
                    item.Redirected = r.Redirected;
                    item.RedirectedRate = (item.SoldLeads > 0 ? Math.Round(((decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0), 2) : 0);
                    item.Profit = (r.Debet - r.Credit);
                    item.AveragePrice = r.AveragePrice;

                    buyers.Add(item);
                    state = r.State;
                }
                else if (item != null)
                {
                    item.TotalLeads += r.TotalLeads;
                    item.SoldLeads += r.SoldLeads;
                    item.RejectedLeads += r.RejectedLeads;
                    item.Cost += r.Debet;
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
                totals.Cost += r.Debet;
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
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5}", prevItem.State, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
            }
            return buyers;
        }
        protected List<AffiliateChannelModelResponse> GetReportBuyersByAffiliateChannelsData(DateTime startDate,
            DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<AffiliateChannelModelResponse> buyers = new List<AffiliateChannelModelResponse>();

            List<ReportBuyersByAffiliateChannels> report = (List<ReportBuyersByAffiliateChannels>)this._reportService.ReportBuyersByAffiliateChannels(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            long affiliateChannelId = 0;

            AffiliateChannelModelResponse totals = new AffiliateChannelModelResponse();
            totals.title = "Total";
            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.AffiliateChannelId = 0;
            totals.AffiliateChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.Cost = 0;
            totals.Credit = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;
            totals.Profit = 0;
            totals.AveragePrice = 0;

            AffiliateChannelModelResponse item = null;
            AffiliateChannelModelResponse prevItem = null;

            foreach (ReportBuyersByAffiliateChannels r in report)
            {
                if (r.AffiliateChannelId != affiliateChannelId)
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", prevItem.AffiliateChannelName, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
                    }

                    affiliateChannelId = r.AffiliateChannelId;
                    item = new AffiliateChannelModelResponse();
                    buyers.Add(item);
                }

                item = buyers[buyers.Count - 1];
                item.title = r.AffiliateChannelName;
                item.BuyerId = r.BuyerId;
                item.BuyerName = r.BuyerName;
                item.AffiliateChannelId = r.AffiliateChannelId;
                item.AffiliateChannelName = r.AffiliateChannelName;

                item.TotalLeads += r.TotalLeads;
                item.SoldLeads += r.SoldLeads;
                item.RejectedLeads += r.RejectedLeads;
                item.Cost += r.Debet;
                item.Credit += r.Credit;
                item.Redirected += r.Redirected;
                item.RedirectedRate = (item.SoldLeads > 0 ? Math.Round(((decimal)item.Redirected / (decimal)item.SoldLeads * (decimal)100.0), 2) : 0);
                item.Profit += (r.Debet - r.Credit);
                item.AveragePrice += r.AveragePrice;


                item.AcceptRate = (item.TotalLeads > 0 ? Math.Round(((decimal)item.SoldLeads / (decimal)item.TotalLeads * (decimal)100.0), 2) : 0);

                totals.TotalLeads += r.TotalLeads;
                totals.SoldLeads += r.SoldLeads;
                totals.RejectedLeads += r.RejectedLeads;
                totals.Cost += r.Debet;
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
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", prevItem.AffiliateChannelName, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
            }

            return buyers;
        }

        protected List<BuyerChanneModelResponse> GetBuyerReportByBuyerChannelData(
            DateTime startDate, DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<BuyerChanneModelResponse> buyers = new List<BuyerChanneModelResponse>();

            List<BuyerReportByBuyerChannel> report = (List<BuyerReportByBuyerChannel>)this._reportService.BuyerReportByBuyerChannels(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            long buyerChannelId = 0;

            BuyerChanneModelResponse totals = new BuyerChanneModelResponse();
            totals.Title = "Total";
            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.BuyerChannelId = 0;
            totals.BuyerChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.LoanedLeads = 0;
            totals.Cost = 0;
            totals.CapReached = 0;
            totals.AcceptRate = 0;
            totals.AveragePrice = 0;
            totals.Profit = 0;
            totals.Rank = 0;
            totals.CapHit = false;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;

            BuyerChanneModelResponse item = null;
            BuyerChanneModelResponse prevItem = null;

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
                                    prevItem.CapReached += s.Quantity;
                                    totals.CapReached += s.Quantity;

                                    if (s.LeadStatus.HasValue)
                                    {
                                        switch (s.LeadStatus.Value)
                                        {
                                            case -1: if (prevItem.CapReached < prevItem.TotalLeads) prevItem.CapHit = true; break;
                                            case 1: if (prevItem.CapReached < prevItem.SoldLeads) prevItem.CapHit = true; break;
                                            case 3: if (prevItem.CapReached < prevItem.RejectedLeads) prevItem.CapHit = true; break;
                                        }
                                    }
                                }
                            }
                        }

                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", prevItem.BuyerChannelName, prevItem.CapReached, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
                    }

                    item = new BuyerChanneModelResponse();

                    item.Title = r.BuyerChannelName;
                    item.BuyerId = r.BuyerId;
                    item.BuyerName = r.BuyerName;
                    item.BuyerChannelId = r.BuyerChannelId;
                    item.BuyerChannelName = r.BuyerChannelName;

                    item.TotalLeads = 0;
                    item.SoldLeads = 0;
                    item.RejectedLeads = 0;
                    item.LoanedLeads = 0;
                    item.Cost = 0;
                    item.CapReached = 0;
                    item.AveragePrice = 0;
                    item.Profit = 0;
                    item.Rank = 0;
                    item.CapHit = false;
                    item.Redirected = 0;
                    item.RedirectedRate = 0;

                    buyers.Add(item);

                    buyerChannelId = r.BuyerChannelId;
                }

                BuyerChanneModelResponse b = item;
                b.Title = r.BuyerChannelName;
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
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", prevItem.BuyerChannelName, prevItem.CapReached, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
            }

            buyers.Add(totals);

            return buyers;
        }

        protected List<BuyerChanneModelResponse> GetBuyerReportByBuyerData(DateTime startDate,
           DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds, bool byBuyer = true)
        {
            List<BuyerChanneModelResponse> buyers = new List<BuyerChanneModelResponse>();

            List<BuyerReportByBuyerChannel> report = (List<BuyerReportByBuyerChannel>)this._reportService.BuyerReportByBuyerChannels(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            long curId = 0;

            BuyerChanneModelResponse totals = new BuyerChanneModelResponse();
            totals.Title = "Total";
            totals.BuyerId = 0;
            totals.BuyerName = "";
            totals.BuyerChannelId = 0;
            totals.BuyerChannelName = "";
            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.LoanedLeads = 0;
            totals.Cost = 0;
            totals.CapReached = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.RedirectedRate = 0;
            totals.Profit = 0;
            totals.AveragePrice = 0;

            BuyerChanneModelResponse item = null;
            BuyerChanneModelResponse prevItem = null;

            foreach (BuyerReportByBuyerChannel r in report)
            {
                DateTime? lastSoldDate = null;

                if (r.LastSoldDate.HasValue)
                    lastSoldDate = _settingService.GetTimeZoneDate(r.LastSoldDate.Value);

                if ((byBuyer && r.BuyerId != curId) || (!byBuyer && r.BuyerChannelId != curId))
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", prevItem.Title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.LastSoldDate));
                    }

                    item = new BuyerChanneModelResponse();
                    item.Title = byBuyer ? r.BuyerName : r.BuyerChannelName;
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
                    item.CapReached = 0;
                    item.Redirected = 0;

                    buyers.Add(item);

                    curId = r.BuyerId;
                }

                BuyerChanneModelResponse b = item;

                b.Title = r.BuyerName;
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
                        b.CapReached += s.Quantity;
                        totals.CapReached += s.Quantity;
                    }
                }

                prevItem = item;
            }
            if (prevItem != null)
            {
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", prevItem.Title, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate, prevItem.LastSoldDate));
            }
            buyers.Add(totals);

            return buyers;
        }
        protected List<BuyersReportResponseModel> GetReportBuyersByDatesData(DateTime startDate, DateTime endDate, string buyerIds, string buyerChannelIds, string affiliateIds, string campaignIds)
        {
            List<BuyersReportResponseModel> buyers = new List<BuyersReportResponseModel>();

            List<ReportBuyersByDates> report = (List<ReportBuyersByDates>)this._reportService.ReportBuyersByDates(startDate, endDate, (string.IsNullOrEmpty(buyerIds) || buyerIds == "null" ? "" : buyerIds), (string.IsNullOrEmpty(buyerChannelIds) || buyerChannelIds == "null" ? "" : buyerChannelIds), (string.IsNullOrEmpty(affiliateIds) || affiliateIds == "null" ? "" : affiliateIds), (string.IsNullOrEmpty(campaignIds) || campaignIds == "null" ? "" : campaignIds));

            string date = "";

            BuyersReportResponseModel totals = new BuyersReportResponseModel();
            totals.title = "Total";
            totals.Date = "";

            totals.TotalLeads = 0;
            totals.SoldLeads = 0;
            totals.RejectedLeads = 0;
            totals.LoanedLeads = 0;
            totals.Cost = 0;
            totals.Credit = 0;
            totals.AcceptRate = 0;
            totals.Redirected = 0;
            totals.AveragePrice = 0;
            totals.Profit = 0;

            BuyersReportResponseModel item = null;
            BuyersReportResponseModel prevItem = null;

            foreach (ReportBuyersByDates r in report)
            {
                if (date != r.Date.ToShortDateString())
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", prevItem.Date, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
                    }

                    item = new BuyersReportResponseModel();
                    item.Date = r.Date.ToShortDateString();
                    item.title = r.Date.ToShortDateString();
                    item.TotalLeads = r.TotalLeads;
                    item.SoldLeads = r.SoldLeads;
                    item.RejectedLeads = r.RejectedLeads;
                    item.LoanedLeads = r.LoanedLeads;
                    item.Cost = r.Debet;
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
                    item.Cost += r.Debet;
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
                totals.Cost += r.Debet;
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
                ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", prevItem.Date, prevItem.TotalLeads, prevItem.SoldLeads, prevItem.RejectedLeads, prevItem.Cost, prevItem.Profit, prevItem.AveragePrice, prevItem.AcceptRate, prevItem.RedirectedRate));
            }

            return buyers;
        }

        private void CalculateReportBuyersComparisonPercent(ReportBuyersComparisonRowModel reportBuyersComparisonTotalRowModel)
        {
            //**************** Posted leads
            if (reportBuyersComparisonTotalRowModel.PostedLeads.Value2 > 0)
                reportBuyersComparisonTotalRowModel.PostedLeads.Percent2.Percent =
                    Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.PostedLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.PostedLeads.Value2) / (double)reportBuyersComparisonTotalRowModel.PostedLeads.Value2 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.PostedLeads.Value1 > 0)
                    reportBuyersComparisonTotalRowModel.PostedLeads.Percent2.Percent = 100;

            if (reportBuyersComparisonTotalRowModel.PostedLeads.Value3 > 0)
                reportBuyersComparisonTotalRowModel.PostedLeads.Percent3.Percent =
                    Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.PostedLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.PostedLeads.Value3) / (double)reportBuyersComparisonTotalRowModel.PostedLeads.Value3 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.PostedLeads.Value1 > 0)
                    reportBuyersComparisonTotalRowModel.PostedLeads.Percent3.Percent = 100;

            if (reportBuyersComparisonTotalRowModel.PostedLeads.Value2 > reportBuyersComparisonTotalRowModel.PostedLeads.Value1)
                reportBuyersComparisonTotalRowModel.PostedLeads.Percent2.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.PostedLeads.Value2 < reportBuyersComparisonTotalRowModel.PostedLeads.Value1)
                reportBuyersComparisonTotalRowModel.PostedLeads.Percent2.Direction = 2;

            if (reportBuyersComparisonTotalRowModel.PostedLeads.Value3 > reportBuyersComparisonTotalRowModel.PostedLeads.Value1)
                reportBuyersComparisonTotalRowModel.PostedLeads.Percent3.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.PostedLeads.Value3 < reportBuyersComparisonTotalRowModel.PostedLeads.Value1)
                reportBuyersComparisonTotalRowModel.PostedLeads.Percent3.Direction = 2;

            //********************

            //****************** Sold leads

            if (reportBuyersComparisonTotalRowModel.SoldLeads.Value2 > 0)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent2.Percent =
                    Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.SoldLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.SoldLeads.Value2) / (double)reportBuyersComparisonTotalRowModel.SoldLeads.Value2 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.SoldLeads.Value1 > 0)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent2.Percent = 100;


            if (reportBuyersComparisonTotalRowModel.SoldLeads.Value3 > 0)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent3.Percent =
                    Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.SoldLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.SoldLeads.Value3) / (double)reportBuyersComparisonTotalRowModel.SoldLeads.Value3 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.SoldLeads.Value1 > 0)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent3.Percent = 100;

            if (reportBuyersComparisonTotalRowModel.SoldLeads.Value2 > reportBuyersComparisonTotalRowModel.SoldLeads.Value1)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent2.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.SoldLeads.Value2 < reportBuyersComparisonTotalRowModel.SoldLeads.Value1)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent2.Direction = 2;

            if (reportBuyersComparisonTotalRowModel.SoldLeads.Value3 > reportBuyersComparisonTotalRowModel.SoldLeads.Value1)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent3.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.SoldLeads.Value3 < reportBuyersComparisonTotalRowModel.SoldLeads.Value1)
                reportBuyersComparisonTotalRowModel.SoldLeads.Percent3.Direction = 2;
            //**************

            //****************** Redirected leads

            if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value2 > 0)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent2.Percent =
                    Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.RejectedLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.RejectedLeads.Value2) / (double)reportBuyersComparisonTotalRowModel.RejectedLeads.Value2 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value1 > 0)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent2.Percent = 100;


            if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value3 > 0)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent3.Percent =
                Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.RejectedLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.RejectedLeads.Value3) / (double)reportBuyersComparisonTotalRowModel.RejectedLeads.Value3 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value1 > 0)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent3.Percent = 100;


            if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value2 > reportBuyersComparisonTotalRowModel.RejectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent2.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value2 < reportBuyersComparisonTotalRowModel.RejectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent2.Direction = 2;

            if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value3 > reportBuyersComparisonTotalRowModel.RejectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent3.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.RejectedLeads.Value3 < reportBuyersComparisonTotalRowModel.RejectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RejectedLeads.Percent3.Direction = 2;
            //************************
            //****************** Redirected leads

            if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value2 > 0)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent2.Percent =
                    Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.RedirectedLeads.Value2) / (double)reportBuyersComparisonTotalRowModel.RedirectedLeads.Value2 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1 > 0)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent2.Percent = 100;


            if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value3 > 0)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent3.Percent =
                Math.Round(Math.Abs((double)reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1 - (double)reportBuyersComparisonTotalRowModel.RedirectedLeads.Value3) / (double)reportBuyersComparisonTotalRowModel.RedirectedLeads.Value3 * (float)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1 > 0)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent3.Percent = 100;


            if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value2 > reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent2.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value2 < reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent2.Direction = 2;

            if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value3 > reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent3.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.RedirectedLeads.Value3 < reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1)
                reportBuyersComparisonTotalRowModel.RedirectedLeads.Percent3.Direction = 2;
            //*************************
            //************ Revenue

            if (reportBuyersComparisonTotalRowModel.Revenue.Value2 > 0)
                reportBuyersComparisonTotalRowModel.Revenue.Percent2.Percent =
                  (double)Math.Round(Math.Abs(reportBuyersComparisonTotalRowModel.Revenue.Value1 - reportBuyersComparisonTotalRowModel.Revenue.Value2) / reportBuyersComparisonTotalRowModel.Revenue.Value2 * (decimal)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.Revenue.Value1 > 0)
                reportBuyersComparisonTotalRowModel.Revenue.Percent2.Percent = 100;


            if (reportBuyersComparisonTotalRowModel.Revenue.Value3 > 0)
                reportBuyersComparisonTotalRowModel.Revenue.Percent3.Percent =
                (double)Math.Round(Math.Abs(reportBuyersComparisonTotalRowModel.Revenue.Value1 - reportBuyersComparisonTotalRowModel.Revenue.Value3) / reportBuyersComparisonTotalRowModel.Revenue.Value3 * (decimal)100, 2);
            else
                if (reportBuyersComparisonTotalRowModel.Revenue.Value1 > 0)
                reportBuyersComparisonTotalRowModel.Revenue.Percent3.Percent = 100;


            if (reportBuyersComparisonTotalRowModel.Revenue.Value2 > reportBuyersComparisonTotalRowModel.Revenue.Value1)
                reportBuyersComparisonTotalRowModel.Revenue.Percent2.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.Revenue.Value2 < reportBuyersComparisonTotalRowModel.Revenue.Value1)
                reportBuyersComparisonTotalRowModel.Revenue.Percent2.Direction = 2;

            if (reportBuyersComparisonTotalRowModel.Revenue.Value3 > reportBuyersComparisonTotalRowModel.Revenue.Value1)
                reportBuyersComparisonTotalRowModel.Revenue.Percent3.Direction = 1;
            else if (reportBuyersComparisonTotalRowModel.Revenue.Value3 < reportBuyersComparisonTotalRowModel.Revenue.Value1)
                reportBuyersComparisonTotalRowModel.Revenue.Percent3.Direction = 2;
            //**********************
        }

        private ReportBuyersComparisonModel GetReportBuyersComparisonData(DateTime date1, DateTime date2, DateTime date3, long[] buyerIdsLong, long[] campaignIds, short byBuyers = 1, bool isBuyer = true)
        {
            ReportBuyersComparisonModel reportBuyersComparisonModel = new ReportBuyersComparisonModel();

            ReportBuyersComparisonRowModel reportBuyersComparisonTotalRowModel = new ReportBuyersComparisonRowModel();

            if (campaignIds.Length == 0 || (campaignIds.Length == 1 && campaignIds[0] == 0))
            {
                for (int i = 0; i < buyerIdsLong.Length; i++)
                {
                    ReportBuyersComparisonRowModel reportBuyersComparisonRowModel = new ReportBuyersComparisonRowModel();

                    if (buyerIdsLong[i] == 0) continue;

                    string name = "";

                    if (isBuyer)
                    {
                        if (byBuyers == 1)
                        {
                            Buyer buyer = _buyerService.GetBuyerById(buyerIdsLong[i]);
                            reportBuyersComparisonRowModel.Name = buyer.Name;
                        }
                        else
                        {
                            BuyerChannel buyerChannel = _buyerChannelService.GetBuyerChannelById(buyerIdsLong[i]);
                            reportBuyersComparisonRowModel.Name = buyerChannel.Name;
                        }
                    }
                    else
                    {
                        if (byBuyers == 1)
                        {
                            Affiliate aff2 = _affiliateService.GetAffiliateById(buyerIdsLong[i], false);
                            reportBuyersComparisonRowModel.Name = aff2.Name;
                        }
                        else
                        {
                            AffiliateChannel affCh2 = _affiliateChannelService.GetAffiliateChannelById(buyerIdsLong[i]);
                            reportBuyersComparisonRowModel.Name = affCh2.Name;
                        }
                    }

                    List<ReportBuyersComparison> reportRows = new List<ReportBuyersComparison>();

                    if (isBuyer)
                    {
                        if (byBuyers == 1)
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparison(buyerIdsLong[i], 0, 0, date1, date2, date3);
                        else
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparisonBuyerChannels(buyerIdsLong[i], 0, date1, date2, date3);
                    }
                    else
                    {
                        if (byBuyers == 1)
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportAffiliatesComparison(buyerIdsLong[i], 0, 0, date1, date2, date3);
                        else
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparisonBuyerChannels(buyerIdsLong[i], 0, date1, date2, date3);

                    }
                    foreach (ReportBuyersComparison row in reportRows)
                    {
                        if (row.Created.Day == date1.Day && row.Created.Month == date1.Month && row.Created.Year == date1.Year)
                        {
                            reportBuyersComparisonRowModel.PostedLeads.Value1 += row.Posted;
                            reportBuyersComparisonRowModel.SoldLeads.Value1 += row.Sold;
                            reportBuyersComparisonRowModel.RejectedLeads.Value1 += row.Rejected;
                            reportBuyersComparisonRowModel.RedirectedLeads.Value1 += row.Redirected;
                            reportBuyersComparisonRowModel.Revenue.Value1 += row.Revenue;

                            reportBuyersComparisonTotalRowModel.PostedLeads.Value1 += row.Posted;
                            reportBuyersComparisonTotalRowModel.SoldLeads.Value1 += row.Sold;
                            reportBuyersComparisonTotalRowModel.RejectedLeads.Value1 += row.Rejected;
                            reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1 += row.Redirected;
                            reportBuyersComparisonTotalRowModel.Revenue.Value1 += row.Revenue;
                        }

                        if (row.Created.Day == date2.Day && row.Created.Month == date2.Month && row.Created.Year == date2.Year)
                        {
                            reportBuyersComparisonRowModel.PostedLeads.Value2 += row.Posted;
                            reportBuyersComparisonRowModel.SoldLeads.Value2 += row.Sold;
                            reportBuyersComparisonRowModel.RejectedLeads.Value2 += row.Rejected;
                            reportBuyersComparisonRowModel.RedirectedLeads.Value2 += row.Redirected;
                            reportBuyersComparisonRowModel.Revenue.Value2 += row.Revenue;

                            reportBuyersComparisonTotalRowModel.PostedLeads.Value2 += row.Posted;
                            reportBuyersComparisonTotalRowModel.SoldLeads.Value2 += row.Sold;
                            reportBuyersComparisonTotalRowModel.RejectedLeads.Value2 += row.Rejected;
                            reportBuyersComparisonTotalRowModel.RedirectedLeads.Value2 += row.Redirected;
                            reportBuyersComparisonTotalRowModel.Revenue.Value2 += row.Revenue;
                        }

                        if (row.Created.Day == date3.Day && row.Created.Month == date3.Month && row.Created.Year == date3.Year)
                        {
                            reportBuyersComparisonRowModel.PostedLeads.Value3 += row.Posted;
                            reportBuyersComparisonRowModel.SoldLeads.Value3 += row.Sold;
                            reportBuyersComparisonRowModel.RejectedLeads.Value3 += row.Rejected;
                            reportBuyersComparisonRowModel.RedirectedLeads.Value3 += row.Redirected;
                            reportBuyersComparisonRowModel.Revenue.Value3 += row.Revenue;

                            reportBuyersComparisonTotalRowModel.PostedLeads.Value3 += row.Posted;
                            reportBuyersComparisonTotalRowModel.SoldLeads.Value3 += row.Sold;
                            reportBuyersComparisonTotalRowModel.RejectedLeads.Value3 += row.Rejected;
                            reportBuyersComparisonTotalRowModel.RedirectedLeads.Value3 += row.Redirected;
                            reportBuyersComparisonTotalRowModel.Revenue.Value3 += row.Revenue;
                        }
                    }

                    CalculateReportBuyersComparisonPercent(reportBuyersComparisonRowModel);
                    CalculateReportBuyersComparisonPercent(reportBuyersComparisonTotalRowModel);

                    reportBuyersComparisonModel.Rows.Add(reportBuyersComparisonRowModel);
                }
            }
            else
            {
                for (int i = 0; i < campaignIds.Length; i++)
                {
                    if (campaignIds[i] == 0) continue;

                    string campaignName = "";

                    ReportBuyersComparisonRowModel reportBuyersComparisonRowModel = new ReportBuyersComparisonRowModel();

                    Campaign campaign = _campaignService.GetCampaignById(campaignIds[i]);
                    reportBuyersComparisonRowModel.Name = campaign.Name;

                    List<ReportBuyersComparison> reportRows = null;

                    if (isBuyer)
                    {
                        if (byBuyers == 1)
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparison(0, 0, campaignIds[i], date1, date2, date3);
                        else
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparisonBuyerChannels(0, campaignIds[i], date1, date2, date3);
                    }
                    else
                    {
                        if (byBuyers == 1)
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportAffiliatesComparison(0, 0, campaignIds[i], date1, date2, date3);
                        else
                            reportRows = (List<ReportBuyersComparison>)this._reportService.ReportBuyersComparisonBuyerChannels(0, campaignIds[i], date1, date2, date3);
                    }

                    foreach (ReportBuyersComparison row in reportRows)
                    {
                        if (row.Created.Day == date1.Day && row.Created.Month == date1.Month && row.Created.Year == date1.Year)
                        {
                            reportBuyersComparisonRowModel.PostedLeads.Value1 += row.Posted;
                            reportBuyersComparisonRowModel.SoldLeads.Value1 += row.Sold;
                            reportBuyersComparisonRowModel.RejectedLeads.Value1 += row.Rejected;
                            reportBuyersComparisonRowModel.RedirectedLeads.Value1 += row.Redirected;
                            reportBuyersComparisonRowModel.Revenue.Value1 += row.Revenue;

                            reportBuyersComparisonTotalRowModel.PostedLeads.Value1 += row.Posted;
                            reportBuyersComparisonTotalRowModel.SoldLeads.Value1 += row.Sold;
                            reportBuyersComparisonTotalRowModel.RejectedLeads.Value1 += row.Rejected;
                            reportBuyersComparisonTotalRowModel.RedirectedLeads.Value1 += row.Redirected;
                            reportBuyersComparisonTotalRowModel.Revenue.Value1 += row.Revenue;
                        }

                        if (row.Created.Day == date2.Day && row.Created.Month == date2.Month && row.Created.Year == date2.Year)
                        {
                            reportBuyersComparisonRowModel.PostedLeads.Value2 += row.Posted;
                            reportBuyersComparisonRowModel.SoldLeads.Value2 += row.Sold;
                            reportBuyersComparisonRowModel.RejectedLeads.Value2 += row.Rejected;
                            reportBuyersComparisonRowModel.RedirectedLeads.Value2 += row.Redirected;
                            reportBuyersComparisonRowModel.Revenue.Value2 += row.Revenue;

                            reportBuyersComparisonTotalRowModel.PostedLeads.Value2 += row.Posted;
                            reportBuyersComparisonTotalRowModel.SoldLeads.Value2 += row.Sold;
                            reportBuyersComparisonTotalRowModel.RejectedLeads.Value2 += row.Rejected;
                            reportBuyersComparisonTotalRowModel.RedirectedLeads.Value2 += row.Redirected;
                            reportBuyersComparisonTotalRowModel.Revenue.Value2 += row.Revenue;
                        }

                        if (row.Created.Day == date3.Day && row.Created.Month == date3.Month && row.Created.Year == date3.Year)
                        {
                            reportBuyersComparisonRowModel.PostedLeads.Value3 += row.Posted;
                            reportBuyersComparisonRowModel.SoldLeads.Value3 += row.Sold;
                            reportBuyersComparisonRowModel.RejectedLeads.Value3 += row.Rejected;
                            reportBuyersComparisonRowModel.RedirectedLeads.Value3 += row.Redirected;
                            reportBuyersComparisonRowModel.Revenue.Value3 += row.Revenue;

                            reportBuyersComparisonTotalRowModel.PostedLeads.Value3 += row.Posted;
                            reportBuyersComparisonTotalRowModel.SoldLeads.Value3 += row.Sold;
                            reportBuyersComparisonTotalRowModel.RejectedLeads.Value3 += row.Rejected;
                            reportBuyersComparisonTotalRowModel.RedirectedLeads.Value3 += row.Redirected;
                            reportBuyersComparisonTotalRowModel.Revenue.Value3 += row.Revenue;
                        }
                    }

                    CalculateReportBuyersComparisonPercent(reportBuyersComparisonRowModel);
                    CalculateReportBuyersComparisonPercent(reportBuyersComparisonTotalRowModel);

                    reportBuyersComparisonModel.Rows.Add(reportBuyersComparisonRowModel);
                }
            }

            
            reportBuyersComparisonModel.Rows.Add(reportBuyersComparisonTotalRowModel);


            ReportCSVGenerator.Instance.Init("Company,,Posted Leads,,,Sold Leads,,,Rejected Leads,,,Redirected Leads,,,Revenue,,");

            ReportCSVGenerator.Instance.AddRow(
                $"{string.Empty}," +
                $"{date1.ToShortDateString()}, {date2.ToShortDateString()},{date3.ToShortDateString()}," +
                $"{date1.ToShortDateString()}, {date2.ToShortDateString()},{date3.ToShortDateString()}," +
                $"{date1.ToShortDateString()}, {date2.ToShortDateString()},{date3.ToShortDateString()}," +
                $"{date1.ToShortDateString()}, {date2.ToShortDateString()},{date3.ToShortDateString()}," +
                $"{date1.ToShortDateString()}, {date2.ToShortDateString()},{date3.ToShortDateString()}");

            foreach(var row in reportBuyersComparisonModel.Rows)
            {
                ReportCSVGenerator.Instance.AddRow(
                    $"{row.Name}, " +
                    $"{row.PostedLeads.Value1},{row.PostedLeads.Value2},{row.PostedLeads.Value3}," +
                    $"{row.SoldLeads.Value1},{row.SoldLeads.Value2},{row.SoldLeads.Value3}," +
                    $"{row.RejectedLeads.Value1},{row.RejectedLeads.Value2},{row.RejectedLeads.Value3}," +
                    $"{row.RedirectedLeads.Value1},{row.RedirectedLeads.Value2},{row.RedirectedLeads.Value3}," +
                    $"{row.Revenue.Value1},{row.Revenue.Value2},{row.Revenue.Value3}");
            }

            return reportBuyersComparisonModel;
        }



        private List<ReportBuyersByTrafficEstimatorModel> GetReportBuyersByTrafficEstimatorData(DateTime startDate, DateTime endDate, List<long> buyerChannelIds, List<long> campaignIds, List<ReportBuyersTrafficEstimatorFieldFilterModel> fields)
        {
            //ReportCSVGenerator.Instance.Init("Buyer,Total leads,Unique leads");

            Dictionary<long, ReportBuyersByTrafficEstimatorModel> buyers = new Dictionary<long, ReportBuyersByTrafficEstimatorModel>();

            List<List<ReportBuyersByTrafficEstimate>> reports = new List<List<ReportBuyersByTrafficEstimate>>();

            List<ReportBuyersByTrafficEstimate> report = null;

            report = (List<ReportBuyersByTrafficEstimate>)this._reportService.ReportBuyersByTrfficEstimate(startDate, endDate, string.Join(",", buyerChannelIds.ToArray()), string.Join(",", campaignIds.ToArray()), fields.Select(x => x.FieldName).ToList(), fields.Select(x => x.Value).ToList(), fields.Select(x => x.ValueType).ToList(), fields.Select(x => x.Condition).ToList(), fields.Select(x => false).ToList());

            ReportBuyersByTrafficEstimatorModel item = null;
            ReportBuyersByTrafficEstimatorModel prevItem = null;
            ReportBuyersByTrafficEstimatorModel total = new ReportBuyersByTrafficEstimatorModel() { Title = "Total", Quantity = 0, UQuantity = 0 };

            foreach (var r in report)
            {
                item = new ReportBuyersByTrafficEstimatorModel();

                if (buyers.ContainsKey(r.BuyerChannelId)) item = buyers[r.BuyerChannelId];
                else
                {
                    if (prevItem != null)
                    {
                        ReportCSVGenerator.Instance.AddRow(string.Format("{0},{1},{2}", prevItem.BuyerChannelName, prevItem.Quantity, prevItem.UQuantity));
                    }
                    buyers.Add(r.BuyerChannelId, item);
                }

                item.Title = r.BuyerChannelName;
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

            var items = buyers.Select(d => d.Value).ToList();
            items.Add(total);
            return items;
        }


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
                throw;
            }

            if (convertToUtc)
            {
                startDate = _settingService.GetUTCDate(startDate);
                endDate = _settingService.GetUTCDate(endDate);
            }
        }
        protected ResponseMessageResult PerformCsvDownload(string fileName, string mediaType)
        {
            string data = ReportCSVGenerator.Instance.ToString();
            if (string.IsNullOrEmpty(data))
                data = " ";

            byte[] textAsBytes = new System.Text.UTF8Encoding().GetBytes(data);

            MemoryStream stream = new MemoryStream(textAsBytes);

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            return responseMessageResult;
        }

        protected ResponseMessageResult PerformPDFDownload(string fileName, string mediaType)
        {
            byte[] textAsBytes = new System.Text.UTF8Encoding().GetBytes(ReportCSVGenerator.Instance.ToString());

            MemoryStream stream = new MemoryStream(textAsBytes);

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            return responseMessageResult;
        }

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

        #endregion

        #region Fields
        /// <summary>
        /// Reportng Service
        /// </summary>
        private readonly IReportService _reportService;

        private readonly IPlanService _planService;

        private readonly IBuyerService _buyerService;

        private readonly IBuyerChannelService _buyerChannelService;

        private readonly ICampaignService _campaignService;

        private readonly IStateProvinceService _stateProvinceService;

        private readonly ILeadMainResponseService _leadMainResponseService;

        /// <summary>
        /// Application context
        /// </summary>
        private readonly IAppContext _appContext;
        /// <summary>
        /// The setting service
        /// </summary>
        private readonly ISettingService _settingService;
        /// <summary>
        /// The lead schedule service
        /// </summary>
        private readonly IBuyerChannelScheduleService _buyerChannelScheduleService;
        /// <summary>
        /// The affiliate service
        /// </summary>
        private readonly IAffiliateService _affiliateService;

        private readonly IAffiliateChannelService _affiliateChannelService;

        private readonly IUserService _userService;


        /// <summary>
        /// State Province Service
        /// </summary>
        private readonly ILeadMainService _leadMainService;
        #endregion

        #region Constructor
        public ReportController(IAppContext appContext
            , IReportService reportService
            , IPlanService planService
            , ISettingService settingService
            , IBuyerChannelScheduleService buyerChannelScheduleService
            , IAffiliateService affiliateService
            , IBuyerService buyerService
            , IBuyerChannelService buyerChannelService
            , ICampaignService campaignService
            , IAffiliateChannelService affiliateChannelService
            , ILeadMainService leadMainService
            , ILeadMainResponseService leadMainResponseService
            , IStateProvinceService stateProvinceService
            , IUserService userService)
        {

            this._leadMainResponseService = leadMainResponseService;
            this._reportService = reportService;
            this._planService = planService;
            this._appContext = appContext;
            
            this._settingService = settingService;
            this._buyerChannelScheduleService = buyerChannelScheduleService;
            this._affiliateService = affiliateService;
            this._affiliateChannelService = affiliateChannelService;
            _buyerService = buyerService;
            _buyerChannelService = buyerChannelService;
            _campaignService = campaignService;
            _leadMainService = leadMainService;
            _stateProvinceService = stateProvinceService;
            _userService = userService;
        }
        #endregion


    }
}
