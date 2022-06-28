using Adrack.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class BuyerInvitationModel
    {
        [Required]
        public long BuyerId { get; set; }

        public long BuyerInvitationId { get; set; }

        [Required]
        [EmailAddress]
        public string RecipientEmail { get; set; }

        public DateTime InvitationDate { get; set; }

        public BuyerInvitationStatuses Status { get; set; }
        [Required]
        public int Role { get; set; }

        public bool IsSendInvitation { get; set; }

        public bool CanSendEmail { get; set; } = true;
    }
}