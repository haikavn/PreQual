﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Adrack.Core;
using Adrack.Core.Domain.Lead;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;

namespace Adrack.WebApi.Models.BuyerChannels
{
    public class BuyerChannelNoteModel
    {
        public long NoteId { get; set; }
        [Required]
        public long BuyerChannelId { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime? UpdatedDate { get; set; }
     
    }
}