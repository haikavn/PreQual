using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerViewModel
    {
        public long BuyerId { get; internal set; }
        public long? ManagerId { get; internal set; }
        public string Manager { get; set; }
        public long CountryId { get; internal set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
        public long? StateProvinceId { get; set; }
        public string StateProvinceName { get; set; }
        public string Phone { get; set; }
        public string Name { get; internal set; }
        public short Status { get; internal set; }
        public BuyerType BuyerType { get; set; }
    }
}