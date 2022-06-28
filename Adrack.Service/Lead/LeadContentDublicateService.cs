// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LeadContentDublicateService.cs" company="Adrack.com">
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
    /// Implements the <see cref="Adrack.Service.Lead.ILeadContentDublicateService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.ILeadContentDublicateService" />
    public partial class LeadContentDublicateService : ILeadContentDublicateService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_LEAD_MAIN_DUBLICATE_BY_ID_KEY = "App.Cache.LeadContentDublicate.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_LEAD_MAIN_DUBLICATE_ALL_KEY = "App.Cache.LeadContentDublicate.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_LEAD_MAIN_DUBLICATE_PATTERN_KEY = "App.Cache.LeadContentDublicate.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<LeadContentDublicate> _leadContentDublicateRepository;

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
        /// <param name="leadContentDublicateRepository">The lead content dublicate repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        public LeadContentDublicateService(IRepository<LeadContentDublicate> leadContentDublicateRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IDataProvider dataProvider)
        {
            this._leadContentDublicateRepository = leadContentDublicateRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
            this._dataProvider = dataProvider;
        }

        #endregion Constructor

        #region Methods

        // <summary>
        /// <summary>
        /// Gets the lead content dublicate by identifier.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadContentDublicate.</returns>
        public virtual LeadContentDublicate GetLeadContentDublicateById(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_LEAD_MAIN_DUBLICATE_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () =>
            {
                return _leadContentDublicateRepository.GetById(leadId);
            });
        }

        /// <summary>
        /// GetLeadContentDublicateByLeadId
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>LeadContentDublicate</returns>
        public virtual IList<LeadContentDublicate> GetLeadContentDublicateByLeadId(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_LEAD_MAIN_DUBLICATE_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _leadContentDublicateRepository.Table
                            where x.LeadId == leadId
                            select x;
                var dublicateLeads = query.ToList();

                return dublicateLeads;
            });
        }

        /// <summary>
        /// Gets the lead content dublicate by SSN.
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <param name="ssn">The SSN.</param>
        /// <returns>IList&lt;LeadContentDublicate&gt;.</returns>
        public virtual IList<LeadContentDublicate> GetLeadContentDublicateBySSN(long leadId, string ssn)
        {
            var leadIdParam = _dataProvider.GetParameter();
            leadIdParam.ParameterName = "leadId";
            leadIdParam.Value = leadId;
            leadIdParam.DbType = DbType.Int64;

            var ssnParam = _dataProvider.GetParameter();
            ssnParam.ParameterName = "ssn";
            ssnParam.Value = ssn;
            ssnParam.DbType = DbType.String;

            return _leadContentDublicateRepository.GetDbClientContext().SqlQuery<LeadContentDublicate>("EXECUTE [dbo].[GetLeadContentDublicateBySSN] @ssn", ssnParam).ToList();
        }

        /// <summary>
        /// Get All Leads
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<LeadContentDublicate> GetAllLeads()
        {
            string key = CACHE_LEAD_MAIN_DUBLICATE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _leadContentDublicateRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="leadContentDublicate">The lead content dublicate.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">leadContentDublicate</exception>
        public virtual long InsertLeadContentDublicate(LeadContentDublicate leadContentDublicate)
        {
            if (leadContentDublicate == null)
                throw new ArgumentNullException("leadContentDublicate");

            _leadContentDublicateRepository.Insert(leadContentDublicate);

            _cacheManager.RemoveByPattern(CACHE_LEAD_MAIN_DUBLICATE_PATTERN_KEY);
            
            _appEventPublisher.EntityInserted(leadContentDublicate);

            return leadContentDublicate.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="leadContentDublicate">The lead content dublicate.</param>
        /// <exception cref="ArgumentNullException">leadContentDublicate</exception>
        public virtual void UpdateLeadContentDublicate(LeadContentDublicate leadContentDublicate)
        {
            if (leadContentDublicate == null)
                throw new ArgumentNullException("leadContentDublicate");

            _leadContentDublicateRepository.Update(leadContentDublicate);

            _cacheManager.RemoveByPattern(CACHE_LEAD_MAIN_DUBLICATE_PATTERN_KEY);
            
            _appEventPublisher.EntityUpdated(leadContentDublicate);
        }

        /// <summary>
        /// Delete LeadContentDublicate
        /// </summary>
        /// <param name="leadContentDublicate">The lead content dublicate.</param>
        /// <exception cref="ArgumentNullException">leadContentDublicate</exception>
        public virtual void DeleteLeadContentDublicate(LeadContentDublicate leadContentDublicate)
        {
            if (leadContentDublicate == null)
                throw new ArgumentNullException("leadContentDublicate");

            _leadContentDublicateRepository.Delete(leadContentDublicate);

            _cacheManager.RemoveByPattern(CACHE_LEAD_MAIN_DUBLICATE_PATTERN_KEY);
            
            _appEventPublisher.EntityDeleted(leadContentDublicate);
        }

        #endregion Methods
    }
}