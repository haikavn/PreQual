using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.Core.Domain.Security;

namespace Adrack.Data.Domain.Security
{
    public class RolePermissionMap : AppEntityTypeConfiguration<RolePermission>
    {
        public RolePermissionMap()
        {
            this.ToTable("RolePermission");
            this.HasKey(x => x.Id);

            //this.HasKey(x=>x.PermissionId);
            // this.HasKey(x => x.RoleId);
           
            //this.HasKey(x => new {x.RoleId, x.PermissionId});
            this.Property(x => x.State).IsRequired();
        }
    }
}
