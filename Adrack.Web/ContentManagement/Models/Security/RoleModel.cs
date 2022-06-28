// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="RoleModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core;
using Adrack.Core.Domain.Security;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adrack.Web.ContentManagement.Models.Security
{
    /// <summary>
    /// Class RoleModel.
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppEntityModel" />
    public partial class RoleModel : BaseAppEntityModel
    {
        #region Fields

        /// <summary>
        /// Gets or sets the type of the list user.
        /// </summary>
        /// <value>The type of the list user.</value>
        public IList<SelectListItem> ListUserType { get; set; }

        #endregion Fields



        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public RoleModel()
        {
            ListUserType = new List<SelectListItem>();
        }

        #endregion Constructor



        #region Properties

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        public long RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RoleModel"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public List<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the permission statuses.
        /// </summary>
        /// <value>The permission statuses.</value>
        public List<bool> PermissionStatuses { get; set; }

        /// <summary>
        /// Gets or sets the user type identifier.
        /// </summary>
        /// <value>The user type identifier.</value>
        public UserTypes UserType { get; set; }

        #endregion Properties
    }
}