// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportTotalsByDate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportTotalsByDate.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportTotalsByDate : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the received.
        /// </summary>
        /// <value>The received.</value>
        public int received { get; set; }

        /// <summary>
        /// Gets or sets the posted.
        /// </summary>
        /// <value>The posted.</value>
        public int posted { get; set; }

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
        /// Gets or sets the loaned.
        /// </summary>
        /// <value>The loaned.</value>
        public int loaned { get; set; }

        /// <summary>
        /// Gets or sets the aprice.
        /// </summary>
        /// <value>The aprice.</value>
        public decimal aprice { get; set; }

        /// <summary>
        /// Gets or sets the bprice.
        /// </summary>
        /// <value>The bprice.</value>
        public decimal bprice { get; set; }

        /// <summary>
        /// Gets or sets the profit.
        /// </summary>
        /// <value>The profit.</value>
        public decimal profit { get; set; }

        #endregion Properties
    }
}