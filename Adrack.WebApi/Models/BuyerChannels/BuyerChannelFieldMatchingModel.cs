using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelFieldMatchingModel
    {
       public long BuyerChannelFieldId { get; set; }
       public string InputValue { get; set; }
       public string OutputValue { get; set; }

       public static explicit operator BuyerChannelTemplateMatching(BuyerChannelFieldMatchingModel buyerChannelFieldMatchingModel)
       {
           return new BuyerChannelTemplateMatching
           {
               Id = 0,
               BuyerChannelTemplateId = 0,
               InputValue = buyerChannelFieldMatchingModel.InputValue,
               OutputValue = buyerChannelFieldMatchingModel.OutputValue
           };
        }
    }
}