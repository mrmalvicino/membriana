namespace Mvc.Services.Api.Interfaces
{
    public interface IUserApiService
    {
        Task<int> GetOrganizationIdAsync();
    }
}
