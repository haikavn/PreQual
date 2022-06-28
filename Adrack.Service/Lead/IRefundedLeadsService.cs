// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IRefundedLeadsService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Refunded Leads Service
    /// </summary>
    public partial interface IRefundedLeadsService
    {
        #region Methods

        /// <summary>
        /// Get RefundedLeads By Id
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>Profile Item</returns>
        RefundedLeads GetRefundedLeadById(long leadId);

        /// <summary>
        /// Get All RefundedLeads
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<RefundedLeads> GetAllRefundedLeads();

        /// <summary>
        /// Insert RefundedLeads
        /// </summary>
        /// <param name="refundedLead">The refunded lead.</param>
        /// <returns>System.Int64.</returns>
        long InsertRefundedLead(RefundedLeads refundedLead);

        /// <summary>
        /// Update RefundedLeads
        /// </summary>
        /// <param name="refundedLeadMain">The refunded lead main.</param>
        void UpdateRefundedLead(RefundedLeads refundedLeadMain);

        /// <summary>
        /// Delete RefundedLeads
        /// </summary>
        /// <param name="refundedLead">The refunded lead.</param>
        void DeleteRefundedLead(RefundedLeads refundedLead);

        #endregion Methods
    }
}