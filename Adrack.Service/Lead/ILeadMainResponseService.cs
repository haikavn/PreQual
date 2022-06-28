// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILeadMainResponseService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ILeadMainResponseService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="leadMainResponseId">The lead main response identifier.</param>
        /// <returns>Profile Item</returns>
        LeadMainResponse GetLeadMainResponseById(long leadMainResponseId);

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>Profile Item</returns>
        IList<LeadResponse> GetLeadMainResponseByLeadId(long leadId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<LeadMainResponse> GetAllLeadResponses();

        /// <summary>
        /// Gets the leads count by period.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="status">The status.</param>
        /// <returns>System.Int32.</returns>
        int GetLeadsCountByPeriod(long buyerChannelId, DateTime fromDate, DateTime toDate, short status);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        long InsertLeadMainResponse(LeadMainResponse leadMain);

        long InsertLeadMainResponseList(IEnumerable<LeadMainResponse> leadMain);
        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void UpdateLeadMainResponse(LeadMainResponse leadMain);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void DeleteLeadMainResponse(LeadMainResponse leadMain);

        /// <summary>
        /// GetLeadMainResponsesByLeadIdBuyerId
        /// </summary>
        /// <param name="LeadId">The lead identifier.</param>
        /// <param name="BuyerId">The buyer identifier.</param>
        /// <returns>Profile Collection Item</returns>
        LeadMainResponse GetLeadMainResponsesByLeadIdBuyerId(long LeadId, long BuyerId);

        /// <summary>
        /// Gets the dublicate lead by buyer.
        /// </summary>
        /// <param name="ssn">The SSN.</param>
        /// <param name="created">The created.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="fromBuyer">if set to <c>true</c> [from buyer].</param>
        /// <returns>bool.</returns>
        bool GetDublicateLeadByBuyer(string ssn, DateTime created, long id, bool fromBuyer, long leadId = 0);

        LeadMainResponse GetLeadMainResponsesByLeadIdBuyerChannelId(long LeadId, long BuyerChannelId);

        #endregion Methods
    }
}