// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 05-04-2021
//
// Last Modified By : Grigori
// Last Modified On : 05-04-2021
// ***********************************************************************
// <copyright file="PermissionAddonMap.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a PermissionAddon Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.PermissionAddon}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.PermissionAddon}" />
    public partial class PermissionAddonMap : AppEntityTypeConfiguration<PermissionAddon>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public PermissionAddonMap() // elite group
        {
            this.ToTable("PermissionAddon");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}