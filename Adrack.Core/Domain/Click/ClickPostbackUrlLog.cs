using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Click
{

    public class ClickPostbackUrlLog : BaseEntity
    {
        public DateTime Created { get; set; }

        public string PostedUrl { get; set; }
        public long LeadId { get; set; }
    }
}


