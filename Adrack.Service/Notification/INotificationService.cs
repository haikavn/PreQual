using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Notification;

namespace Adrack.Service.Notification
{
    public interface INotificationService
    {
        void BulkSendNotification(List<User> user, Adrack.Core.Domain.Notification.Notification notification);
        Adrack.Core.Domain.Notification.Notification Read(long notificationId);
        Adrack.Core.Domain.Notification.Notification Send(Adrack.Core.Domain.Notification.Notification notification);
        List<Adrack.Core.Domain.Notification.UserNotification> GetUserNotifications(User user);

        Adrack.Core.Domain.Notification.Notification GetNotification(long id);

        Adrack.Core.Domain.Notification.Notification GetNotification(long notificationId, long userId);

        IList<Adrack.Core.Domain.Notification.Notification> GetNotifications(long userId);

    }
}
