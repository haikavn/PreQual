// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="RefundedLeadsService.cs" company="Adrack.com">
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
    /// Represents a RefundedLeadsService
    /// Implements the <see cref="Adrack.Service.Lead.IRefundedLeadsService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IRefundedLeadsService" />
    public partial class RefundedLeadsService : IRefundedLeadsService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PROFILE_BY_ID_KEY = "App.Cache.RefundedLeads.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PROFILE_ALL_KEY = "App.Cache.RefundedLeads.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROFILE_PATTERN_KEY = "App.Cache.RefundedLeads.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<RefundedLeads> _refundedLeadsRepository;

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
        /// <param name="refundedLeadsRepository">The refunded leads repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        public RefundedLeadsService(IRepository<RefundedLeads> refundedLeadsRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher)
        {
            this._refundedLeadsRepository = refundedLeadsRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;
        }

        #endregion Constructor

        #region Methods

        // <summary>
        /// <summary>
        /// Get RefundedLeads By Id
        /// </summary>
        /// <param name="leadId">The lead identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual RefundedLeads GetRefundedLeadById(long leadId)
        {
            if (leadId == 0)
                return null;

            string key = string.Format(CACHE_PROFILE_BY_ID_KEY, leadId);

            return _cacheManager.Get(key, () => { return _refundedLeadsRepository.GetById(leadId); });
        }

        /// <summary>
        /// Get All Leads
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<RefundedLeads> GetAllRefundedLeads()
        {
            string key = CACHE_PROFILE_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _refundedLeadsRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Inserts the refunded lead.
        /// </summary>
        /// <param name="refundedLeads">The refunded leads.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">refundedLeads</exception>
        public virtual long InsertRefundedLead(RefundedLeads refundedLeads)
        {
            if (refundedLeads == null)
                throw new ArgumentNullException("refundedLeads");

            _refundedLeadsRepository.Insert(refundedLeads);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(refundedLeads);

            return refundedLeads.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="refundedLeads">The refunded leads.</param>
        /// <exception cref="ArgumentNullException">refundedLeads</exception>
        public virtual void UpdateRefundedLead(RefundedLeads refundedLeads)
        {
            if (refundedLeads == null)
                throw new ArgumentNullException("refundedLeads");

            _refundedLeadsRepository.Update(refundedLeads);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityUpdated(refundedLeads);
        }

        /// <summary>
        /// Delete LeadMain
        /// </summary>
        /// <param name="refundedLeads">The refunded leads.</param>
        /// <exception cref="ArgumentNullException">refundedLeads</exception>
        public virtual void DeleteRefundedLead(RefundedLeads refundedLeads)
        {
            if (refundedLeads == null)
                throw new ArgumentNullException("refundedLeads");

            _refundedLeadsRepository.Delete(refundedLeads);

            _cacheManager.RemoveByPattern(CACHE_PROFILE_PATTERN_KEY);
            _cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityDeleted(refundedLeads);
        }

        #endregion Methods
    }
}