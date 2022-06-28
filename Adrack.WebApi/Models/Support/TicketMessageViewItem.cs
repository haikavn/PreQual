using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Support
{
    public class TicketMessageViewItem : BaseModel, IBaseOutModel
    {
        public long TicketId { get; internal set; }
        public long AuthorId { get; internal set; }

        public string PriorityString { get; internal set; }
        public string StatusString { get; internal set; }

        public string Reporter { get; internal set; } // User Name
        public string Assignee { get; internal set; } // Manager Name

        public string Subject { get; internal set; }
        public string Message { get; internal set; }

        public string CreateTime { get; internal set; }

        public string FilePath { get; internal set; }

        public string NewCount { get; internal set; }
        public bool IsOwner { get; internal set; }
    }
}