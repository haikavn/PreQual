// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Navigation.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Common
{
    /// <summary>
    /// Represents a Navigation
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Navigation : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Parent Identifier
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or Sets the Layout
        /// </summary>
        /// <value>The layout.</value>
        public string Layout { get; set; }

        /// <summary>
        /// Gets or Sets the Key
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or Sets the Controller
        /// </summary>
        /// <value>The controller.</value>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or Sets the Action
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or Sets the Permission
        /// </summary>
        /// <value>The permission.</value>
        public string Permission { get; set; }

        /// <summary>
        /// Gets or Sets the Html Class
        /// </summary>
        /// <value>The HTML class.</value>
        public string HtmlClass { get; set; }

        /// <summary>
        /// Gets or Sets the Url
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets the Image Url
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or Sets the Published
        /// </summary>
        /// <value><c>true</c> if published; otherwise, <c>false</c>.</value>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or Sets the Deleted
        /// </summary>
        /// <value><c>true</c> if deleted; otherwise, <c>false</c>.</value>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or Sets the DisplayOrder
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or Sets the Menu Item Color
        /// </summary>
        /// <value>The color.</value>
        public string Color { get; set; }

        #endregion Properties
    }
}