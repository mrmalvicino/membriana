using Api.Filters;
using Application.Dtos.Employee;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class EmployeesController : BaseController<Employee, IEmployeeRepository, EmployeeReadDto, EmployeeCreateDto, EmployeeUpdateDto>
    {
        public EmployeesController(
            IEmployeeRepository repository,
            IUserService userService,
            IMapper mapper
        ) : base(repository, userService, mapper)
        {

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
            return await base.Update(id, updateDto);
        }

        [ServiceFilter(typeof(TenancyRouteFilter<Employee, IEmployeeRepository>))]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
