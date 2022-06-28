// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 12-17-2020
//
// Last Modified By : Grigori
// Last Modified On : 12-17-2020
// ***********************************************************************
// <copyright file="BuyerChannelScheduleTimePeriodMap.cs" company="Adrack.com">
//     Copyright © 2020
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelScheduleTimePeriodMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelScheduleTimePeriod}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelScheduleTimePeriod}" />
    public partial class BuyerChannelScheduleTimePeriodMap : AppEntityTypeConfiguration<BuyerChannelScheduleTimePeriod>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerChannelScheduleTimePeriodMap() // elite group
        {
            this.ToTable("BuyerChannelScheduleTimePeriod");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}