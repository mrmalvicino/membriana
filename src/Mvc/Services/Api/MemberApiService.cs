using AutoMapper;
using Mvc.Dtos.Member;
using Mvc.Models;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Services.Api
{
    public class MemberApiService : IMemberApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public MemberApiService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<MemberViewModel>> GetAllAsync(int organizationId)
        {
            var response = await _httpClient
                .GetAsync($"api/members?organizationId={organizationId}");

            response.EnsureSuccessStatusCode();

            var readDtos = await response.Content.ReadFromJsonAsync<List<MemberReadDto>>() ?? new();

            return _mapper.Map<List<MemberViewModel>>(readDtos);
        }

        public async Task<MemberViewModel?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/members/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var readDto = await response.Content.ReadFromJsonAsync<MemberReadDto>();

            if (readDto == null)
            {
                return null;
            }

            return _mapper.Map<MemberViewModel>(readDto);
        }

        public async Task<MemberViewModel?> CreateAsync(MemberViewModel viewModel)
        {
            var createDto = _mapper.Map<MemberCreateDto>(viewModel);

            var response = await _httpClient
                .PostAsJsonAsync("api/members", createDto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var readDto = await response.Content.ReadFromJsonAsync<MemberReadDto>();

            return readDto == null ? null : _mapper.Map<MemberViewModel>(readDto);
        }

        public async Task<MemberViewModel?> UpdateAsync(MemberViewModel viewModel)
        {
            var updateDto = _mapper.Map<MemberUpdateDto>(viewModel);

            var response = await _httpClient
                .PutAsJsonAsync($"api/members/{viewModel.Id}", updateDto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var readDto = await response.Content.ReadFromJsonAsync<MemberReadDto>();

            return readDto == null ? null : _mapper.Map<MemberViewModel>(readDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient
                .DeleteAsync($"api/members/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
