// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="EmailSubscription.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Message
{
    /// <summary>
    /// Represents a Email Subscription
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class EmailSubscription : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Globally Unique Identifier
        /// </summary>
        /// <value>The gu identifier.</value>
        public string GuId { get; set; }

        /// <summary>
        /// Gets or Sets the Email
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the Active
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or Sets the Created On
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }

        #endregion Properties
    }
}