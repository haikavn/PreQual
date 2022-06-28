using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Common
{
    public class CountryViewModel : BaseIdentifiedItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ZipCodeLength { get; set; }
    }
}