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

        //var mockService = new Mock<IValidateBarcodeService>();
        //var fakeModel = new ValidateBarcode { SiteId = 1, Barcode = "TEST" };

        //mockService.Setup(t => t.ElasticSearchForBarcode(fakeModel)).Returns(Task.FromResult(new List<BarcodeDocument>()));

        //var controller = new BarcodesController(mockService.Object);

        //var result = await controller.ValidateBarcode(fakeModel);

        //Assert.IsType<BadRequestObjectResult>(result);


        //[Fact]
        //public void Get_WhenCalled_ReturnsOkResult()
        //{
        //    // Act
        //    var okResult = _controller.Get();

        //    // Assert
        //    Assert.IsType<OkObjectResult>(okResult.Result);
        //}

        //[Fact]
        //public void Get_WhenCalled_ReturnsAllItems()
        //{
        //    // Act
        //    var okResult = _controller.Get().Result as OkObjectResult;

        //    // Assert
        //    var items = Assert.IsType<List<ShoppingItem>>(okResult.Value);
        //    Assert.Equal(3, items.Count);
        //}

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
