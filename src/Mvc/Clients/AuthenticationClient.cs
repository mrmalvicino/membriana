using AutoMapper;
using Contracts.Dtos.Authentication;
using Mvc.Clients.Helpers;
using Mvc.Clients.Interfaces;
using Mvc.ViewModels;

namespace Mvc.Clients;

public class AuthenticationClient : IAuthenticationClient
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public AuthenticationClient(
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

        var url = $"{_apiBaseUrl}api/authentication/login";
        var response = await _httpClient.PostAsJsonAsync(url, loginRequestDto);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "Credenciales inválidas.");

        return await ReadRequiredAsync<LoginResponseDto>(
            response,
            "La API devolvió una respuesta vacía al iniciar sesión."
        );
    }

    public async Task<RegisterResponseDto?> RegisterAsync(RegisterViewModel registerViewModel)
    {
        var registerRequestDto = _mapper.Map<RegisterRequestDto>(registerViewModel);

        var url = $"{_apiBaseUrl}api/authentication/register";
        var response = await _httpClient.PostAsJsonAsync(url, registerRequestDto);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "Datos inválidos.");

        return await ReadRequiredAsync<RegisterResponseDto>(
            response,
            "La API devolvió una respuesta vacía al registrar la cuenta."
        );
    }

    public async Task<ResendConfirmationResponseDto?> ResendConfirmationAsync(
        ResendConfirmationViewModel resendConfirmationViewModel
    )
    {
        var resendConfirmationRequestDto = _mapper.Map<ResendConfirmationRequestDto>(resendConfirmationViewModel);

        var url = $"{_apiBaseUrl}api/authentication/resend-confirmation";
        var response = await _httpClient.PostAsJsonAsync(url, resendConfirmationRequestDto);

        await ApiErrorResponseHandler.EnsureSuccessAsync(
            response,
            "No pudimos reenviar el correo. Intenta nuevamente."
        );

        return await ReadRequiredAsync<ResendConfirmationResponseDto>(
            response,
            "La API devolvió una respuesta vacía al reenviar la confirmación."
        );
    }

    public async Task<ConfirmEmailResponseDto> ConfirmEmailAsync(
        ConfirmEmailViewModel confirmEmailViewModel
    )
    {
        var confirmEmailRequestDto = _mapper.Map<ConfirmEmailRequestDto>(confirmEmailViewModel);

        var url = $"{_apiBaseUrl}api/authentication/confirm-email";
        var response = await _httpClient.PostAsJsonAsync(url, confirmEmailRequestDto);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo confirmar el email.");

        return await ReadRequiredAsync<ConfirmEmailResponseDto>(
            response,
            "La API devolvió una respuesta vacía al confirmar el email."
        );
    }

    private static async Task<T> ReadRequiredAsync<T>(
        HttpResponseMessage response,
        string emptyResponseMessage
    ) where T : class
    {
        var dto = await response.Content.ReadFromJsonAsync<T>();

        if (dto == null)
        {
            throw new InvalidOperationException(emptyResponseMessage);
        }

        return dto;
    }

}
