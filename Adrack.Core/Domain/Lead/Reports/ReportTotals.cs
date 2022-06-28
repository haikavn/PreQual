// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportTotals.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportTotals.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportTotals : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        public int num { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the received.
        /// </summary>
        /// <value>The received.</value>
        public int received { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int total { get; set; }

        /// <summary>
        /// Gets or sets the receivedp.
        /// </summary>
        /// <value>The receivedp.</value>
        public decimal receivedp { get; set; }

        /// <summary>
        /// Gets or sets the totalp.
        /// </summary>
        /// <value>The totalp.</value>
        public decimal totalp { get; set; }

        /// <summary>
        /// Gets or sets the sold.
        /// </summary>
        /// <value>The sold.</value>
        public int sold { get; set; }

        /// <summary>
        /// Gets or sets the soldp.
        /// </summary>
        /// <value>The soldp.</value>
        public decimal soldp { get; set; }

        /// <summary>
        /// Gets or sets the debit.
        /// </summary>
        /// <value>The debit.</value>
        public decimal debit { get; set; }

        /// <summary>
        /// Gets or sets the debitp.
        /// </summary>
        /// <value>The debitp.</value>
        public decimal debitp { get; set; }

        /// <summary>
        /// Gets or sets the profit.
        /// </summary>
        /// <value>The profit.</value>
        public decimal profit { get; set; }

        /// <summary>
        /// Gets or sets the profitp.
        /// </summary>
        /// <value>The profitp.</value>
        public decimal profitp { get; set; }

        /// <summary>
        /// Gets or sets the redirected.
        /// </summary>
        /// <value>The redirected.</value>
        public int redirected { get; set; }

        /// <summary>
        /// Gets or sets the redirectedp.
        /// </summary>
        /// <value>The redirectedp.</value>
        public decimal redirectedp { get; set; }

        #endregion Properties
    }
}