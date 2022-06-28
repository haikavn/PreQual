// ***********************************************************************
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
    /// Represents a PlanFeature
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public class PlanFeature : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the PlanId identifier.
        /// </summary>
        /// <value>The PlanId identifier.</value>
        public long PlanId { get; set; }

        /// <summary>
        /// Gets or sets the FeatureId identifier.
        /// </summary>
        /// <value>The FeatureId identifier.</value>
        public long FeatureId { get; set; }




        #endregion Properties
    }
}