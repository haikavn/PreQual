// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 05-03-2021
//
// Last Modified By : Grigori
// Last Modified On : 05-03-2021
// ***********************************************************************
// <copyright file="AddonMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a UserAddons Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.UserAddons}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.UserAddons}" />
    public partial class UserAddonsMap : AppEntityTypeConfiguration<UserAddons>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public UserAddonsMap() // elite group
        {
            this.ToTable("UserAddons");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}