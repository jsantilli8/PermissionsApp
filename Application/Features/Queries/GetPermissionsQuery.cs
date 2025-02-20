using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Queries
{
    public class GetPermissionsQuery : IRequest<List<GetPermissionsResponse>>
    {
        public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, List<GetPermissionsResponse>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IKafkaProducer _kafkaProducer;
            private readonly ILoggerService _logger;

            public GetPermissionsHandler(IUnitOfWork unitOfWork, IKafkaProducer kafkaProducer, ILoggerService logger)
            {
                _unitOfWork = unitOfWork;
                _kafkaProducer = kafkaProducer;
                _logger = logger;   
            }

            public async Task<List<GetPermissionsResponse>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var permissions = await _unitOfWork.Permissions.GetAllWithPermissionTypeAsync();
                    await _kafkaProducer.SendOperationAsync("Get");


                    return permissions.Select(p => new GetPermissionsResponse
                    {
                        Id = p.Id,
                        EmployeeForeName = p.EmployeeForeName,
                        EmployeeSurName = p.EmployeeSurName,
                        PermissionTypeId = p.PermissionTypeId,
                        PermissionTypeName = p.PermissionType?.Description ?? "Permission Type",
                        Date = p.PermissionDate
                    }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error fetching permissions: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
