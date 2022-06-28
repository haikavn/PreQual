using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelAttachOrDetachModel
    {
        public long BuyerChannelId { get; set; }
        public bool IsAttach { get; set; }
    }
}