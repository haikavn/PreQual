// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="SettingMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Configuration;

namespace Adrack.Data.Domain.Configuration
{
    /// <summary>
    /// Represents a Setting Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Configuration.Setting}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Configuration.Setting}" />
    public partial class SettingMap : AppEntityTypeConfiguration<Setting>
    {
        #region Constructor

        /// <summary>
        /// Setting Map
        /// </summary>
        public SettingMap()
        {
            this.ToTable("Setting");

            this.HasKey(x => x.Id);

            this.Property(x => x.Key).IsRequired().HasMaxLength(150);
            this.Property(x => x.Value).IsRequired().HasMaxLength(1000);
        }

        #endregion Constructor
    }
}