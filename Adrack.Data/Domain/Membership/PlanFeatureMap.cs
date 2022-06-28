// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 05-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 05-03-2021
// ***********************************************************************
// <copyright file="PlanMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a PlanFeature Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.PlanFeature}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.PlanFeature}" />
    public partial class PlanFeatureMap : AppEntityTypeConfiguration<PlanFeature>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public PlanFeatureMap() // elite group
        {
            this.ToTable("PlanFeature");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}