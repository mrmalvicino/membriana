using Contracts.Enums;

namespace Mvc.Services.Api.Interfaces;

public interface IMemberStatusApiService
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
