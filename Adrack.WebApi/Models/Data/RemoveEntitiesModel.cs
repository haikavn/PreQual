using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.Employee
{
    public class RemoveEntitiesModel
    {
        public EntityTypes EntityType { get; set; }
        public List<int> EntityIds { get; set; } = new List<int>();
        public string Passport { get; set; }
    }
}