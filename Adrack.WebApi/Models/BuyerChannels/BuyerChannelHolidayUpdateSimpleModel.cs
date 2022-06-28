using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelHolidayUpdateSimpleModel
    {
        [MaxLength(50)]
        public string Name { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

    }
}