﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsService
    {
        Task<IReadOnlyCollection<NotificationModel>> GetAllNotifications();
        Task<IReadOnlyCollection<NotificationModel>> GetNotificationsByUser(int userId);
        TemplateModel GetTemplate(string eventType);
        void AddNotification(NotificationModel notification);
        Task<bool> SaveChangesAsync();
    }
}
