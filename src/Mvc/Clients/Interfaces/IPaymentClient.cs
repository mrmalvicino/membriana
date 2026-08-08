using Mvc.ViewModels;

namespace Mvc.Clients.Interfaces;

public interface IPaymentClient
{
    Task<List<PaymentViewModel>> GetAllAsync(int organizationId);
    Task<List<PaymentViewModel>> GetAllByMemberIdAsync(int memberId);
    Task<List<PaymentViewModel>> GetAllForLoggedUser();
    Task<PaymentViewModel?> GetByIdForLoggedUserAsync(int id);
    Task<PaymentViewModel?> GetByIdAsync(int id);
    Task<PaymentViewModel?> CreateAsync(PaymentViewModel payment);
    Task<PaymentViewModel?> UpdateAsync(PaymentViewModel payment);
    Task DeleteAsync(int id);
    Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month);
}
