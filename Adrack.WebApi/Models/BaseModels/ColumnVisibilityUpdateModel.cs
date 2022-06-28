using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BaseModels
{
    public class ColumnVisibilityUpdateModel
    {
        public string Page { get; set; }
        public List<ColumnsVisibilityModel> ColumnsVisibilities { get; set; }
    }
}