using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.History
{
    public class HistoryFilterOutModel
    {
        public HistoryFilterModelWithPaging HistoryFilterModel { get; set; }
        public long Count { get; set; }
        public List<HistoryViewModel> HistoryViewModel { get; set; } = new List<HistoryViewModel>();
    }
}