using Domain.Entities;

namespace Application.Services;

public interface IBusinessLogger
{
    void LogUserRegistered(AppUser user, Employee employee, string organizationReferenceCode);
    void LogEmailConfirmed(AppUser user, string organizationReferenceCode);
    void LogMemberStatusEventCreated(MemberStatusEvent memberStatusEvent, Member member, AppUser changedByUser, string organizationReferenceCode);
}
