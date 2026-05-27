using AutoMapper;
using Contracts.Dtos.Employee;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Clients.Helpers;
using Mvc.Clients.Interfaces;

namespace Mvc.Clients;

public class EmployeeClient : IEmployeeClient
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public EmployeeClient(
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
        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener la lista de empleados.");
        var readDtos = await response.Content.ReadFromJsonAsync<List<EmployeeReadDto>>() ?? new();
        return _mapper.Map<List<EmployeeViewModel>>(readDtos);
    }

    public async Task<EmployeeViewModel?> GetByIdAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/employees/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo obtener el empleado.");

        var readDto = await response.Content.ReadFromJsonAsync<EmployeeReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al obtener el empleado.");
        }

        return _mapper.Map<EmployeeViewModel>(readDto);
    }

    public async Task<EmployeeViewModel?> CreateAsync(EmployeeViewModel viewModel)
    {
        var createDto = _mapper.Map<EmployeeCreateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/employees";
        var response = await _httpClient.PostAsJsonAsync(url, createDto);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo crear el empleado.");

        var readDto = await response.Content.ReadFromJsonAsync<EmployeeReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al crear el empleado.");
        }

        return _mapper.Map<EmployeeViewModel>(readDto);
    }

    public async Task<EmployeeViewModel?> UpdateAsync(EmployeeViewModel viewModel)
    {
        var updateDto = _mapper.Map<EmployeeUpdateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/employees/{viewModel.Id}";
        var response = await _httpClient.PutAsJsonAsync(url, updateDto);

        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo actualizar el empleado.");

        var readDto = await response.Content.ReadFromJsonAsync<EmployeeReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al actualizar el empleado.");
        }

        return _mapper.Map<EmployeeViewModel>(readDto);
    }

    public async Task DeleteAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/employees/{id}";
        var response = await _httpClient.DeleteAsync(url);
        await ApiErrorResponseHandler.EnsureSuccessAsync(response, "No se pudo eliminar el empleado.");
    }
}
