using Application.Features.Commands;
using Application.Interfaces;
using Domain.Entities;
using Moq;
using static Application.Features.Commands.AddPermissionCommand;

namespace Application.Tests.Features.Permissions
{
    [TestClass]
    public class AddPermissionCommandTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IKafkaProducer> _kafkaProducerMock;
        private Mock<ILoggerService> _loggerMock;
        private AddPermissionHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();
            _loggerMock = new Mock<ILoggerService>();

            _handler = new AddPermissionHandler(_unitOfWorkMock.Object, _kafkaProducerMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldAddPermissionAndReturnResponse()
        {
            // Arrange
            var command = new AddPermissionCommand
            {
                EmployeeForeName = "Benjamin",
                EmployeeSurName = "Ibarra",
                PermissionTypeId = 1
            };

            var permission = new Permission
            {
                Id = 1,
                EmployeeForeName = "Benjamin",
                EmployeeSurName = "Ibarra",
                PermissionTypeId = 1,
                PermissionDate = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(x => x.Permissions.AddAsync(It.IsAny<Permission>()))
                .Callback<Permission>(p => p.Id = 1)
                .ReturnsAsync(permission);

            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Id);
        }
    }
}