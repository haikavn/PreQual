using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports
{
    public class AffiliatesReportRequestModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the sold leads.
        /// </summary>
        /// <value>The sold leads.</value>
        public List<long> AffiliateChannelIds { get; set; }

        /// <summary>
        /// Gets or sets the rejected leads.
        /// </summary>
        /// <value>The rejected leads.</value>
        public List<long> AffiliateIds { get; set; }


        public int CountryId { get; set; }

        /// <summary>
        /// State Ids for buyers report.
        /// </summary>
        public List<long> States { get; set; }



        /// <summary>
        /// Indicates CSV download.
        /// </summary>
        /// <value>Download CSV.</value>

        public bool IsCsv { get; set; }
    }
}