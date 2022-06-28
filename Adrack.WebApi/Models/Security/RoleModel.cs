using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Security;
using Adrack.WebApi.Models.Users;

namespace Adrack.WebApi.Models.Security
{
    public class RoleModel
    {
        #region Properties

        public long Id { get; set; }
        public string Name { get; set; }
        public UserTypes UserType { get; set; }
        public bool IsActive { get; set; }
        public List<UserSimpleModel> Users { get; set; }
        public List<PermissionModel> Permissions { get; set; }

        #endregion Properties

        public static explicit operator Role(RoleModel roleModel)
        {
            return new Role
            {
                Id = roleModel.Id,
                Name = roleModel.Name,
                UserType = roleModel.UserType,
                Active = roleModel.IsActive
            };
        }

        public static explicit operator RoleModel(Role role)
        {
            return new RoleModel
            {
                Id = role.Id,
                Name = role.Name,
                IsActive = role.Active,
                UserType = role.UserType,
                Users = new List<UserSimpleModel>(),
                Permissions = new List<PermissionModel>()
            };
        }
    }
}