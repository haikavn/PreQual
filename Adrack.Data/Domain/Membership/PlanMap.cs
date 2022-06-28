// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 05-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 05-03-2021
// ***********************************************************************
// <copyright file="AddonMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a Addon Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Plan}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Plan}" />
    public partial class PlanMap : AppEntityTypeConfiguration<PlanModel>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public PlanMap() // elite group
        {
            this.ToTable("Plans");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}