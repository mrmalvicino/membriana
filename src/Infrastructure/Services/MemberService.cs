using Application.Repositories;
using Application.Services;
using Domain.Entities;

namespace Infrastructure.Services;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IBusinessLogger _businessLogger;

    public MemberService(
        IUnitOfWork unitOfWork,
        IUserService userService,
        IBusinessLogger businessLogger
    )
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _businessLogger = businessLogger;
    }

    public async Task<Member> AddAsync(Member member)
    {
        var loggedUser = await _userService.GetLoggedUserAsync();

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var createdMember = await _unitOfWork.MemberRepository.AddAsync(member);

            var organizationReferenceCode =
                await _unitOfWork.OrganizationRepository
                .GetReferenceCodeByIdAsync(createdMember.OrganizationId);

            if (organizationReferenceCode == null)
            {
                throw new InvalidOperationException("Organización no encontrada.");
            }

            var createdMemberStatusEvent = await _unitOfWork.MemberStatusEventRepository.AddAsync(
                new MemberStatusEvent
                {
                    MemberId = createdMember.Id,
                    OrganizationId = createdMember.OrganizationId,
                    PreviousStatus = null,
                    NewStatus = createdMember.MemberStatus,
                    ChangedAtDateTime = DateTime.UtcNow,
                    ChangedByUserId = loggedUser.Id
                }
            );

            await _unitOfWork.CommitAsync();

            _businessLogger.LogMemberStatusEventCreated(
                createdMemberStatusEvent,
                createdMember,
                loggedUser,
                organizationReferenceCode
            );

            return createdMember;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<Member> UpdateAsync(Member member)
    {
        var loggedUser = await _userService.GetLoggedUserAsync();
        var currentMember = await _unitOfWork.MemberRepository.GetByIdAsync(member.Id);

        if (currentMember == null)
        {
            throw new KeyNotFoundException("Socio no encontrado.");
        }

        var previousStatus = currentMember.MemberStatus;

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var updatedMember = await _unitOfWork.MemberRepository.UpdateAsync(member);

            var organizationReferenceCode =
                await _unitOfWork.OrganizationRepository
                .GetReferenceCodeByIdAsync(updatedMember.OrganizationId);

            if (organizationReferenceCode == null)
            {
                throw new InvalidOperationException("Organización no encontrada.");
            }

            var createdMemberStatusEvent = await _unitOfWork.MemberStatusEventRepository.AddAsync(
                new MemberStatusEvent
                {
                    MemberId = updatedMember.Id,
                    OrganizationId = updatedMember.OrganizationId,
                    PreviousStatus = previousStatus,
                    NewStatus = updatedMember.MemberStatus,
                    ChangedAtDateTime = DateTime.UtcNow,
                    ChangedByUserId = loggedUser.Id
                }
            );

            await _unitOfWork.CommitAsync();

            _businessLogger.LogMemberStatusEventCreated(
                createdMemberStatusEvent,
                updatedMember,
                loggedUser,
                organizationReferenceCode
            );

            return updatedMember;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
