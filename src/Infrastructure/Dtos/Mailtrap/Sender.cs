using System.Text.Json.Serialization;

namespace Infrastructure.Dtos.Mailtrap;

public class Sender
{
    [JsonPropertyName("email")]
    public string EmailAddress { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}
