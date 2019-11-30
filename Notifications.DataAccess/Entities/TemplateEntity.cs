using System;
using Notifications.Common.Models;

namespace Notifications.DataAccess.Entities
{
    public class TemplateEntity
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
    }
}
