using Api.Filters;
using Contracts.Dtos.MembershipPlan;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Policy = "Employee")]
public class MembershipPlansController : BaseController<
    MembershipPlan,
    IMembershipPlanRepository,
    MembershipPlanReadDto,
    MembershipPlanCreateDto,
    MembershipPlanUpdateDto
>
{
    public MembershipPlansController(
        IMembershipPlanRepository repository,
        IUserService userService,
        IMapper mapper
    ) : base(repository, userService, mapper)
    {

    }

    [ServiceFilter(typeof(TenancyRouteFilter<MembershipPlan, IMembershipPlanRepository>))]
    public override async Task<ActionResult<MembershipPlanReadDto>> Get(int id)
    {
        return await base.Get(id);
    }

    [ServiceFilter(typeof(TenancyRouteFilter<MembershipPlan, IMembershipPlanRepository>))]
    public override async Task<ActionResult<MembershipPlanReadDto>> Update(
        int id,
        [FromBody] MembershipPlanUpdateDto updateDto
    )
    {
        return await base.Update(id, updateDto);
    }

    [ServiceFilter(typeof(TenancyRouteFilter<MembershipPlan, IMembershipPlanRepository>))]
    public override async Task<IActionResult> Delete(int id)
    {
        return await base.Delete(id);
    }
}
