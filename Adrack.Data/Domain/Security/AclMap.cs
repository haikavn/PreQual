// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AclMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Security;

namespace Adrack.Data.Domain.Security
{
    /// <summary>
    /// Represents a Access Control Level Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Security.Acl}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Security.Acl}" />
    public partial class AclMap : AppEntityTypeConfiguration<Acl>
    {
        #region Constructor

        /// <summary>
        /// Acl Map
        /// </summary>
        public AclMap()
        {
            this.ToTable("Acl");

            this.HasKey(x => x.Id);

            this.Property(x => x.EntityId).IsRequired();
            this.Property(x => x.EntityName).IsRequired().HasMaxLength(150);

            this.HasRequired(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .WillCascadeOnDelete(true);
        }

        #endregion Constructor
    }
}