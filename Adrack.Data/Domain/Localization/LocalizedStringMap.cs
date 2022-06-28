// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LocalizedStringMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;

namespace Adrack.Data.Domain.Localization
{
    /// <summary>
    /// Represents a Localized String Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Localization.LocalizedString}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Localization.LocalizedString}" />
    public partial class LocalizedStringMap : AppEntityTypeConfiguration<LocalizedString>
    {
        #region Constructor

        /// <summary>
        /// Localized String Map
        /// </summary>
        public LocalizedStringMap()
        {
            this.ToTable("LocalizedString");

            this.HasKey(x => x.Id);

            this.Property(x => x.Key).IsRequired().HasMaxLength(250);
            this.Property(x => x.Value).IsRequired().HasMaxLength(1000);

            this.HasRequired(x => x.Language)
                .WithMany(x => x.LocalizedStrings)
                .HasForeignKey(x => x.LanguageId);
        }

        #endregion Constructor
    }
}