using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class LeadCommonInfoItemModel
    {
        public string Name { get; set; }

        public string SectionName { get; set; }

        public string Value { get; set; }

        public string EncryptedValue { get; set; }

        public bool IsEncrypted { get; set; }

    }
}