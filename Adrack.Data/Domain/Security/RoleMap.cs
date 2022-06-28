// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RoleMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;

namespace Adrack.Data.Domain.Security
{
    /// <summary>
    /// Represents a Role Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Security.Role}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Security.Role}" />
    public partial class RoleMap : AppEntityTypeConfiguration<Role>
    {
        #region Constructor

        /// <summary>
        /// Role Map
        /// </summary>
        public RoleMap()
        {
            this.ToTable("Role");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(100);
            this.Property(x => x.Key).IsRequired().HasMaxLength(150);
            this.Property(x => x.Description).HasMaxLength(200);

            this.HasMany(x => x.RolePermissions).
                WithRequired(x => x.Role).
                HasForeignKey(x => x.RoleId);
        }

        #endregion Constructor
    }
}