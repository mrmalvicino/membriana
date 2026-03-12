using System.Text.Json.Serialization;

namespace Infrastructure.Dtos.Mailtrap;

public class Recipient
{
    [JsonPropertyName("email")]
    public string EmailAddress { get; set; } = null!;
}
