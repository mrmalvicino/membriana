using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Services.Api.Interfaces;

public interface IPaymentApiService
{
    Task<List<PaymentViewModel>> GetAllAsync(int organizationId);
    Task<PaymentViewModel?> GetByIdAsync(int id);
    Task<PaymentViewModel?> CreateAsync(PaymentViewModel payment);
    Task<PaymentViewModel?> UpdateAsync(PaymentViewModel payment);
    Task<bool> DeleteAsync(int id);
}