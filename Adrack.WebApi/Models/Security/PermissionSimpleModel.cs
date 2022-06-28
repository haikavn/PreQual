using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Security
{
    public class PermissionSimpleModel
    {
        public long Id { get; set; }
        public bool IsAccess { get; set; }
        public List<PermissionSimpleModel> SubPermissions { get; set; }
    }
}