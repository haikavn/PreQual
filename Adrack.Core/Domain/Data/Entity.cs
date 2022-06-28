using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Data
{
    public class Entity
    {
        public EntityTypes EntityType { get; set; }
        public string EntityIds { get; set; }
        public string Passport { get; set; }
    }
}
