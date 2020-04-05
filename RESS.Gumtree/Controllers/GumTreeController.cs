using Microsoft.AspNetCore.Mvc;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Services;
using RESS.Gumtree.Validators;
using RESS.Shared.Exceptions;
using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using RESS.Gumtree.Cqrs.Queries;

namespace RESS.Gumtree.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GumTreeController : ControllerBase
    {
        private readonly IGumTreeService _service;
        private readonly IGumTreeDtoValidator _validator;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public GumTreeController(IGumTreeService service, IGumTreeDtoValidator validator, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            _service = service;
            _validator = validator;
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<ActionResult<GumtreeTopicDto>> GetById(GetById query)
        {
            var result = await _queryDispatcher.QueryAsync(query);
            return Ok(result);
        }

        [HttpGet("city" , Name = "GetByCity")]
        public async Task<ActionResult<PagedResult<GumtreeTopicDto>>> GetByCity([FromBody] GetByCity query)
        {
            var result = await _queryDispatcher.QueryAsync(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post(GumtreeTopicDto dto)
        {
            _validator.Validate(dto).ThrowIfInvalid();

            await _service.CreateAsync(dto);
            return Created(dto.Id.ToString(), null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, GumtreeTopicDto dto)
        {
            dto.Id = id;
            _validator.Validate(dto).ThrowIfInvalid();

            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}