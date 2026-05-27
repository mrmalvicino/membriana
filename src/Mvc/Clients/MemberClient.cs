using AutoMapper;
using Contracts.Dtos.Member;
using Mvc.Areas.Admin.ViewModels;
using Mvc.Clients.Helpers;
using Mvc.Clients.Interfaces;

namespace Mvc.Clients;

public class MemberClient : IMemberClient
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public MemberClient(
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
        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener la lista de socios.");
        var readDtos = await response.Content.ReadFromJsonAsync<List<MemberReadDto>>() ?? new();
        return _mapper.Map<List<MemberViewModel>>(readDtos);
    }

    public async Task<MemberViewModel?> GetByIdAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/members/{id}";
        var response = await _httpClient.GetAsync(url);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo obtener el socio.");

        var readDto = await response.Content.ReadFromJsonAsync<MemberReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al obtener el socio.");
        }

        return _mapper.Map<MemberViewModel>(readDto);
    }

    public async Task<MemberViewModel?> CreateAsync(MemberViewModel viewModel)
    {
        var createDto = _mapper.Map<MemberCreateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/members";
        var response = await _httpClient.PostAsJsonAsync(url, createDto);

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo crear el socio.");

        var readDto = await response.Content.ReadFromJsonAsync<MemberReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al crear el socio.");
        }

        return _mapper.Map<MemberViewModel>(readDto);
    }

    public async Task<MemberViewModel?> UpdateAsync(MemberViewModel viewModel)
    {
        var updateDto = _mapper.Map<MemberUpdateDto>(viewModel);
        var url = $"{_apiBaseUrl}api/members/{viewModel.Id}";
        var response = await _httpClient.PutAsJsonAsync(url, updateDto);

        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo actualizar el socio.");

        var readDto = await response.Content.ReadFromJsonAsync<MemberReadDto>();

        if (readDto == null)
        {
            throw new InvalidOperationException("La API devolvió una respuesta vacía al actualizar el socio.");
        }

        return _mapper.Map<MemberViewModel>(readDto);
    }

    public async Task DeleteAsync(int id)
    {
        var url = $"{_apiBaseUrl}api/members/{id}";
        var response = await _httpClient.DeleteAsync(url);
        await ApiErrorMessageReader.EnsureSuccessAsync(response, "No se pudo eliminar el socio.");
    }
}
