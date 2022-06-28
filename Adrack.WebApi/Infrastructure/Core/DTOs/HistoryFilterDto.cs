using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Service.Content;

namespace Adrack.WebApi.Infrastructure.Core.DTOs
{
    public class HistoryFilterDto
    {
        public int PageStart { get; internal set; }
        public int PageSize { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public HistoryAction Action { get; set; }
        public long UserId { get; set; }
        public string UserIds { get; set; }
        public bool SimpleMode { get; set; }
        public string Entity { get; set; }
        public long EntityId { get; set; }
    }
}