// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ProcessingLogService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Cache;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Helpers;
using Adrack.Service.Infrastructure.ApplicationEvent;
using Adrack.Service.Membership;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Represents a Profile Service
    /// Implements the <see cref="Adrack.Service.Lead.IProcessingLogService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Lead.IProcessingLogService" />
    public partial class ProcessingLogService : IProcessingLogService
    {
        #region Constants

        /// <summary>
        /// Cache Profile By Id Key
        /// </summary>
        private const string CACHE_PROCESSING_LOG_BY_ID_KEY = "App.Cache.ProcessingLog.By.Id-{0}";

        /// <summary>
        /// Cache Profile All Key
        /// </summary>
        private const string CACHE_PROCESSING_LOG_ALL_KEY = "App.Cache.ProcessingLog.All";

        /// <summary>
        /// Cache Profile Pattern Key
        /// </summary>
        private const string CACHE_PROCESSING_LOG_PATTERN_KEY = "App.Cache.ProcessingLog.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Profile
        /// </summary>
        private readonly IRepository<ProcessingLog> _processingLogRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Application Event Publisher
        /// </summary>
        private readonly IAppEventPublisher _appEventPublisher;

        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService _userService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Profile Service
        /// </summary>
        /// <param name="processingLogRepository">The processing log repository.</param>
        /// <param name="cacheManager">Cache Manager</param>
        /// <param name="appEventPublisher">Application Event Publisher</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userService">The user service.</param>
        public ProcessingLogService(IRepository<ProcessingLog> processingLogRepository, ICacheManager cacheManager, IAppEventPublisher appEventPublisher, IUserService userService)
        {
            this._processingLogRepository = processingLogRepository;
            this._cacheManager = cacheManager;
            this._appEventPublisher = appEventPublisher;

            this._userService = userService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="processingLogId">The processing log identifier.</param>
        /// <returns>Profile Item</returns>
        public virtual ProcessingLog GetProcessingLogById(long processingLogId)
        {
            if (processingLogId == 0)
                return null;

            string key = string.Format(CACHE_PROCESSING_LOG_BY_ID_KEY, processingLogId);

            return _cacheManager.Get(key, () => { return _processingLogRepository.GetById(processingLogId); });
        }

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        public virtual IList<ProcessingLog> GetAllProcessingLogs()
        {
            string key = CACHE_PROCESSING_LOG_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _processingLogRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;
            });
        }

        /// <summary>
        /// Gets all processing logs.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>IList&lt;ProcessingLog&gt;.</returns>
        public virtual IList<ProcessingLog> GetAllProcessingLogs(User user)
        {
            long managerId = 0;
            long processingLogId = 0;

            if (user != null && user.UserType != SharedData.BuiltInUserTypeId)
            {
                managerId = user.Id;
                if (user.UserType == SharedData.AffiliateUserTypeId)
                    processingLogId = user.ParentId;
            }

                var query = from x in _processingLogRepository.Table
                            orderby x.Id descending
                            select x;

                var profiles = query.ToList();

                return profiles;           
        }

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="processingLog">The processing log.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="ArgumentNullException">processingLog</exception>
        public virtual long InsertProcessingLog(ProcessingLog processingLog)
        {
            if (processingLog == null)
                throw new ArgumentNullException("processingLog");

            _processingLogRepository.Insert(processingLog);

            _cacheManager.RemoveByPattern(CACHE_PROCESSING_LOG_PATTERN_KEY);
            //_cacheManager.ClearRemoteServersCache();
            _appEventPublisher.EntityInserted(processingLog);

            return processingLog.Id;
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="processingLog">The processing log.</param>
        /// <exception cref="ArgumentNullException">processingLog</exception>
        public virtual void UpdateProcessingLog(ProcessingLog processingLog)
        {
            if (processingLog == null)
                throw new ArgumentNullException("processingLog");

            _processingLogRepository.Update(processingLog);

            _cacheManager.RemoveByPattern(CACHE_PROCESSING_LOG_PATTERN_KEY);            
            _appEventPublisher.EntityUpdated(processingLog);
        }

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="processingLog">The processing log.</param>
        /// <exception cref="ArgumentNullException">processingLog</exception>
        public virtual void DeleteProcessingLog(ProcessingLog processingLog)
        {
            if (processingLog == null)
                throw new ArgumentNullException("processingLog");

            _processingLogRepository.Delete(processingLog);

            _cacheManager.RemoveByPattern(CACHE_PROCESSING_LOG_PATTERN_KEY);

            _appEventPublisher.EntityDeleted(processingLog);
        }

        /// <summary>
        /// GetProcessingLogsStatusCounts
        /// </summary>
        /// <returns>IList&lt;StatusCountsClass&gt;.</returns>
        /// <return> List KeyValuePair </return>
        public virtual IList<StatusCountsClass> GetProcessingLogsStatusCounts()
        {
            List<StatusCountsClass> res = _processingLogRepository.GetDbClientContext().SqlQuery<StatusCountsClass>("EXECUTE [dbo].[GetProcessingLogsStatusCounts]").ToList();

            return res;
        }

        #endregion Methods
    }
}