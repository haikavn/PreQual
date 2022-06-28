// ***********************************************************************
// Assembly         : Adrack.Data
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannelScheduleMap.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelScheduleMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelSchedule}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.BuyerChannelSchedule}" />
    public partial class BuyerChannelScheduleMap : AppEntityTypeConfiguration<BuyerChannelSchedule>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public BuyerChannelScheduleMap() // elite group
        {
            this.ToTable("BuyerChannelSchedule");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}