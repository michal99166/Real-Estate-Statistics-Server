using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Exceptions;
using RESS.Gumtree.Mongo;
using RESS.Gumtree.Mongo.Documents;

namespace RESS.Gumtree.Services
{
    public class GumTreeService : IGumTreeService
    {
        private readonly IMongoRepository<GumtreeTopicDocument, Guid> _repository;

        public GumTreeService(IMongoRepository<GumtreeTopicDocument, Guid> repository)
        {
            _repository = repository;
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