using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Features.Commands;
using Application.Features.Queries;
using System.Collections.Generic;
using Application.Dtos;

namespace Tests.Controllers
{
    [TestClass]
    public class PermissionsControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private PermissionsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PermissionsController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task AddPermission_ReturnsOkResult_WithValidRequest()
        {
            // Arrange
            var request = new AddPermissionRequest
            {
                EmployeeForeName = "John",
                EmployeeSurName = "Doe",
                PermissionTypeId = 1
            };

            var response = new AddPermissionResponse { Id = 123 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<AddPermissionCommand>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.RequestPermission(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(response, okResult.Value);
        }

        [TestMethod]
        public async Task ModifyPermission_ReturnsOkResult_WhenPermissionExists()
        {
            // Arrange
            var request = new ModifyPermissionRequest
            {
                Id = 123,
                EmployeForeName= "John",
                EmployeeSurName = "Doe",
                PermissionTypeId = 2
            };

            var response = new ModifyPermissionResponse { Id = 123 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ModifyPermissionCommand>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.ModifyPermission(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(response, okResult.Value);
        }

        [TestMethod]
        public async Task GetPermissions_ReturnsListOfPermissions()
        {
            // Arrange
            var response = new List<GetPermissionsResponse>
            {
                new GetPermissionsResponse { Id = 1, EmployeeForeName = "Alice", EmployeeSurName = "Smith", PermissionTypeId = 1 },
                new GetPermissionsResponse { Id = 2, EmployeeForeName = "Bob", EmployeeSurName = "Johnson", PermissionTypeId = 2 }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPermissionsQuery>(), default))
                         .ReturnsAsync(response);

            // Act
            var result = await _controller.GetPermissions();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            CollectionAssert.AreEqual(response, (List<GetPermissionsResponse>)okResult.Value);
        }
    }
}
