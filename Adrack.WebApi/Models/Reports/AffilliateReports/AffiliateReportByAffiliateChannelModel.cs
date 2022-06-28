﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.AffilliateReports
{
    /// <summary>
    /// Class ReportAffiliatesByAffiliateChannelsModel.
    /// </summary>
    public class AffiliateReportByAffiliateChannelModel// : BaseAppModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string title { get; set; }
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
        /// Gets or sets the credit.
        /// </summary>
        /// <value>The credit.</value>
        public decimal Credit { get; set; }

        /// <summary>
        /// Gets or sets the redirects.
        /// </summary>
        /// <value>The redirects.</value>
        public int Redirected { get; set; }

        /// <summary>
        /// Gets or sets the profit.
        /// </summary>
        /// <value>The profit.</value>
        public decimal Profit { get; set; }

        /// <summary>
        /// Gets or sets the accept rate.
        /// </summary>
        /// <value>The accept rate.</value>
        public decimal AcceptRate { get; set; }

        /// <summary>
        /// Gets or sets the epl.
        /// </summary>
        /// <value>The epl.</value>
        public decimal EPL { get; set; }

        /// <summary>
        /// Gets or sets the redirect rate.
        /// </summary>
        /// <value>The redirect rate.</value>
        public decimal RedirectedRate { get; set; }

        /// <summary>
        /// Gets or sets the epa.
        /// </summary>
        /// <value>The epa.</value>
        public decimal EPA { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<AffiliateReportByAffiliateChannelModel> children { get; set; }

        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public AffiliateReportByAffiliateChannelModel()
        {
            this.children = new List<AffiliateReportByAffiliateChannelModel>();
        }

        #endregion Constructor
        #region Properties
        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal Cost { get; set; }

        #endregion Properties
    }
}