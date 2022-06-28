using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports
{
    public class BuyerChanneModelResponse
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the buyer identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer.
        /// </summary>
        /// <value>The name of the buyer.</value>
        public string BuyerName { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer channel identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer channel.
        /// </summary>
        /// <value>The name of the buyer channel.</value>
        public string BuyerChannelName { get; set; }

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

        public int RejectedLeads { get; set; }

        /// <summary>
        /// Gets or sets the loaned leads.
        /// </summary>
        /// <value>The loaned leads.</value>
        public int LoanedLeads { get; set; }

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>The cost.</value>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the cap.
        /// </summary>
        /// <value>The cap.</value>
        public int CapReached { get; set; }

        /// <summary>
        /// Gets or sets the accept rate.
        /// </summary>
        /// <value>The accept rate.</value>
        public decimal AcceptRate { get; set; }

        /// <summary>
        /// Gets or sets the redirected.
        /// </summary>
        /// <value>The redirected.</value>
        public int Redirected { get; set; }

        /// <summary>
        /// Gets or sets the redirected rate.
        /// </summary>
        /// <value>The redirected rate.</value>
        public decimal RedirectedRate { get; set; }

        /// <summary>
        /// Gets or sets the profit.
        /// </summary>
        /// <value>The profit.</value>
        public decimal Profit { get; set; }

        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The sum of affiliate price.</value>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int Rank { get; set; }

        public bool CapHit { get; set; }

        public string LastSoldDate { get; set; }

        #region Constructor

        /// <summary>
        /// Register Model
        /// </summary>
        public BuyerChanneModelResponse()
        {
        }

        #endregion Constructor

        #region Properties

       

        #endregion Properties
    }
}