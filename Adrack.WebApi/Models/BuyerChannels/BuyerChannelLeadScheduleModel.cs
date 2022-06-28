using System;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelScheduleModel
    {
        public short WeekDay { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public int PostedWait { get; set; }
        public int SoldWait { get; set; }
        public decimal? PriceLimit { get; set; }
        public int LeadLimit { get; set; }

        public static explicit operator BuyerChannelSchedule( BuyerChannelScheduleModel buyerChannelScheduleModel)
        {
           return new BuyerChannelSchedule
            {
                Id = 0,
                DayValue = buyerChannelScheduleModel.WeekDay,
                FromTime = (int)buyerChannelScheduleModel.FromTime.TotalMinutes,
                ToTime = (int)buyerChannelScheduleModel.ToTime.TotalMinutes,
                Quantity = 0,
                PostedWait = buyerChannelScheduleModel.PostedWait,
                SoldWait = buyerChannelScheduleModel.SoldWait,
                HourMax = 0,
                BuyerChannelId = 0,
                Price = buyerChannelScheduleModel.PriceLimit,
                LeadStatus = null
            };
        }
    }
}