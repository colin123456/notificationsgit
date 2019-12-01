using System;
using Notifications.Common.Models;

namespace Notifications.DataAccess.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string EventType { get; set; }
        public string Body { get; set; }
    }
}
