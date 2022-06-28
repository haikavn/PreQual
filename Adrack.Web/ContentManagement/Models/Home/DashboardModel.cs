// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="DashboardModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Web.Framework.Mvc;

namespace Adrack.Web.ContentManagement.Models.Home
{
    /// <summary>
    /// Represents a Dashboard Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class DashboardModel : BaseAppModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardModel"/> class.
        /// </summary>
        public DashboardModel()
        {
            DisplayType = 0;
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the display type.
        /// </summary>
        /// <value>The display type.</value>
        public short DisplayType { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short Status { get; set; }

        #endregion Properties
    }
}