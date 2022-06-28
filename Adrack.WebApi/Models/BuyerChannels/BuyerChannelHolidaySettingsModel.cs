using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelHolidaySettingsModel
    {
        public long BuyerChannelId { get; set; }
        public long CountryId { get; set; }

        public short HolidayYear { get; set; }

        public bool HolidayAnnualAutoRenew { get; set; }

        public bool HolidayIgnore { get; set; }

        public List<BuyerChannelHolidayUpdateSimpleModel> Holidays { get; set; }

        public BuyerChannelHolidaySettingsModel()
        {
            Holidays = new List<BuyerChannelHolidayUpdateSimpleModel>();
        }
    }
}