using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Security
{
    public partial class RolePermission : BaseEntity
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        public byte State { get; set; }

        public Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
