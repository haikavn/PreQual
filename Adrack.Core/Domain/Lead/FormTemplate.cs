// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FormTemplate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class FormTemplate.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class FormTemplate : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public string Properties { get; set; }
        public DateTime Created { get; set; }
        public long AffiliateChannelId { get; set; }
        public FormTemplateType Type { get; set; }
        public IntegrationType IntegrationType { get; set; }

        public int Submissions { get; set; }
        public DateTime LastModified { get; set; }

        #endregion Properties
    }
}