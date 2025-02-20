using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands
{
    public class AddPermissionCommand : IRequest<AddPermissionResponse>
    {
        public string EmployeeForeName { get; set; } = string.Empty;
        public string EmployeeSurName { get; set; } = string.Empty;
        public int PermissionTypeId { get; set; }


        public class AddPermissionHandler : IRequestHandler<AddPermissionCommand, AddPermissionResponse>
        {
            private readonly IKafkaProducer _kafkaProducer;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILoggerService _logger;

            public AddPermissionHandler(IUnitOfWork unitOfWork,IKafkaProducer kafkaProducer, ILoggerService logger)
            {
                _unitOfWork = unitOfWork;
                _kafkaProducer = kafkaProducer;
                _logger = logger;
            }

            public async Task<AddPermissionResponse> Handle(AddPermissionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var permission = new Permission
                    {
                        EmployeeForeName = request.EmployeeForeName,
                        EmployeeSurName = request.EmployeeSurName,
                        PermissionTypeId = request.PermissionTypeId,
                        PermissionDate = DateTime.UtcNow
                    };

                    await _unitOfWork.Permissions.AddAsync(permission);
                    _logger.LogInformation("Permission created successfully with ID {PermissionId}", permission.Id);
                    await _kafkaProducer.SendOperationAsync("Request");

                    return new AddPermissionResponse { Id = permission.Id };
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error creating permission: {ErrorMessage}", ex.Message);
                    throw;
                }
            }
        }
    }
}
