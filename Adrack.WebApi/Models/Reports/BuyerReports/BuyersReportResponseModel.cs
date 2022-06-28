using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports
{
    public class BuyersReportResponseModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string title { get; set; }
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date { get; set; }

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
        /// Gets or sets the rejected leads.
        /// </summary>
        /// <value>The rejected leads.</value>
        public int RejectedLeads { get; set; }

        /// <summary>
        /// Gets or sets the loaned leads.
        /// </summary>
        /// <value>The loaned leads.</value>
        public int LoanedLeads { get; set; }

        /// <summary>
        /// Gets or sets the debit.
        /// </summary>
        /// <value>The debit.</value>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the credit.
        /// </summary>
        /// <value>The credit.</value>
        public decimal Credit { get; set; }

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
    }
}