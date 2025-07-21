using AutoMapper;
using Mvc.Dtos.Authentication;
using Mvc.Models;
using Mvc.Services.Api.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mvc.Services.Api
{
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
                return null;
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
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonSerializer.Deserialize<ErrorResponseDto>(
                    errorContent,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                throw new ApplicationException(
                    errorResponse != null &&
                    errorResponse.Errors != null
                    ? string.Join(", ", errorResponse.Errors)
                    : "Datos inválidos"
                );
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
    }
}
