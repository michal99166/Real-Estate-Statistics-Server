using Convey.CQRS.Queries;
using RESS.Modules.GumTree.Application.DTO;
using System;
using System.Threading.Tasks;

namespace RESS.Modules.GumTree.Application.Services
{
    public interface IGumTreeService
    {
        Task CreateAsync(GumtreeTopicDto dto);
        Task<GumtreeTopicDto> GetAsync(Guid id);
        Task UpdateAsync(GumtreeTopicDto dto);
        Task DeleteAsync(Guid id);
        Task<int> CountOfAllTopicAsync();
        Task<PagedResult<GumtreeTopicDto>> GetAsync(TestingQuery query);

    }
}
