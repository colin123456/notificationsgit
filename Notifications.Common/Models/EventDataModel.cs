using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Notifications.Common.Models
{
    public class EventDataModel
    {
      public string FirstName { get; set; }
      public DateTime AppointmentDateTime { get; set; }
      public string OrganisationName { get; set; }
      public string Reason { get; set; }
    }
}
