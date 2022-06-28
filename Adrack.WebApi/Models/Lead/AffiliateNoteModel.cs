using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.Lead
{
    public class AffiliateNoteModel : IBaseOutModel
    {
        public long NoteId { get; set; }
        [Required]
        public long AffiliateChannelId { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Note { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}