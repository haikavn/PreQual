using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Notification
{
    public partial class UserNotification : BaseEntity
    {
        public long UserId { get; set; }
        public long NotificationId { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public bool IsRead { get; set; }
    }
}
