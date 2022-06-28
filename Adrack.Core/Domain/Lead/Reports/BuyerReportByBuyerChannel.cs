// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerReportByBuyerChannel.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class BuyerReportByBuyerChannel.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerReportByBuyerChannel : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer.
        /// </summary>
        /// <value>The name of the buyer.</value>
        public string BuyerName { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer channel.
        /// </summary>
        /// <value>The name of the buyer channel.</value>
        public string BuyerChannelName { get; set; }

        /// <summary>
        /// Gets or sets the total leads.
        /// </summary>
        /// <value>The total leads.</value>
        public int TotalLeads { get; set; }

        /// <summary>
        /// Gets or sets the sold leads.
        /// </summary>
        /// <value>The sold leads.</value>
        public int SoldLeads { get; set; }

        public int RejectedLeads { get; set; }

        /// <summary>
        /// Gets or sets the loaned leads.
        /// </summary>
        /// <value>The loaned leads.</value>
        public int LoanedLeads { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the redirected.
        /// </summary>
        /// <value>The redirected.</value>
        public int Redirected { get; set; }

        /// <summary>
        /// Gets or sets the sum of affiliate price.
        /// </summary>
        /// <value>The sum of affiliate price.</value>
        public decimal AffiliatePrice { get; set; }

        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price price.</value>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int OrderNum { get; set; }

        public DateTime? LastSoldDate { get; set; }

        #endregion Properties
    }
}