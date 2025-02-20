using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly PermissionsDbContext _context;
    public IPermissionRepository Permissions { get; }

    public UnitOfWork(PermissionsDbContext context, IPermissionRepository permissions)
    {
        _context = context;
        Permissions = permissions;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
