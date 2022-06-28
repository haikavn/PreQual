// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 12-17-2020
//
// Last Modified By : Grigori
// Last Modified On : 12-17-2020
// ***********************************************************************
// <copyright file="BuyerChannelScheduleTimePeriod.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.Core;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelSchedule.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerChannelScheduleTimePeriod : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the day value.
        /// </summary>
        /// <value>The Schedule DayId.</value>
        public long ScheduleDayId { get; set; }

        /// <summary>
        /// Gets or sets from time.
        /// </summary>
        /// <value>From time.</value>
        public int FromTime { get; set; }

        /// <summary>
        /// Converts to time.
        /// </summary>
        /// <value>To time.</value>
        public int ToTime { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the posted wait.
        /// </summary>
        /// <value>The posted wait.</value>
        public int PostedWait { get; set; }

        /// <summary>
        /// Gets or sets the sold wait.
        /// </summary>
        /// <value>The sold wait.</value>
        public int SoldWait { get; set; }

        /// <summary>
        /// Gets or sets the hour maximum.
        /// </summary>
        /// <value>The hour maximum.</value>
        public int HourMax { get; set; }


        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public decimal? Price { get; set; }

        public short? LeadStatus { get; set; }

        #endregion Properties
    }
}