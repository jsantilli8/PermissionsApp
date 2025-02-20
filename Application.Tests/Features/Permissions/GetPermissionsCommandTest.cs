using Application.Features.Queries;
using Application.Interfaces;
using Domain.Entities;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Application.Features.Queries.GetPermissionsQuery;

namespace Application.Tests.Features.Permissions
{
    [TestClass]
    public class GetPermissionsCommandTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IKafkaProducer> _kafkaProducerMock;
        private Mock<ILoggerService> _loggerMock;
        private GetPermissionsHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();
            _loggerMock = new Mock<ILoggerService>();

            _handler = new GetPermissionsHandler(
                _unitOfWorkMock.Object,
                _kafkaProducerMock.Object,
                _loggerMock.Object
            );
        }

        [TestMethod]
        public async Task Handle_ShouldReturnListOfPermissions()
        {
            // Arrange
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, EmployeeForeName = "Benjamin", EmployeeSurName = "Ibarra", PermissionTypeId = 1, PermissionDate = DateTime.UtcNow },
                new Permission { Id = 2, EmployeeForeName = "Mariano", EmployeeSurName = "Moreno", PermissionTypeId = 2, PermissionDate = DateTime.UtcNow }
            };

            _unitOfWorkMock
                .Setup(x => x.Permissions.GetAllWithPermissionTypeAsync())
                .ReturnsAsync(permissions);

            // Act
            var response = await _handler.Handle(new GetPermissionsQuery(), CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
        }
    }
}
