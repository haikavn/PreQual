using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Addon
{
    public class UserAddonActivationModel
    {
        public long UserId { get; set; }

        public long AddonId { get; set; }

        public short Status { get; set; }

        public short IsTrial { get; set; } = 0;

    }
}