﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Services;
using RESS.Gumtree.Validators;
using RESS.Shared.Exceptions;

namespace RESS.Gumtree.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GumTreeController : ControllerBase
    {
        private readonly IGumTreeService _service;
        private readonly IGumTreeDtoValidator _validator;

        public GumTreeController(IGumTreeService service, IGumTreeDtoValidator validator)
        {
            _service = service;
            _validator = validator;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GumtreeTopicDto>> Get(Guid id)
        {
            var cinema = await _service.GetAsync(id).ThrowIfNotFoundAsync();
            return Ok(cinema);
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