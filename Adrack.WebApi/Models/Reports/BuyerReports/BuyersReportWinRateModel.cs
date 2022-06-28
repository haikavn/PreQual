using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class BuyersReportWinRateModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is folder.
        /// </summary>
        public bool Folder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TreeItem"/> is expanded.
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Gets or sets the buyer channel identifier.
        /// </summary>
        public long BuyerChannelId { get; set; }

        /// <summary>
        /// Gets or sets the name of the buyer channel.
        /// </summary>
        public string BuyerChannelName { get; set; }

        /// <summary>
        /// Gets or sets the total leads.
        /// </summary>
        public int TotalLeads { get; set; }

        /// <summary>
        /// Gets or sets the sold leads.
        /// </summary>
        public int SoldLeads { get; set; }

        /// <summary>
        /// Gets or sets the rejected leads.
        /// </summary>
        public int RejectedLeads { get; set; }


        /// <summary>
        /// Gets or sets the min price error leads.
        /// </summary>
        public int MinPriceErrorLeads { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        public decimal BuyerPrice { get; set; }

        /// <summary>
        /// Gets or sets the buyer price.
        /// </summary>
        public decimal AffiliatePrice { get; set; }

        /// <summary>
        /// Gets or sets the other buyer price.
        /// </summary>
        public decimal OtherBuyerPrice  { get; set; }

        #endregion Properties
    }
}