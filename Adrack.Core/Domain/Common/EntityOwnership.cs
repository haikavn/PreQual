using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Common
{
    public class EntityOwnership : BaseEntity
    {
        public long? UserId { get; set; }

        public long EntityId { get; set; }

        public string EntityName { get; set; }

        public bool IsApproved { get; set; }

        //public Guid AccountGuid { get; set; }
    }
}
