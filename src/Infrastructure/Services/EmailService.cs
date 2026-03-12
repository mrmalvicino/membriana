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
                OrganizationName = organizationName
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

        try
        {
            var response = httpClient.Post(request);

            if (!response.IsSuccessful)
            {
                throw new Exception(response.Content);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
}
