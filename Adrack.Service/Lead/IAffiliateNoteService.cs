// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IAffiliateNoteService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IAffiliateNoteService
    /// </summary>
    public partial interface IAffiliateNoteService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        AffiliateNote GetAffiliateNoteById(long Id);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<AffiliateNote> GetAllAffiliateNotes();

        /// <summary>
        /// Gets all affiliate notes by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>IList&lt;AffiliateNote&gt;.</returns>
        IList<AffiliateNote> GetAllAffiliateNotesByAffiliateId(long affiliateId);

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateNote">The affiliate note.</param>
        /// <returns>System.Int64.</returns>
        long InsertAffiliateNote(AffiliateNote affiliateNote);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateNote">The affiliate note.</param>
        void UpdateAffiliateNote(AffiliateNote affiliateNote);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateNote">The affiliate note.</param>
        void DeleteAffiliateNote(AffiliateNote affiliateNote);

        /// <summary>
        /// Deletes the affiliate notes.
        /// </summary>
        /// <param name="affiliateid">The affiliateid.</param>
        void DeleteAffiliateNotes(long affiliateid);

        #endregion Methods
    }
}