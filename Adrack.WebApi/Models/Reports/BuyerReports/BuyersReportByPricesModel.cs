using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class BuyersReportByPricesModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        /// <value>The buyer identifier.</value>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer channel.
        /// </summary>
        /// <value>The name of the buyer.</value>
        public string BuyerChannelName { get; set; }

        /// <summary>
        /// Gets or sets the sold quantity.
        /// </summary>
        /// <value>The sold quantity.</value>
        public int SoldLeads { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int TotalLeads { get; set; }

        /// <summary>
        /// Gets or sets the u quantity.
        /// </summary>
        /// <value>The u quantity.</value>
        public int UniqueLeads { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        /// <value>The buyer price.</value>
        public decimal BuyerPrice { get; set; }
    }
}