// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Setting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;

namespace Adrack.Core.Domain.Configuration
{
    /// <summary>
    /// Represents a Setting
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// Implements the <see cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    /// <seealso cref="Adrack.Core.Domain.Localization.ILocalizedEntity" />
    public partial class Setting : BaseEntity, ILocalizedEntity
    {
        #region Constructor

        /// <summary>
        /// Setting
        /// </summary>
        public Setting() { }

        /// <summary>
        /// Setting
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="description">Description</param>
        public Setting(string key, string value, string description)
        {
            this.Key = key;
            this.Value = value;
            this.Description = description;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return Key;
        }

        #endregion Methods

        #region Properties

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

        /// <summary>
        /// Gets or Sets the Description
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        #endregion Properties
    }
}