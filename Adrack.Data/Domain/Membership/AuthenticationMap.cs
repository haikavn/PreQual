// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AuthenticationMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a External Authentication Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Authentication}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.Authentication}" />
    public partial class AuthenticationMap : AppEntityTypeConfiguration<Authentication>
    {
        #region Constructor

        /// <summary>
        /// Authentication Map
        /// </summary>
        public AuthenticationMap()
        {
            this.ToTable("Authentication");

            this.HasKey(x => x.Id);

            this.HasRequired(x => x.User)
                .WithMany(x => x.Authentications)
                .HasForeignKey(x => x.UserId);
        }

        #endregion Constructor
    }
}