using Api.Filters;
using Contracts.Dtos.Employee;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "Admin")]
    public class EmployeesController : BaseController<
        Employee,
        IEmployeeRepository,
        EmployeeReadDto,
        EmployeeCreateDto,
        EmployeeUpdateDto
    >
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(
            IEmployeeRepository repository,
            IUserService userService,
            IMapper mapper,
            IUnitOfWork unitOfWork
        ) : base(repository, userService, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        [ServiceFilter(typeof(TenancyRouteFilter<Employee, IEmployeeRepository>))]
        public override async Task<ActionResult<EmployeeReadDto>> Get(int id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public override async Task<ActionResult<EmployeeReadDto>> Create(
            [FromBody] EmployeeCreateDto createDto
        )
        {
            int userOrgId = await _userService.GetOrganizationIdAsync();

            if (createDto.OrganizationId != userOrgId)
            {
                return Forbid();
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var user = new AppUser
                {
                    UserName = createDto.Email,
                    Email = createDto.Email,
                    NormalizedEmail = createDto.Email,
                    EmailConfirmed = true,
                    OrganizationId = createDto.OrganizationId
                };

                var result = await _unitOfWork.IdentityService.CreateUser(user, "Password123-");

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    await _unitOfWork.RollbackAsync();
                    return BadRequest(new { errors });
                }

                await _unitOfWork.IdentityService.AddToRole(user, Domain.Enums.AppRole.Employee);

                var employee = _mapper.Map<Employee>(createDto);
                employee.UserId = user.Id;
                var created = await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var readDto = _mapper.Map<EmployeeReadDto>(created);

                await _unitOfWork.CommitAsync();

                return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        [ServiceFilter(typeof(TenancyRouteFilter<Employee, IEmployeeRepository>))]
        public override async Task<ActionResult<EmployeeReadDto>> Update(
            int id,
            [FromBody] EmployeeUpdateDto updateDto
        )
        {
            return await base.Update(id, updateDto);
        }

        [ServiceFilter(typeof(TenancyRouteFilter<Employee, IEmployeeRepository>))]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
