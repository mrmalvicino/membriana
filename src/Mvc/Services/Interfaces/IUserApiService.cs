namespace Mvc.Services.Interfaces
{
    public interface IUserApiService
    {
        Task<int> GetOrganizationIdAsync();
    }
}
