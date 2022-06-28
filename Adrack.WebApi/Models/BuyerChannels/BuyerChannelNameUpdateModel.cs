using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelNameUpdateModel:BaseEntity
    {
        public string Name { get; set; }
    }
}