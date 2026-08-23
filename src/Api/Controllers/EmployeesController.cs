using Api.Filters;
using Api.Helpers;
using Contracts.Dtos.Employee;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Authorize(Policy = "Admin")]
public class EmployeesController : BaseController<
    Employee,
    IEmployeeRepository,
    EmployeeReadDto,
    EmployeeCreateDto,
    EmployeeUpdateDto
>
{
	private readonly IEmployeeRepository _repository;
	private readonly IImageService _imageService;

	/// <summary>
    /// Constructor principal.
    /// </summary>
	public EmployeesController(
		IEmployeeRepository repository,
		IImageService imageService,
		IUserService userService,
		IMapper mapper
	) : base(repository, userService, mapper)
	{
		_repository = repository;
		_imageService = imageService;
    }

    [ServiceFilter(typeof(TenancyRouteFilter<Employee, IEmployeeRepository>))]
    public override async Task<ActionResult<EmployeeReadDto>> Get(int id)
    {
        return await base.Get(id);
    }

    [ServiceFilter(typeof(TenancyRouteFilter<Employee, IEmployeeRepository>))]
	public override async Task<ActionResult<EmployeeReadDto>> Update(
		int id,
		[FromBody] EmployeeUpdateDto updateDto
	)
	{
		if (id != updateDto.Id)
		{
			return BadRequest(
				ErrorResponseFactory.Create("El ID de la ruta no coincide con el ID del recurso.")
			);
		}

		var entity = await _repository.GetByIdAsync(id);

		if (entity == null)
		{
			return NotFound(ErrorResponseFactory.Create("El recurso no existe."));
		}

		_mapper.Map(updateDto, entity);
		_imageService.ApplyProfileImage(entity, updateDto.ProfileImageUrl);

		try
		{
			var updated = await _repository.UpdateAsync(entity);
			var readDto = _mapper.Map<EmployeeReadDto>(updated);
			return Ok(readDto);
		}
		catch (DbUpdateException ex) when (
			DbUpdateExceptionHelper.TryCreateConflictMessage(ex, out var message)
		)
		{
			return Conflict(ErrorResponseFactory.Create(message));
		}
	}

    [ServiceFilter(typeof(TenancyRouteFilter<Employee, IEmployeeRepository>))]
	public override async Task<IActionResult> Delete(int id)
	{
		var entity = await _repository.GetByIdAsync(id);

		if (entity == null)
		{
			return NotFound(ErrorResponseFactory.Create("El recurso no existe."));
		}

		_imageService.RemoveProfileImage(entity);
		return await base.Delete(id);
	}
}
