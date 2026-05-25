using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Clients.Interfaces;

public interface IPaymentClient
{
    Task<List<PaymentViewModel>> GetAllAsync(int organizationId);
    Task<PaymentViewModel?> GetByIdAsync(int id);
    Task<PaymentViewModel?> CreateAsync(PaymentViewModel payment);
    Task<PaymentViewModel?> UpdateAsync(PaymentViewModel payment);
    Task<bool> DeleteAsync(int id);
    Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month);
}
