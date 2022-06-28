// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="RegistrationRequestMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a Profile Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.RegistrationRequest}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.RegistrationRequest}" />
    public partial class RegistrationRequestMap : AppEntityTypeConfiguration<RegistrationRequest>
    {
        #region Constructor

        /// <summary>
        /// Profile Map
        /// </summary>
        public RegistrationRequestMap()
        {
            this.ToTable("RegistrationRequest");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}