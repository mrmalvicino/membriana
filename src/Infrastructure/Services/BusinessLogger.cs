using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class BusinessLogger : IBusinessLogger
{
    private readonly ILogger<BusinessLogger> _logger;

    public BusinessLogger(ILogger<BusinessLogger> logger)
    {
        _logger = logger;
    }

    public void LogUserRegistered(AppUser user, Employee employee, string organizationReferenceCode)
    {
        _logger.LogInformation(
            "Se registró el usuario {UserReferenceCode} ({UserId}, {UserEmail}) en la organización {OrganizationReferenceCode} como el empleado {EmployeeReferenceCode}.",
            user.ReferenceCode,
            user.Id,
            user.Email,
            organizationReferenceCode,
            employee.ReferenceCode
        );
    }

    public void LogEmailConfirmed(AppUser user, string organizationReferenceCode)
    {
        _logger.LogInformation(
            "Email confirmado por usuario {UserReferenceCode} ({UserId}, {UserEmail}) en organización {OrganizationReferenceCode}.",
            user.ReferenceCode,
            user.Id,
            user.Email,
            organizationReferenceCode
        );
    }

    public void LogMemberStatusEventCreated(
        MemberStatusEvent memberStatusEvent,
        Member member,
        AppUser changedByUser,
        string organizationReferenceCode
    )
    {
        _logger.LogInformation(
            "Cambio de estado {MemberStatusEventReferenceCode} del socio {MemberReferenceCode} ({MemberId}) realizado por el usuario {UserReferenceCode} ({UserId}) en organización {OrganizationReferenceCode}. Estado anterior: {PreviousStatus}. Nuevo estado: {NewStatus}.",
            memberStatusEvent.ReferenceCode,
            member.ReferenceCode,
            member.Id,
            changedByUser.ReferenceCode,
            changedByUser.Id,
            organizationReferenceCode,
            memberStatusEvent.PreviousStatus,
            memberStatusEvent.NewStatus
        );
    }
}
