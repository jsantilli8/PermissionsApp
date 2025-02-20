using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<List<Permission>> GetAllWithPermissionTypeAsync();

    }
}
