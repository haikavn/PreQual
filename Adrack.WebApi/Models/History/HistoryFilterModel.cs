using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.History
{
    public class HistoryFilterModel
    {
        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool WithChanges { get; set; }
    }
}