// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 12-17-20209
//
// Last Modified By : Grigori
// Last Modified On : 12-17-2020
// ***********************************************************************
// <copyright file="BuyerChannelScheduleDayMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelScheduleDayMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelScheduleDay}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelScheduleDay}" />
    public partial class BuyerChannelScheduleDayMap : AppEntityTypeConfiguration<BuyerChannelScheduleDay>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerChannelScheduleDayMap() // elite group
        {
            this.ToTable("BuyerChannelScheduleDay");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}