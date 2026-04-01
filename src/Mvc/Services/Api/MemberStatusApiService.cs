using Contracts.Dtos.MemberStatus;
using Contracts.Enums;
using Mvc.Services.Api.Interfaces;

namespace Mvc.Services.Api;

public class MemberStatusApiService : IMemberStatusApiService
{
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;

    public MemberStatusApiService(
        IConfiguration configuration,
        HttpClient httpClient
    )
    {
        _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl");
        _httpClient = httpClient;
    }

    public async Task<int> CountMembersWithStatusAsync(
        int organizationId,
        int year,
        int month,
        MemberStatus status
    )
    {
        var url =
            $"{_apiBaseUrl}api/memberstatuses?organizationId={organizationId}&year={year}&month={month}&status={status}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var amounts = await response.Content.ReadFromJsonAsync<List<AmountResponse>>();
        var amount = amounts?.FirstOrDefault();

        if (amount is null)
        {
            throw new InvalidOperationException($"Respuesta vacía de {url}");
        }

        return amount.Amount;
    }
}
