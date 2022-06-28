using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.Core.Domain.Lead;

namespace Adrack.WebApi.Models.AffiliateChannel
{
    public class AffiliateChannelCodeSampleEmailModel
    {
        public string CodeSample { get; set; }
        public string CodeSampleName { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }

    }


}