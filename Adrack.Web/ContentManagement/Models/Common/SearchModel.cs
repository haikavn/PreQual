// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SearchModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Common
{
    /// <summary>
    /// Represents a Search Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class SearchModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Query String
        /// </summary>
        /// <value>The q.</value>
        [AllowHtml]
        public string q { get; set; }

        /// <summary>
        /// Gets or Sets the No Results
        /// </summary>
        /// <value><c>true</c> if [no results]; otherwise, <c>false</c>.</value>
        public bool NoResults { get; set; }

        /// <summary>
        /// Gets or Sets the Warning
        /// </summary>
        /// <value>The warning.</value>
        public string Warning { get; set; }

        #endregion Properties
    }
}