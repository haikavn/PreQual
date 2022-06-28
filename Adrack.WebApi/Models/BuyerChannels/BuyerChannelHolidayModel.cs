using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelHolidayModel
    {
        public long Id { get; set; }
        public long BuyerChannelId { get; set; }

        public bool IsCustomHoliday { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

    }
}