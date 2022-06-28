using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.CustomReports
{
    public partial class ReportVariable:BaseEntity
    {
        public long ReportTypeId { get; set; }
        public long ReportVariableTypeId { get; set; }
        public long VariableOrder { get; set; }
    }
}
