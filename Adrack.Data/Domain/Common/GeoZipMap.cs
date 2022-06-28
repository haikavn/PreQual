// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 05-05-2020
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 05-05-2020
// ***********************************************************************
// <copyright file="GeoZipMap.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Common;

namespace Adrack.Data.Domain.Common
{
    /// <summary>
    /// Represents a GeoZip Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.Navigation}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Common.Navigation}" />
    public partial class GeoZipMap : AppEntityTypeConfiguration<GeoZip>
    {
        #region Constructor

        /// <summary>
        /// GeoZip Map
        /// </summary>
        public GeoZipMap()
        {
            this.ToTable("GeoZip");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}