// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 10-11-2021
//
// Last Modified By : Grigori
// Last Modified On : 10-11-2021
// ***********************************************************************
// <copyright file="FeatureMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a Feature Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Feature}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Feature}" />
    public partial class FeatureMap : AppEntityTypeConfiguration<Feature>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public FeatureMap() // elite group
        {
            this.ToTable("Features");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}