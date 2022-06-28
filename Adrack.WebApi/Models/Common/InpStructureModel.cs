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
    public class InpStructureModel
    {
        public string Value { get; set; }
        public string ReturnModelType { get; set; }
    }
}