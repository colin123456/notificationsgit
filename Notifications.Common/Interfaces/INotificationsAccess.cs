using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsAccess
    {
        IEnumerable<NotificationModel> GetNotificationsByUser(int userId);
        IEnumerable<NotificationModel> GetAllNotifications();
        TemplateModel GetTemplate(string eventType);
        void AddNotification(NotificationModel notification);
        Task <bool> SaveChangesAsync();
    }
}
