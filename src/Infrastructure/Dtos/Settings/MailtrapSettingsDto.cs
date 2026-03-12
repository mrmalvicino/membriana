namespace Infrastructure.Dtos.Settings;

/// <summary>
/// DTO que contiene la configuración necesaria para interactuar con la API de Mailtrap.
/// </summary>
public class MailtrapSettingsDto
{
    public string ApiUrl { get; set; } = null!;
    public string ApiToken { get; set; } = null!;
    public string EmailConfirmationTemplateUuid { get; set; } = null!;
}
