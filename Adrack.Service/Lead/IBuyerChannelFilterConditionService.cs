// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBuyerChannelFilterConditionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBuyerChannelFilterConditionService
    /// </summary>
    public partial interface IBuyerChannelFilterConditionService
    {
        #region Methods

        BuyerChannelFilterCondition GetFilterConditionById(long Id);

        /// <summary>
        /// Gets the filter conditions by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;BuyerChannelFilterCondition&gt;.</returns>
        IList<BuyerChannelFilterCondition> GetFilterConditionsByBuyerChannelId(long buyerChannelId, long parentId = 0);

        /// <summary>
        /// Gets the filter conditions by buyer channel identifier and campaign template identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>IList&lt;BuyerChannelFilterCondition&gt;.</returns>
        IList<BuyerChannelFilterCondition> GetFilterConditionsByBuyerChannelIdAndCampaignTemplateId(long buyerChannelId, long campaignTemplateId, long parentId = 0);

        bool HasChildren(long Id);

        /// <summary>
        /// Inserts the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        long InsertFilterCondition(BuyerChannelFilterCondition filterCondition);

        /// <summary>
        /// Updates the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        void UpdateFilterCondition(BuyerChannelFilterCondition filterCondition);

        /// <summary>
        /// Deletes the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        void DeleteFilterCondition(BuyerChannelFilterCondition filterCondition);

        /// <summary>
        /// Deletes the filter conditions.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        void DeleteFilterConditions(long buyerChannelId);

        #endregion Methods
    }
}