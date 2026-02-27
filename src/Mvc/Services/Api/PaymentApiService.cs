using AutoMapper;
using Contracts.Dtos.Payment;
using Mvc.Models;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Services.Api
{
    public class PaymentApiService : IPaymentApiService
    {
        private readonly string _apiBaseUrl;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public PaymentApiService(
            IConfiguration configuration,
            HttpClient httpClient,
            IMapper mapper
        )
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<PaymentViewModel>> GetAllAsync(int organizationId)
        {
            var url = $"{_apiBaseUrl}api/payments?organizationId={organizationId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var readDtos = await response.Content.ReadFromJsonAsync<List<PaymentReadDto>>() ?? new();
            return _mapper.Map<List<PaymentViewModel>>(readDtos);
        }

        public async Task<PaymentViewModel?> GetByIdAsync(int id)
        {
            var url = $"{_apiBaseUrl}api/payments/{id}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var readDto = await response.Content.ReadFromJsonAsync<PaymentReadDto>();

            if (readDto == null)
            {
                return null;
            }

            return _mapper.Map<PaymentViewModel>(readDto);
        }

        public async Task<PaymentViewModel?> CreateAsync(PaymentViewModel viewModel)
        {
            var createDto = _mapper.Map<PaymentCreateDto>(viewModel);
            var url = $"{_apiBaseUrl}api/payments";
            var response = await _httpClient.PostAsJsonAsync(url, createDto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var readDto = await response.Content.ReadFromJsonAsync<PaymentReadDto>();
            return readDto == null ? null : _mapper.Map<PaymentViewModel>(readDto);
        }

        public async Task<PaymentViewModel?> UpdateAsync(PaymentViewModel viewModel)
        {
            var updateDto = _mapper.Map<PaymentUpdateDto>(viewModel);
            var url = $"{_apiBaseUrl}api/payments/{viewModel.Id}";
            var response = await _httpClient.PutAsJsonAsync(url, updateDto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var readDto = await response.Content.ReadFromJsonAsync<PaymentReadDto>();
            return readDto == null ? null : _mapper.Map<PaymentViewModel>(readDto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var url = $"{_apiBaseUrl}api/payments/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
