using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Microsoft.Extensions.Logging;

namespace Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationsService notificationsService, ILogger<NotificationsController> logger)
        {
            this._notificationsService = notificationsService;
            this._logger = logger;
        }

        //public NotificationsController(INotificationsService notificationsService)
        //{
        //    this._notificationsService = notificationsService;
            
        //}

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var notifications = await _notificationsService.GetAllNotifications();
                if (notifications == null || notifications.Count == 0)
                {
                    _logger.LogWarning($"No Notifications found ");
                    return NotFound();
                }
                return Ok(notifications);

            }
            catch (Exception e)
            {
                _logger.LogCritical($"Exception {e}: while getting Top 5 movies by user rating ");
                return StatusCode(500, "A problem happened with handling your request.");
            }

        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {

            try
            {
                var notifications = await _notificationsService.GetNotificationsByUser(userId);
                if (notifications == null  || notifications.Count == 0)
                {
                    _logger.LogWarning($"No Notifications found ");
                    return NotFound();
                }
                return Ok(notifications);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Exception {e}: while getting notifications by user ");
                return StatusCode(500, "A problem happened with handling your request.");
            }
        }

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
                    _logger.LogError($"did not meet one of the validation rules");
                    return BadRequest();
                }

                //Read the template from  store based on EventType
                var cancelledTemplate = _notificationsService.GetTemplate(eventModel.Type);

                //Create new user Notification by combining the template with the data received in the event
                var interpolatedBodyData = ReplaceTemplateBody(cancelledTemplate.Body, eventModel.Data);
                
                NotificationModel notification = new NotificationModel()
                {
                    Id = Guid.NewGuid(), Body = interpolatedBodyData, EventType = eventModel.Type,
                    UserId = eventModel.UserId
                };
                //Store the User notification.
                _notificationsService.AddNotification(notification);

                if (!(await _notificationsService.SaveChangesAsync()))
                {
                    return StatusCode(500, "A problem happened with handling your request.");
                }

                //send web socket
                //var context = ControllerContext.HttpContext;
                ////var isSocketRequest = context.WebSockets.IsWebSocketRequest;
                //var ct = context.RequestAborted;
                //WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                //await SendStringAsync(webSocket, eventModel.ToString(), ct);
               
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Exception {e}: while adding event");
                return StatusCode(500, "A problem happened with handling your request.");
            }
        }

        private  string ReplaceTemplateBody(string templateBody, EventDataModel eventData)
        {
            StringBuilder sb = new StringBuilder(templateBody);

            sb.Replace("{Firstname}", eventData.FirstName);
                sb.Replace("{AppointmentDateTime}", eventData.AppointmentDateTime.ToShortDateString());
                sb.Replace("{OrganisationName}", eventData.OrganisationName);
                sb.Replace("{Reason}", eventData.Reason);
                return sb.ToString();
        }
        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
    }
}