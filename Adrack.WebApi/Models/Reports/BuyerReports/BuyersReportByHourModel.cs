using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports.BuyerReports
{
    public class BuyersReportByHourModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

  
        /// <summary>
        /// Gets or sets the total leads.
        /// </summary>
        /// <value>The total leads.</value>
        public int TotalLeads1 { get; set; }

        /// <summary>
        /// Gets or sets the sold leads.
        /// </summary>
        /// <value>The sold leads.</value>
        public int SoldLeads1 { get; set; }

        public int TotalLeads2 { get; set; }

        public int SoldLeads2 { get; set; }

        public int TotalLeads3 { get; set; }

        public int SoldLeads3 { get; set; }


    }
}