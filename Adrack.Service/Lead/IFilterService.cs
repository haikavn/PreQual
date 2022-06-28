// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IFilterService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IFilterService
    /// </summary>
    public partial interface IFilterService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        Filter GetFilterById(long Id);

        /// <summary>
        /// Get filter condition By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Filtet condition Item</returns>
        FilterCondition GetFilterConditionById(long Id);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<Filter> GetAllFilters();

        /// <summary>
        /// Gets the filter conditions by filter identifier.
        /// </summary>
        /// <param name="filterId">The filter identifier.</param>
        /// <returns>IList&lt;FilterCondition&gt;.</returns>
        IList<FilterCondition> GetFilterConditionsByFilterId(long filterId, long parentId = 0);

        IList<FilterCondition> GetFilterConditionsByCampaignFieldId(long filterId, long campaignFieldId);


        /// <summary>
        /// Gets the filters by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <returns>IList&lt;Filter&gt;.</returns>
        IList<Filter> GetFiltersByCampaignId(long campaignId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>System.Int64.</returns>
        long InsertFilter(Filter filter);

        /// <summary>
        /// Inserts the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        void InsertFilterCondition(FilterCondition filterCondition);

        /// <summary>
        /// Update filter set
        /// </summary>
        /// <param name="filter">The filter.</param>
        void UpdateFilter(Filter filter);

        /// <summary>
        /// Update filter set condition
        /// </summary>
        /// <param name="filter">The filter.</param>
        void UpdateFilterCondition(FilterCondition filterCondition);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="filter">The filter.</param>
        void DeleteFilter(Filter filter);

        /// <summary>
        /// Deletes the filter condition.
        /// </summary>
        /// <param name="filterCondition">The filter condition.</param>
        void DeleteFilterCondition(FilterCondition filterCondition);

        /// <summary>
        /// Deletes the filter conditions.
        /// </summary>
        /// <param name="filterId">The filter identifier.</param>
        void DeleteFilterConditions(long filterId);

        #endregion Methods
    }
}