﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Notifications.Common.Models
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string EventType { get; set; }
        public string Body { get; set; }
    }
}
