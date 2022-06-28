using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.CustomReports
{
    public partial class UserReport : BaseEntity
    {
        public long UserId { get; set; }
        public long ReportId { get; set; }
        public bool IsOwner { get; set; }
    }
}
