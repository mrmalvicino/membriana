namespace Mvc.ViewModels;

/// <summary>
/// Representa el resultado de una operación realizada desde la interfaz de usuario.
/// Se utiliza como clase base para ViewModels que necesitan informar si una operación
/// fue exitosa y mostrar un mensaje al usuario.
/// </summary>
public class ResponseViewModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
