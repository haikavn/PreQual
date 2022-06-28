using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Infrastructure.Enums;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Support
{
    public class TicketMessagesModel : BaseModel, IBaseInModel
    {
        public string Message { get; set; }

        public string FilePath { get; set; }
    }
}