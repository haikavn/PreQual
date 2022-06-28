// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="StateProvinceMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;

namespace Adrack.Data.Domain.Directory
{
    /// <summary>
    /// Represents a State Province Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.StateProvince}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.StateProvince}" />
    public partial class StateProvinceMap : AppEntityTypeConfiguration<StateProvince>
    {
        #region Constructor

        /// <summary>
        /// State Province Map
        /// </summary>
        public StateProvinceMap()
        {
            this.ToTable("StateProvince");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired();

            this.HasRequired(x => x.Country)
                .WithMany(x => x.StateProvinces)
                .HasForeignKey(x => x.CountryId);
        }

        #endregion Constructor
    }
}