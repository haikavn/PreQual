// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 29-04-2021
//
// Last Modified By : Grigori
// Last Modified On : 29-04-2021
// ***********************************************************************
// <copyright file="Plan.cs" company="Plan.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Security;
using System;
using System.Collections.Generic;

namespace Adrack.Core.Domain.Membership
{
    /// <summary>
    /// Represents a UserPlan
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class UserPlan : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        /// <value>The plan identifier.</value>
        public long PlanId { get; set; }


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

        #endregion Properties
    }
}