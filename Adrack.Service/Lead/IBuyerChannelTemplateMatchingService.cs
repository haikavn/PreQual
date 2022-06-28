// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBuyerChannelTemplateMatchingService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBuyerChannelTemplateMatchingService
    /// </summary>
    public partial interface IBuyerChannelTemplateMatchingService
    {
        #region Methods

        /// <summary>
        /// Gets the buyer channel template matching by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>BuyerChannelTemplateMatching.</returns>
        BuyerChannelTemplateMatching GetBuyerChannelTemplateMatchingById(long Id);

        /// <summary>
        /// Gets the buyer channel template matchings by template identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>IList&lt;BuyerChannelTemplateMatching&gt;.</returns>
        IList<BuyerChannelTemplateMatching> GetBuyerChannelTemplateMatchingsByTemplateId(long Id);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="BuyerChannelTemplateMatching">The buyer channel template matching.</param>
        /// <returns>System.Int64.</returns>
        long InsertBuyerChannelTemplateMatching(BuyerChannelTemplateMatching BuyerChannelTemplateMatching);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="BuyerChannelTemplateMatching">The buyer channel template matching.</param>
        void UpdateBuyerChannelTemplateMatching(BuyerChannelTemplateMatching BuyerChannelTemplateMatching);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="BuyerChannelTemplateMatching">The buyer channel template matching.</param>
        void DeleteBuyerChannelTemplateMatching(BuyerChannelTemplateMatching BuyerChannelTemplateMatching);

        /// <summary>
        /// Deletes the buyer channel template matchings by template identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void DeleteBuyerChannelTemplateMatchingsByTemplateId(long id);

        #endregion Methods
    }
}