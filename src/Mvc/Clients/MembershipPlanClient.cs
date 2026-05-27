using AutoMapper;
using Contracts.Dtos.MembershipPlan;
using Mvc.Clients.Helpers;
using Mvc.Clients.Interfaces;
using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Clients;

public class MembershipPlanClient : IMembershipPlanClient
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public MembershipPlanClient(
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
        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener la lista de planes de membresía.");
        var dtos = await response.Content.ReadFromJsonAsync<List<MembershipPlanReadDto>>() ?? new();
        return _mapper.Map<List<MembershipPlanViewModel>>(dtos);
    }

    public async Task<MembershipPlanViewModel?> GetByIdAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/membershipplans/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener el plan de membresía.");

        var dto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();

        if (dto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al obtener el plan de membresía.");
        }

        return _mapper.Map<MembershipPlanViewModel>(dto);
    }

    public async Task<MembershipPlanViewModel?> CreateAsync(MembershipPlanViewModel viewModel)
    {
        var createDto = _mapper.Map<MembershipPlanCreateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/membershipplans";
        var response = await _httpClient.PostAsJsonAsync(url, createDto);

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo crear el plan de membresía.");

        var readDto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al crear el plan de membresía.");
        }

        return _mapper.Map<MembershipPlanViewModel>(readDto);
    }

    public async Task<MembershipPlanViewModel?> UpdateAsync(MembershipPlanViewModel viewModel)
    {
        var updateDto = _mapper.Map<MembershipPlanUpdateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/membershipplans/{viewModel.Id}";
        var response = await _httpClient.PutAsJsonAsync(url, updateDto);

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo actualizar el plan de membresía.");

        var readDto = await response.Content.ReadFromJsonAsync<MembershipPlanReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al actualizar el plan de membresía.");
        }

        return _mapper.Map<MembershipPlanViewModel>(readDto);
    }

    public async Task DeleteAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/membershipplans/{id}";
        var response = await _httpClient.DeleteAsync(url);

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo eliminar el plan de membresía.");
    }
}
