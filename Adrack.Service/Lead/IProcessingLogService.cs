// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="IProcessingLogService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using System.Collections.Generic;

namespace Adrack.Service.Lead
{
    /// <summary>
    /// Interface IProcessingLogService
    /// </summary>
    public partial interface IProcessingLogService
    {
        #region Methods

        /// <summary>
        /// Get Profile By Id
        /// </summary>
        /// <param name="processingLogId">The processing log identifier.</param>
        /// <returns>Profile Item</returns>
        ProcessingLog GetProcessingLogById(long processingLogId);

        /// <summary>
        /// Get All Profiles
        /// </summary>
        /// <returns>Profile Collection Item</returns>
        IList<ProcessingLog> GetAllProcessingLogs();

        /// <summary>
        /// Gets all processing logs.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>IList&lt;ProcessingLog&gt;.</returns>
        IList<ProcessingLog> GetAllProcessingLogs(User user);

        /// <summary>
        /// GetProcessingLogsStatusCounts
        /// </summary>
        /// <returns>List KeyValuePair</returns>
        IList<StatusCountsClass> GetProcessingLogsStatusCounts();

        /// <summary>
        /// Insert Profile
        /// </summary>
        /// <param name="processingLog">The processing log.</param>
        /// <returns>System.Int64.</returns>
        long InsertProcessingLog(ProcessingLog processingLog);

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="processingLog">The processing log.</param>
        void UpdateProcessingLog(ProcessingLog processingLog);

        /// <summary>
        /// Delete Profile
        /// </summary>
        /// <param name="profile">Profile</param>
        void DeleteProcessingLog(ProcessingLog profile);

        #endregion Methods
    }
}