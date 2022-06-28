// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="PermissionMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;

namespace Adrack.Data.Domain.Security
{
    /// <summary>
    /// Represents a Permission Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Security.Permission}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Security.Permission}" />
    public partial class PermissionMap : AppEntityTypeConfiguration<Permission>
    {
        #region Constructor

        /// <summary>
        /// Permission Map
        /// </summary>
        public PermissionMap()
        {
            this.ToTable("Permission");

            this.HasKey(x => x.Id);

            this.Property(x => x.Name).IsRequired().HasMaxLength(100);
            this.Property(x => x.Key).IsRequired().HasMaxLength(150);
            this.Property(x => x.EntityName).IsRequired().HasMaxLength(150);
            this.Property(x => x.Description).HasMaxLength(200);

            this.HasMany(x => x.RolePermissions).
                WithRequired(x => x.Permission).
                HasForeignKey(x => x.PermissionId);
        }

        #endregion Constructor
    }
}