using System;
using System.Threading.Tasks;
using RESS.Gumtree.DTO;

namespace RESS.Gumtree.Services
{
    public interface IGumTreeService
    {
        Task<GumtreeTopicDto> GetAsync(Guid id);
        Task CreateAsync(GumtreeTopicDto dto);
        Task UpdateAsync(GumtreeTopicDto dto);
        Task DeleteAsync(Guid id);
    }
}