using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adrack.WebApi.Models.Users
{
    public class UserInviteModel
    {
        public long UserId { get; set; }

        public string EntityName { get; set; }

        public long EntityId { get; set;}
    }
}