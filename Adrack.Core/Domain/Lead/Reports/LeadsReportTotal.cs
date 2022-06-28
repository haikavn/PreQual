using System;

namespace Adrack.Core.Domain.Lead.Reports
{
    /// <summary>
    /// Class BuyerReportByHour.
    /// Implements the <see cref="Adrack.Core.BaseEntity" />
    /// </summary>
    /// <seealso cref="Adrack.Core.BaseEntity" />
    public partial class LeadsReportTotal
    {
        public int Leads { get; set; }

        public int Pings { get; set; }
    }
}