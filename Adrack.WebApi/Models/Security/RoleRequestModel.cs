using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Security;
using Adrack.WebApi.Models.Users;

namespace Adrack.WebApi.Models.Security
{
    public class RoleRequestModel
    {
       #region Properties
        public string Name { get; set; }
        [EnumDataType(typeof(UserTypes))]
        public UserTypes UserType { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionSimpleModel> Permissions { get; set; }

        #endregion Properties
    }
}