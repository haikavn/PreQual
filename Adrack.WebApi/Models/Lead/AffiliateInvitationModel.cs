using Adrack.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateInvitationModel
    {
        public long AffiliateInvitationId { get; set; }

        [Required]
        public long AffiliateId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string RecipientEmail { get; set; }

        public DateTime InvitationDate { get; set; }

        public AffiliateInvitationStatuses Status { get; set; }

        [Required]

        public int Role { get; set; }

        public bool IsSendInvitation { get; set; }

        public bool CanSendEmail { get; set; } = true;
    }
}