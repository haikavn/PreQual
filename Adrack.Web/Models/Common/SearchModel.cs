// ***********************************************************************
// Assembly         : Adrack.Web
// Author           : AdRack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SearchModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;
using System.Web.Mvc;

namespace Adrack.Web.Models.Common
{
    /// <summary>
    ///     Represents a Search Model
    ///     Implements the <see cref="BaseAppModel" />
    /// </summary>
    /// <seealso cref="BaseAppModel" />
    public class SearchModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        ///     Gets or Sets the Query String
        /// </summary>
        /// <value>The q.</value>
        [AllowHtml]
        public string q { get; set; }

        /// <summary>
        ///     Gets or Sets the No Results
        /// </summary>
        /// <value><c>true</c> if [no results]; otherwise, <c>false</c>.</value>
        public bool NoResults { get; set; }

        /// <summary>
        ///     Gets or Sets the Warning
        /// </summary>
        /// <value>The warning.</value>
        public string Warning { get; set; }

        #endregion Properties
    }
}