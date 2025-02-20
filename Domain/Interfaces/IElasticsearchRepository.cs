using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IElasticsearchRepository
    {
        Task IndexPermissionAsync(Permission permission);
        Task<IEnumerable<Permission>> SearchPermissionsAsync(string query);
    }
}
