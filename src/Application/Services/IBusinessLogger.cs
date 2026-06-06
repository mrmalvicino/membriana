using Domain.Entities;

namespace Application.Services;

public interface IBusinessLogger
{
    void LogUserRegistered(AppUser user, Employee employee);
    void LogEmailConfirmed(AppUser user);
    void LogMemberStatusEventCreated(MemberStatusEvent memberStatusEvent, Member member, AppUser changedByUser);
}
