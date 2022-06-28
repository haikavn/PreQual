// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="LogExtensions.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Audit;
using Adrack.Core.Domain.Membership;
using System;

namespace Adrack.Service.Audit
{
    /// <summary>
    /// Represents a Log Extensions
    /// </summary>
    public static class LogExtensions
    {
        #region Utilities

        /// <summary>
        /// Filter Log
        /// </summary>
        /// <param name="logService">Log Service</param>
        /// <param name="logLevel">Log Level</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        private static void FilterLog(ILogService logService, LogLevel logLevel, string message, Exception exception = null, User user = null)
        {
            if ((exception != null) && (exception is System.Threading.ThreadAbortException))
                return;

            if (logService.IsEnabled(logLevel))
            {
                var exceptionValue = exception == null ? string.Empty : exception.ToString();

                logService.InsertLog(logLevel, message, exceptionValue, user);
            }
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="logService">Log Service</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public static void Debug(this ILogService logService, string message, Exception exception = null, User user = null)
        {
            FilterLog(logService, LogLevel.Debug, message, exception, user);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="logService">Log Service</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public static void Information(this ILogService logService, string message, Exception exception = null, User user = null)
        {
            FilterLog(logService, LogLevel.Information, message, exception, user);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="logService">Log Service</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public static void Warning(this ILogService logService, string message, Exception exception = null, User user = null)
        {
            FilterLog(logService, LogLevel.Warning, message, exception, user);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="logService">Log Service</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public static void Error(this ILogService logService, string message, Exception exception = null, User user = null)
        {
            FilterLog(logService, LogLevel.Error, message, exception, user);
        }

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="logService">Log Service</param>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public static void Fatal(this ILogService logService, string message, Exception exception = null, User user = null)
        {
            FilterLog(logService, LogLevel.Fatal, message, exception, user);
        }

        #endregion Methods
    }
}