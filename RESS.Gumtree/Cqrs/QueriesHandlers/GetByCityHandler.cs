using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RESS.Gumtree.Cqrs.Queries;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Mongo;
using RESS.Gumtree.Mongo.Documents;
using RESS.Gumtree.Services;

namespace RESS.Gumtree.Cqrs.QueriesHandlers
{
    public class GetByCityHandler : IQueryHandler<GetByCity, PagedResult<GumtreeTopicDto>>
    {
        private readonly IMongoRepository<GumtreeTopicDocument, Guid> _repository;

        public GetByCityHandler(IMongoRepository<GumtreeTopicDocument, Guid> repository) => _repository = repository;

        public async Task<PagedResult<GumtreeTopicDto>> HandleAsync(GetByCity query)
        {
            var document = await _repository.BrowseAsync(x => x.City == query.City, query);
            return document?.Map(d => d.AsDto());
        }
    }
}