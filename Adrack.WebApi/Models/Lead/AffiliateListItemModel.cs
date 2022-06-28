using Adrack.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateListItemModel
    {
        public long AffiliateId { get; set; }
        public string AffiliateName { get; set; }
        public int AllChannels { get; set; }
        public int ActiveChannels { get; set; }
        public string Manager { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RegistrationIpAddress { get; set; }
        public EntityStatus IsActive { get; set; }
        public string Icon { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}