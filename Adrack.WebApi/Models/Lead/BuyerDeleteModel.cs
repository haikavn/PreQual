using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Filter = Adrack.Core.Domain.Lead.Filter;
using Adrack.Core;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerDeleteModel
    {
        public DeletedStatus DeleteStatus { get; set; }
    }
}