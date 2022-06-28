using Adrack.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerInvitationRoleUpdateModel
    {
        [Required]
        public long BuyerId { get; set; }
        [Required]
        public long BuyerInvitationId { get; set; }
        [Required]
        public BuyerInvitationRoles Role { get; set; }
    }
}