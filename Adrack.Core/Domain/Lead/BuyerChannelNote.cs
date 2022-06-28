// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateNote.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class AffiliateNote.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerChannelNote : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or Sets the First name
        /// </summary>
        /// <value>The note.</value>
        [MaxLength(1000)]
        public string Note { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long BuyerChannelId { get; set; }
        public DateTime? Updated { get; set; }

        #endregion Properties
    }
}