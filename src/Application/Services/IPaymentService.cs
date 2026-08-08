using Domain.Entities;

namespace Application.Services;

public interface IPaymentService
{
    Task<Payment?> GetByIdAsync(int paymentId);
    Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month);
    Task<List<Payment>> GetAllByMemberIdAsync(int memberId);
}
