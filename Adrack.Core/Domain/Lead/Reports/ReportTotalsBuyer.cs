// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportTotalsBuyer.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportTotalsBuyer.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportTotalsBuyer : BaseEntity
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
        /// Gets or sets the sold.
        /// </summary>
        /// <value>The sold.</value>
        public int sold { get; set; }

        /// <summary>
        /// Gets or sets the rejected.
        /// </summary>
        /// <value>The rejected.</value>
        public int rejected { get; set; }

        /// <summary>
        /// Gets or sets the debit.
        /// </summary>
        /// <value>The debit.</value>
        public decimal debit { get; set; }

        /// <summary>
        /// Gets or sets the profit.
        /// </summary>
        /// <value>The profit.</value>
        public decimal profit { get; set; }

        #endregion Properties
    }
}