using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.Core;
using Adrack.Core.Domain.Membership;
using Adrack.Core.Domain.Notification;
using Adrack.Core.Infrastructure.Data;
using Adrack.Data;
using Adrack.Service.Infrastructure.ApplicationEvent;

namespace Adrack.Service.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Core.Domain.Notification.Notification> _notificationRepository;
        private readonly IRepository<Core.Domain.Notification.UserNotification> _userNotificationRepository;
        private readonly IAppContext _appContext;
        private readonly IAppEventPublisher _appEventPublisher;

        public NotificationService(IRepository<Core.Domain.Notification.Notification> notificationRepository
                                    ,IRepository<Core.Domain.Notification.UserNotification> userNotificationRepository
                                    , IAppContext appContext
                                    , IAppEventPublisher appEventPublisher)
        {
            this._appContext = appContext;
            this._notificationRepository = notificationRepository;
            this._userNotificationRepository = userNotificationRepository;
            this._appEventPublisher = appEventPublisher;
        }

        public void BulkSendNotification(List<User> user, Core.Domain.Notification.Notification notification)
        {
            if (user != null && user.Any())
            {

                user.ForEach(u =>
                {
                    notification.UserId = u.Id;
                    notification.DateTimeUtc = DateTime.UtcNow;

                    _notificationRepository.Insert(notification);
                    _appEventPublisher.EntityInserted(notification);
                });
            }
        }

        public List<Core.Domain.Notification.UserNotification> GetUserNotifications(User user)
        {
            return _userNotificationRepository.Table.Where(x => x.UserId == user.Id).ToList();
        }

        public Core.Domain.Notification.Notification Read(long notificationId)
        {
            var notification = GetNotification(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _notificationRepository.Update(notification);
                _appEventPublisher.EntityUpdated(notification);
            }

            return notification;                                                                                
        }

        public Adrack.Core.Domain.Notification.Notification Send(Adrack.Core.Domain.Notification.Notification notification)
        {
            _notificationRepository.Insert(notification);
            _appEventPublisher.EntityInserted(notification);

            return notification;
        }

        public Adrack.Core.Domain.Notification.Notification GetNotification(long id)
        {
            return _notificationRepository.GetById(id);
        }

        public Adrack.Core.Domain.Notification.Notification GetNotification(long notificationId, long userId)
        {
            return _notificationRepository.Table.FirstOrDefault(n => n.Id == notificationId && n.UserId == userId);
        }

        public IList<Adrack.Core.Domain.Notification.Notification> GetNotifications(long userId)
        {
            return _notificationRepository.Table.Where(n => n.UserId == userId).ToList();
        }
    }
}
