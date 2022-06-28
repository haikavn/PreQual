using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Users
{
    public class UserSimpleModel : BaseIdentifiedItem
    {
        public string FullName { get; set; }
    }
}