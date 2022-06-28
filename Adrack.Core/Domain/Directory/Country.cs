// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Country.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Directory
{
    /// <summary>
    /// Represents a Country
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    public partial class Country : BaseEntity, ILocalizedEntity
    {
        #region Fields

        /// <summary>
        /// State Province
        /// </summary>
        private ICollection<StateProvince> _stateProvinces;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Two Letter Code
        /// </summary>
        /// <value>The two lettero code.</value>
        public string TwoLetteroCode { get; set; }

        /// <summary>
        /// Gets or Sets the Three Letter Code
        /// </summary>
        /// <value>The three lettero code.</value>
        public string ThreeLetteroCode { get; set; }

        /// <summary>
        /// Gets or Sets the Numeric Code
        /// </summary>
        /// <value>The numeric code.</value>
        public int NumericCode { get; set; }

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

        public int? ZipLength { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the State Province
        /// </summary>
        /// <value>The state provinces.</value>
        public virtual ICollection<StateProvince> StateProvinces
        {
            get { return _stateProvinces ?? (_stateProvinces = new List<StateProvince>()); }
            protected set { _stateProvinces = value; }
        }

        #endregion Navigation Properties
    }
}