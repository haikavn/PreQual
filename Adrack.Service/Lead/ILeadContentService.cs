// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILeadContentService.cs" company="Adrack.com">
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
    public partial interface ILeadContentService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="leadContentId">The lead content identifier.</param>
        /// <returns>Profile Item</returns>
        LeadContent GetLeadContentById(long leadContentId);

        /// <summary>
        /// Gets the lead content by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadContent.</returns>
        LeadContent GetLeadContentByLeadId(long leadId);

        /// <summary>
        /// Gets the dublicate lead.
        /// </summary>
        /// <param name="ssn">The SSN.</param>
        /// <param name="created">The created.</param>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>LeadContent.</returns>
        LeadContent GetDublicateLead(string ssn, DateTime created, long affiliateId);

        /// <summary>
        /// Checks for dublicate.
        /// </summary>
        /// <param name="ssn">The SSN.</param>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadContent.</returns>
        List<LeadContent> CheckForDublicate(string ssn, long leadId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<LeadContent> GetAllLeads();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        /// <returns>System.Int64.</returns>
        long InsertLeadContent(LeadContent leadMain);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void UpdateLeadContent(LeadContent leadMain);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="leadMain">The lead main.</param>
        void DeleteLeadContent(LeadContent leadMain);

        #endregion Methods
    }
}