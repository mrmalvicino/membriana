using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mvc.Dtos.MembershipPlan;
using Mvc.Exceptions;
using Mvc.Models;
using Mvc.Services.Api.Interfaces;
using System.Net;
using System.Text.Json;

namespace Mvc.Services.Api
{
    public class MembershipPlanApiService : IMembershipPlanApiService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public MembershipPlanApiService(
            IConfiguration configuration,
            HttpClient httpClient,
            IMapper mapper
        )
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<MembershipPlanViewModel>> GetAllAsync(int organizationId)
        {
            var url = $"{_apiBaseUrl}api/membershipplans?organizationId={organizationId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var dtos = await response.Content.ReadFromJsonAsync<List<MembershipPlanReadDto>>() ?? new();
            return _mapper.Map<List<MembershipPlanViewModel>>(dtos);
        }

        public async Task<MembershipPlanViewModel?> GetByIdAsync(int id)
        {
            var url = $"{_apiBaseUrl}api/membershipplans/{id}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var dto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();

            return dto == null ? null : _mapper.Map<MembershipPlanViewModel>(dto);
        }

        public async Task<MembershipPlanViewModel?> CreateAsync(MembershipPlanViewModel viewModel)
        {
            var createDto = _mapper.Map<MembershipPlanCreateDto>(viewModel);
            var url = $"{_apiBaseUrl}api/membershipplans";
            var response = await _httpClient.PostAsJsonAsync(url, createDto);
            response.EnsureSuccessStatusCode();
            var readDto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();
            return readDto == null ? null : _mapper.Map<MembershipPlanViewModel>(readDto);
        }

        public async Task<MembershipPlanViewModel?> UpdateAsync(MembershipPlanViewModel viewModel)
        {
            var updateDto = _mapper.Map<MembershipPlanUpdateDto>(viewModel);
            var url = $"{_apiBaseUrl}api/membershipplans/{viewModel.Id}";
            var response = await _httpClient.PutAsJsonAsync(url, updateDto);
            response.EnsureSuccessStatusCode();
            var readDto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();
            return readDto == null ? null : _mapper.Map<MembershipPlanViewModel>(readDto);
        }

        public async Task DeleteAsync(int id)
        {
            var url = $"{_apiBaseUrl}api/membershipplans/{id}";
            var response = await _httpClient.DeleteAsync(url);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var body = await response.Content.ReadAsStringAsync();

                string message = "No se puede eliminar el recurso.";

                if (!string.IsNullOrWhiteSpace(body))
                {
                    try
                    {
                        var problem = JsonSerializer.Deserialize<ProblemDetails>(
                            body,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        message = problem?.Detail ?? problem?.Title ?? message;
                    }
                    catch
                    {
                        message = body;
                    }
                }

                throw new BusinessRuleException(message);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("El plan de membresía no existe.");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
