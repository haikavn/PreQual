// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ScheduleTaskService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Cache;
using Adrack.Core.Domain.Agent;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Agent
{
    /// <summary>
    /// Represents a Schedule Task Service
    /// Implements the <see cref="Adrack.Service.Agent.IScheduleTaskService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Agent.IScheduleTaskService" />
    public partial class ScheduleTaskService : IScheduleTaskService
    {
        #region Constants

        /// <summary>
        /// Cache Schedule Task By Id Key
        /// </summary>
        private const string CACHE_SCHEDULETASK_BY_ID_KEY = "App.Cache.ScheduleTask.By.Id-{0}";

        /// <summary>
        /// Cache Schedule Task By Service Type Key
        /// </summary>
        private const string CACHE_SCHEDULETASK_BY_SERVICETYPE_KEY = "App.Cache.ScheduleTask.By.ServiceType-{0}";

        /// <summary>
        /// Cache Schedule Task All Key
        /// </summary>
        private const string CACHE_SCHEDULETASK_ALL_KEY = "App.Cache.ScheduleTask.All";

        /// <summary>
        /// Cache Schedule Task Pattern Key
        /// </summary>
        private const string CACHE_SCHEDULETASK_PATTERN_KEY = "App.Cache.ScheduleTask.";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Schedule Task
        /// </summary>
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;

        /// <summary>
        /// Cache Manager
        /// </summary>
        private readonly ICacheManager _cacheManager;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Schedule Task Service
        /// </summary>
        /// <param name="scheduleTaskRepository">Schedule Task Repository</param>
        /// <param name="cacheManager">Cache Manager</param>
        public ScheduleTaskService(IRepository<ScheduleTask> scheduleTaskRepository, ICacheManager cacheManager)
        {
            this._scheduleTaskRepository = scheduleTaskRepository;
            this._cacheManager = cacheManager;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Schedule Task By Id
        /// </summary>
        /// <param name="scheduleTaskId">Schedule Task Identifier</param>
        /// <returns>Schedule Task Item</returns>
        public virtual ScheduleTask GetScheduleTaskById(long scheduleTaskId)
        {
            if (scheduleTaskId == 0)
                return null;

            string key = string.Format(CACHE_SCHEDULETASK_BY_ID_KEY, scheduleTaskId);

            return _cacheManager.Get(key, () => { return _scheduleTaskRepository.GetById(scheduleTaskId); });
        }

        /// <summary>
        /// Get Schedule Task By Service Type
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <returns>Schedule Task Item</returns>
        public virtual ScheduleTask GetScheduleTaskByServiceType(string serviceType)
        {
            if (String.IsNullOrWhiteSpace(serviceType))
                return null;

            string key = string.Format(CACHE_SCHEDULETASK_BY_SERVICETYPE_KEY, serviceType);

            return _cacheManager.Get(key, () =>
            {
                var query = from x in _scheduleTaskRepository.Table
                            orderby x.Id descending
                            where x.ServiceType == serviceType
                            select x;

                var scheduleTasks = query.FirstOrDefault(); 

                return scheduleTasks;
            });
        }

        /// <summary>
        /// Get All Schedule Tasks
        /// </summary>
        /// <returns>Schedule Task Collection Item</returns>
        public virtual IList<ScheduleTask> GetAllScheduleTasks()
        {
            string key = CACHE_SCHEDULETASK_ALL_KEY;

            return _cacheManager.Get(key, () =>
            {
                List<ScheduleTask> scheduleTasks = new List<ScheduleTask>();

                try
                {
                    var query = from x in _scheduleTaskRepository.Table
                                orderby x.ServiceType, x.Id
                                select x;

                    scheduleTasks = query.ToList();
                }
                catch
                {

                }

                return scheduleTasks;
            });
        }

        /// <summary>
        /// Insert Schedule Task
        /// </summary>
        /// <param name="scheduleTask">Schedule Task</param>
        /// <exception cref="ArgumentNullException">scheduleTask</exception>
        public virtual void InsertScheduleTask(ScheduleTask scheduleTask)
        {
            if (scheduleTask == null)
                throw new ArgumentNullException("scheduleTask");

            _scheduleTaskRepository.Insert(scheduleTask);

            _cacheManager.RemoveByPattern(CACHE_SCHEDULETASK_PATTERN_KEY);
        }

        /// <summary>
        /// Update Schedule Task
        /// </summary>
        /// <param name="scheduleTask">Schedule Task</param>
        /// <exception cref="ArgumentNullException">scheduleTask</exception>
        public virtual void UpdateScheduleTask(ScheduleTask scheduleTask)
        {
            if (scheduleTask == null)
                throw new ArgumentNullException("scheduleTask");

            _scheduleTaskRepository.Update(scheduleTask);

            _cacheManager.RemoveByPattern(CACHE_SCHEDULETASK_PATTERN_KEY);
        }

        /// <summary>
        /// Delete Schedule Task
        /// </summary>
        /// <param name="scheduleTask">Schedule Task</param>
        /// <exception cref="ArgumentNullException">scheduleTask</exception>
        public virtual void DeleteScheduleTask(ScheduleTask scheduleTask)
        {
            if (scheduleTask == null)
                throw new ArgumentNullException("scheduleTask");

            _scheduleTaskRepository.Delete(scheduleTask);

            _cacheManager.RemoveByPattern(CACHE_SCHEDULETASK_PATTERN_KEY);
        }

        #endregion Methods
    }
}