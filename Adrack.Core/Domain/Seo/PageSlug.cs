// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PageSlug.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;

namespace Adrack.Core.Domain.Seo
{
    /// <summary>
    /// Represents a Page Slug
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class PageSlug : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Language Identifier
        /// </summary>
        /// <value>The language identifier.</value>
        public long LanguageId { get; set; }

        /// <summary>
        /// Gets or Sets the Entity Identifier
        /// </summary>
        /// <value>The entity identifier.</value>
        public long EntityId { get; set; }

        /// <summary>
        /// Gets or Sets the Entity Name
        /// </summary>
        /// <value>The name of the entity.</value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Active
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }

        #endregion Properties

        #region Navigation Properties

        /// <summary>
        /// Gets or Sets the Language
        /// </summary>
        /// <value>The language.</value>
        public virtual Language Language { get; set; }

        #endregion Navigation Properties
    }
}