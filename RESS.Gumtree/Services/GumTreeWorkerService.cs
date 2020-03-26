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

        public GumTreeWorkerService(IMongoRepository<GumtreeTopicDocument, Guid> repository, ILogger<GumTreeWorkerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task CreateAsync(GumtreeTopicDto dto)
        {
            if (await _repository.ExistsAsync(c => c.Url == dto.Url && c.Price != dto.Price))
            {
                await _repository.AddAsync(dto.AsRelatedDocument());
                _logger.LogWarning($"Ogłoszenie istnieje w bazie {dto.Id}, ale wykryto różnice cen.");
                return;
            }

            var result = await _repository.GetAsync(c => c.Url == dto.Url);
            if (result is {})
            {
                var document = dto.AsUpdateDocument();
                await _repository.UpdateAsync(document);
                _logger.LogWarning($"Zaktualizowano dokument {dto.Id}.");
                return;
            }

            await _repository.AddAsync(dto.AsDocument());
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
            return Task.Run(()=>_repository.Collection.AsQueryable().Count());
        }
    }
}