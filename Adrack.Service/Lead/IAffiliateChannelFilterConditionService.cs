// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAffiliateChannelFilterConditionService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IAffiliateChannelFilterConditionService
    /// </summary>
    public partial interface IAffiliateChannelFilterConditionService
    {
        #region Methods

        /// <summary>
        /// Gets the filter conditions by affiliate channel identifier.
        /// </summary>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        /// <returns>IList&lt;AffiliateChannelFilterCondition&gt;.</returns>
        IList<AffiliateChannelFilterCondition> GetFilterConditionsByAffiliateChannelId(long AffiliateChannelId, long parentId = 0);

        /// <summary>
        /// Gets the filter conditions by affiliate channel identifier and campaign template identifier.
        /// </summary>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>IList&lt;AffiliateChannelFilterCondition&gt;.</returns>
        IList<AffiliateChannelFilterCondition> GetFilterConditionsByAffiliateChannelIdAndCampaignTemplateId(long AffiliateChannelId, long campaignTemplateId, long parentId = 0);

        bool HasChildren(long Id);

        /// <summary>
        /// Get the filter condition.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        AffiliateChannelFilterCondition GetFilterConditionById(long Id);

        /// <summary>
        /// Inserts the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        void InsertFilterCondition(AffiliateChannelFilterCondition filterCondition);

        /// <summary>
        /// Update the filter condition.
        /// </summary>
        /// <param name="filterCondition"></param>
        void UpdateFilterCondition(AffiliateChannelFilterCondition filterCondition);

        /// <summary>
        /// Deletes the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        void DeleteFilterCondition(AffiliateChannelFilterCondition filterCondition);

        /// <summary>
        /// Deletes the filter conditions.
        /// </summary>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        void DeleteFilterConditions(long AffiliateChannelId);

        #endregion Methods
    }
}