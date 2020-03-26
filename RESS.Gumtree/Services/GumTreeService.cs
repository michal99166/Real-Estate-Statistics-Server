using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.Logging;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Exceptions;
using RESS.Gumtree.Mongo;
using RESS.Gumtree.Mongo.Documents;

namespace RESS.Gumtree.Services
{
    public class GumTreeService : IGumTreeService
    {
        private readonly IMongoRepository<GumtreeTopicDocument, Guid> _repository;
        private readonly ILogger<GumTreeService> _logger;

        public GumTreeService(IMongoRepository<GumtreeTopicDocument, Guid> repository, ILogger<GumTreeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GumtreeTopicDto> GetAsync(Guid id)
        {
            var document = await _repository.GetAsync(id);
            return document?.AsDto();
        }

        public async Task CreateAsync(GumtreeTopicDto dto)
        {
            var alreadyExists = await _repository.ExistsAsync(c => c.Id == dto.Id);

            if (alreadyExists)
            {
                throw new GumTreeTopicAlreadyExistsException(dto.Id);
            }

            await _repository.AddAsync(dto.AsDocument());
        }

        public Task UpdateAsync(GumtreeTopicDto dto)
            => _repository.UpdateAsync(dto.AsDocument());


        public Task DeleteAsync(Guid id)
            => _repository.DeleteAsync(id);
    }
}