using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using notification = Adrack.Core.Domain.Notification;

namespace Adrack.WebApi.Models.Notification
{
    public class SendBulkNotificationModel
    {
        public List<int> UserIds { get; set; } = new List<int>();
        public notification.Notification Notification { get; set; }
    }
}