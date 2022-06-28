﻿// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 10-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 10-03-2021
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
using System.ComponentModel.DataAnnotations.Schema;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a UserAddons
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class UserAddons : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the Addon identifier.
        /// </summary>
        /// <value>The Addon identifier.</value>
        public long AddonId { get; set; }

        /// <summary>
        /// Gets or sets from Date.
        /// </summary>
        /// <value>Date.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>The Status.</value>
        public short? Status { get; set; }


        /// <summary>
        /// Gets or sets the Amount.
        /// </summary>
        /// <value>The Amount.</value>
        public double? Amount { get; set; }


        /// <summary>
        /// Gets or sets the IsTrial.
        /// </summary>
        /// <value>The IsTrial.</value>
        public short? IsTrial { get; set; }

        #endregion Properties
    }
}