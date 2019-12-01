using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsAccess notificationsAccess;

        public NotificationsService(INotificationsAccess notificationsAccess)
        {
            this.notificationsAccess = notificationsAccess;
        }

        public void AddNotification(NotificationModel notification)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            //return true if 1 or more entities were changed.
            return (await this.notificationsAccess.SaveChangesAsync());
        }

        public IReadOnlyCollection<NotificationModel> GetAllNotifications()
        {
            return this.notificationsAccess.GetAllNotifications().ToList();
        }

        public TemplateModel GetTemplate(string eventType)
        {
            return this.notificationsAccess.GetTemplate(eventType);
        }

        public IReadOnlyCollection<NotificationModel> GetNotificationsByUser(int userId)
        {
            return this.notificationsAccess.GetNotificationsByUser(userId).ToList();
        }
    }
}
