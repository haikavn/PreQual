// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a User Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.User}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.User}" />
    public partial class UserMap : AppEntityTypeConfiguration<User>
    {
        #region Constructor

        /// <summary>
        /// User Map
        /// </summary>
        public UserMap()
        {
            this.ToTable("User");

            this.HasKey(x => x.Id);

            this.Property(x => x.Username).HasMaxLength(50);
            this.Property(x => x.Email).HasMaxLength(100);

            //this.HasRequired(x => x.UserType)
            //    .WithMany(x => x.Users)
            //    .HasForeignKey(x => x.UserTypeId)
            //    .WillCascadeOnDelete(false);

            this.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(x => { x.MapLeftKey("UserId"); x.MapRightKey("RoleId"); x.ToTable("UserRole"); });

            this.HasMany(x => x.Permissions)
                .WithMany(x => x.Users)
                .Map(x => { x.MapLeftKey("UserId"); x.MapRightKey("PermissionId"); x.ToTable("UserPermission"); });
        }

        #endregion Constructor
    }
}