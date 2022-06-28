// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LocalizedPropertyMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;

namespace Adrack.Data.Domain.Localization
{
    /// <summary>
    /// Represents a Localized Property Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Localization.LocalizedProperty}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Localization.LocalizedProperty}" />
    public partial class LocalizedPropertyMap : AppEntityTypeConfiguration<LocalizedProperty>
    {
        #region Constructor

        /// <summary>
        /// Localized Property Map
        /// </summary>
        public LocalizedPropertyMap()
        {
            this.ToTable("LocalizedProperty");

            this.HasKey(x => x.Id);

            this.Property(x => x.KeyGroup).IsRequired().HasMaxLength(250);
            this.Property(x => x.Key).IsRequired().HasMaxLength(250);
            this.Property(x => x.Value).IsRequired();

            this.HasRequired(x => x.Language)
                .WithMany(x => x.LocalizedProperties)
                .HasForeignKey(x => x.LanguageId);
        }

        #endregion Constructor
    }
}