using System;

namespace Adrack.WebApi.Models.New.Support
{
    public class GetSupportTicketsInModel
    {
        public string UserIds { get; set; }
        public string ManagerIds { get; set; }
        public int Status { get; set; }
        public DateTime DateTime { get; set; }
    }
}