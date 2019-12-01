using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Controllers;
using Microsoft.Extensions.Logging;

namespace Notifications.Tests
{

    public class NotificationControllerTests
    {
        //[Fact]
        //public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IBrainstormSessionRepository>();
        //    mockRepo.Setup(repo => repo.ListAsync())
        //        .ReturnsAsync(GetTestSessions());
        //    var controller = new HomeController(mockRepo.Object);

        //    // Act
        //    var result = await controller.Index();

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<StormSessionViewModel>>(
        //        viewResult.ViewData.Model);
        //    Assert.Equal(2, model.Count());
        //}

        [Fact]
        public async Task Get_ReturnAllNotifications()
        {
            //Arrange
            var mockService = new Mock<INotificationsService>();
            var mockLogger = new Mock<ILogger<NotificationsController>>();

            mockService.Setup(s => s.GetAllNotifications())
                .ReturnsAsync(GetTestNotifications());
            var controller = new NotificationsController(mockService.Object, mockLogger.Object);
           
            // Act
            var resullt = await controller.Request

            //Assert
        }


        private IReadOnlyCollection<NotificationModel> GetTestNotifications()
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

        //private List<BrainstormSession> GetTestSessions()
        //{
        //    var sessions = new List<BrainstormSession>();
        //    sessions.Add(new BrainstormSession()
        //    {
        //        DateCreated = new DateTime(2016, 7, 2),
        //        Id = 1,
        //        Name = "Test One"
        //    });
        //    sessions.Add(new BrainstormSession()
        //    {
        //        DateCreated = new DateTime(2016, 7, 1),
        //        Id = 2,
        //        Name = "Test Two"
        //    });
        //    return sessions;
        //}
    }
}
