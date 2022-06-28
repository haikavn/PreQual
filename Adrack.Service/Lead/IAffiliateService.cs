// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAffiliateService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial class StatusCountsClass
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the counts.
        /// </summary>
        /// <value>The counts.</value>
        public int Counts { get; set; }
    }

    /// <summary>
    /// Interface IAffiliateService
    /// </summary>
    public partial interface IAffiliateService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>Profile Item</returns>
        Affiliate GetAffiliateById(long affiliateId, bool cache);

        /// <summary>
        /// Gets the name of the affiliate by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>Affiliate.</returns>
        Affiliate GetAffiliateByName(string name, long exceptId);


        /// <summary>
        /// Check Affiliate Name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Affiliate.</returns>
        Affiliate CheckAffiliateName(string name);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        IList<Affiliate> GetAllAffiliates(short deleted = 0);

        /// <summary>
        /// Gets all affiliates.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Affiliate&gt;.</returns>
        IList<Affiliate> GetAllAffiliates(User user, short deleted = 0);

        /// <summary>
        /// Gets all affiliates.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="deleted">The deleted.</param>
        /// <param name="status">The status.</param>
        /// <returns>IList&lt;Affiliate&gt;.</returns>
        IList<Affiliate> GetAllAffiliates(string name
            , EntityFilterByStatus status  = EntityFilterByStatus.Active
            ,EntityFilterByDeleted deleted = EntityFilterByDeleted.NotDeleted);


        /// <summary>
        /// Get Affiliates By User.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="status">The status.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;Affiliate&gt;.</returns>
        IList<Affiliate> GetAffiliatesByUser(string name
            , EntityFilterByStatus status = EntityFilterByStatus.Active
            , EntityFilterByDeleted deleted = EntityFilterByDeleted.NotDeleted);

        /// <summary>
        /// GetAffiliatesStatusCounts
        /// </summary>
        /// <returns>List KeyValuePair</returns>
        IList<StatusCountsClass> GetAffiliatesStatusCounts();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        /// <returns>System.Int64.</returns>
        long InsertAffiliate(Affiliate affiliate);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliate">The affiliate.</param>
        void UpdateAffiliate(Affiliate affiliate);

        /// <summary>
        /// Insert Affiliate Invitation
        /// </summary>
        /// <param name="affiliateInvitation">The affiliate.</param>
        /// <returns>System.Int64.</returns>
        long InsertAffiliateInvitation(AffiliateInvitation affiliateInvitation);

        /// <summary>
        /// Update Status
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateInvitationEmail</exception>
        void UpdateInvitationStatus(string email);

        /// <summary>
        /// Update Affiliate Invitation
        /// </summary>
        /// <param name="affiliateInvitation">The affiliate.</param>
        /// <returns>void.</returns>
        void UpdateAffiliateInvitation(AffiliateInvitation affiliateInvitation);

        /// <summary>
        /// Get Affiliate Invitations
        /// </summary>
        /// <param name="affiliateId">The affiliate id</param>
        /// <param name="user">User</param>
        List<AffiliateInvitation> GetAffiliateInvitations(long affiliateId);

        List<AffiliateInvitation> GetAllAffiliateInvitations();

        void DeleteAffiliateInvitation(long invitatationId);


        AffiliateInvitation GetAffiliateInvitation(long affiliateId, string email);

        AffiliateInvitation GetAffiliateInvitationByEmail(string email);
        List<AffiliateInvitation> GetAffiliateInvitationsByEmail(string email);


        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void DeleteAffiliate(Affiliate profile);

        #endregion Methods
    }
}