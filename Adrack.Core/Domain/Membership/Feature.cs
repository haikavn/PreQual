// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 05-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 05-03-2021
// ***********************************************************************
// <copyright file="Addon.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a Feature
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class Feature : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        /// <value>The Name value.</value>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <value>The Value.</value>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets from TypeValue.
        /// </summary>
        /// <value>TypeValue.</value>
        public string TypeValue { get; set; }



        #endregion Properties
    }
}