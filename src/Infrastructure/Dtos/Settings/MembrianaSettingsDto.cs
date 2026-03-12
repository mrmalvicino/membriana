namespace Infrastructure.Dtos.Settings;

/// <summary>
/// DTO que contiene la configuración de Deploy de Membriana.
/// </summary>
public class MembrianaSettingsDto
{
    public string HostingUrl { get; set; } = null!;
    public string MailingEmail { get; set; } = null!;
}
