// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IBuyerService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using System.Collections.Generic;
using Adrack.Core;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IBuyerService
    /// </summary>
    public partial interface IBuyerService
    {
        #region Methods

        /// <summary>
        /// Get Buyer By Id
        /// </summary>
        /// <param name="buyerId">The buyer identifier.</param>
        /// <returns>Profile Item</returns>
        Buyer GetBuyerById(long buyerId, bool cached = false);

        /// <summary>
        /// Gets the name of the buyer by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>Buyer.</returns>
        Buyer GetBuyerByName(string name, long exceptId);

        /// <summary>
        /// Gets buyer by account id.
        /// </summary>
        /// <param name="accountid">The account id.</param>
        /// <returns>Buyer.</returns>
        Buyer GetBuyerByAccountId(int accountId);



        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        IList<Buyer> GetAllBuyers(short deleted = 0);

        /// <summary>
        /// Get All Buyers By Status
        /// </summary>
        /// <param name="status"></param>
        /// <param name="deleted"></param>
        /// <returns></returns>
        IList<Buyer> GetAllBuyersByStatus(EntityFilterByStatus status = EntityFilterByStatus.Active,
                                  EntityFilterByDeleted deleted = EntityFilterByDeleted.NotDeleted);

        /// <summary>
        /// Gets all buyers.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Buyer&gt;.</returns>
        IList<Buyer> GetAllBuyers(User user, short deleted = 0);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        /// <returns>System.Int64.</returns>
        long InsertBuyer(Buyer buyer);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        void UpdateBuyer(Buyer buyer);


        /// <summary>
        /// Update buyer list
        /// </summary>
        /// <param name="buyerList">The buyer.</param>

        void UpdateBuyerList(IEnumerable<Buyer> buyerList);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="buyer">The buyer.</param>
        void DeleteBuyer(Buyer buyer);

        List<BuyerInvitation> GetBuyerInvitations(long buyerId);
        long InsertBuyerInvitation(BuyerInvitation buyerInvitation);

        BuyerInvitation GetBuyerInvitationByEmail(string email);
        List<BuyerInvitation> GetBuyerInvitationsByEmail(string email);


        List<BuyerInvitation> GetAllBuyerInvitations();

        /// Update Status
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">buyerInvitationEmail</exception>
        void UpdateInvitationStatus(string email);

        void UpdateBuyerInvitation(BuyerInvitation buyerInvitation);
        void DeleteBuyerInvitation(long invitatationId);

        Buyer GetBuyerByIdWithDeletedStatus(long buyerId, bool considerDeleted = false);
        #endregion Methods
    }
}