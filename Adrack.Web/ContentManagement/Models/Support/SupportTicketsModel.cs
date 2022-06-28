// ***********************************************************************
// Assembly         : Adrack.Web.ContentManagement
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="SupportTicketsModel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;
using Adrack.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Adrack.Web.ContentManagement.Models.Support
{
    /// <summary>
    /// Represents a Activation Model
    /// Implements the <see cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    /// </summary>
    /// <seealso cref="Adrack.Web.Framework.Mvc.BaseAppModel" />
    public partial class SupportTicketsModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Result
        /// </summary>
        /// <value>The result.</value>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the users list.
        /// </summary>
        /// <value>The users list.</value>
        public IList<User> UsersList { get; set; }

        /// <summary>
        /// Gets or sets the users name list.
        /// </summary>
        /// <value>The users name list.</value>
        public List<KeyValuePair<string, long>> UsersNameList { get; set; }

        #endregion Properties
    }
}