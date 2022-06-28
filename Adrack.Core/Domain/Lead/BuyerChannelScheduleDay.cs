// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 12-17-2020
//
// Last Modified By : Grigori
// Last Modified On : 12-17-2020
// ***********************************************************************
// <copyright file="BuyerChannelScheduleDay.cs" company="Adrack.com">
//     Copyright © 2020
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
    public partial class BuyerChannelScheduleDay : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the day value.
        /// </summary>
        /// <value>The day value.</value>
        public short DayValue { get; set; }


        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }


        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The Daily Limit.</value>
        public int DailyLimit { get; set; }

        /// <summary>
        /// Gets or sets no limit.
        /// </summary>
        /// <value>No limit setting.</value>
        public bool NoLimit { get; set; }

        public bool IsEnabled { get; set; }


        #endregion Properties
    }
}