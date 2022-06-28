// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="INoteTitleService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// </summary>
    public partial interface INoteTitleService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="noteId">Profile Identifier</param>
        /// <returns>Profile Item</returns>
        NoteTitle GetNoteTitleById(long noteId);

        /// <summary>
        /// GetAllLeadNotes
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<NoteTitle> GetAllNoteTitles();

        /// <summary>
        /// Gets all note titles sorted.
        /// </summary>
        /// <returns>IList&lt;NoteTitle&gt;.</returns>
        IList<NoteTitle> GetAllNoteTitlesSorted();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="noteTitle">Profile</param>
        /// <returns>System.Int64.</returns>
        long InsertNoteTitle(NoteTitle noteTitle);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="noteTitle">Profile</param>
        void UpdateNoteTitle(NoteTitle noteTitle);

        /// <summary>
        /// Gets the lead note.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadNote.</returns>
        LeadNote GetLeadNote(long leadId);

        /// <summary>
        /// Gets the lead notes.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>IList&lt;LeadNote&gt;.</returns>
        IList<LeadNote> GetLeadNotes(long leadId);

        /// <summary>
        /// Inserts the lead note.
        /// </summary>
        /// <param name="leadNote">The lead note.</param>
        /// <returns>System.Int64.</returns>
        long InsertLeadNote(LeadNote leadNote);

        /// <summary>
        /// Updates the lead note.
        /// </summary>
        /// <param name="leadNote">The lead note.</param>
        void UpdateLeadNote(LeadNote leadNote);

        #endregion Methods
    }
}