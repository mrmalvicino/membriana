using Domain.Entities;

namespace Application.Repositories;

/// <summary>
/// Interfaz del repositorio para la entidad Payment.
/// </summary>
public interface IPaymentRepository : IBaseRepository<Payment>
{
    Task<decimal> GetMonthlyIncomeAsync(int organizationId, int year, int month);
}
