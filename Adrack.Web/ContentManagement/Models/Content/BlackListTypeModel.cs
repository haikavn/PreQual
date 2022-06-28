// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="BlackListTypeModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;

namespace Adrack.Web.ContentManagement.Models.Content
{
    /// <summary>
    /// Class BlackListTypeModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class BlackListTypeModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the black list type identifier.
        /// </summary>
        /// <value>The black list type identifier.</value>
        public long BlackListTypeId { get; set; }

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        #endregion Properties
    }
}