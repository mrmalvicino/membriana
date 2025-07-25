using Api.Filters;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController
        <TEntity, TIRepository, TReadDto, TCreateDto, TUpdateDto>
        : ControllerBase
        where TEntity : class, IIdentifiable
        where TIRepository : IBaseRepository<TEntity>
        where TReadDto : class, IIdentifiable
        where TCreateDto : class, ITenantable
        where TUpdateDto : class, IIdentifiable
    {
        private readonly TIRepository _repository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public BaseController(
            TIRepository repository,
            IUserService userService,
            IMapper mapper
        )
        {
            _repository = repository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ServiceFilter(typeof(TenancyQueryFilter))]
        public virtual async Task<ActionResult<IEnumerable<TReadDto>>> GetAll(
            [FromQuery] int organizationId
        )
        {
            var entities = await _repository.GetAllAsync(organizationId);
            var readDtos = _mapper.Map<IEnumerable<TReadDto>>(entities);
            return Ok(readDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<ActionResult<TReadDto>> Get(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var readDto = _mapper.Map<TReadDto>(entity);

            return Ok(readDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public virtual async Task<ActionResult<TReadDto>> Create(
            [FromBody] TCreateDto createDto
        )
        {
            int userOrgId = await _userService.GetOrganizationIdAsync();

            if (createDto.OrganizationId != userOrgId)
            {
                return Forbid();
            }

            var entity = _mapper.Map<TEntity>(createDto);
            var created = await _repository.AddAsync(entity);
            var readDto = _mapper.Map<TReadDto>(created);

            return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<ActionResult<TReadDto>> Update(
            int id,
            [FromBody] TUpdateDto updateDto
        )
        {
            if (id != updateDto.Id)
            {
                return BadRequest();
            }

            var entity = _mapper.Map<TEntity>(updateDto);
            var updated = await _repository.UpdateAsync(entity);
            var readDto = _mapper.Map<TReadDto>(updated);

            return Ok(readDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
