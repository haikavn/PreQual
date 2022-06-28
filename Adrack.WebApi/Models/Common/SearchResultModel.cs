using Adrack.WebApi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adrack.Core.Domain.Lead;
using Adrack.Core;

namespace Adrack.WebApi.Models.Common
{
    public class SearchResultModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Entity { get; set; }
    }
}