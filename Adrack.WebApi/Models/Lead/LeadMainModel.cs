using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadMainModel : IBaseInModel
    {
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
        public short CampaignType { get; set; }

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
        public long Id { get; internal set; }
        public string EmailFields { get; internal set; }

        #endregion Properties

    }
}