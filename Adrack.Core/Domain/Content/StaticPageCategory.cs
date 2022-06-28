// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 22-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 22-03-2021
// ***********************************************************************
// <copyright file="StaticPageCategory.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Security;
using System;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Represents a StaticPages
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class StaticPageCategory : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>The Name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ParentId.
        /// </summary>
        /// <value>The ParentId.</value>
        public long ParentId { get; set; }

        
        #endregion Properties
    }
}