using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using RESS.Gumtree.Cqrs.Queries;
using RESS.Gumtree.DTO;

namespace RESS.Gumtree.Services
{
    public interface IGumTreeService
    {
        Task CreateAsync(GumtreeTopicDto dto);
        Task<GumtreeTopicDto> GetAsync(Guid id);
        Task UpdateAsync(GumtreeTopicDto dto);
        Task DeleteAsync(Guid id);
        Task<int> CountOfAllTopicAsync();
    }
}