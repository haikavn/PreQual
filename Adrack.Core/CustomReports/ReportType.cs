using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.CustomReports
{
    public partial class ReportType:BaseEntity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
