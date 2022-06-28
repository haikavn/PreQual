// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Log.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using System;

namespace Adrack.Core.Domain.Audit
{
    /// <summary>
    /// Represents a Log
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Log : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the User Identifier
        /// </summary>
        /// <value>The user identifier.</value>
        public long? UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Log Level Identifier
        /// </summary>
        /// <value>The log level identifier.</value>
        public int LogLevelId { get; set; }

        /// <summary>
        /// Gets or Sets the Message
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets the Exception
        /// </summary>
        /// <value>The exception.</value>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or Sets the Ip Address
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or Sets the Page Url
        /// </summary>
        /// <value>The page URL.</value>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Referrer Url
        /// </summary>
        /// <value>The referrer URL.</value>
        public string ReferrerUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Created On
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets the Log Level
        /// </summary>
        /// <value>The log level.</value>
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)this.LogLevelId;
            }
            set
            {
                this.LogLevelId = (int)value;
            }
        }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the User
        /// </summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }

        #endregion Navigation Properties
    }
}