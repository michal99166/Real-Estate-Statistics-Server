using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Exceptions;
using RESS.Gumtree.Mongo;
using RESS.Gumtree.Mongo.Documents;

namespace RESS.Gumtree.Services
{
    public class GumTreeWorkerService : IGumTreeWorkerService
    {
        private readonly IMongoRepository<GumtreeTopicDocument, Guid> _repository;
        private readonly ILogger<GumTreeWorkerService> _logger;
        private readonly IMongoDatabase _database;

        public GumTreeWorkerService(IMongoRepository<GumtreeTopicDocument, Guid> repository, ILogger<GumTreeWorkerService> logger, IMongoDatabase database)
        {
            _repository = repository;
            _logger = logger;
            _database = database;
        }

        public async Task CreateAsync(GumtreeTopicDto dto)
        {
            var collection = _database.GetCollection<GumtreeTopicDocument>("GumTreeTopics").AsQueryable().OrderBy(x => x.TimeStamp).Where(x => x.Url == dto.Url).ToList();
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

        public async Task CreateAsync(IEnumerable<GumtreeTopicDto> dto)
        {
            foreach (var d in dto)
            {
                if (await _repository.ExistsAsync(c => c.Url == d.Url && c.Price != d.Price))
                {
                    _logger.LogWarning($"Ogłoszenie istnieje w bazie {d.Url}, ale wykryto różnice cen.");
                    await _repository.AddAsync(d.AsDocument());
                    return;
                }
                if (await _repository.ExistsAsync(c => c.Url == d.Url))
                {
                    return;
                }

                await _repository.AddAsync(d.AsDocument());
            }
        }

        public Task<int> CountOfAllTopicAsync()
        {
            return Task.Run(() => _repository.Collection.AsQueryable().Count());
        }
    }
}