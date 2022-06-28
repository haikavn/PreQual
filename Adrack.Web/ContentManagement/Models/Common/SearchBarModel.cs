// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SearchBarModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;

namespace Adrack.Web.ContentManagement.Models.Common
{
    /// <summary>
    /// Represents a Search Bar Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class SearchBarModel : BaseAppModel
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