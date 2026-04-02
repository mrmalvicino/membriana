using Contracts.Enums;

namespace Application.Services;

public interface IMemberStatusService
{
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
    Task<int> CountMembersWithStatusAsync(
        int organizationId,
        int year,
        int month,
        MemberStatus targetStatus
    );

    /// <summary>
    /// Obtiene la cantidad de miembros que se dieron de alta por primera vez en un mes.
    /// </summary>
    Task<int> CountFirstTimeSignupsAsync(
        int organizationId,
        int year,
        int month
    );

    /// <summary>
    /// Obtiene la cantidad de miembros que se dieron de baja por primera vez en un mes.
    /// </summary>
    Task<int> CountFirstTimeCancellationsAsync(
        int organizationId,
        int year,
        int month
    );
}
