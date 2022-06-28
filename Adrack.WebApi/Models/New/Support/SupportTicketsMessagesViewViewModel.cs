using System;

namespace Adrack.WebApi.Models.New.Support
{
    public class SupportTicketsMessagesViewViewModel
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public DateTime DateTime { get; set; }
        public long AuthorId { get; set; }
        public bool IsNew { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }
}