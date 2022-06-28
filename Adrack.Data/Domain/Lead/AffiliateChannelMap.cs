// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="AffiliateChannelMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class AffiliateChannelMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannel}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.AffiliateChannel}" />
    public partial class AffiliateChannelMap : AppEntityTypeConfiguration<AffiliateChannel>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public AffiliateChannelMap() // elite group
        {
            this.ToTable("AffiliateChannel");

            this.HasKey(x => x.Id);

            /*this.HasMany(x => x.AttachedBuyerChannels)
                .WithMany(x => x.AttachedAffiliateChannels)
                .Map(x => { x.MapLeftKey("AffiliateChannelId"); x.MapRightKey("BuyerChannelId"); x.ToTable("AttachedChannel"); });*/
        }

        #endregion Constructor
    }
}