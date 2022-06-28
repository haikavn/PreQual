// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportBuyersByStates.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportBuyersWinRateReport.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportBuyersWinRateReport : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties
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

        /// <summary>
        /// Gets or sets the rejected leads.
        /// </summary>
        /// <value>The rejected leads.</value>
        public int RejectedLeads { get; set; }

        /// <summary>
        /// Gets or sets the min price error leads.
        /// </summary>
        /// <value>The rejected leads.</value>
        public int MinPriceErrorLeads { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        public decimal BuyerPrice { get; set; }

        public decimal TotalBuyerPrice { get; set; }

        #endregion Properties
    }
}