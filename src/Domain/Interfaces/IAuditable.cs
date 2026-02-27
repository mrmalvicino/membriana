using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// Define la información de auditoría para las entidades que implementan esta interfaz.
/// </summary>
public interface IAuditable
{
    DateTime ChangedAtDateTime { get; set; }
    AppUser ChangedByUser { get; set; }
    string? Details { get; set; }
}
