namespace Contracts.Interfaces;

/// <summary>
/// Define un contrato para aquellas entidades que deben pertenecer a una organización.
/// </summary>
/// <remarks>
/// Las entidades que implementan esta interfaz están asociadas a una única
/// organización y participan en el modelo multi-tenant del sistema.
/// El valor de <see cref="OrganizationId"/> se utiliza para identificar
/// dicha pertenencia en entidades genéricas.
/// </remarks>
public interface ITenantable
{
    int OrganizationId { get; set; }
}
