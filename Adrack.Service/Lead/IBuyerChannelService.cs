// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBuyerChannelService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBuyerChannelService
    /// </summary>
    public partial interface IBuyerChannelService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        BuyerChannel GetBuyerChannelById(long Id, bool cached = false);

        /// <summary>
        /// Gets buyer channel by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>BuyerChannel.</returns>
        BuyerChannel GetBuyerChannelByName(string name, long exceptId, bool fromCache = true);

        List<BuyerChannel> GetBuyerChannelByContainingName(string name, long exceptId);

        BuyerChannel GetBuyerChannelByUniqueMappingId(string uniqueId);

        BuyerChannel GetBuyerChannelByBuyerIdAndUniqueMappingId(long buyerId, string uniqueId);

        XmlDocument GetBuyerChannelXML(BuyerChannel channel);
        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        IList<BuyerChannel> GetAllBuyerChannels(short deleted = 0);

        IList<BuyerChannel> GetBuyerChannels(long[] ids, short deleted = 0);

        IList<long> GetAllBuyerChannelIds(short deleted = 0);
        /// <summary>
        /// Gets all buyer channels.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        IList<BuyerChannel> GetAllBuyerChannels(long buyerId, short deleted = 0);

        /// <summary>
        /// Gets all buyer channels by order.
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        IList<BuyerChannel> GetAllBuyerChannelsByOrder(short deleted = 0);

        /// <summary>
        /// Gets all buyer channels by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        IList<BuyerChannel> GetAllBuyerChannelsByCampaignId(long campaignId, short deleted = 0);

        /// <summary>
        /// Gets all buyer channels by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="zipCode">The zip code.</param>
        /// <param name="state">The state.</param>
        /// <param name="age">The age.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        BuyerChannel[] GetAllBuyerChannelsByCampaignId(long campaignId, string zipCode, string state, short age, long? pingTreeId = null);

        /// <summary>
        /// Gets all buyer channels by buyer identifier.
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;BuyerChannel&gt;.</returns>
        IList<BuyerChannel> GetAllBuyerChannelsByBuyerId(long buyerId, short deleted = 0);

        IList<BuyerChannel> GetAllBuyerChannelsByMultipleBuyerId(List<long> buyerId, short deleted = 0);

        /// <summary>
        /// Gets the allowed affiliate channels.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;AffiliateChannel&gt;.</returns>
        IList<AffiliateChannel> GetAttachedAffiliateChannels(long Id);

        /// <summary>
        /// Checks the allowed affiliate channel.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CheckAllowedAffiliateChannel(long affiliateChannelId, long buyerChannelId);

        /// <summary>
        /// Updates the allowed.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="add">if set to <c>true</c> [add].</param>
        void UpdateAllowed(long affiliateChannelId, long buyerChannelId, bool add);

        /// <summary>
        /// Clones the specified buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.Int64.</returns>
        long Clone(long buyerChannelId, string name);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyerChannel">The buyer channel.</param>
        /// <returns>System.Int64.</returns>
        long InsertBuyerChannel(BuyerChannel buyerChannel);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyerChannel">The buyer channel.</param>
        void UpdateBuyerChannel(BuyerChannel buyerChannel);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyerChannel">The buyer channel.</param>
        void DeleteBuyerChannel(BuyerChannel buyerChannel);

        /// <summary>
        /// Check Sum buyer channel's LeadAcceptRate By GroupNum.
        /// </summary>
        /// <param name="groupNum">The buyer channel's groupNum</param>
        /// <param name="buyerChannelId">The buyer channel Id</param>
        /// <param name="leadAcceptRate">The buyer channel's leadAcceptRate</param>        
        bool CheckSumLeadAcceptRateByGroupNum(int groupNum, long buyerChannelId, float leadAcceptRate = 0);

        List<BuyerChannelNote> GetAllBuyerChannelNotesByBuyerChannelId(long buyerChannelId);

        void DeleteBuyerChannelNote(BuyerChannelNote buyerChannelNote);

        long InsertBuyerChannelNote(BuyerChannelNote buyerChannelNote);

        void UpdateBuyerChannelNote(BuyerChannelNote buyerChannelNote);

        BuyerChannelNote GetBuyerChannelNoteById(long Id);

        IList<BuyerChannelScheduleDay> GetBuyerChannelScheduleDays(long buyerChannelId, short dayValue = 0, bool fromCache = true);

        BuyerChannelScheduleDay GetBuyerChannelScheduleDay(long id);


        IList<BuyerChannelScheduleTimePeriod> GetBuyerChannelScheduleTimePeriod(long scheduleDayId, bool fromCache = true);

        long InsertBuyerChannelScheduleDay(BuyerChannelScheduleDay buyerChannelScheduleDay);

        void UpdateBuyerChannelScheduleDay(BuyerChannelScheduleDay buyerChannelScheduleDay);

        long InsertBuyerChannelScheduleTimePeriod(
            BuyerChannelScheduleTimePeriod buyerChannelScheduleTimePeriod);
        BuyerChannelScheduleTimePeriod InsertBuyerChannelScheduleTimePeriod(long scheduleDayId, int fromTime = 0,
            int toTime = 0, int quantity = 0, int postedWait = 0, int soldWait = 0, int hourMax = 0, decimal price = 0,
            short leadStatus = -1);
      
        BuyerChannelScheduleTimePeriod UpdateBuyerChannelScheduleTimePeriod(long id, int fromTime = 0, int toTime = 0,
            int quantity = 0, int postedWait = 0, int soldWait = 0, int hourMax = 0, decimal price = 0,
            short leadStatus = -1);

        void DeleteBuyerChannelScheduleTimePeriod(long id);

        void ResetBuyerChannelSchedule(long id);
        void ResetBuyerChannelHolidays(long id, int year);


        IList<BuyerChannelHoliday> GetBuyerChannelHolidays(long buyerChannelId, int year = 0, bool fromCache = true);

        BuyerChannelHoliday GetBuyerChannelHoliday(long buyerChannelId, DateTime date);

        BuyerChannelHoliday GetBuyerChannelHolidayById(long Id);

        long InsertBuyerChannelHoliday(BuyerChannelHoliday buyerChannelHoliday);
      
        void UpdateBuyerChannelHoliday(BuyerChannelHoliday buyerChannelHoliday);

        void DeleteBuyerChannelHoliday(BuyerChannelHoliday buyerChannelHoliday);


        #endregion Methods
    }
}