using Application.Repositories;
using Application.Services;
using Contracts.Dtos.Authentication;
using Contracts.Dtos.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IAccountService _accountService;

    public AuthenticationController(
        IUnitOfWork unitOfWork,
        IUserService userService,
        IAccountService accountService
    )
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var user = await _unitOfWork.IdentityService.FindByEmail(dto.Email);

        if (user == null)
        {
            return Unauthorized(CreateErrorResponse("Credenciales inválidas."));
        }

        if (!await _unitOfWork.IdentityService.PasswordIsValid(user, dto.Password))
        {
            return Unauthorized(CreateErrorResponse("Credenciales inválidas."));
        }

        if (!user.EmailConfirmed)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                CreateErrorResponse("Cuenta no confirmada.")
            );
        }

        var token = await _userService.GenerateTokenAsync(user);

        return Ok(
            new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            }
        );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        if (await _unitOfWork.IdentityService.FindByEmail(dto.UserEmail) != null)
        {
            return Conflict(CreateErrorResponse("Mail de usuario en uso."));
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Crear organización
            var organization = new Organization
            {
                Active = true,
                Name = dto.OrganizationName,
                Email = dto.OrganizationEmail,
                PricingPlanId = (int)Domain.Enums.PricingPlan.Free,
            };

            organization = await _unitOfWork.OrganizationRepository.AddAsync(organization);

            // Crear usuario con rol Admin
            var user = new AppUser
            {
                UserName = dto.UserEmail,
                Email = dto.UserEmail,
                NormalizedEmail = dto.UserEmail,
                EmailConfirmed = false,
                OrganizationId = organization.Id,
            };

            var result = await _unitOfWork.IdentityService.CreateUser(user, dto.Password);

            if (!result.Succeeded)
            {
                await _unitOfWork.RollbackAsync();
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(CreateErrorResponse(errors));
            }

            await _unitOfWork.IdentityService.AddToRole(user, Domain.Enums.AppRole.Admin);

            // Crear empleado asociado al usuario
            var employee = new Employee
            {
                Name = dto.UserName,
                Email = dto.UserEmail,
                OrganizationId = organization.Id,
                UserId = user.Id,
            };

            await _unitOfWork.EmployeeRepository.AddAsync(employee);

            // Confirmar transacción y enviar mail de confirmación
            await _unitOfWork.CommitAsync();
            await _accountService.SendConfirmationAsync(user);

            return Ok(
                new RegisterResponseDto
                {
                    Message = "¡Cuenta creada! Revisá tu correo para confirmar el email.",
                    UserEmail = user.Email!
                }
            );
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                CreateErrorResponse("Ocurrió un error inesperado al registrar la cuenta.")
            );
        }
    }

    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(
                new ResendConfirmationResponseDto
                {
                    Message = "Email inválido."
                }
            );
        }

        var user = await _unitOfWork.IdentityService.FindByEmail(dto.Email);

        if (user == null)
        {
            return Ok(
                new ResendConfirmationResponseDto
                {
                    Message = "Si el email existe, se enviará una confirmación."
                }
            );
        }

        if (user.EmailConfirmed)
        {
            return Ok(
                new ResendConfirmationResponseDto
                {
                    Message = "El email ya está confirmado."
                }
            );
        }

        await _accountService.SendConfirmationAsync(user);

        return Ok(
            new ResendConfirmationResponseDto
            {
                Message = "Se envió el correo de confirmación."
            }
        );
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Token))
        {
            return BadRequest(
                new ConfirmEmailResponseDto
                {
                    Message = "Parámetros inválidos."
                }
            );
        }

        // Buscar usuario
        var user = await _unitOfWork.IdentityService.FindById(dto.UserId);

        if (user == null)
        {
            return BadRequest(
                new ConfirmEmailResponseDto
                {
                    Message = "Usuario no encontrado."
                }
            );
        }

        if (user.EmailConfirmed)
        {
            return Ok(
                new ConfirmEmailResponseDto
                {
                    Message = "El email ya fue confirmado."
                }
            );
        }

        // Decodificar token
        string decodedToken;

        try
        {
            var decodedBytes = WebEncoders.Base64UrlDecode(dto.Token);
            decodedToken = Encoding.UTF8.GetString(decodedBytes);
        }
        catch
        {
            return BadRequest(
                new ConfirmEmailResponseDto
                {
                    Message = "Token inválido."
                }
            );
        }

        // Confirmar email
        var result = await _unitOfWork.IdentityService.ConfirmEmail(user, decodedToken);

        if (!result.Succeeded)
        {
            return BadRequest(
                new ConfirmEmailResponseDto
                {
                    Message = "No se pudo confirmar el email."
                }
            );
        }

        return Ok(
            new ConfirmEmailResponseDto
            {
                Message = "Email confirmado correctamente."
            }
        );
    }

    private static ErrorResponseDto CreateErrorResponse(params string[] errors)
    {
        return new ErrorResponseDto
        {
            Errors = errors.ToList()
        };
    }
}
