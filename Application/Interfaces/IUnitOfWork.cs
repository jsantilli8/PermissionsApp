using Domain.Interfaces;

namespace Application.Interfaces;

public interface IUnitOfWork
{
    IPermissionRepository Permissions { get; }
    Task<int> CompleteAsync();
}
