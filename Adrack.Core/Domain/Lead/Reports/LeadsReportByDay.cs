using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class BuyerReportByHour.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class LeadsReportByDay
    {
        public DateTime Created { get; set; }
        public int? Received { get; set; }
        public int? Posted { get; set; }
        public int? Pings { get; set; }
        public int? Sold { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Profit { get; set; }
    }
}