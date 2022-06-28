// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ProfileMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a Profile Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Profile}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Profile}" />
    public partial class ProfileMap : AppEntityTypeConfiguration<Profile>
    {
        #region Constructor

        /// <summary>
        /// Profile Map
        /// </summary>
        public ProfileMap()
        {
            this.ToTable("Profile");

            this.HasKey(x => x.Id);

            this.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            this.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        }

        #endregion Constructor
    }
}