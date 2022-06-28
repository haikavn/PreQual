using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.CustomReports
{
    public partial class ReportFilterSetting : BaseEntity
    {
        public long ReportTypeId { get; set; }
        public ReportEntityType ReportEntityType1 { get; set; }
        public string ReportEntityIds1 { get; set; }
        public ReportEntityType? ReportEntityType2 { get; set; }
        public string ReportEntityIds2 { get; set; }
        public ReportPeriodType? ReportPeriodType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsAscending { get; set; }

        public int? OrderVariableId { get; set; }
    }
}
