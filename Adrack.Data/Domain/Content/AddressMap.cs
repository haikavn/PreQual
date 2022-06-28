// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AddressMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Content;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Represents a Address Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.Address}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.Address}" />
    public partial class AddressMap : AppEntityTypeConfiguration<Address>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AddressMap()
        {
            this.ToTable("Address");

            this.HasKey(x => x.Id);

            this.Property(x => x.UserId).IsRequired();

            //this.HasRequired(x => x.AddressType)
            //    .WithMany()
            //    .HasForeignKey(x => x.AddressTypeId)
            //    .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.StateProvince)
                .WithMany()
                .HasForeignKey(x => x.StateProvinceId)
                .WillCascadeOnDelete(false);
        }

        #endregion Constructor
    }
}