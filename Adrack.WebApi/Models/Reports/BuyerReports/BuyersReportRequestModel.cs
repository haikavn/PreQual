using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Reports
{
    public class BuyersReportRequestModel
    {
            /// <summary>
            /// Gets or sets the date.
            /// </summary>
            /// <value>The date.</value>
            public DateTime StartDate { get; set; }

            /// <summary>
            /// Gets or sets the date.
            /// </summary>
            /// <value>The date.</value>
            public DateTime EndDate { get; set; }

            /// <summary>
            /// Gets or sets the total leads.
            /// </summary>
            /// <value>The total leads.</value>
            public List<long> BuyerIds { get; set; }

            /// <summary>
            /// Gets or sets the sold leads.
            /// </summary>
            /// <value>The sold leads.</value>
            public List<long> BuyerChannelIds { get; set; }

            /// <summary>
            /// Gets or sets the rejected leads.
            /// </summary>
            /// <value>The rejected leads.</value>
            public List<long> AffiliateIds { get; set; }

            public List<long> AffiliateChannelIds { get; set; }

            /// <summary>
            /// Gets or sets the loaned leads.
            /// </summary>
            /// <value>The loaned leads.</value>
            public List<long> CampaignIds { get; set; }

            public int CountryId { get; set; }

            public List<long> States { get; set; }
            /// <summary>
            /// Indicates CSV download.
            /// </summary>
            /// <value>Download CSV.</value>
            public bool IsCsv { get; set; }
    }
}