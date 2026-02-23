namespace Domain.Enums
{
    /// <summary>
    /// Representa el estado actual de un socio dentro de la organización.
    /// </summary>
    public enum MemberStatus
    {
        /// <summary>
        /// Estado activo. 
        /// El socio participa normalmente y cuenta como "Activo" en el dashboard.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Estado con deuda.
        /// El socio continúa vinculado pero puede tener restricciones.
        /// Cuenta como activo para métricas generales.
        /// </summary>
        Debtor = 2,

        /// <summary>
        /// Estado inactivo o dado de baja.
        /// Cuenta como "Inactivo" en el dashboard.
        /// </summary>
        Inactive = 3
    }
}