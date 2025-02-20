using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PermissionService
    {
        private readonly IElasticsearchRepository _elasticsearchRepository;

        public PermissionService(IElasticsearchRepository elasticsearchRepository)
        {
            _elasticsearchRepository = elasticsearchRepository;
        }

        public async Task CreatePermissionAsync(Permission permission)
        {
            await _elasticsearchRepository.IndexPermissionAsync(permission);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsAsync(string query)
        {
            return await _elasticsearchRepository.SearchPermissionsAsync(query);
        }
    }
}
