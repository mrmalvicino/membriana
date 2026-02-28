using AutoMapper;
using Contracts.Dtos.Employee;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Services.Api;

public class EmployeeApiService : IEmployeeApiService
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public EmployeeApiService(
        IConfiguration configuration,
        HttpClient httpClient,
        IMapper mapper
    )
    {
        _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<List<EmployeeViewModel>> GetAllAsync(int organizationId)
    {
        var url = $"{_apiBaseUrl}api/employees?organizationId={organizationId}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var readDtos = await response.Content.ReadFromJsonAsync<List<EmployeeReadDto>>() ?? new();
        return _mapper.Map<List<EmployeeViewModel>>(readDtos);
    }

    public async Task<EmployeeViewModel?> GetByIdAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/employees/{id}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var readDto = await response.Content.ReadFromJsonAsync<EmployeeReadDto>();

        if (readDto == null)
        {
            return null;
        }

        return _mapper.Map<EmployeeViewModel>(readDto);
    }

    public async Task<EmployeeViewModel?> CreateAsync(EmployeeViewModel viewModel)
    {
        var createDto = _mapper.Map<EmployeeCreateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/employees";
        var response = await _httpClient.PostAsJsonAsync(url, createDto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var readDto = await response.Content.ReadFromJsonAsync<EmployeeReadDto>();
        return readDto == null ? null : _mapper.Map<EmployeeViewModel>(readDto);
    }

    public async Task<EmployeeViewModel?> UpdateAsync(EmployeeViewModel viewModel)
    {
        var updateDto = _mapper.Map<EmployeeUpdateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/employees/{viewModel.Id}";
        var response = await _httpClient.PutAsJsonAsync(url, updateDto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var readDto = await response.Content.ReadFromJsonAsync<EmployeeReadDto>();
        return readDto == null ? null : _mapper.Map<EmployeeViewModel>(readDto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/employees/{id}";
        var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }
}
