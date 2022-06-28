using System;
using System.ComponentModel.DataAnnotations;

namespace Adrack.WebApi.Models.New.Support
{
    public class SupportTicketsCreateModel
    {
        public long? Id { get; set; }
        public long UserId { get; set; }
        public long ManagerId { get; set; }
        
        [MaxLength(255)]
        public string Subject { get; set; }
        
        [MaxLength(1024)]
        public string Message { get; set; }
        
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? DueDate { get; set; }
        public byte? TicketType { get; set; }
        //public string TicketFilePath { get; set; }
    }
}