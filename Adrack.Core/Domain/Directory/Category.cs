// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="Category.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Directory
{
    /// <summary>
    /// Represents a Category
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class Category : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Parent Identifier
        /// </summary>
        /// <value>The parent identifier.</value>
        public long ParentId { get; set; }

        /// <summary>
        /// Gets or Sets the Name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

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
        /// Gets or Sets the Display Order
        /// </summary>
        /// <value>The display order.</value>
        public int DisplayOrder { get; set; }

        #endregion Properties
    }
}