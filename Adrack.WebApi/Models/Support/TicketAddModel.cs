using Adrack.WebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Support
{
    public class TicketAddModel : BaseModel, IBaseInModel
    {
        public string Message { get; set; }
        public string Cc { get; set; }
        public string Priority { get; set; }
        public string Subject { get; set; }
        public string SendTo { get; set; }
        public string ManagerId { get; set; }
    }
}