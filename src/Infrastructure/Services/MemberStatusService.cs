using Application.Repositories;
using Application.Services;
using Contracts.Enums;

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

    /// <summary>
    /// Cuenta cuántos socios de la organización del usuario autenticado tenían el estado indicado
    /// al cierre del mes especificado.
    /// </summary>
    /// <remarks>
    /// El cálculo reconstruye el estado vigente de cada socio tomando el último
    /// <see cref="Domain.Entities.MemberStatusEvent"/> ocurrido hasta el final del mes.
    /// Si un socio no tiene eventos registrados para ese período, se usa el estado actual
    /// almacenado en la entidad <see cref="Domain.Entities.Member"/>.
    /// Para métricas de dashboard, el estado <see cref="MemberStatus.Active"/> incluye también
    /// a los socios en estado <see cref="MemberStatus.Debtor"/>.
    /// </remarks>
    public async Task<int> CountMembersWithStatusAsync(
        int year,
        int month,
        MemberStatus targetStatus
    )
    {
        int orgId = await _userService.GetOrganizationIdAsync();

        var endOfMonth = new DateTime(year, month, 1)
            .AddMonths(1)
            .AddTicks(-1);

        var members = await _memberRepository.GetAllAsync(orgId);
        var events = await _memberStatusEventRepository.GetAllAsync(orgId);
        var lastStatusByMemberId = new Dictionary<int, MemberStatus>();

        // Recorre los eventos en orden cronológico para que el diccionario tenga el último
        // cambio de estado al cierre de mes de cada socio.
        foreach (var e in events.OrderBy(e => e.ChangedAtDateTime))
        {
            if (e.ChangedAtDateTime > endOfMonth)
            {
                continue;
            }

            lastStatusByMemberId[e.MemberId] = e.NewStatus;
        }

        int count = 0;

        foreach (var m in members)
        {
            MemberStatus endMonthStatus;

            // Si el socio no tuvo cambios de estado antes del cierre del mes, se toma el actual
            if (lastStatusByMemberId.TryGetValue(m.Id, out var memberStatus))
            {
                endMonthStatus = memberStatus;
            }
            else
            {
                endMonthStatus = m.MemberStatus;
            }

            if (MatchesDashboardStatus(endMonthStatus, targetStatus))
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
