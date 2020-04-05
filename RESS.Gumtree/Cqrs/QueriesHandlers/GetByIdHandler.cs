using System.Threading.Tasks;
using Convey.CQRS.Queries;
using RESS.Gumtree.Cqrs.Queries;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Services;
using RESS.Shared.Exceptions;

namespace RESS.Gumtree.Cqrs.QueriesHandlers
{
    public class GetByIdHandler : IQueryHandler<GetById, GumtreeTopicDto>
    {
        private readonly IGumTreeService _gumTreeService;

        public GetByIdHandler(IGumTreeService gumTreeService)
        {
            _gumTreeService = gumTreeService;
        }
        public async Task<GumtreeTopicDto> HandleAsync(GetById query)
        {
            return await _gumTreeService.GetAsync(query.Id).ThrowIfNotFoundAsync();
        }
    }
}