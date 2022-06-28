// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IScheduleTaskService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Agent;
using System.Collections.Generic;

namespace Adrack.Service.Agent
{
    /// <summary>
    /// Represents a Schedule Task Service
    /// </summary>
    public partial interface IScheduleTaskService
    {
        #region Methods

        /// <summary>
        /// Get Schedule Task By Id
        /// </summary>
        /// <param name="scheduleTaskId">Schedule Task Identifier</param>
        /// <returns>Schedule Task Item</returns>
        ScheduleTask GetScheduleTaskById(long scheduleTaskId);

        /// <summary>
        /// Get Schedule Task By Service Type
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <returns>Schedule Task Item</returns>
        ScheduleTask GetScheduleTaskByServiceType(string serviceType);

        /// <summary>
        /// Get All Schedule Tasks
        /// </summary>
        /// <returns>Schedule Task Collection Item</returns>
        IList<ScheduleTask> GetAllScheduleTasks();

        /// <summary>
        /// Insert Schedule Task
        /// </summary>
        /// <param name="scheduleTask">Schedule Task</param>
        void InsertScheduleTask(ScheduleTask scheduleTask);

        /// <summary>
        /// Update Schedule Task
        /// </summary>
        /// <param name="scheduleTask">Schedule Task</param>
        void UpdateScheduleTask(ScheduleTask scheduleTask);

        /// <summary>
        /// Delete Schedule Task
        /// </summary>
        /// <param name="scheduleTask">Schedule Task</param>
        void DeleteScheduleTask(ScheduleTask scheduleTask);

        #endregion Methods
    }
}