using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this._notificationsService = notificationsService;
        }

        [Route("")]
        [HttpGet]
        public IReadOnlyCollection<NotificationModel> Get()
        {
            return _notificationsService.GetAllNotifications();
        }


        //[HttpGet("{userId}")]
        //public ActionResult<NotificationModel> Get(int userId)
        //{
        //    return "value";
        //}

        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddEvent([FromBody] EventModel eventModel)
        {
            try
            {
                if (eventModel == null)
                {
                    //_logger.LogError($"Missing event data");
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    //_logger.LogError($"did not meet one of the validation rules");
                    return BadRequest();
                }

                //Read the template from  store based on EventType
                var cancelledTemplate = _notificationsService.GetTemplate(eventModel.Type);

                //Create new user Notification by combining the template with the data received in the event
                var interpolatedBodyData = ReplaceTemplateBody(cancelledTemplate.Body, eventModel.Data);
                
                NotificationModel notification = new NotificationModel()
                {
                    Id = new Guid(), Body = interpolatedBodyData, EventType = eventModel.Type,
                    UserId = eventModel.UserId
                };
                //Store the User notification.
                _notificationsService.AddNotification(notification);

                if (!(await _notificationsService.SaveChangesAsync()))
                {
                    return StatusCode(500, "A problem happened with handling your request.");
                }

                return Ok();
            }
            catch (Exception e)
            {
               // _logger.LogCritical($"Exception {e}: while adding user rating");
                return StatusCode(500, "A problem happened with handling your request.");
            }
        }

        private  string ReplaceTemplateBody(string templateBody, EventDataModel eventData)
        {
            StringBuilder sb = new StringBuilder(templateBody);

            sb.Replace("{FirstName}", eventData.FirstName);
                sb.Replace("{AppointmentDateTime}", eventData.AppointmentDateTime.ToShortDateString());
                sb.Replace("{OrganisationName}", eventData.OrganisationName);
                sb.Replace("{Reason}", eventData.Reason);
                return sb.ToString();
        }
    }
}