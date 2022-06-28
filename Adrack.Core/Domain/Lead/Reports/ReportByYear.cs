// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportByYear.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportByYear.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportByYear : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        /// <value>The activity.</value>
        public string Activity { get; set; }

        /// <summary>
        /// Gets or sets the leads.
        /// </summary>
        /// <value>The leads.</value>
        public int Leads { get; set; }

        /// <summary>
        /// Gets or sets the yr.
        /// </summary>
        /// <value>The yr.</value>
        public int Yr { get; set; }

        /// <summary>
        /// Gets or sets the mt.
        /// </summary>
        /// <value>The mt.</value>
        public int Mt { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        public decimal BuyerPrice { get; set; }

        /// <summary>
        /// Gets or sets the profit.
        /// </summary>
        /// <value>The profit.</value>
        public decimal Profit { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price.
        /// </summary>
        /// <value>The affiliate price.</value>
        public decimal AffiliatePrice { get; set; }

        #endregion Properties
    }
}