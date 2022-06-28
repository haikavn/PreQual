using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Core;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerBillingSettingModel
    {
        public bool AutosendInvoice { get; set; }
        public string BillFrequency { get; set; }
        public int FrequencyValue { get; set; }
        public decimal Credit { get; set; }
    }
}