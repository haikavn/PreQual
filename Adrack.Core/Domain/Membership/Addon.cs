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
    /// Represents a Addon
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class Addon : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        /// <value>The Name value.</value>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets from Key.
        /// </summary>
        /// <value>Key.</value>
        [MaxLength(50)]
        public string Key { get; set; }

        /// <summary>
        /// Is Deleted.
        /// </summary>
        /// <value>Deleted.</value>
        public bool? Deleted { get; set; }


        /// <summary>
        /// Gets or sets from AddData.
        /// </summary>
        /// <value>AddData.</value>
        public string AddData { get; set; }


        /// <summary>
        /// Gets or sets the Amount.
        /// </summary>
        /// <value>The Amount.</value>
        public double? Amount { get; set; }

        /// <summary>
        /// Gets or sets from Description.
        /// </summary>
        /// <value>Description.</value>
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the Deactivate.
        /// </summary>
        /// <value>The Deactivate.</value>
        public short Deactivate { get; set; }

        #endregion Properties
    }
}