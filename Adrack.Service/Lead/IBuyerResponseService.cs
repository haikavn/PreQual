// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBuyerResponseService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBuyerResponseService
    /// </summary>
    public partial interface IBuyerResponseService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>Profile Item</returns>
        BuyerResponse GetBuyerResponseById(long buyerId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<BuyerResponse> GetAllBuyerResponses();

        /// <summary>
        /// Gets the buyer responses by buyer channel identifier.
        /// </summary>
        /// <param name="BuyerChannelId">The buyer channel identifier.</param>
        /// <returns>IList&lt;BuyerResponse&gt;.</returns>
        IList<BuyerResponse> GetBuyerResponsesByBuyerChannelId(long buyerChannelId);

        /// <summary>
        /// Gets the buyer responses by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>IList&lt;BuyerResponse&gt;.</returns>
        IList<BuyerResponse> GetBuyerResponsesByLeadId(long leadId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <returns>System.Int64.</returns>
        long InsertBuyerResponse(BuyerResponse buyer);

        long InsertBuyerResponseList(IEnumerable<BuyerResponse> list);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        void UpdateBuyerResponse(BuyerResponse buyer);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void DeleteBuyerResponse(BuyerResponse profile);

        #endregion Methods
    }
}