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
}
