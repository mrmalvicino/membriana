namespace Mvc.Exceptions
{
    /// <summary>
    /// Representa una excepción de regla de negocio en la capa MVC.
    /// </summary>
    /// <remarks>
    /// Se utiliza para indicar que una operación solicitada al backend fue
    /// rechazada debido a una condición de negocio válida.
    /// No representa un error técnico del sistema, sino una situación
    /// controlada que debe ser manejada por el controlador para mostrar un
    /// mensaje adecuado al usuario.
    /// </remarks>
    public sealed class BusinessRuleException : Exception
    {
        /// <summary>
        /// Constructor principal.
        /// </summary>
        public BusinessRuleException(string message) : base(message)
        {

        }
    }
}
