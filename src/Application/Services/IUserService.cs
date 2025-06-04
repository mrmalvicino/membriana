namespace Application.Services
{
    public interface IUserService
    {
        Task<int> GetOrganizationIdAsync();
    }
}
