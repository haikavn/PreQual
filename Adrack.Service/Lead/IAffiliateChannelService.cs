// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAffiliateChannelService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IAffiliateChannelService
    /// </summary>
    public partial interface IAffiliateChannelService
    {
        #region Methods
        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        AffiliateChannel GetAffiliateChannelById(long Id, bool cached = false);

        /// <summary>
        /// Gets the affiliate channel by key.
        /// </summary>
        /// <param name="affiliateChannelKey">The affiliate channel key.</param>
        /// <returns>AffiliateChannel.</returns>
        AffiliateChannel GetAffiliateChannelByKey(string affiliateChannelKey);

        /// <summary>
        /// Gets the name of the affiliate channel by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="exceptId">The except identifier.</param>
        /// <returns>AffiliateChannel.</returns>
        AffiliateChannel GetAffiliateChannelByName(string name, long exceptId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <param name="deleted">The deleted.</param>
        /// <returns>Profile Collection Item</returns>
        IList<AffiliateChannel> GetAllAffiliateChannels(short deleted = 0);

        /// <summary>
        /// Gets all affiliate channels by campaign identifier.
        /// </summary>
        /// <param name="campaignId">The campaign identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;AffiliateChannel&gt;.</returns>
        IList<AffiliateChannel> GetAllAffiliateChannelsByCampaignId(long campaignId, short deleted = 0);

        /// <summary>
        /// Gets all affiliate channels by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns>IList&lt;AffiliateChannel&gt;.</returns>
        IList<AffiliateChannel> GetAllAffiliateChannelsByAffiliateId(long affiliateId, short deleted = 0);

        IList<long> GetAllAffiliateChannelIds(short deleted = 0);
        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        /// <returns>System.Int64.</returns>
        long InsertAffiliateChannel(AffiliateChannel affiliateChannel);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        void UpdateAffiliateChannel(AffiliateChannel affiliateChannel);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateChannel">The affiliate channel.</param>
        void DeleteAffiliateChannel(AffiliateChannel affiliateChannel);

        void DeleteAffiliateNote(AffiliateChannelNote affiliateChannelNote);

        long InsertAffiliateChannelNote(AffiliateChannelNote affiliateChannelNote);

        void UpdateAffiliateChannelNote(AffiliateChannelNote affiliateChannelNote);

        AffiliateChannelNote GetAffiliateChannelNoteById(long Id);

        IList<AffiliateChannelNote> GetAllAffiliateChannelNotesByAffiliateChannelId(long affiliateChannelId);

        IList<AffiliateChannel> GetAllAffiliateChannelsByMultipleCampaignId(List<long> campaignId, short deleted = 0);

        IList<AffiliateChannel> GetAllAffiliateChannelsByMultipleAffiliateIds(List<long> affiliateIds, short deleted = 0);

        IList<BuyerChannel> GetAttachedBuyerChannels(long Id);

        void AttachBuyerChannel(long affiliateChannelId, long buyerChannelId);

        void DettachBuyerChannel(long affiliateChannelId, long buyerChannelId);


        #endregion Methods
    }
}