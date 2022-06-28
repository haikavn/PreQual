using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerSettingsModel
    {
        public bool KeepConsistentLeadId { get; set; }
        public short MaxDuplicateDays { get; set; }

        public int DailyCap { get; set; }

        public bool CoolOffEnabled { get; set; }

        public DateTime? CoolOffStart { get; set; }

        public DateTime? CoolOffEnd { get; set; }
    }
}