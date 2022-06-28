// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LanguageMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Localization;

namespace Adrack.Data.Domain.Localization
{
    /// <summary>
    /// Represents a Language Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Localization.Language}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Localization.Language}" />
    public partial class LanguageMap : AppEntityTypeConfiguration<Language>
    {
        #region Constructor

        /// <summary>
        /// Language Map
        /// </summary>
        public LanguageMap()
        {
            this.ToTable("Language");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
            this.Property(x => x.Culture).IsRequired().HasMaxLength(15);
            this.Property(x => x.CultureId).IsRequired();
        }

        #endregion Constructor
    }
}