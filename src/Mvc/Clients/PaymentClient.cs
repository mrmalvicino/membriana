using AutoMapper;
using Contracts.Dtos.Payment;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Clients.Helpers;
using Mvc.Clients.Interfaces;

namespace Mvc.Clients;

public class PaymentClient : IPaymentClient
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public PaymentClient(
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
        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener la lista de pagos.");
        var readDtos = await response.Content.ReadFromJsonAsync<List<PaymentReadDto>>() ?? new();
        return _mapper.Map<List<PaymentViewModel>>(readDtos);
    }

    public async Task<PaymentViewModel?> GetByIdAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/payments/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener el pago.");

        var readDto = await response.Content.ReadFromJsonAsync<PaymentReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al obtener el pago.");
        }

        return _mapper.Map<PaymentViewModel>(readDto);
    }

    public async Task<PaymentViewModel?> CreateAsync(PaymentViewModel viewModel)
    {
        var createDto = _mapper.Map<PaymentCreateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/payments";
        var response = await _httpClient.PostAsJsonAsync(url, createDto);

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo registrar el pago.");

        var readDto = await response.Content.ReadFromJsonAsync<PaymentReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al registrar el pago.");
        }

        return _mapper.Map<PaymentViewModel>(readDto);
    }

    public async Task<PaymentViewModel?> UpdateAsync(PaymentViewModel viewModel)
    {
        var updateDto = _mapper.Map<PaymentUpdateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/payments/{viewModel.Id}";
        var response = await _httpClient.PutAsJsonAsync(url, updateDto);

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo actualizar el pago.");

        var readDto = await response.Content.ReadFromJsonAsync<PaymentReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al actualizar el pago.");
        }

        return _mapper.Map<PaymentViewModel>(readDto);
    }

    public async Task DeleteAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/payments/{id}";
        var response = await _httpClient.DeleteAsync(url);
        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo eliminar el pago.");
    }

    public async Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month)
    {
        var url = $"{_apiBaseUrl}api/payments/get-monthly-income" +
            $"?organizationId={organizationId}&year={year}&month={month}";

        var response = await _httpClient.GetAsync(url);
        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener el ingreso mensual.");
        var dto = await response.Content.ReadFromJsonAsync<MonthlyIncomeResponseDto>();

        if (dto is null)
        {
            throw new InvalidOperationException($"Respuesta vacía de {url}");
        }

        return dto.Amount;
    }
}
