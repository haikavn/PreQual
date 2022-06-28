using Adrack.Core;
using Adrack.Core.Domain.Security;
using Adrack.Service.Membership;
using Adrack.Service.Security;
using Adrack.WebApi.Infrastructure.Core.Interfaces;
using Adrack.WebApi.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Infrastructure.Core.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IPermissionService _permissionService;
        private readonly IAppContext _appContext;
        private readonly IAddonService _addonService;

        public RolePermissionService(IPermissionService permissionService, IAppContext appContext, IAddonService addonService)
        {
            _permissionService = permissionService;
            _appContext = appContext;
            _addonService = addonService;
        }
        public PermissionModel BuildPermissionTreeWithState(PermissionModel permission, List<RolePermission> rolePermissions)
        {
            var childPermissions = _permissionService.GetAllPermissions(permission.Id);
            foreach (var item in childPermissions)
            {
                var permissionModel = (PermissionModel)item;
                var rolePermission = rolePermissions.Where(x => x.PermissionId == item.Id).FirstOrDefault();

                var addonPermissions = _addonService.GetAddonsByPermissionId(item.Id);

                if (addonPermissions.Count == 0)
                    permissionModel.IsAccess = (_appContext.AppUser.UserType == UserTypes.Super || (rolePermission != null && rolePermission.State == 1)) ? true : false;
                else
                    permissionModel.IsAccess = (rolePermission != null && rolePermission.State == 1) ? true : false;

                permission.SubPermissions.Add(permissionModel);
                BuildPermissionTreeWithState(permissionModel, rolePermissions);
            }
            return permission;
        }

        public PermissionModel BuildPermissionTree(PermissionModel permission, List<Permission> permissions, UserTypes? userType = null)
        {
            if (permissions == null || !permissions.Any())
                permissions = _permissionService.GetAllPermissions().
                    Where(x => !x.Deleted).ToList();
            var childPermissions = _permissionService.GetAllPermissions(permission.Id);
            foreach (var item in childPermissions)
            {
                if (userType.HasValue && userType.Value != UserTypes.Super && userType.Value != UserTypes.Network &&
                    !item.UserTypeList.Contains(userType.Value))
                    continue;

                var permissionModel = (PermissionModel)item;
                permission.SubPermissions.Add(permissionModel);
                BuildPermissionTree(permissionModel, permissions, userType);
            }
            return permission;
        }
    }
}