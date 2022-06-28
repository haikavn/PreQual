// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadFieldsContentService.cs" company="Adrack.com">
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
using System.Data;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Lead Service
    /// Implements the <see cref="Adrack.Service.Lead.ILeadFieldsContentService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ILeadFieldsContentService" />
    public partial class LeadFieldsContentService : ILeadFieldsContentService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_LEAD_BY_ID_KEY = "App.Cache.LeadFieldsContent.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_LEAD_ALL_KEY = "App.Cache.LeadFieldsContent.All";

        
        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_LEAD_PATTERN_KEY = "App.Cache.LeadFieldsContent.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<LeadFieldsContent> _leadFieldsContentRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The data provider
        /// </summary>
        private readonly IDataProvider _dataProvider;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="leadFieldsContentRepository">The lead fields content repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        public LeadFieldsContentService(IRepository<LeadFieldsContent> leadFieldsContentRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._leadFieldsContentRepository = leadFieldsContentRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        // <summary>
        /// <summary>
        /// Gets the lead fields content by identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadFieldsContent.</returns>
        public virtual LeadFieldsContent GetLeadFieldsContentById(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_LEAD_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () => { return _leadFieldsContentRepository.GetById(leadId); });
        }

        /// <summary>
        /// Gets the lead fields content by lead identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadFieldsContent.</returns>
        public virtual LeadFieldsContent GetLeadFieldsContentByLeadId(long leadId)
        {
            if (leadId == 0)
                return null;

            var query = from x in _leadFieldsContentRepository.Table
                        where x.LeadId == leadId
                        select x;

            var profiles = query.FirstOrDefault();

            return profiles;
        }

        /// <summary>
        /// Inserts the content of the lead fields.
        /// </summary>
        /// <param name="leadFieldsContent">Content of the lead fields.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">leadFieldsContent</exception>
        public virtual long InsertLeadFieldsContent(LeadFieldsContent leadFieldsContent)
        {
            if (leadFieldsContent == null)
                throw new ArgumentNullException("leadFieldsContent");

            _leadFieldsContentRepository.Insert(leadFieldsContent);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(leadFieldsContent);

            return leadFieldsContent.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadFieldsContent">Content of the lead fields.</param>
        /// <exception cref="ArgumentNullException">leadFieldsContent</exception>
        public virtual void UpdateLeadFieldsContent(LeadFieldsContent leadFieldsContent)
        {
            if (leadFieldsContent == null)
                throw new ArgumentNullException("leadFieldsContent");

            _leadFieldsContentRepository.Update(leadFieldsContent);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(leadFieldsContent);
        }

        /// <summary>
        /// Delete LeadFieldsContent
        /// </summary>
        /// <param name="leadFieldsContent">Content of the lead fields.</param>
        /// <exception cref="ArgumentNullException">leadFieldsContent</exception>
        public virtual void DeleteLeadFieldsContent(LeadFieldsContent leadFieldsContent)
        {
            if (leadFieldsContent == null)
                throw new ArgumentNullException("leadFieldsContent");

            _leadFieldsContentRepository.Delete(leadFieldsContent);

            _cacheManager.RemoveByPattern(CACHE_LEAD_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(leadFieldsContent);
        }

        #endregion Methods
    }
}