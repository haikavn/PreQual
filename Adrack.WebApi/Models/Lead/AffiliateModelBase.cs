using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateModelBase : IBaseInModel
    {
        public long AffiliateId { get; set; }
        public string Name { get; set; }
    }
}