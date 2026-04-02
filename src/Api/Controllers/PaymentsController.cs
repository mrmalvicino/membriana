using Api.Filters;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Contracts.Dtos.Payment;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Controlador para la gestión de pagos.
/// </summary>
[Authorize(Policy = "Employee")]
public class PaymentsController : BaseController<
    Payment,
    IPaymentRepository,
    PaymentReadDto,
    PaymentCreateDto,
    PaymentUpdateDto
>
{
    private readonly IPaymentService _paymentService;

    /// <summary>
    /// Constructor principal.
    /// </summary>
    public PaymentsController(
        IPaymentRepository repository,
        IPaymentService paymentService,
        IUserService userService,
        IMapper mapper
    ) : base(repository, userService, mapper)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Endpoint que obtiene un pago por su ID.
    /// </summary>
    [ServiceFilter(typeof(TenancyRouteFilter<Payment, IPaymentRepository>))]
    public override async Task<ActionResult<PaymentReadDto>> Get(int id)
    {
        return await base.Get(id);
    }

    /// <summary>
    /// Endpoint que modifica un pago existente.
    /// </summary>
    [ServiceFilter(typeof(TenancyRouteFilter<Payment, IPaymentRepository>))]
    public override async Task<ActionResult<PaymentReadDto>> Update(
        int id,
        [FromBody] PaymentUpdateDto updateDto
    )
    {
        return await base.Update(id, updateDto);
    }

    /// <summary>
    /// Endpoint que elimina un pago.
    /// </summary>
    [ServiceFilter(typeof(TenancyRouteFilter<Payment, IPaymentRepository>))]
    public override async Task<IActionResult> Delete(int id)
    {
        return await base.Delete(id);
    }

    /// <summary>
    /// Endpoint para obtener los pagos que una organización recibió durante un mes de cierto año.
    /// </summary>
    [HttpGet("get-monthly-income")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ServiceFilter(typeof(TenancyQueryFilter))]
    public async Task<ActionResult<MonthlyIncomeResponseDto>> GetMonthlyIncome(
        [FromQuery] int organizationId,
        [FromQuery] int year,
        [FromQuery] int month
    )
    {
        var payments = await _paymentService.GetMonthlyIncomeAsync(organizationId, year, month);
        return Ok(new MonthlyIncomeResponseDto(payments));
    }
}
