using Contracts.Enums;

namespace Application.Services;

public interface IMemberStatusService
{
    Task<int> CountMembersWithStatusAsync(
        int organizationId,
        int year,
        int month,
        MemberStatus targetStatus
    );

    Task<int> CountFirstTimeSignupsAsync(
        int organizationId,
        int year,
        int month
    );

    Task<int> CountFirstTimeCancellationsAsync(
        int organizationId,
        int year,
        int month
    );
}
