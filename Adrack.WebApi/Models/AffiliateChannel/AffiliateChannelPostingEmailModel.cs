using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelPostingEmailModel
    {
        public string Email { get; set; }
        public string Comment { get; set; }

        public long ChannelId { get; set; }
    }
}