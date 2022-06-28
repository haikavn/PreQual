using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AddRefundLeadsModel
    {
        public long LeadId { get; set; }

        public string Note { get; set; }

        public long BuyerId { get; set; }
    }
}