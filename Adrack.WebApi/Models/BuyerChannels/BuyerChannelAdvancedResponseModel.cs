using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelAdvancedResponseModel
    {
        public long BuyerChannelId { get; set; }
        public List<ChildChannel> ChildChannels { get; set; } = new List<ChildChannel>();
        public bool CustomPriceReject { get; set; }
        public bool CustomDynamicValues { get; set; }
        [Required]
        public string WinResponseUrl { get; set; }
        [Required]
        public string WinResponsePostMethod { get; set; }
        [Required]
        public string PriceReject { get; set; }
        [Required]
        public double LeadId { get; set; }
        public short TypeId { get; set; }
    }

    public class ChildChannel
    {
        public string Name { get; set; }
        public long Id { get; set; }
    }
}