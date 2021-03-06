// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CurrencyMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;

namespace Adrack.Data.Domain.Directory
{
    /// <summary>
    /// Represents a Currency Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.Currency}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.Currency}" />
    public partial class CurrencyMap : AppEntityTypeConfiguration<Currency>
    {
        #region Constructor

        /// <summary>
        /// Currency Map
        /// </summary>
        public CurrencyMap()
        {
            this.ToTable("Currency");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
            this.Property(x => x.Code).IsRequired().HasMaxLength(3);
            this.Property(x => x.Rate).IsRequired();
        }

        #endregion Constructor
    }
}