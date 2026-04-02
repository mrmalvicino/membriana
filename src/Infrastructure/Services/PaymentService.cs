using Application.Repositories;
using Application.Services;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month)
    {
        return await _paymentRepository.GetMonthlyIncomeAsync(organizationId, year, month);
    }
}
