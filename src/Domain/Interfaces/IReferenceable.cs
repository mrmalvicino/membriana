namespace Domain.Interfaces;

/// <summary>
/// Contrato para aquellas entidades que poseen un código de referencia estable.
/// </summary>
public interface IReferenceable
{
    string ReferenceCode { get; set; }
}
