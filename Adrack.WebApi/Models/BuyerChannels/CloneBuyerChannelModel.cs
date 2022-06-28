using System;
using System.Collections.Generic;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.Lead;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class CloneBuyerChannelModel
    {
        public long Id { get; set; }

        public string Name { get; set; }


        public CloneBuyerChannelModel()
        {
        }
    }
}