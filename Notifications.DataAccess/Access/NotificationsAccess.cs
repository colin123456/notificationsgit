using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess.Access
{
    public class NotificationsAccess : INotificationsAccess
    {
        private readonly NotificationsDbContext dbContext;

        public NotificationsAccess(NotificationsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddNotification(NotificationModel notification)
        {
            if (notification == null)
            {
                throw new ArgumentException(nameof(notification));
            }
            this.dbContext.Add(notification);
        }

        public async Task<bool> SaveChangesAsync()
        {
            //return true if 1 or more entities were changed.
            return (await this.dbContext.SaveChangesAsync() > 0);
        }
        public IEnumerable<NotificationModel> GetAllNotifications()
        {
            return dbContext.Notifications.Select(x => new NotificationModel()
            {
                Id = x.Id,
                UserId = x.UserId,
                Body = x.Body,
                EventType = x.EventType
            });
        }
      
        public TemplateModel GetTemplate(string eventType)
        {
            return dbContext.Templates.Select(x => new TemplateModel()
            {
                Id = x.Id,
                Body = x.Body,
                EventType = x.EventType,
                Title = x.Title
            }).FirstOrDefault(x => x.EventType == eventType);
        }

        public IEnumerable<NotificationModel> GetNotificationsByUser(int userId)
        {
            return dbContext.Notifications.Where(x => x.UserId ==  userId).Select(x => new NotificationModel()
            {
                Id = x.Id,
                UserId = x.UserId,
                Body = x.Body,
                EventType = x.EventType
            });
        }
    }
}
