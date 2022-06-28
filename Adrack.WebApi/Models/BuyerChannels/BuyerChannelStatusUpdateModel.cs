using Adrack.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelStatusUpdateModel:BaseEntity
    {
        [Required]
        [EnumDataType(typeof(BuyerChannelStatuses))]
        public BuyerChannelStatuses Status { get; set; }
    }
}