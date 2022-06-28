using System;
using System.Collections.Generic;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Lead;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelScheduleTimePeriodInpModel
    {
        public long Id { get; set; }
        public int FromTime { get; set; }
        public int ToTime { get; set; }
        public int Quantity { get; set; }
        public int PostedWait { get; set; }
        public int SoldWait { get; set; }
        public int HourMax { get; set; }
        public decimal? Price { get; set; }
        public short? LeadStatus { get; set; }

        public static explicit operator BuyerChannelScheduleTimePeriod(BuyerChannelScheduleTimePeriodInpModel buyerChannelScheduleTimePeriodModel)
        {
            return new BuyerChannelScheduleTimePeriod
            {
                FromTime = buyerChannelScheduleTimePeriodModel.FromTime,
                HourMax = buyerChannelScheduleTimePeriodModel.HourMax,
                LeadStatus = buyerChannelScheduleTimePeriodModel.LeadStatus,
                PostedWait = buyerChannelScheduleTimePeriodModel.PostedWait,
                Price = buyerChannelScheduleTimePeriodModel.Price,
                Quantity = buyerChannelScheduleTimePeriodModel.Quantity,
                SoldWait = buyerChannelScheduleTimePeriodModel.SoldWait,
                ToTime = buyerChannelScheduleTimePeriodModel.ToTime,
                Id = buyerChannelScheduleTimePeriodModel.Id
            };
        }
    }
}