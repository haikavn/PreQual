// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="LeadMain.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Adrack.Core.Domain.Lead
{
    /// <summary>
    /// Represents a Lead
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class LeadMain : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Address Item</returns>
        public object Clone()
        {
            var lead = new LeadMain()
            {
            };

            return lead;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>The campaign identifier.</value>
        public long CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier.
        /// </summary>
        /// <value>The affiliate identifier.</value>
        public long AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets the affiliate channel identifier.
        /// </summary>
        /// <value>The affiliate channel identifier.</value>
        public long AffiliateChannelId { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long? BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the type of the campaign.
        /// </summary>
        /// <value>The type of the campaign.</value>
        public CampaignTypes CampaignType { get; set; }

        /// <summary>
        /// Gets or sets the lead number.
        /// </summary>
        /// <value>The lead number.</value>
        public long LeadNumber { get; set; }

        /// <summary>
        /// Gets or sets the warning.
        /// </summary>
        /// <value>The warning.</value>
        public short Warning { get; set; }

        /// <summary>
        /// Gets or sets the processing time.
        /// </summary>
        /// <value>The processing time.</value>
        public double? ProcessingTime { get; set; }

        /// <summary>
        /// Gets or sets the dublicate lead identifier.
        /// </summary>
        /// <value>The dublicate lead identifier.</value>
        public long DublicateLeadId { get; set; }

        /// <summary>
        /// Gets or sets the received data.
        /// </summary>
        /// <value>The received data.</value>
        public string ReceivedData { get; set; }

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public short? ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the view date.
        /// </summary>
        /// <value>The view date.</value>
        public DateTime? ViewDate { get; set; }

        /// <summary>
        /// Gets or sets the sold date.
        /// </summary>
        /// <value>The sold date.</value>
        public DateTime? SoldDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the real ip.
        /// </summary>
        /// <value>The real ip.</value>
        public string RealIp { get; set; }

        /// <summary>
        /// Gets or sets the risk score.
        /// </summary>
        /// <value>The risk score.</value>
        public int? RiskScore { get; set; }

        /// <summary>
        /// Gets or sets the affiliate price.
        /// </summary>
        /// <value>The affiliate price.</value>
        public decimal? AffiliatePrice { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        public decimal? BuyerPrice { get; set; }

        #endregion Properties
    }
}