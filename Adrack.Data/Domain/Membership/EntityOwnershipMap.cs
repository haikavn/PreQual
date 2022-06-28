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

using Adrack.Core.Domain.Common;
using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a User Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.User}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.User}" />
    public partial class EntityOwnershipMap : AppEntityTypeConfiguration<EntityOwnership>
    {
        #region Constructor

        /// <summary>
        /// User Map
        /// </summary>
        public EntityOwnershipMap()
        {
            this.ToTable("EntityOwnership");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}