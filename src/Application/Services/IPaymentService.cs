using Domain.Entities;

namespace Application.Services;

public interface IPaymentService
{
    Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month);
    Task<List<Payment>> GetAllByMemberIdAsync(int memberId);
}
