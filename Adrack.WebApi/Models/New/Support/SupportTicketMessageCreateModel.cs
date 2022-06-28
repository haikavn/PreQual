using System;

namespace Adrack.WebApi.Models.New.Support
{
    public class SupportTicketMessageCreateModel
    {
        public long TicketId { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public DateTime DateTime { get; set; }
        public long AuthorId { get; set; }
        public bool IsNew { get; set; }
    }
}