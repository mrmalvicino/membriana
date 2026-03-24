using Application.Services;
using Infrastructure.Dtos.Mailtrap;
using Infrastructure.Dtos.Settings;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly MembrianaSettingsDto _membrianaSettings;
    private readonly MailtrapSettingsDto _mailtrapSettings;

    public EmailService(
        IOptions<MembrianaSettingsDto> membrianaSettings,
        IOptions<MailtrapSettingsDto> mailtrapSettings
    )
    {
        _membrianaSettings = membrianaSettings.Value;
        _mailtrapSettings = mailtrapSettings.Value;
    }

    public async Task SendConfirmationEmailAsync(
        string userEmail,
        string organizationName,
        string confirmationLink
    )
    {
        var httpClient = new RestClient(_mailtrapSettings.ApiUrl);
        var request = new RestRequest();

        request.AddHeader("Authorization", $"Bearer {_mailtrapSettings.ApiToken}");
        request.AddHeader("Content-Type", "application/json");

        var emailRequest = new EmailRequest<ConfirmationEmailTemplateVariables>
        {
            From = new Sender
            {
                EmailAddress = _membrianaSettings.MailingEmail,
                Name = _membrianaSettings.ProductName
            },
            To = new List<Recipient>
            {
                new Recipient
                {
                    EmailAddress = userEmail
                }
            },
            TemplateUUId = _mailtrapSettings.EmailConfirmationTemplateUuid,
            TemplateVariables = new ConfirmationEmailTemplateVariables
            {
                HostingUrl = _membrianaSettings.HostingUrl,
                ConfirmationLink = confirmationLink,
                OrganizationName = organizationName,
                ProductName = _membrianaSettings.ProductName
            }
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        string jsonParameters = JsonSerializer.Serialize(emailRequest, options);
        request.AddParameter("application/json", jsonParameters, ParameterType.RequestBody);

        var response = await httpClient.PostAsync(request);

        if (response is null)
        {
            throw new HttpRequestException("La API de Mailtrap no devolvió ninguna Response.");
        }

        if (!response.IsSuccessful)
        {
            throw new HttpRequestException(
                $"La Request de Mailtrap falló con status code {(int)response.StatusCode}: {response.Content}"
            );
        }
    }
}
