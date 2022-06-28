using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Adrack.Core;

namespace Adrack.WebApi.Models.Buyers
{
    public class BuyerIdentificationModel
    {
        [EnumDataType(typeof(BuyerType))]
        public short TypeId { get; set; }
        [EnumDataType(typeof(EntityFilterByStatus))]
        public short Status { get; set; }

        public bool SendStatementReport { get; set; }
    }
}