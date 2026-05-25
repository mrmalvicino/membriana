using Contracts.Enums;

namespace Mvc.Clients.Interfaces;

public interface IMemberStatusClient
{
    Task<int> CountMembersWithStatusAsync(
        int organizationId,
        int year,
        int month,
        MemberStatus status
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
