namespace Contracts.Interfaces;

/// <summary>
/// Define un contrato para aquellas entidades que deben poseer un ID inequívoco.
/// </summary>
/// <remarks>
/// Al garantizar que una entidad tiene la propiedad <see cref="Id"/>, es posible
/// desarrollar infraestructura reusable como Mappers y Controllers genéricos
/// sin necesidad de usar Reflection o casts inseguros.
/// </remarks>
public interface IIdentifiable
{
    int Id { get; set; }
}
