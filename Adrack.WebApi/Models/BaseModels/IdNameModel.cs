using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BaseModels
{
    public class IdNameModel : BaseIdentifiedItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}