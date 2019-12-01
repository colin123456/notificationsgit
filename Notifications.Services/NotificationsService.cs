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

        public async Task<IReadOnlyCollection<NotificationModel>> GetAllNotifications()
        {
            return (IReadOnlyCollection<NotificationModel>) await this.notificationsAccess.GetAllNotifications();
        }

        public TemplateModel GetTemplate(string eventType)
        {
            return this.notificationsAccess.GetTemplate(eventType);
        }

        public async Task<IReadOnlyCollection<NotificationModel>> GetNotificationsByUser(int userId)
        {
            return (IReadOnlyCollection<NotificationModel>) await this.notificationsAccess.GetNotificationsByUser(userId);
        }
    }
}
