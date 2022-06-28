// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportAffiliatesByAffiliateChannels.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportAffiliatesByAffiliateChannels.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportClickMain : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the row number.
        /// </summary>
        /// <value>The row number.</value>
        public int RowNum { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the name of the affiliate.
        /// </summary>
        /// <value>The name of the affiliate.</value>
        public string AffiliateName { get; set; }

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the affiliate channel.
        /// </summary>
        /// <value>The name of the affiliate channel.</value>
        public string AffiliateChannelName { get; set; }

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
        /// Gets or sets the debet.
        /// </summary>
        /// <value>The debet.</value>
        public decimal Debet { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>The credit.</value>
        public decimal Credit { get; set; }

        /// <summary>
        /// Gets or sets the redirects.
        /// </summary>
        /// <value>The redirects.</value>
        public int Redirected { get; set; }

        public int Hits { get; set; }

        public int UniqueClicks { get; set; }

        public decimal ClickProfit { get; set; }

        #endregion Properties
    }
}