using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        PermissionsDbContext _dbContext;
        public PermissionRepository(PermissionsDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<List<Permission>> GetAllWithPermissionTypeAsync()
        {
            return await _dbContext.Permissions.Include(p => p.PermissionType).ToListAsync();
        }
    }
}
