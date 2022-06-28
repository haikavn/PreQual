using System;
using System.Collections.Generic;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Lead;
using System.Linq;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelScheduleDayModel
    {
        public long Id { get; set; }
        public short DayValue { get; set; }
        public int DailyLimit { get; set; }

        public bool NoLimit { get; set; }

        public bool IsEnabled { get; set; }

        public List<BuyerChannelScheduleTimePeriodInpModel> ScheduleTimePeriods { get; set; }

        public bool ValidateDailyLimit()
        {
            int totalLimits = 0;

            if (ScheduleTimePeriods != null)
            {
                totalLimits = ScheduleTimePeriods.Sum(x => x.Quantity);
            }

            if (totalLimits > DailyLimit) return false;

            return true;
        }

        public BuyerChannelScheduleDayModel()
        {
            ScheduleTimePeriods = new List<BuyerChannelScheduleTimePeriodInpModel>();
        }
    }
}