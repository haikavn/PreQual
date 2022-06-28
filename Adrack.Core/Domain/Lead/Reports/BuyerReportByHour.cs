// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 04-08-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="BuyerReportByHour.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class BuyerReportByHour.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class BuyerReportByHour : BaseEntity
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
        /// Gets or sets the hour.
        /// </summary>
        /// <value>The hour.</value>
        public int Hour { get; set; }

        #endregion Properties
    }
}