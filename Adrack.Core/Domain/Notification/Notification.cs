using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Notification
{
    public partial class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public long UserId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public bool IsRead { get; set; }

    }
}
