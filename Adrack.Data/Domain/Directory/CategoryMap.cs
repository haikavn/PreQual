// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="CategoryMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Directory;

namespace Adrack.Data.Domain.Directory
{
    /// <summary>
    /// Represents a Category Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.Category}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Directory.Category}" />
    public partial class CategoryMap : AppEntityTypeConfiguration<Category>
    {
        #region Constructor

        /// <summary>
        /// Category Map
        /// </summary>
        public CategoryMap()
        {
            this.ToTable("Category");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(50);
        }

        #endregion Constructor
    }
}