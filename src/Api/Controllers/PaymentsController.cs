using Api.Filters;
using Application.Dtos.Payment;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de pagos.
    /// </summary>
    [Authorize(Policy = "Member")]
    public class PaymentsController : BaseController<
        Payment,
        IPaymentRepository,
        PaymentReadDto,
        PaymentCreateDto,
        PaymentUpdateDto
    >
    {
        /// <summary>
        /// Constructor principal.
        /// </summary>
        public PaymentsController(
            IPaymentRepository repository,
            IUserService userService,
            IMapper mapper
        ) : base(repository, userService, mapper)
        {

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
    }
}
