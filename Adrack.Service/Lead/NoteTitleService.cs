// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="NoteTitleService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Lead.INoteTitleService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.INoteTitleService" />
    public partial class NoteTitleService : INoteTitleService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_LEAD_NOTE_TITLE_BY_ID = "App.Cache.LeadMain.LeadNote.By.Id-{0}";

        /// <summary>
        /// The cache lead note by lead identifier key
        /// </summary>
        private const string CACHE_LEAD_NOTE_BY_LEAD_ID_KEY = "App.Cache.LeadMain.LeadNote.By.LeadId-{0}";

        
        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_LEAD_PATTERN_KEY = "App.Cache.LeadMain.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// LeadNote
        /// </summary>
        private readonly IRepository<LeadNote> _leadNoteRepository;

        /// <summary>
        /// NoteTitle
        /// </summary>
        private readonly IRepository<NoteTitle> _leadNoteTitleRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="leadNoteTitleRepository">leadNoteRepository</param>
        /// <param name="leadNoteRepository">leadNoteRepository</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Cache Manager</param>
        public NoteTitleService(
                                IRepository<NoteTitle> leadNoteTitleRepository,
                                IRepository<LeadNote> leadNoteRepository,
                                ICacheManager cacheManager,
                                IAppEventPublisher appEventPublisher)
        {
            this._leadNoteTitleRepository = leadNoteTitleRepository;
            this._leadNoteRepository = leadNoteRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get LeadMain By Id
        /// </summary>
        /// <param name="noteId">noteId</param>
        /// <returns>Profile Item</returns>
        public virtual NoteTitle GetNoteTitleById(long noteId)
        {
            if (noteId == 0)
                return null;

            return _leadNoteTitleRepository.GetById(noteId);
        }

        /// <summary>
        /// Get All Leads
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<NoteTitle> GetAllNoteTitles()
        {
                var query = from x in _leadNoteTitleRepository.Table
                            orderby x.Id ascending
                            select x;

                return query.ToList();         
        }

        /// <summary>
        /// Gets all note titles sorted.
        /// </summary>
        /// <returns>IList&lt;NoteTitle&gt;.</returns>
        public virtual IList<NoteTitle> GetAllNoteTitlesSorted()
        {
                var query = from x in _leadNoteTitleRepository.Table
                            orderby x.Title ascending
                            select x;

                return query.ToList();            
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="noteTitle">Profile</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">noteTitle</exception>
        public virtual long InsertNoteTitle(NoteTitle noteTitle)
        {
            if (noteTitle == null)
                throw new ArgumentNullException("noteTitle");

            _leadNoteTitleRepository.Insert(noteTitle);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(noteTitle);

            return noteTitle.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="noteTitle">Profile</param>
        /// <exception cref="ArgumentNullException">noteTitle</exception>
        public virtual void UpdateNoteTitle(NoteTitle noteTitle)
        {
            if (noteTitle == null)
                throw new ArgumentNullException("noteTitle");

            _leadNoteTitleRepository.Update(noteTitle);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(noteTitle);
        }

        /// <summary>
        /// Get Leads GeoData by ID
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadGeoData</returns>

        public virtual LeadNote GetLeadNote(long leadId)
        {
            return (from x in _leadNoteRepository.Table
                    where x.LeadId == leadId
                    orderby x.Created descending
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// Gets the lead notes.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>IList&lt;LeadNote&gt;.</returns>
        public virtual IList<LeadNote> GetLeadNotes(long leadId)
        {
                var query = from x in _leadNoteRepository.Table
                            where x.LeadId == leadId
                            orderby x.Created descending
                            select x;

                return query.ToList();            
        }

        /// <summary>
        /// Inserts the lead note.
        /// </summary>
        /// <param name="leadNote">The lead note.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">leadNote</exception>
        public virtual long InsertLeadNote(LeadNote leadNote)
        {
            if (leadNote == null)
                throw new ArgumentNullException("leadNote");

            _leadNoteRepository.Insert(leadNote);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(leadNote);

            return leadNote.Id;
        }

        /// <summary>
        /// Updates the lead note.
        /// </summary>
        /// <param name="leadNote">The lead note.</param>
        /// <exception cref="ArgumentNullException">leadNote</exception>
        public virtual void UpdateLeadNote(LeadNote leadNote)
        {
            if (leadNote == null)
                throw new ArgumentNullException("leadNote");

            LeadNote ln = _leadNoteRepository.GetById(leadNote.Id);
            ln.LeadId = leadNote.LeadId;
            ln.Note = leadNote.Note;
            ln.NoteTitleId = leadNote.NoteTitleId;

            _leadNoteRepository.Update(ln);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(leadNote);
        }

        #endregion Methods
    }
}