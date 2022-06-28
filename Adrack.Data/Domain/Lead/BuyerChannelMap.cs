// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannelMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannel}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannel}" />
    public partial class BuyerChannelMap : AppEntityTypeConfiguration<BuyerChannel>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerChannelMap() // elite group
        {
            this.ToTable("BuyerChannel");

            this.HasKey(x => x.Id);

            /*this.HasMany(x => x.AttachedAffiliateChannels)
                .WithMany(x => x.AttachedBuyerChannels)
                .Map(x => { x.MapLeftKey("BuyerChannelId"); x.MapRightKey("AffiliateChannelId"); x.ToTable("AttachedChannel"); });*/

        }

        #endregion Constructor
    }
}