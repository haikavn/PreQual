// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AddressTypeMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;

namespace Adrack.Data.Domain.Directory
{
    /// <summary>
    /// Represents a Address Type Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.AddressType}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.AddressType}" />
    public partial class AddressTypeMap : AppEntityTypeConfiguration<AddressType>
    {
        #region Constructor

        /// <summary>
        /// Address Type Map
        /// </summary>
        public AddressTypeMap()
        {
            this.ToTable("AddressType");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
        }

        #endregion Constructor
    }
}