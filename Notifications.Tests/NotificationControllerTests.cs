using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Controllers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Notifications.Tests
{

    public class NotificationControllerTests
    {
       

        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResult()
        {
            //Arrange
            var mockService = new Mock<INotificationsService>();
            var mockLogger = new Mock<ILogger<NotificationsController>>();

            mockService.Setup(s => s.GetAllNotifications())
                .ReturnsAsync(GetTestNotifications());
            var controller = new NotificationsController(mockService.Object, mockLogger.Object);
           
            // Act
            //var result = await controller.Get();
            var okResult = await controller.Get();

            //Assert
            Assert.IsType<OkObjectResult>(okResult);
           
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsAllNotifications()
        {
            //Arrange
            var mockService = new Mock<INotificationsService>();
            var mockLogger = new Mock<ILogger<NotificationsController>>();

            mockService.Setup(s => s.GetAllNotifications())
                .ReturnsAsync(GetTestNotifications());
            var controller = new NotificationsController(mockService.Object, mockLogger.Object);

            // Act
            var okResult = await controller.Get() as OkObjectResult;

            // Assert
            if (okResult != null)
            {
                var items = Assert.IsAssignableFrom<IReadOnlyCollection<NotificationModel>>(okResult.Value);
                Assert.Equal(2, items.Count);
            }
        }
       

        [Fact]
        public void GetByUserId_UnknownIntPassed_ReturnsNotFoundResult()
        {

            //Arrange
            var mockService = new Mock<INotificationsService>();
            var mockLogger = new Mock<ILogger<NotificationsController>>();
            var userId = 0;
            mockService.Setup(s => s.GetNotificationsByUser(userId))
                .ReturnsAsync(GetUserTestNotifications(userId));
            var controller = new NotificationsController(mockService.Object, mockLogger.Object);
            // Act
            var notFoundResult = controller.Get(userId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public async Task GetByUser_WhenCalled_ReturnsOkResult()
        {
            //Arrange
            var mockService = new Mock<INotificationsService>();
            var mockLogger = new Mock<ILogger<NotificationsController>>();
            var userId = 2;
            mockService.Setup(s => s.GetNotificationsByUser(userId))
                .ReturnsAsync(GetUserTestNotifications(userId));
            var controller = new NotificationsController(mockService.Object, mockLogger.Object);

            // Act
            //var result = await controller.Get();
            var okResult = await controller.Get(userId);

            //Assert
            Assert.IsType<OkObjectResult>(okResult);

        }

        [Fact]
        public async Task GetByUser_WhenCalled_ReturnsUserNotifications()
        {
            //Arrange
            var mockService = new Mock<INotificationsService>();
            var mockLogger = new Mock<ILogger<NotificationsController>>();
            var userId = 2;
            mockService.Setup(s => s.GetNotificationsByUser(userId))
                .ReturnsAsync(GetUserTestNotifications(userId));
            var controller = new NotificationsController(mockService.Object, mockLogger.Object);

            // Act
            var okResult = await controller.Get(userId) as OkObjectResult;

            // Assert
            if (okResult != null)
            {
                var items = Assert.IsAssignableFrom<IReadOnlyCollection<NotificationModel>>(okResult.Value);
                Assert.Equal(2, items.Count);
            }
        }

       

        [Fact]
        public async Task AddEvent_InvalidObjectPassed_ReturnsBadRequest()
        {
            
            //Arrange
            var mockService = new Mock<INotificationsService>();
            var mockLogger = new Mock<ILogger<NotificationsController>>();
            var controller = new NotificationsController(mockService.Object, mockLogger.Object);


            //public string Type { get; set; }
            //public EventDataModel Data { get; set; }
            //public int UserId { get; set; }

            EventModel eventModel = null;

            // Act
            var badResponse = await controller.AddEvent(eventModel);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse);
        }

        private static IReadOnlyCollection<NotificationModel> GetTestNotifications()
        {
            var sessions = new List<NotificationModel>
            {
                new NotificationModel()
                {
                    Id = Guid.NewGuid(), Body = "test", EventType = "AppointmentCancelled", UserId = 1
                },
                new NotificationModel()
                {
                    Id = Guid.NewGuid(), Body = "test2", EventType = "AppointmentCancelled", UserId = 1
                }
            };

            return sessions.AsReadOnly();
        }

        private static IReadOnlyCollection<NotificationModel> GetUserTestNotifications(int userId)
        {
            var sessions = new List<NotificationModel>
            {
                new NotificationModel()
                {
                    Id = Guid.NewGuid(), Body = "test", EventType = "AppointmentCancelled", UserId = 1
                },
                new NotificationModel()
                {
                    Id = Guid.NewGuid(), Body = "test2", EventType = "AppointmentCancelled", UserId = 2
                },
                new NotificationModel()
                {
                    Id = Guid.NewGuid(), Body = "test3", EventType = "AppointmentCancelled", UserId = 2
                }
            };

            return sessions.Where(x => x.UserId == userId).ToList().AsReadOnly();
        }
    }
}
