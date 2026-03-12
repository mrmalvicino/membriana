using System.Text.Json.Serialization;

namespace Infrastructure.Dtos.Mailtrap;

public class EmailRequest<T>
{
    [JsonPropertyName("from")]
    public Sender From { get; set; } = null!;

    [JsonPropertyName("to")]
    public List<Recipient> To { get; set; } = null!;

    [JsonPropertyName("template_uuid")]
    public string TemplateUUId { get; set; } = null!;

    [JsonPropertyName("template_variables")]
    public T? TemplateVariables { get; set; }
}
