using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.BaseModels
{
    public class BasePagedModel : IPagedData
    {
        public int RecordsStart { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
    }
}