using System.Collections.Generic;
using System.Threading.Tasks;
using RESS.Gumtree.DTO;

namespace RESS.Gumtree.Services
{
    public interface IGumTreeWorkerService
    {
        Task CreateAsync(GumtreeTopicDto dto);
        Task CreateAsync(IEnumerable<GumtreeTopicDto> dto);
        Task<int> CountOfAllTopicAsync();
    }
}