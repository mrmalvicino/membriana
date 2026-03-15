using System.Text.Json.Serialization;

namespace Infrastructure.Dtos.Mailtrap;

public class ConfirmationEmailTemplateVariables
{
    [JsonPropertyName("hosting_url")]
    public string HostingUrl { get; set; } = null!;

    [JsonPropertyName("confirmation_link")]
    public string ConfirmationLink { get; set; } = null!;

    [JsonPropertyName("organization_name")]
    public string OrganizationName { get; set; } = null!;

    [JsonPropertyName("product_name")]
    public string ProductName { get; set; } = null!;
}
