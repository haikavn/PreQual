// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="DoNotPresent.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class Filter.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class SubIdWhiteList : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the subid
        /// </summary>
        /// <value>The email.</value>
        public string SubId { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel id.
        /// </summary>
        /// <value>The expiration date.</value>
        public long BuyerChannelId { get; set; }

        #endregion Properties
    }
}