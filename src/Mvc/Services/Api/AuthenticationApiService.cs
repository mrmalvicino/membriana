using AutoMapper;
using Contracts.Dtos.Authentication;
using Contracts.Dtos.Common;
using Mvc.Services.Api.Interfaces;
using Mvc.ViewModels;
using System.Text;
using System.Text.Json;

namespace Mvc.Services.Api;

public class AuthenticationApiService : IAuthenticationApiService
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public AuthenticationApiService(
        IConfiguration configuration,
        HttpClient httpClient,
        IMapper mapper
    )
    {
        _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginViewModel loginViewModel)
    {
        var loginRequestDto = _mapper.Map<LoginRequestDto>(loginViewModel);

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequestDto),
            Encoding.UTF8, "application/json"
        );

        var url = $"{_apiBaseUrl}api/authentication/login";
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(await ReadErrorMessageAsync(response, "Credenciales inválidas."));
        }

        var json = await response.Content.ReadAsStringAsync();

        var loginResponseDto = JsonSerializer.Deserialize<LoginResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        return loginResponseDto;
    }

    public async Task<RegisterResponseDto?> RegisterAsync(RegisterViewModel registerViewModel)
    {
        var registerRequestDto = _mapper.Map<RegisterRequestDto>(registerViewModel);

        var content = new StringContent(
            JsonSerializer.Serialize(registerRequestDto),
            Encoding.UTF8, "application/json"
        );

        var url = $"{_apiBaseUrl}api/authentication/register";
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(await ReadErrorMessageAsync(response, "Datos inválidos"));
        }

        var json = await response.Content.ReadAsStringAsync();

        var registerResponseDto = JsonSerializer.Deserialize<RegisterResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        return registerResponseDto;
    }

    public async Task<ResendConfirmationResponseDto?> ResendConfirmationAsync(
        ResendConfirmationViewModel resendConfirmationViewModel
    )
    {
        var resendConfirmationRequestDto = _mapper.Map<ResendConfirmationRequestDto>(resendConfirmationViewModel);

        var content = new StringContent(
            JsonSerializer.Serialize(resendConfirmationRequestDto),
            Encoding.UTF8, "application/json"
        );

        var url = $"{_apiBaseUrl}api/authentication/resend-confirmation";
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();

            var errorResponse = JsonSerializer.Deserialize<ResendConfirmationResponseDto>(
                errorContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            throw new ApplicationException(
                errorResponse?.Message ?? "No pudimos reenviar el correo. Intenta nuevamente."
            );
        }

        var json = await response.Content.ReadAsStringAsync();

        var resendConfirmationResponseDto = JsonSerializer.Deserialize<ResendConfirmationResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        return resendConfirmationResponseDto;
    }

    public async Task<ConfirmEmailResponseDto> ConfirmEmailAsync(
        ConfirmEmailViewModel confirmEmailViewModel
    )
    {
        var confirmEmailRequestDto = _mapper.Map<ConfirmEmailRequestDto>(confirmEmailViewModel);

        var content = new StringContent(
            JsonSerializer.Serialize(confirmEmailRequestDto),
            Encoding.UTF8,
            "application/json"
        );

        var url = $"{_apiBaseUrl}api/authentication/confirm-email";
        var response = await _httpClient.PostAsync(url, content);
        var json = await response.Content.ReadAsStringAsync();

        var confirmEmailResponseDto = JsonSerializer.Deserialize<ConfirmEmailResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(
                confirmEmailResponseDto?.Message ?? "No se pudo confirmar el email."
            );
        }

        return confirmEmailResponseDto ?? new ConfirmEmailResponseDto
        {
            Message = "Email confirmado correctamente."
        };
    }

    private async Task<string> ReadErrorMessageAsync(
        HttpResponseMessage response,
        string fallbackMessage
    )
    {
        var errorContent = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(errorContent))
        {
            return fallbackMessage;
        }

        try
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorResponseDto>(
                errorContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (errorResponse?.Errors is { Count: > 0 })
            {
                return string.Join(", ", errorResponse.Errors);
            }
        }
        catch (JsonException)
        {
        }

        return errorContent;
    }
}
