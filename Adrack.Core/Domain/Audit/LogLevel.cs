// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LogLevel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Audit
{
    /// <summary>
    /// Represents a Log Level Enumeration
    /// </summary>
    public enum LogLevel
    {
        #region Enumeration

        /// <summary>
        /// Debug Log Level
        /// </summary>
        Debug = 100,

        /// <summary>
        /// Success Log Level
        /// </summary>
        Success = 200,

        /// <summary>
        /// Information Log Level
        /// </summary>
        Information = 300,

        /// <summary>
        /// Warning Log Level
        /// </summary>
        Warning = 400,

        /// <summary>
        /// Error Log Level
        /// </summary>
        Error = 500,

        /// <summary>
        /// Fatal Log Level
        /// </summary>
        Fatal = 600

        #endregion Enumeration
    }
}