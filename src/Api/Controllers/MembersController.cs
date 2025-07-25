using Api.Filters;
using Application.Dtos.Member;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MembersController : BaseController<
        Member,
        IMemberRepository,
        MemberReadDto,
        MemberCreateDto,
        MemberUpdateDto
    >
    {
        public MembersController(
            IMemberRepository repository,
            IUserService userService,
            IMapper mapper
        ) : base(repository, userService, mapper)
        {

        }

        [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
        public override async Task<ActionResult<MemberReadDto>> Get(int id)
        {
            return await base.Get(id);
        }

        [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
        public override async Task<ActionResult<MemberReadDto>> Update(
            int id,
            [FromBody] MemberUpdateDto updateDto
        )
        {
            return await base.Update(id, updateDto);
        }

        [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }
    }
}
