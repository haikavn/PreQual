// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SearchSetting.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Infrastructure.Configuration;

namespace Adrack.Core.Domain.Common
{
    /// <summary>
    /// Represents a Search Setting
    /// Implements the <see cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    /// </summary>
    /// <seealso cref="Adrack.Core.Infrastructure.Configuration.ISetting" />
    public class SearchSetting : ISetting
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Search Minimum Length
        /// </summary>
        /// <value>The minimum length of the search.</value>
        public int SearchMinimumLength { get; set; }

        /// <summary>
        /// Gets or Sets the Auto Complete Enabled
        /// </summary>
        /// <value><c>true</c> if [automatic complete enabled]; otherwise, <c>false</c>.</value>
        public bool AutoCompleteEnabled { get; set; }

        /// <summary>
        /// Gets or Sets the Show Images In Search Auto Complete
        /// </summary>
        /// <value><c>true</c> if [show images in search automatic complete]; otherwise, <c>false</c>.</value>
        public bool ShowImagesInSearchAutoComplete { get; set; }

        #endregion Properties
    }
}