// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBuyerChannelTemplateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBuyerChannelTemplateService
    /// </summary>
    public partial interface IBuyerChannelTemplateService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        BuyerChannelTemplate GetBuyerChannelTemplateById(long Id);

        /// <summary>
        /// Gets the buyer channel template.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <param name="field">The field.</param>
        /// <param name="section">The section.</param>
        /// <returns>BuyerChannelTemplate.</returns>
        BuyerChannelTemplate GetBuyerChannelTemplate(long buyerChannelId, string field, string section);

        /// <summary>
        /// Gets the buyer channel template.
        /// </summary>
        /// <param name="campaignTemplateId">The campaign template identifier.</param>
        /// <returns>BuyerChannelTemplate.</returns>
        BuyerChannelTemplate GetBuyerChannelTemplate(long campaignTemplateId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<BuyerChannelTemplate> GetAllBuyerChannelTemplates();

        /// <summary>
        /// Gets all buyer channel templates by buyer channel identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;BuyerChannelTemplate&gt;.</returns>
        IList<BuyerChannelTemplate> GetAllBuyerChannelTemplatesByBuyerChannelId(long Id);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyerChannelTemplate">The buyer channel template.</param>
        /// <returns>System.Int64.</returns>
        long InsertBuyerChannelTemplate(BuyerChannelTemplate buyerChannelTemplate);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyerChannelTemplate">The buyer channel template.</param>
        void UpdateBuyerChannelTemplate(BuyerChannelTemplate buyerChannelTemplate);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyerChannelTemplate">The buyer channel template.</param>
        void DeleteBuyerChannelTemplate(BuyerChannelTemplate buyerChannelTemplate);

        /// <summary>
        /// Deletes the buyer channel templates by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        void DeleteBuyerChannelTemplatesByBuyerChannelId(long buyerChannelId);

        #endregion Methods
    }
}