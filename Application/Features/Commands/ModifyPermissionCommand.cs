using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands
{
    public class ModifyPermissionCommand : IRequest<ModifyPermissionResponse>
    {
        public int Id { get; set; }
        public string EmployeeForeName { get; set; } = string.Empty;
        public string EmployeeSurName { get; set; } = string.Empty;
        public int PermissionTypeId { get; set; }

        public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, ModifyPermissionResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IKafkaProducer _kafkaProducer;
            private readonly ILoggerService _logger;

            public ModifyPermissionHandler(IUnitOfWork unitOfWork, IKafkaProducer kafkaProducer, ILoggerService logger)
            {
                _unitOfWork = unitOfWork;
                _kafkaProducer = kafkaProducer;
                _logger = logger;
            }

            public async Task<ModifyPermissionResponse> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
                    if (permission == null)
                    {
                        _logger.LogWarning("Permission not found with ID {PermissionId}", request.Id);
                        return new ModifyPermissionResponse { Id = request.Id, Message = "Permission not found" };
                    }

                    permission.PermissionTypeId = request.PermissionTypeId;
                    permission.EmployeeForeName = request.EmployeeForeName;
                    permission.EmployeeSurName = request.EmployeeSurName;
                    permission.PermissionDate = DateTime.UtcNow;

                    await _unitOfWork.Permissions.UpdateAsync(permission);
                    _logger.LogInformation($"Permission modified successfully with ID {permission.Id}");
                    await _kafkaProducer.SendOperationAsync("Modify");

                    return new ModifyPermissionResponse { Id = permission.Id };
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error modifying permission: {ErrorMessage}", ex.Message);
                    throw;
                }
            }
        }
    }
}
