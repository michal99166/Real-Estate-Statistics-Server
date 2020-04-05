using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using RESS.Modules.GumTree.Application.DTO;
using RESS.Modules.GumTree.Application.Services;

namespace RESS.Modules.GumTree.Infrastructure.Services
{
    public class GumTreeService : IGumTreeService
    {
        private readonly IMongoRepository<GumtreeTopicDocument, Guid> _repository;
        private readonly ILogger<GumTreeService> _logger;
        private readonly IMongoDatabase _database;

        public GumTreeService(IMongoRepository<GumtreeTopicDocument, Guid> repository, ILogger<GumTreeService> logger, IMongoDatabase database)
        {
            _repository = repository;
            _logger = logger;
            _database = database;
        }

        public async Task CreateAsync(GumtreeTopicDto dto)
        {
            var collection = _database.GetCollection<GumtreeTopicDocument>("GumTreeTopics")
                .AsQueryable()
                .OrderBy(x => x.TimeStamp)
                .Where(x => x.Url == dto.Url)
                .ToList();

            if (!collection.Any())
            {
                await _repository.AddAsync(dto.AsDocument());
                return;
            }

            var firstDocument = collection.ElementAt(0);
            var lastDocument = collection.TakeLast(1).FirstOrDefault(c => c.Url == dto.Url && c.Price != dto.Price);

            if (lastDocument is { })
            {
                await _repository.UpdateAsync(firstDocument.AsUpdateParentDocument());
                await _repository.AddAsync(dto.AsRelatedDocument(firstDocument.Id));
                _logger.LogWarning($"Ogłoszenie istnieje w bazie {firstDocument.Id}, ale wykryto różnice cen.");
                return;
            }

            if (firstDocument is { })
            {
                await _repository.UpdateAsync(firstDocument.AsUpdateDocument());
            }
        }

        public async Task<int> CountOfAllTopicAsync()
        {
            return await Task.Run(() => _repository.Collection.AsQueryable().Count());
        }

        public async Task<PagedResult<GumtreeTopicDto>> GetAsync(TestingQuery query)
        {
            var document = await _repository.BrowseAsync(x => x.City == query.City, query);
            return document?.Map(d=>d.AsDto());
        }
         
        public async Task<GumtreeTopicDto> GetAsync(Guid id)
        {
            var document = await _repository.GetAsync(id);
            return document?.AsDto();
        }

        public Task UpdateAsync(GumtreeTopicDto dto)
            => _repository.UpdateAsync(dto.AsDocument());

        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }
}
