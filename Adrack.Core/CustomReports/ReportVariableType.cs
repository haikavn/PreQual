using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.CustomReports
{
    public partial class ReportVariableType : BaseEntity
    {
        public string Name { get; set; }
        public string FieldName { get; set; }
        public string GroupName { get; set; }
        public string Comment { get; set; }

        public bool? IsGroupBy { get; set; }
    }
}
