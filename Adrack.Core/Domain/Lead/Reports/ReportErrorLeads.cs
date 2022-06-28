// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="ReportErrorLeads.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class ReportErrorLeads.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class ReportErrorLeads : BaseEntity
    {
        #region Fields

        // private ICollection<User> _users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the lead identifier.
        /// </summary>
        /// <value>The lead identifier.</value>
        public long LeadId { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public short? ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>The validator.</value>
        public short? Validator { get; set; }

        /// <summary>
        /// Gets or sets the name of the affiliate.
        /// </summary>
        /// <value>The name of the affiliate.</value>
        public string AffiliateName { get; set; }

        /// <summary>
        /// Gets or sets the name of the affiliate channel.
        /// </summary>
        /// <value>The name of the affiliate channel.</value>
        public string AffiliateChannelName { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer.
        /// </summary>
        /// <value>The name of the buyer.</value>
        public string BuyerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer channel.
        /// </summary>
        /// <value>The name of the buyer channel.</value>
        public string BuyerChannelName { get; set; }

        public string CampaignName { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        public string State { get; set; }

        public short Status { get; set; }

        public decimal? Minprice { get; set; }

        #endregion Properties
    }
}