using Api.Filters;
using Application.Dtos.Member;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Policy = "Employee")]
    public class MembersController : BaseController<
        Member,
        IMemberRepository,
        MemberReadDto,
        MemberCreateDto,
        MemberUpdateDto
    >
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembersController(
            IMemberRepository repository,
            IUserService userService,
            IMapper mapper,
            IUnitOfWork unitOfWork
        ) : base(repository, userService, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        [ServiceFilter(typeof(TenancyRouteFilter<Member, IMemberRepository>))]
        public override async Task<ActionResult<MemberReadDto>> Get(int id)
        {
            return await base.Get(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public override async Task<ActionResult<MemberReadDto>> Create(
            [FromBody] MemberCreateDto createDto
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

                await _unitOfWork.IdentityService.AddToRole(user, Domain.Enums.AppRole.Member);

                var member = _mapper.Map<Member>(createDto);
                member.UserId = user.Id;
                var created = await _unitOfWork.MemberRepository.AddAsync(member);
                var readDto = _mapper.Map<MemberReadDto>(created);

                await _unitOfWork.CommitAsync();

                return CreatedAtAction(nameof(Get), new { id = readDto.Id }, readDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
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
