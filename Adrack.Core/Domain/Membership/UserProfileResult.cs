using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Membership
{
    public class UserProfileResult
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public DateTime LoginDate { get; set; }
        public bool Deleted { get; set; }
        public bool Active { get; set; }
        public short UserType { get; set; }
    }
}
