// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="AffiliateNoteService.cs" company="Adrack.com">
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
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Class AffiliateNoteService.
    /// Implements the <see cref="Adrack.Service.Lead.IAffiliateNoteService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IAffiliateNoteService" />
    public partial class AffiliateNoteService : IAffiliateNoteService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_AFFILIATE_NOTE_BY_ID_KEY = "App.Cache.AffiliateNote.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_AFFILIATE_NOTE_ALL_KEY = "App.Cache.AffiliateNote.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_AFFILIATE_NOTE_PATTERN_KEY = "App.Cache.AffiliateNote.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<AffiliateNote> _affiliateNoteRepository;

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
        /// <param name="affiliateNoteRepository">The affiliate note repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public AffiliateNoteService(IRepository<AffiliateNote> affiliateNoteRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._affiliateNoteRepository = affiliateNoteRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual AffiliateNote GetAffiliateNoteById(long Id)
        {
            if (Id == 0)
                return null;

            return _affiliateNoteRepository.GetById(Id);
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<AffiliateNote> GetAllAffiliateNotes()
        {
            string key = CACHE_AFFILIATE_NOTE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _affiliateNoteRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets all affiliate notes by affiliate identifier.
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier.</param>
        /// <returns>IList&lt;AffiliateNote&gt;.</returns>
        public virtual IList<AffiliateNote> GetAllAffiliateNotesByAffiliateId(long affiliateId)
        {
                var query = from x in _affiliateNoteRepository.Table
                            where x.AffiliateId == affiliateId
                            select x;

                var profiles = query.ToList();

                return profiles;            
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="affiliateNote">The affiliate note.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">affiliateNote</exception>
        public virtual long InsertAffiliateNote(AffiliateNote affiliateNote)
        {
            if (affiliateNote == null)
                throw new ArgumentNullException("affiliateNote");

            _affiliateNoteRepository.Insert(affiliateNote);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_NOTE_PATTERN_KEY);

            _appEventPublisher.EntityInserted(affiliateNote);
            _cacheManager.ClearRemoteServersCache();
            return affiliateNote.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="affiliateNote">The affiliate note.</param>
        /// <exception cref="ArgumentNullException">affiliateNote</exception>
        public virtual void UpdateAffiliateNote(AffiliateNote affiliateNote)
        {
            if (affiliateNote == null)
                throw new ArgumentNullException("affiliateNote");

            _affiliateNoteRepository.Update(affiliateNote);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_NOTE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(affiliateNote);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="affiliateNote">The affiliate note.</param>
        /// <exception cref="ArgumentNullException">affiliateNote</exception>
        public virtual void DeleteAffiliateNote(AffiliateNote affiliateNote)
        {
            if (affiliateNote == null)
                throw new ArgumentNullException("affiliateNote");

            _affiliateNoteRepository.Delete(affiliateNote);

            _cacheManager.RemoveByPattern(CACHE_AFFILIATE_NOTE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(affiliateNote);
        }

        /// <summary>
        /// Deletes the affiliate notes.
        /// </summary>
        /// <param name="affiliateid">The affiliateid.</param>
        public virtual void DeleteAffiliateNotes(long affiliateid)
        {
            List<AffiliateNote> list = (List<AffiliateNote>)GetAllAffiliateNotesByAffiliateId(affiliateid);
            foreach (var item in list)
            {
                DeleteAffiliateNote(item);
            }
        }

        #endregion Methods
    }
}