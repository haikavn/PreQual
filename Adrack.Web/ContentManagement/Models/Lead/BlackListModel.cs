// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BlackListModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Lead
{
    /// <summary>
    /// Class BlackListModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public class BlackListModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public BlackListModel()
        {
            BlackListValues = new List<SelectListItem>();
            BlackListNames = new List<SelectListItem>();
            Conditions = new List<SelectListItem>();
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the black list type identifier.
        /// </summary>
        /// <value>The black list type identifier.</value>
        public long BlackListTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the black.
        /// </summary>
        /// <value>The type of the black.</value>
        public short BlackType { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public short Condition { get; set; }

        /// <summary>
        /// Gets or sets the black list values.
        /// </summary>
        /// <value>The black list values.</value>
        public List<SelectListItem> BlackListValues { get; set; }

        /// <summary>
        /// Gets or sets the black list names.
        /// </summary>
        /// <value>The black list names.</value>
        public List<SelectListItem> BlackListNames { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        public List<SelectListItem> Conditions { get; set; }

        #endregion Properties
    }
}