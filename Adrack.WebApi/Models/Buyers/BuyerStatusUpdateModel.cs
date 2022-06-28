using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Core;
using Adrack.WebApi.Models.BaseModels;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerStatusUpdateModel
    {
        [Required]
        [EnumDataType(typeof(EntityStatus))]
        public EntityStatus Status { get; set; }
    }
}