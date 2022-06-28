// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="VerifyAccountMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a Verify Account Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.VerifyAccount}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.VerifyAccount}" />
    public partial class VerifyAccountMap : AppEntityTypeConfiguration<VerifyAccount>
    {
        #region Constructor

        /// <summary>
        /// Verify Account Map
        /// </summary>
        public VerifyAccountMap()
        {
            this.ToTable("VerifyAccount");

            this.HasKey(x => x.Id);

            this.Property(x => x.UserId).IsRequired();
            this.Property(x => x.Username).IsRequired().HasMaxLength(50);
            this.Property(x => x.Email).IsRequired().HasMaxLength(100);
            this.Property(x => x.Password).IsRequired().HasMaxLength(100);
            this.Property(x => x.SaltKey).IsRequired().HasMaxLength(50);
        }

        #endregion Constructor
    }
}