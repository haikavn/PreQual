using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadModel : IBaseOutModel
    {
        public int TotalRowsCount { get; internal set; }
        public int RowsPerPage { get; internal set; }
        public int PageCount { get; internal set; }
        public string TimeZoneNowStr { get; internal set; }
        public DateTime TimeZoneNow { get; internal set; }
        public bool ShowNotes { get; internal set; }
    }
}