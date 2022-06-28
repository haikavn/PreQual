using System;
using System.Collections.Generic;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Lead;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelScheduleDayInpModel
    {
        public long Id { get; set; }
        public long BuyerChannelId { get; set; }
        public short DayValue { get; set; }
        public int DailyLimit { get; set; }

        public bool NoLimit { get; set; }

        public bool IsEnabled { get; set; }
    }
}