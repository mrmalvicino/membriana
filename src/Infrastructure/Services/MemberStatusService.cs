using Application.Repositories;
using Application.Services;
using Domain.Enums;

namespace Infrastructure.Services;

public class MemberStatusService : IMemberStatusService
{
    private readonly IMemberStatusEventRepository _memberStatusEventRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUserService _userService;

    public MemberStatusService(
        IMemberStatusEventRepository memberStatusEventRepository,
        IMemberRepository memberRepository,
        IUserService userService
    )
    {
        _memberStatusEventRepository = memberStatusEventRepository;
        _memberRepository = memberRepository;
        _userService = userService;
    }

    public async Task<int> CountMembersWithStatusAsync(int year, int month, MemberStatus targetStatus)
    {
        int orgId = await _userService.GetOrganizationIdAsync();

        var endOfMonth = new DateTime(year, month, 1)
            .AddMonths(1)
            .AddTicks(-1);

        var members = await _memberRepository.GetAllAsync(orgId);
        var events = await _memberStatusEventRepository.GetAllAsync(orgId);
        var lastStatusByMemberId = new Dictionary<int, MemberStatus>();

        foreach (var e in events)
        {
            if (e.ChangedAtDateTime > endOfMonth)
            {
                continue;
            }

            if (!lastStatusByMemberId.ContainsKey(e.MemberId))
            {
                lastStatusByMemberId[e.MemberId] = e.NewStatus;
            }
        }

        int count = 0;

        foreach (var m in members)
        {
            var actualStatus = lastStatusByMemberId.TryGetValue(m.Id, out var s)
                ? s
                : m.MemberStatus;

            if (MatchesDashboardStatus(actualStatus, targetStatus))
            {
                count++;
            }
        }

        return count;
    }

    private bool MatchesDashboardStatus(
        MemberStatus actualStatus,
        MemberStatus targetStatus
    )
    {
        if (targetStatus == MemberStatus.Active)
        {
            return actualStatus == MemberStatus.Active || actualStatus == MemberStatus.Debtor;
        }

        return actualStatus == targetStatus;
    }
}
