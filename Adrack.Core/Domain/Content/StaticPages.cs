// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 17-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 17-03-2021
// ***********************************************************************
// <copyright file="StaticPages.cs" company="Adrack.com">
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
    public class StaticPages : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>The Title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Body.
        /// </summary>
        /// <value>The Body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets from Created Date.
        /// </summary>
        /// <value>CreatedDate.</value>
        public DateTime? CreatedDate { get; set; }
        
        #endregion Properties
    }
}