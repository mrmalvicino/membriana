using Contracts.Enums;

namespace Application.Services;

public interface IMemberStatusService
{
    Task<int> CountMembersWithStatusAsync(
        int year,
        int month,
        MemberStatus targetStatus
    );

    Task<int> CountFirstTimeSignupsAsync(
        int year,
        int month
    );

    Task<int> CountFirstTimeCancellationsAsync(
        int year,
        int month
    );
}
