// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerChannelSchedule.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Class BuyerChannelSchedule.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerChannelSchedule : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the day value.
        /// </summary>
        /// <value>The day value.</value>
        public short DayValue { get; set; }

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
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>The price.</value>
        public decimal? Price { get; set; }

        public short? LeadStatus { get; set; }

        #endregion Properties
    }
}