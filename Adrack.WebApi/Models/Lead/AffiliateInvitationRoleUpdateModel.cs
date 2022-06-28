using Adrack.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateInvitationRoleUpdateModel
    {
        [Required]
        public long AffiliateId { get; set; }
        [Required]
        public long AffiliateInvitationId { get; set; }
        [Required]
        public AffiliateInvitationRole Role { get; set; }
    }
}