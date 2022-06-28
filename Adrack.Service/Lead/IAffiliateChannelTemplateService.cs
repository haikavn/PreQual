// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAffiliateChannelTemplateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IAffiliateChannelTemplateService
    /// </summary>
    public partial interface IAffiliateChannelTemplateService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        AffiliateChannelTemplate GetAffiliateChannelTemplateById(long Id);

        /// <summary>
        /// Gets the affiliate channel template.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>AffiliateChannelTemplate.</returns>
        AffiliateChannelTemplate GetAffiliateChannelTemplate(long channelId, long campaignTemplateId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<AffiliateChannelTemplate> GetAllAffiliateChannelTemplates();

        /// <summary>
        /// Gets all affiliate channel templates by affiliate channel identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;AffiliateChannelTemplate&gt;.</returns>
        IList<AffiliateChannelTemplate> GetAllAffiliateChannelTemplatesByAffiliateChannelId(long Id);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateChannelTemplate">The affiliate channel template.</param>
        /// <returns>System.Int64.</returns>
        long InsertAffiliateChannelTemplate(AffiliateChannelTemplate affiliateChannelTemplate);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateChannelTemplate">The affiliate channel template.</param>
        void UpdateAffiliateChannelTemplate(AffiliateChannelTemplate affiliateChannelTemplate);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateChannelTemplate">The affiliate channel template.</param>
        void DeleteAffiliateChannelTemplate(AffiliateChannelTemplate affiliateChannelTemplate);

        /// <summary>
        /// Deletes the affiliate channel templates by affiliate channel identifier.
        /// </summary>
        /// <param name="affiliateChannelId">The affiliate channel identifier.</param>
        void DeleteAffiliateChannelTemplatesByAffiliateChannelId(long affiliateChannelId);

        #endregion Methods
    }
}