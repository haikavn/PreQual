// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 25-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 25-03-2021
// ***********************************************************************
// <copyright file="StaticPageCategoryRelation.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Represents a StaticPageCategoryRelation
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class StaticPageCategoryRelation : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Page identifier.
        /// </summary>
        /// <value>The Page identifier.</value>
        public long PageId { get; set; }

        /// <summary>
        /// Gets or sets the Category identifier.
        /// </summary>
        /// <value>The Category identifier.</value>
        public long CategoryId { get; set; }


        #endregion Properties
    }
}