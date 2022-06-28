using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.WebApi.Models.Users;
using Newtonsoft.Json;

namespace Adrack.WebApi.Models.Security
{
    public class PermissionModel
    {
        public long ParentId { get; set; }
        public string Name { get; set; }

        public string Key { get; set; }
        public string Description { get; set; }
        public long Id { get; set; }
        public bool IsAccess { get; set; }
        public List<PermissionModel> SubPermissions { get; set; }

        public static explicit operator Permission(PermissionModel permissionModel)
        {
            return new Permission
            {
                Id = permissionModel.Id,
                ParentId = permissionModel.ParentId,
                Name = permissionModel.Name,
                Key = permissionModel.Key,
                Description = permissionModel.Description
            };
        }

        public static explicit operator PermissionModel(Permission permission)
        {
            return new PermissionModel()
            {
                Id = permission.Id,
                ParentId = permission.ParentId,
                Name = permission.Name,
                Key = permission.Key,
                Description = permission.Description,
                SubPermissions = new List<PermissionModel>()
            };
        }
    }
}