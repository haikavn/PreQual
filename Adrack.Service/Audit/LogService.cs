// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LogService.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;
using Adrack.Core.Domain.Audit;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adrack.Service.Audit
{
    /// <summary>
    /// Represents a Log Service
    /// Implements the <see cref="Adrack.Service.Audit.ILogService" />
    /// </summary>
    /// <seealso cref="Adrack.Service.Audit.ILogService" />
    public partial class LogService : ILogService
    {
        #region Fields

        /// <summary>
        /// Log
        /// </summary>
        private readonly IRepository<Log> _logRepository;

        /// <summary>
        /// Common Helper
        /// </summary>
        private readonly ICommonHelper _iCommonHelper;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Log Service
        /// </summary>
        /// <param name="logRepository">Log Repository</param>
        /// <param name="dbContext">Db Context</param>
        /// <param name="iCommonHelper">Common Helper</param>
        public LogService(IRepository<Log> logRepository, ICommonHelper iCommonHelper)
        {
            this._logRepository = logRepository;
            this._iCommonHelper = iCommonHelper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get Log By Id
        /// </summary>
        /// <param name="id">Log Identifier</param>
        /// <returns>Log Item</returns>
        public virtual Log GetLogById(long id)
        {
            if (id == 0)
                return null;

            return _logRepository.GetById(id);
        }

        /// <summary>
        /// Get Log By Id
        /// </summary>
        /// <param name="id">Log Identifier</param>
        /// <returns>Log Item Collection</returns>
        public virtual IList<Log> GetLogById(long[] id)
        {
            if (id == null || id.Length == 0)
                return new List<Log>();

            var query = from x in _logRepository.Table
                        where id.Contains(x.Id)
                        select x;

            var logItems = query.ToList();

            var sortedLogItems = new List<Log>();

            foreach (long value in id)
            {
                var log = logItems.Find(x => x.Id == value);

                if (log != null)
                    sortedLogItems.Add(log);
            }

            return sortedLogItems;
        }

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
        public virtual IPagination<Log> GetAllLogs(DateTime? start, DateTime? end, string message, LogLevel? logLevel, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _logRepository.Table;

            if (start.HasValue)
                query = query.Where(x => start.Value <= x.CreatedOn);

            if (end.HasValue)
                query = query.Where(x => end.Value >= x.CreatedOn);

            if (logLevel.HasValue)
            {
                int logLevelId = (int)logLevel.Value;

                query = query.Where(x => logLevelId == x.LogLevelId);
            }

            if (!String.IsNullOrEmpty(message))
                query = query.Where(x => x.Message.Contains(message) || x.Exception.Contains(message));

            query = query.OrderByDescending(x => x.CreatedOn);

            var log = new Pagination<Log>(query, pageIndex, pageSize);

            return log;
        }

        /// <summary>
        /// Is Enabled
        /// </summary>
        /// <param name="logLevel">Log Level</param>
        /// <returns>Bool Item</returns>
        public virtual bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Insert Log
        /// </summary>
        /// <param name="logLevel">Log Level</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        /// <returns>Log Item</returns>
        public virtual Log InsertLog(LogLevel logLevel, string message, string exception = "", User user = null)
        {
            var log = new Log()
            {
                User = user,
                LogLevel = logLevel,
                Message = message,
                Exception = exception,
                IpAddress = _iCommonHelper.GetIpAddress(),
                PageUrl = _iCommonHelper.GetPageUrl(true),
                ReferrerUrl = _iCommonHelper.GetUrlReferrer(),
                CreatedOn = DateTime.UtcNow
            };

            try
            {
                _logRepository.Insert(log);
            }
            catch (Exception ex) {
            }

            return log;
        }

        /// <summary>
        /// Clear Log
        /// </summary>
        public virtual void ClearLog()
        {
            _logRepository.GetDbClientContext().ExecuteSqlCommandText("TRUNCATE TABLE [dbo].[Log]");
        }

        /// <summary>
        /// Delete Log
        /// </summary>
        /// <param name="log">Log</param>
        /// <exception cref="ArgumentNullException">log</exception>
        public virtual void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            _logRepository.Delete(log);
        }

        #endregion Methods
    }
}