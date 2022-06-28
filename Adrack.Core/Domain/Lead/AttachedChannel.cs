// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BlackListType.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class BlackListType.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class AttachedChannel : BaseEntity
    {
        #region Properties
        public long AffiliateChannelId { get; set; }

        public long BuyerChannelId { get; set; }

        #endregion Properties
    }
}