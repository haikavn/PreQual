// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="ILogService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Audit;
using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;

namespace Adrack.Service.Audit
{
    /// <summary>
    /// Represents a Log Service
    /// </summary>
    public partial interface ILogService
    {
        #region Methods

        /// <summary>
        /// Get Log By Id
        /// </summary>
        /// <param name="id">Log Identifier</param>
        /// <returns>Log Item</returns>
        Log GetLogById(long id);

        /// <summary>
        /// Get Log By Id
        /// </summary>
        /// <param name="id">Log Identifier</param>
        /// <returns>Log Item Collection</returns>
        IList<Log> GetLogById(long[] id);

        /// <summary>
        /// Get All Logs
        /// </summary>
        /// <param name="start">Start</param>
        /// <param name="end">End</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Log Level</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <returns>Log Item Collection</returns>
        IPagination<Log> GetAllLogs(DateTime? start, DateTime? end, string message, LogLevel? logLevel, int pageIndex, int pageSize);

        /// <summary>
        /// Is Enabled
        /// </summary>
        /// <param name="logLevel">Log Level</param>
        /// <returns>Bool Item</returns>
        bool IsEnabled(LogLevel logLevel);

        /// <summary>
        /// Insert Log
        /// </summary>
        /// <param name="logLevel">Log Level</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        /// <returns>Log Item</returns>
        Log InsertLog(LogLevel logLevel, string message, string exception = "", User user = null);

        /// <summary>
        /// Clear Log
        /// </summary>
        void ClearLog();

        /// <summary>
        /// Delete Log
        /// </summary>
        /// <param name="log">Log</param>
        void DeleteLog(Log log);

        #endregion Methods
    }
}