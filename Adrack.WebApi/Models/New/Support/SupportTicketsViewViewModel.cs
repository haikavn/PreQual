using System;

namespace Adrack.WebApi.Models.New.Support
{
    public class SupportTicketsViewViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public long ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime DateTime { get; set; }
        public int NewCount { get; set; }
    }
}