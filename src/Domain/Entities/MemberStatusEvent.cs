using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Representa un cambio de estado en la situación financiera de un miembro dentro
/// de una organización. Incluye información sobre el estado anterior, el nuevo estado, la
/// fecha y hora del cambio, el usuario que realizó el cambio y otros detalles relevantes.
/// </summary>
public class MemberStatusEvent : IIdentifiable, ITenantable, IAuditable
{
    #region Id
    public int Id { get; set; }
    #endregion

    #region Member
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    #endregion

    #region Organization
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    #endregion

    #region PreviousStatus
    public MemberStatus? PreviousStatus { get; set; }
    #endregion

    #region NewStatus
    public MemberStatus NewStatus { get; set; }
    #endregion

    #region ChangedAtDateTime
    public DateTime ChangedAtDateTime { get; set; }
    #endregion
    
    #region ChangedByUser
    public int ChangedByUserId { get; set; }
    public AppUser ChangedByUser { get; set; } = null!;
    #endregion

    #region Details
    public string? Details { get; set; }
    #endregion
}
