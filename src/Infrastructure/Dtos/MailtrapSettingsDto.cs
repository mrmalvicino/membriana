namespace Infrastructure.Dtos
{
    /// <summary>
    /// DTO que contiene la configuración necesaria para interactuar con la API de Mailtrap.
    /// </summary>
    public class MailtrapSettingsDto
    {
        public string ApiUrl { get; set; } = string.Empty;
        public string ApiToken { get; set; } = string.Empty;
        public string BusinessEmail { get; set; } = string.Empty;
        public string BusinessUrl { get; set; } = string.Empty;
        public string WelcomeTemplateUuid { get; set; } = string.Empty;
        public string ConfirmationTemplateUuid { get; set; } = string.Empty;
    }
}
