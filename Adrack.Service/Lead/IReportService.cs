// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IReportService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.CustomReports;
using Adrack.Core.Domain.Dashboard;
using Adrack.Core.Domain.Lead.Reports;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    public class ChartData
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// Interface IReportService
    /// </summary>
    public partial interface IReportService
    {
        #region Methods

        /// <summary>
        /// Buyers the report by buyer channels.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;BuyerReportByBuyerChannel&gt;.</returns>
        IList<BuyerReportByBuyerChannel> BuyerReportByBuyerChannels(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns);

        List<DashboardPieChartInsightModel> ReportTopStateRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardPieChartInsightModel> ReportTopStatePosted(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardPieChartInsightModel> ReportTopStateRevenue(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardPieChartInsightModel> ReportTopStateSold(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardPieChartInsightModel> ReportTopStateCost(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardPieChartInsightModel> ReportTopStateProfit(DateTime? start, DateTime? end, List<long> campaignIds, long count);

        List<DashboardBarChartInsightModel> ReportTopAffiliatesRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardBarChartInsightModel> ReportTopBuyersRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardBarChartInsightModel> ReportTopBuyerChannelsRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        List<DashboardBarChartInsightModel> ReportTopAffiliateChannelsRecieved(DateTime? start, DateTime? end, List<long> campaignIds, long count);
        /// <summary>
        /// Buyers the report by hour.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;BuyerReportByHour&gt;.</returns>
        IList<BuyerReportByHour> BuyerReportByHour(DateTime date, string buyerChannels, string campaigns);

        /// <summary>
        /// Reports the buyers by campaigns.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByCampaigns&gt;.</returns>
        IList<ReportBuyersByCampaigns> ReportBuyersByCampaigns(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns);

        /// <summary>
        /// Reports the buyers by affiliate channels.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByAffiliateChannels&gt;.</returns>
        IList<ReportBuyersByAffiliateChannels> ReportBuyersByAffiliateChannels(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns);

        /// <summary>
        /// Reports the buyers by states.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <param name="states">The states.</param>
        /// <returns>IList&lt;ReportBuyersByStates&gt;.</returns>
        IList<ReportBuyersByStates> ReportBuyersByStates(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns, string states);

        /// <summary>
        /// Reports the buyers by dates.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByDates&gt;.</returns>
        IList<ReportBuyersByDates> ReportBuyersByDates(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns);

        /// <summary>
        /// Reports the buyers by lead notes.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="affiliateChannels">The affiliate channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByLeadNotes&gt;.</returns>
        IList<ReportBuyersByLeadNotes> ReportBuyersByLeadNotes(DateTime start, DateTime end, string buyers, string buyerChannels, string affiliateChannels, string campaigns);

        /// <summary>
        /// Reports the buyers by reaction time.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerid">The buyerid.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <returns>IList&lt;ReportBuyersByReactionTime&gt;.</returns>
        IList<ReportBuyersByReactionTime> ReportBuyersByReactionTime(DateTime start, DateTime end, long buyerid, string campaigns);

        /// <summary>
        /// Reports the buyers comparison.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <param name="date3">The date3.</param>
        /// <returns>IList&lt;ReportBuyersComparison&gt;.</returns>
        IList<ReportBuyersComparison> ReportBuyersComparison(long buyerId, long buyerChannelId, long campaignId, DateTime date1, DateTime date2, DateTime date3);

        IList<ReportBuyersComparison> ReportAffiliatesComparison(long affiliateId, long affiliateChannelId, long campaignId, DateTime date1, DateTime date2, DateTime date3);


        /// <summary>
        /// Reports the buyer channels comparison.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="date1">The date1.</param>
        /// <param name="date2">The date2.</param>
        /// <param name="date3">The date3.</param>
        /// <returns>IList&lt;ReportBuyersComparison&gt;.</returns>
        IList<ReportBuyersComparison> ReportBuyersComparisonBuyerChannels(long buyerChannelId, long campaignId, DateTime date1, DateTime date2, DateTime date3);



        /// <summary>
        /// Reports the buyers by prices.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <param name="fromPrice">From price.</param>
        /// <param name="toPrice">To price.</param>
        /// <returns>IList&lt;ReportBuyersByPrices&gt;.</returns>
        IList<ReportBuyersByPrices> ReportBuyersByPrices(DateTime start, DateTime end, string buyers, string buyerChannels, string campaigns, decimal fromPrice, decimal toPrice);

        /// <summary>
        /// Reports the buyers by trffic estimate.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="campaigns">The campaigns.</param>
        /// <param name="fields">The fields.</param>
        /// <param name="values">The values.</param>
        /// <param name="valueTypes">The value types.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="excludes">The excludes.</param>
        /// <returns>IList&lt;ReportBuyersByTrafficEstimate&gt;.</returns>
        IList<ReportBuyersByTrafficEstimate> ReportBuyersByTrfficEstimate(DateTime start, DateTime end, string buyerChannels, string campaigns, List<string> fields, List<string> values, List<short> valueTypes, List<short> conditions, List<bool> excludes);


        /// <summary>
        /// Reports the buyers win rate report.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <returns>IList&lt;ReportBuyersWinRateReport&gt;.</returns>
        IList<ReportBuyersWinRateReport> ReportBuyersWinRateReport(DateTime start, DateTime end, string buyerChannels);

        /// <summary>
        /// Reports the buyers Conversion Analysys.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <returns>IList&lt;ReportConversionAnalysys&gt;.</returns>
        IList<ReportBuyersConversionAnalysys> ReportConversionAnalysys(DateTime start, DateTime end, string buyerChannels, string type);

        IList<ReportSendingTime> ReportSendingTime(DateTime start, DateTime end, string campaignIds);

        IList<ReportSendingTime> ReportSendingTimeByFilter(DateTime start, DateTime end, string campaignIds,
            string buyerIds, string buyerChannelIds);


        /// <summary>
        /// Get price points.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerChannels">The buyer channels.</param>
        /// <param name="isNot">Search in provided buyer channels or not.</param>/// 
        /// <returns>IList&lt;decimal&gt;.</returns>
        IList<decimal> GetPricePoints(DateTime start, DateTime end, string buyerChannels, bool isNot);


        /// <summary>
        /// Reports the by days.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByDays&gt;.</returns>
        IList<ReportByDays> ReportByDays(DateTime start, DateTime end, long campaignId, long parentid);

        /// <summary>
        /// Reports the by date.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByMinutes&gt;.</returns>
        IList<ReportByMinutes> ReportByDate(string activity, DateTime start, DateTime end, int delta, long campaignId, long parentid);

        /// <summary>
        /// Reports the totals.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportTotals&gt;.</returns>
        IList<ReportTotals> ReportTotals(DateTime start, DateTime end, long parentid);

        IList<ReportTotals> ReportTotalsByCampaign(DateTime start, DateTime end, long campaignId, long parentid);


        /// <summary>
        /// Reports the totals by date.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="campaignid">The campaignid.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportTotalsByDate&gt;.</returns>
        IList<ReportTotalsByDate> ReportTotalsByDate(DateTime start, DateTime end, long campaignid, long parentid);

        /// <summary>
        /// Reports the totals buyer.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportTotalsBuyer&gt;.</returns>
        IList<ReportTotalsBuyer> ReportTotalsBuyer(DateTime start, DateTime end, long parentid);

        /// <summary>
        /// Reports the by minutes.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByMinutes&gt;.</returns>
        IList<ReportByMinutes> ReportByMinutes(DateTime start, DateTime end, long parentid);

        /// <summary>
        /// Reports the by hour.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <returns>IList&lt;ReportByHour&gt;.</returns>
        IList<ReportByHour> ReportByHour(DateTime start, DateTime end, long parentid);

        /// <summary>
        /// Reports the by year.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="parentid">The parentid.</param>
        /// <param name="campaignid">The campaignid.</param>
        /// <returns>IList&lt;ReportByYear&gt;.</returns>
        IList<ReportByYear> ReportByYear(DateTime start, DateTime end, long parentid, long campaignid);

        /// <summary>
        /// Reports the by sub ids.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="subid">The subid.</param>
        /// <returns>IList&lt;ReportBySubId&gt;.</returns>
        IList<ReportBySubId> ReportBySubIds(DateTime start, DateTime end, string subid);

        /// <summary>
        /// Reports the affiliates by affiliate channels.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByAffiliateChannels&gt;.</returns>
        IList<ReportAffiliatesByAffiliateChannels> ReportAffiliatesByAffiliateChannels(DateTime start, DateTime end, string buyers, string achannels);

        IList<ReportClickMain> ReportClickMain(DateTime start, DateTime end, string achannels);


        /// <summary>
        /// Reports the affiliates by campaigns.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByCampaigns&gt;.</returns>
        IList<ReportAffiliatesByCampaigns> ReportAffiliatesByCampaigns(DateTime start, DateTime end, string buyers, string achannels);

        /// <summary>
        /// Reports the affiliates by states.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByStates&gt;.</returns>
        IList<ReportAffiliatesByStates> ReportAffiliatesByStates(DateTime start, DateTime end, string buyers, string achannels, string states);

        /// <summary>
        /// Reports the affiliates by epl.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyers">The buyers.</param>
        /// <param name="achannels">The achannels.</param>
        /// <returns>IList&lt;ReportAffiliatesByEpl&gt;.</returns>
        IList<ReportAffiliatesByEpl> ReportAffiliatesByEpl(DateTime start, DateTime end, string buyers, string achannels);

        /// <summary>
        /// Reports the by statuses.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>IList&lt;ReportByStatuses&gt;.</returns>
        IList<ReportByStatuses> ReportByStatuses(DateTime start, DateTime end);

        /// <summary>
        /// Reports the by top states.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="Count">The count.</param>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="CampaingnId">The campaingn identifier.</param>
        /// <returns>IList&lt;ReportTopStates&gt;.</returns>
        IList<ReportTopStates> ReportByTopStates(DateTime start, DateTime end, int Count, long BuyerId, long AffiliateId, long CampaingnId);


        /// <summary>
        /// Sales report.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>IList&lt;dynamic&gt;.</returns>
        IList<ChartData> SalesReport(DateTime start, DateTime end, string type);

        IList<GlobalReport> GetGlobalReport(DateTime start, DateTime end, string buyerChannels, string affiliateChannels);



        //string GetReportQueryString(CustomReportType customReportType);

        List<Core.Domain.CustomReports.ReportType> GetAllCustomReports();

        Core.Domain.CustomReports.ReportType GetCustomReportById(long customreportId);
        void UpdateReportType(Core.Domain.CustomReports.ReportType reportType);
        ReportVariableType AddReportVariableType(ReportVariableType reportVariable);
        ReportVariableType GetReportVariableType(long id);

        List<ReportVariableType> GetReportVariableTypes();


        void UpdateReportVariableType(ReportVariableType reportVariableType);
        void DeleteReportVariableType(long id);
        List<ReportVariableType> GetCustomReportVariables(long reportTypeId);
        List<ReportVariable> GetReportVariables(long reportTypeId);
        List<ReportFilterSetting> GetAllReportSettings();
        List<ReportFilterSetting> GetAllReportSettingByReportType(long reportTypeId);
        void UpdateReportSetting(ReportFilterSetting reportSetting);
        long AddReportSetting(ReportFilterSetting reportSetting);
        ReportFilterSetting GetReportSettingById(long settingId);
        ReportFilterSetting GetReportSettingByReportId(long reportId);
        void DeleteReportSettingById(long settingId);
        ReportVariable AttachReportVariableType(ReportVariable reportVariable);
        void DetachReportVariableType(ReportVariable reportVariable);

        void DeleteCustomReport(long reportId);

        void ClearReportVariables(int reportId);

        IList<T> ExecuteCustomReportQuery<T>(string queryStrings);
        IList<string> ExecuteCustomReportQuery(string queryStrings);

        LeadsReportTotal LeadDashboardTotals();

        IList<LeadsReportByHour> LeadDashboardReportByHour(DateTime start, DateTime end, List<long> campaignIds);

        IList<LeadsReportByDay> LeadDashboardReportByDays(DateTime start, DateTime end, List<long> campaignIds);


        Core.Domain.CustomReports.ReportType AddReportType(Core.Domain.CustomReports.ReportType reportType);
        long AttachUserReport(UserReport userReport);
        void DetachUserReport(UserReport userReport);
        List<UserReport> GetUserAllReports(long userId);

        List<UserReport> GetUserAllReports(long reportId, bool isOwner);

        /// <summary>
        /// Clears the cache.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Fills the main report.
        /// </summary>
        void FillMainReport();

        /// <summary>
        /// Get All Reports Viewed.
        /// </summary>
        IList<ReportsViewed> GetReportsViewed();

        /// <summary>
        /// Get Reports Viewed.By User Id
        /// </summary>
        IList<ReportsViewed> GetReportsViewedByUserId(long userId);

        /// <summary>
        /// Insert Reports Viewed
        /// </summary>
        long InsertReportsViewed(string reportName, int? customReportTypeId = null);

        /// <summary>
        /// Reports the by hours total.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        IList<ReportByHourTotal> ReportByHourTotals(DateTime start, DateTime end);

        /// <summary>
        /// Reports the by weekday total.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        IList<ReportByWeekdayTotal> ReportByWeekDayTotals(DateTime start, DateTime end);

        /// <summary>
        /// Reports the byReportBuyersByPricesAndStates.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="buyerId">buyerId.</param>
        /// <returns></returns>
        IList<ReportBuyersByPricesAndStates> ReportBuyersByPricesAndStates(DateTime start, DateTime end, long buyerId = 0);


        TotalRemaining GetRemainingsByPeriod(DateTime start, DateTime end, long buyerId = 0, long buyerChannelId = 0);

        TotalRemainingEntity GetRemainingEntities();


        void ValidateFilters(List<long> FilterCampaignIds = null,
            List<long> FilterAffiliateIds = null,
            List<long> FilterAffiliateChannelIds = null,
            List<long> FilterBuyerIds = null,
            List<long> FilterBuyerChannelIds = null,
            bool fillBuyers = true);

        long ValidateFilter(long? campaignId = null, long? affiliateId = null, long? affiliateChannelId = null, long? buyerId = null, long? buyerChannelId = null);

        #endregion Methods
    }
}