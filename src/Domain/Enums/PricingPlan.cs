namespace Domain.Enums
{
    /// <summary>
    /// Tipos de planes de precios disponibles.
    /// </summary>
    /// <remarks>
    /// Este enum representa categorías funcionales de planes y se encuentra
    /// asociado a la entidad <see cref="Domain.Entities.PricingPlan"/>.
    /// La entidad define los valores persistidos (nombre, monto, relaciones),
    /// mientras que este enum se utiliza para la lógica de negocio.
    /// </remarks>
    public enum PricingPlan
    {
        Free = 1,
        Professional = 2,
        Enterprise = 3
    }
}
