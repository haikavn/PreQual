// ***********************************************************************
// Assembly         : Adrack.Service
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-09-2019
// ***********************************************************************
// <copyright file="DateTimeSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Service.Common
{
    /// <summary>
    /// Represents a Date Time Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class DateTimeSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Application Time Zone Identifier
        /// </summary>
        /// <value>The application time zone identifier.</value>
        public string AppTimeZoneId { get; set; }

        /// <summary>
        /// Gets or Sets the Allow User To Set Time Zone
        /// </summary>
        /// <value><c>true</c> if [allow user to set time zone]; otherwise, <c>false</c>.</value>
        public bool AllowUserToSetTimeZone { get; set; }

        #endregion Properties
    }
}