using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Addon
{
    public class AddonActivationModel
    {
        public long AddonId { get; set; }

        public short Deactivate { get; set; }
    }
}