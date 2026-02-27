using Contracts.Dtos.Authentication;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public AuthenticationController(
        IUnitOfWork unitOfWork,
        IUserService userService
    )
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var user = await _unitOfWork.IdentityService.FindByEmail(dto.Email);

        if (user == null)
        {
            return Unauthorized("Mail inválido.");
        }

        if (!await _unitOfWork.IdentityService.PasswordIsValid(user, dto.Password))
        {
            return Unauthorized("Contraseña inválida.");
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
            return Unauthorized("Mail de usuario en uso.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var organization = new Organization
            {
                Active = true,
                Name = dto.OrganizationName,
                Email = dto.OrganizationEmail,
                PricingPlanId = (int)Domain.Enums.PricingPlan.Free
            };

            organization = await _unitOfWork.OrganizationRepository.AddAsync(organization);

            var user = new AppUser
            {
                UserName = dto.UserEmail,
                Email = dto.UserEmail,
                NormalizedEmail = dto.UserEmail,
                EmailConfirmed = true,
                OrganizationId = organization.Id
            };

            var result = await _unitOfWork.IdentityService.CreateUser(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                await _unitOfWork.RollbackAsync();
                return BadRequest(new { errors });
            }

            await _unitOfWork.IdentityService.AddToRole(user, Domain.Enums.AppRole.Admin);
            await _unitOfWork.CommitAsync();
            var token = await _userService.GenerateTokenAsync(user);

            return Ok(
                new RegisterResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    OrganizationId = organization.Id,
                    OrganizationName = organization.Name,
                    OrganizationEmail = organization.Email,
                    UserId = user.Id,
                    UserEmail = user.Email
                }
            );
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return StatusCode(500, ex.Message);
        }
    }
}
