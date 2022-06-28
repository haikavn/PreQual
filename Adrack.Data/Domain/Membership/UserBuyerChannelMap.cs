// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="UserBuyerChannelMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core.Domain.Membership;

namespace Adrack.Data.Domain.Membership
{
    /// <summary>
    /// Represents a User Map
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.UserBuyerChannel}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Membership.UserBuyerChannel}" />
    public partial class UserBuyerChannelMap : AppEntityTypeConfiguration<UserBuyerChannel>
    {
        #region Constructor

        /// <summary>
        /// User Map
        /// </summary>
        public UserBuyerChannelMap()
        {
            this.ToTable("UserBuyerChannel");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}