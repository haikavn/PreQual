// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PageSlugMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Seo;

namespace Adrack.Data.Domain.Seo
{
    /// <summary>
    /// Represents a Page Slug Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Seo.PageSlug}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Seo.PageSlug}" />
    public partial class PageSlugMap : AppEntityTypeConfiguration<PageSlug>
    {
        #region Constructor

        /// <summary>
        /// Page Slug Map
        /// </summary>
        public PageSlugMap()
        {
            this.ToTable("PageSlug");

            this.HasKey(x => x.Id);

            this.Property(x => x.EntityId).IsRequired();
            this.Property(x => x.EntityName).IsRequired().HasMaxLength(150);
            this.Property(x => x.Name).IsRequired().HasMaxLength(400);

            this.HasRequired(x => x.Language)
                .WithMany(x => x.PageSlugs)
                .HasForeignKey(x => x.LanguageId)
                .WillCascadeOnDelete(false);
        }

        #endregion Constructor
    }
}