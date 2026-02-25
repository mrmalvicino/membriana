using Domain.Enums;

namespace Application.Services;

public interface IMemberStatusService
{
    Task<int> CountMembersWithStatusAsync(
        int year,
        int month,
        MemberStatus targetStatus
    );
}
