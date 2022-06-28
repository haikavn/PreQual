// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LocalizedString.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Localization
{
    /// <summary>
    /// Represents a Localized String
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class LocalizedString : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Language Identifier
        /// </summary>
        /// <value>The language identifier.</value>
        public long LanguageId { get; set; }

        /// <summary>
        /// Gets or Sets the Key
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or Sets the Value
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Language
        /// </summary>
        /// <value>The language.</value>
        public virtual Language Language { get; set; }

        #endregion Navigation Properties
    }
}