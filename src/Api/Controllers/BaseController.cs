using Api.Filters;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador base genérico que define peticiones HTTP para operaciones CRUD.
    /// </summary>
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
        protected readonly IUserService _userService;
        protected readonly IMapper _mapper;

        /// <summary>
        /// Constructor principal.
        /// </summary>
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

        /// <summary>
        /// Endpoint que obtiene todas las entidades de una organización específica.
        /// </summary>
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

        /// <summary>
        /// Endpoint que obtiene una entidad por su ID.
        /// </summary>
        /// <remarks>
        /// Es recomendable definir un controlador heredado para sobreescribir este método y poder
        /// así utilizar el filtro de tenencia <see cref="Api.Filters.TenancyRouteFilter"/>.
        /// </remarks>
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

        /// <summary>
        /// Endpoint que crea una nueva entidad.
        /// </summary>
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

        /// <summary>
        /// Endpoint que modifica una entidad existente.
        /// </summary>
        /// <remarks>
        /// Es recomendable definir un controlador heredado para sobreescribir este método y poder
        /// así utilizar el filtro de tenencia <see cref="Api.Filters.TenancyRouteFilter"/>.
        /// </remarks>
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

        /// <summary>
        /// Endpoint que elimina una entidad.
        /// </summary>
        /// <remarks>
        /// Es recomendable definir un controlador heredado para sobreescribir este método y poder
        /// así utilizar el filtro de tenencia <see cref="Api.Filters.TenancyRouteFilter"/>.
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _repository.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    new ProblemDetails
                    {
                        Title = "No es posible eliminar recurso",
                        Detail = "El recurso está en uso y no puede ser eliminado."
                    }
                );
            }
        }
    }
}
