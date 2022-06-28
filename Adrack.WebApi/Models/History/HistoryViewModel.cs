using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Content;

namespace Adrack.WebApi.Models.History
{
    public class HistoryViewModel
    {
        public long Id { get; set; }

        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public List<PropertyChangeHistory> Changes { get; set; }
    }
}