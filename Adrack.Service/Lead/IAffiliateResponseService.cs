// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAffiliateResponseService.cs" company="Adrack.com">
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
    /// Interface IAffiliateResponseService
    /// </summary>
    public partial interface IAffiliateResponseService
    {
        #region Methods

        void UpdateDatabase();
        void ResetDatabase();

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>Profile Item</returns>
        AffiliateResponse GetAffiliateResponseById(long affiliateId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<AffiliateResponse> GetAllAffiliateResponses();

        /// <summary>
        /// Gets the affiliate responses by affiliate channel identifier.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        /// <returns>IList&lt;AffiliateResponse&gt;.</returns>
        IList<AffiliateResponse> GetAffiliateResponsesByAffiliateChannelId(long affiliateChannelId);

        /// <summary>
        /// Gets the affiliate responses by filters.
        /// </summary>
        /// <param name="AffiliateId">The affiliate identifier.</param>
        /// <param name="AffiliateChannelId">The affiliate channel identifier.</param>
        /// <param name="DateFrom">The date from.</param>
        /// <param name="DateTo">The date to.</param>
        /// <returns>IList&lt;AffiliateResponse&gt;.</returns>
        IList<AffiliateResponse> GetAffiliateResponsesByFilters(long AffiliateId, long AffiliateChannelId, DateTime DateFrom, DateTime DateTo);

        /// <summary>
        /// Gets the affiliate responses by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>IList&lt;AffiliateResponse&gt;.</returns>
        IList<AffiliateResponse> GetAffiliateResponsesByLeadId(long leadId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <returns>System.Int64.</returns>
        long InsertAffiliateResponse(AffiliateResponse affiliate);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        void UpdateAffiliateResponse(AffiliateResponse affiliate);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void DeleteAffiliateResponse(AffiliateResponse profile);

        AffiliateResponse CheckAffiliateResponse(long leadId, decimal minPrice);

        #endregion Methods
    }
}