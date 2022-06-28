// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Currency.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;
using System;

namespace Adrack.Core.Domain.Directory
{
    /// <summary>
    /// Represents a Currency
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    public partial class Currency : BaseEntity, ILocalizedEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Code
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets the Rate
        /// </summary>
        /// <value>The rate.</value>
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or Sets the Symbol
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or Sets the Published
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or Sets the Display Order
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or Sets the Created On
        /// </summary>
        /// <value>The created on.</value>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets the Updated On
        /// </summary>
        /// <value>The updated on.</value>
        public DateTime? UpdatedOn { get; set; }

        #endregion Properties
    }
}