using Application.Dtos;
using Application.Features.Commands;
using Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("request")]
        public async Task<ActionResult<AddPermissionResponse>> RequestPermission([FromBody] AddPermissionRequest request)
        {
            var command = new AddPermissionCommand
            {
                EmployeeForeName = request.EmployeeForeName,
                EmployeeSurName = request.EmployeeSurName,
                PermissionTypeId = request.PermissionTypeId
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("modify")]
        public async Task<ActionResult<ModifyPermissionResponse>> ModifyPermission([FromBody] ModifyPermissionRequest request)
        {

            var command = new ModifyPermissionCommand
            {
                Id = request.Id,
                EmployeeForeName = request.EmployeForeName,
                EmployeeSurName = request.EmployeeSurName,
                PermissionTypeId = request.PermissionTypeId,
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("get")]
        public async Task<ActionResult<List<GetPermissionsResponse>>> GetPermissions()
        {
            var response = await _mediator.Send(new GetPermissionsQuery());
            return Ok(response);
        }
    }
}
