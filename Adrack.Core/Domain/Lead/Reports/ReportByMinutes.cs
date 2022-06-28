// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportByMinutes.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportByMinutes.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportByMinutes : BaseEntity
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
        /// Gets or sets the dy.
        /// </summary>
        /// <value>The dy.</value>
        public int Dy { get; set; }

        /// <summary>
        /// Gets or sets the mn.
        /// </summary>
        /// <value>The mn.</value>
        public int Mn { get; set; }

        /// <summary>
        /// Gets or sets the hr.
        /// </summary>
        /// <value>The hr.</value>
        public int Hr { get; set; }

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