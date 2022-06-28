// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILeadFieldsContentService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface ILeadFieldsContentService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="leadFieldsContentId">The lead fields content identifier.</param>
        /// <returns>Profile Item</returns>
        LeadFieldsContent GetLeadFieldsContentById(long leadFieldsContentId);

        /// <summary>
        /// Gets the lead fields content by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadFieldsContent.</returns>
        LeadFieldsContent GetLeadFieldsContentByLeadId(long leadId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        long InsertLeadFieldsContent(LeadFieldsContent leadMain);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void UpdateLeadFieldsContent(LeadFieldsContent leadMain);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void DeleteLeadFieldsContent(LeadFieldsContent leadMain);

        #endregion Methods
    }
}