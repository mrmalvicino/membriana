using AutoMapper;
using Mvc.Dtos.MembershipPlan;
using Mvc.Models;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Services.Api
{
    public class MembershipPlanApiService : IMembershipPlanApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public MembershipPlanApiService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<MembershipPlanViewModel>> GetAllAsync(int organizationId)
        {
            var response = await _httpClient
                .GetAsync($"api/membershipplans?organizationId={organizationId}");

            response.EnsureSuccessStatusCode();

            var dtos = await response.Content.ReadFromJsonAsync<List<MembershipPlanReadDto>>() ?? new();

            return _mapper.Map<List<MembershipPlanViewModel>>(dtos);
        }

        public async Task<MembershipPlanViewModel?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/membershipplans/{id}");

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

            var response = await _httpClient
                .PostAsJsonAsync("api/membershipplans", createDto);

            response.EnsureSuccessStatusCode();

            var readDto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();

            return readDto == null ? null : _mapper.Map<MembershipPlanViewModel>(readDto);
        }

        public async Task<MembershipPlanViewModel?> UpdateAsync(MembershipPlanViewModel viewModel)
        {
            var updateDto = _mapper.Map<MembershipPlanUpdateDto>(viewModel);

            var response = await _httpClient
                .PutAsJsonAsync($"api/membershipplans/{viewModel.Id}", updateDto);

            response.EnsureSuccessStatusCode();

            var readDto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();

            return readDto == null ? null : _mapper.Map<MembershipPlanViewModel>(readDto);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient
                .DeleteAsync($"api/membershipplans/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
