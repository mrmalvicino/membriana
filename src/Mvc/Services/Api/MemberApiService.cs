using AutoMapper;
using Contracts.Dtos.Member;
using Mvc.Models;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Services.Api
{
    public class MemberApiService : IMemberApiService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public MemberApiService(
            IConfiguration configuration,
            HttpClient httpClient,
            IMapper mapper
        )
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<MemberViewModel>> GetAllAsync(int organizationId)
        {
            var url = $"{_apiBaseUrl}api/members?organizationId={organizationId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var readDtos = await response.Content.ReadFromJsonAsync<List<MemberReadDto>>() ?? new();
            return _mapper.Map<List<MemberViewModel>>(readDtos);
        }

        public async Task<MemberViewModel?> GetByIdAsync(int id)
        {
            var url = $"{_apiBaseUrl}api/members/{id}";
            var response = await _httpClient.GetAsync(url);

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
            var url = $"{_apiBaseUrl}api/members";
            var response = await _httpClient.PostAsJsonAsync(url, createDto);

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
            var url = $"{_apiBaseUrl}api/members/{viewModel.Id}";
            var response = await _httpClient.PutAsJsonAsync(url, updateDto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var readDto = await response.Content.ReadFromJsonAsync<MemberReadDto>();
            return readDto == null ? null : _mapper.Map<MemberViewModel>(readDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = $"{_apiBaseUrl}api/members/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
