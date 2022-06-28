// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILeadMainService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Lead.Reports;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ILeadMainService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="leadMainId">The lead main identifier.</param>
        /// <returns>Profile Item</returns>
        LeadMain GetLeadMainById(long leadMainId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<LeadMain> GetAllLeads();

        /// <summary>
        /// Get Leads
        /// </summary>
        /// <param name="StartAt">The start at.</param>
        /// <param name="Count">The count.</param>
        /// <returns>Lead Collection</returns>
        IList<LeadMain> GetLeads(int StartAt, int Count);

        /// <summary>
        /// Get LeadsAll
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="FilterEmail">The filter email.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="FilterAffiliateChannel">The filter affiliate channel.</param>
        /// <param name="FilterAffiliateChannelSubId">The filter affiliate channel sub identifier.</param>
        /// <param name="FilterBuyer">The filter buyer.</param>
        /// <param name="FilterBuyerChannel">The filter buyer channel.</param>
        /// <param name="FilterCampaign">The filter campaign.</param>
        /// <param name="Status">The status.</param>
        /// <param name="IP">The ip.</param>
        /// <param name="State">The state.</param>
        /// <param name="FilterFirstName">First name of the filter.</param>
        /// <param name="FilterLastName">Last name of the filter.</param>
        /// <param name="FilterBPrice">The filter b price.</param>
        /// <param name="FilterZipCode">The filter zip code.</param>
        /// <param name="Notes">The notes.</param>
        /// <param name="StartAt">The start at.</param>
        /// <param name="Count">The count.</param>
        /// <returns>Lead Collection</returns>
        IList<LeadMainContent> GetLeadsAll(DateTime dateFrom, DateTime dateTo, long leadId, string FilterEmail, long FilterAffiliate, long FilterAffiliateChannel, string FilterAffiliateChannelSubId, long FilterBuyer, long FilterBuyerChannel, long FilterCampaign, short Status, string IP, string State, string FilterFirstName, string FilterLastName, decimal FilterBPrice, string FilterZipCode, string Notes, int StartAt, int Count);

        IList<LeadMainContent> GetLeadsAll(DateTime dateFrom, DateTime dateTo, long leadId, string FilterEmail, string FilterAffiliates, string FilterAffiliateChannels, string FilterAffiliateChannelSubId, string FilterBuyers, string FilterBuyerChannels, string FilterCampaigns, short Status, string IP, string State, string FilterFirstName, string FilterLastName, decimal FilterBPrice, string FilterZipCode, string Notes, int StartAt, int Count);

        /// <summary>
        /// Get LeadsAll
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="date">The date.</param>
        /// <returns>Lead Collection</returns>
        int GetLeadsCountByDay(long buyerChannelId, DateTime date);

        /// <summary>
        /// Gets the processing leads.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetProcessingLeads();

        /// <summary>
        /// Get LeadsAll By LeadId
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>Lead</returns>
        LeadMainContent GetLeadsAllById(long leadId);

        /// <summary>
        /// Get Leads Count
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="FilterEmail">The filter email.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="FilterAffiliateChannel">The filter affiliate channel.</param>
        /// <param name="FilterBuyer">The filter buyer.</param>
        /// <param name="FilterBuyerChannel">The filter buyer channel.</param>
        /// <param name="FilterCampaign">The filter campaign.</param>
        /// <param name="Status">The status.</param>
        /// <param name="IP">The ip.</param>
        /// <param name="State">The state.</param>
        /// <param name="Notes">The notes.</param>
        /// <returns>int</returns>
        int GetLeadsCount(DateTime dateFrom,
                            DateTime dateTo,
                            string FilterEmail,
                            long FilterAffiliate,
                            long FilterAffiliateChannel,
                            long FilterBuyer,
                            long FilterBuyerChannel,
                            long FilterCampaign,
                            short Status,
                            string IP,
                            string State,
                            string Notes);

        int GetLeadsCount(DateTime dateFrom, 
            DateTime dateTo, 
            string FilterEmail, 
            string FilterAffiliates, 
            string FilterAffiliateChannels, 
            string FilterAffiliateChannelSubId, 
            string FilterBuyers, 
            string FilterBuyerChannels, 
            string FilterCampaigns, 
            short Status, 
            string IP, 
            string State, 
            string FilterFirstName, 
            string FilterLastName, 
            decimal FilterBPrice, 
            string FilterZipCode, 
            string Notes);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        long InsertLeadMain(LeadMain leadMain);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void UpdateLeadMain(LeadMain leadMain);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void DeleteLeadMain(LeadMain leadMain);

        /// <summary>
        /// Get Leads GeoData by ID
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadGeoData</returns>
        LeadGeoData GetLeadGeoData(long leadId);

        /// <summary>
        /// GetBadIPClicksReport
        /// </summary>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="LeadIP">The lead ip.</param>
        /// <param name="ClickIP">The click ip.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>LeadGeoData</returns>
        List<ReportBadIPClicks> GetBadIPClicksReport(
                            DateTime dateFrom,
                            DateTime dateTo,
                            long leadId,
                            string FilterAffiliates,
                            string LeadIP,
                            string ClickIP,
                            int page,
                            int pageSize);

        /// <summary>
        /// GetErrorLeadsReport
        /// </summary>
        /// <param name="ErrorType">Type of the error.</param>
        /// <param name="validator">The validator.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="FilterAffiliate">The filter affiliate.</param>
        /// <param name="FilterAffiliateChannel">The filter affiliate channel.</param>
        /// <param name="FilterBuyer">The filter buyer.</param>
        /// <param name="FilterBuyerChannel">The filter buyer channel.</param>
        /// <param name="ReportType">Type of the report.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>LeadGeoData</returns>
        List<ReportErrorLeads> GetErrorLeadsReport(
                            short ErrorType,
                            short validator,
                            DateTime dateFrom,
                            DateTime dateTo,
                            long leadId,
                            short status,
                            long FilterAffiliate,
                            long FilterAffiliateChannel,
                            long FilterBuyer,
                            long FilterBuyerChannel,
                            long FilterCampaign,
                            string FilterState,
                            decimal FilterMinPrice,
                            short ReportType,
                            int page,
                            int pageSize);

        List<ReportErrorLeads> GetErrorLeadsReport(
                            short ErrorType,
                            short validator,
                            DateTime dateFrom,
                            DateTime dateTo,
                            long leadId,
                            short status,
                            List<long> FilterAffiliate,
                            List<long> FilterAffiliateChannel,
                            List<long> FilterBuyer,
                            List<long> FilterBuyerChannel,
                            List<long> FilterCampaign,
                            List<string> FilterState,
                            decimal FilterMinPrice,
                            short ReportType,
                            int page,
                            int pageSize);

        int GetErrorLeadsReportCount(
                             short ErrorType,
                             short validator,
                             DateTime dateFrom,
                             DateTime dateTo,
                             long leadId,
                             short status,
                             long FilterAffiliate,
                             long FilterAffiliateChannel,
                             long FilterBuyer,
                             long FilterBuyerChannel,
                             long FilterCampaign,
                             string FilterState,
                             decimal FilterMinPrice,
                             short ReportTypee);

        
        /// <summary>
        /// Gets the lead count by buyer.
        /// </summary>
        /// <param name="created">The created.</param>
        /// <param name="buyerid">The buyerid.</param>
        /// <returns>System.Int32.</returns>
        int GetLeadCountByBuyer(DateTime created, long buyerid);

        /// <summary>
        /// Clears the sensitive data.
        /// </summary>
        void ClearSensitiveData();

        // <summary>
        /// <summary>
        /// Gets the next and previous leads Ids.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadIds</returns>
        long[] GetNextPrevLeadId(long leadId);


        #endregion Methods
    }
}