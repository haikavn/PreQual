// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="VerifySecurityMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a Verify Security Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.VerifySecurity}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.VerifySecurity}" />
    public partial class VerifySecurityMap : AppEntityTypeConfiguration<VerifySecurity>
    {
        #region Constructor

        /// <summary>
        /// Verify Security Map
        /// </summary>
        public VerifySecurityMap()
        {
            this.ToTable("VerifySecurity");

            this.HasKey(x => x.Id);

            this.Property(x => x.UserId).IsRequired();
            this.Property(x => x.Question).IsRequired();
            this.Property(x => x.Answer).IsRequired();
        }

        #endregion Constructor
    }
}