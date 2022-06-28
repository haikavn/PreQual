// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="StateProvince.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;

namespace Adrack.Core.Domain.Directory
{
    /// <summary>
    /// Represents a State Province
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    public partial class StateProvince : BaseEntity, ILocalizedEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Country Identifier
        /// </summary>
        /// <value>The country identifier.</value>
        public long CountryId { get; set; }

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
        /// Gets or Sets the Published
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or Sets the Display Order
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Country
        /// </summary>
        /// <value>The country.</value>
        public virtual Country Country { get; set; }

        #endregion Navigation Properties
    }
}