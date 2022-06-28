using Adrack.Core;
using Adrack.Core.Domain.Security;
using Adrack.WebApi.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.WebApi.Infrastructure.Core.Interfaces
{
    public interface IRolePermissionService
    {
        PermissionModel BuildPermissionTreeWithState(PermissionModel permission, List<RolePermission> permissions);
        PermissionModel BuildPermissionTree(PermissionModel permission, List<Permission> permissions, UserTypes? userType = null);
    }
}
