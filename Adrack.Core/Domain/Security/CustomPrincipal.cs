using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public User User { get; set; }

        public CustomPrincipal(User user)
        {
            User = user;
        }

    }
}
