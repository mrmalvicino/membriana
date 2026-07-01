using Application.Repositories;
using Application.Services;
using Contracts.Dtos.Authentication;
using Contracts.Dtos.User;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountService _accountService;

    public UserManagementService(
        IUnitOfWork unitOfWork,
        IAccountService accountService
    )
    {
        _unitOfWork = unitOfWork;
        _accountService = accountService;
    }

    public async Task<List<UserReadDto>> GetAllAsync(int organizationId)
    {
        var users = await _unitOfWork.AppUserRepository.GetAllByOrganizationAsync(organizationId);
        var dtos = new List<UserReadDto>(users.Count);

        foreach (var user in users)
        {
            dtos.Add(await MapToReadDtoAsync(user));
        }

        return dtos;
    }

    public async Task<UserReadDto?> GetByIdAsync(string id, int organizationId)
    {
        var user = await _unitOfWork.AppUserRepository.GetByIdAsync(id, organizationId);

        if (user == null)
        {
            return null;
        }

        return await MapToReadDtoAsync(user);
    }

    public async Task<List<UserCandidateDto>> GetEligibleMembersAsync(int organizationId)
    {
        var members = await _unitOfWork.MemberRepository.GetAllAsync(organizationId);

        return members
            .Where(member => string.IsNullOrWhiteSpace(member.UserId))
            .OrderBy(member => member.Name)
            .Select(
                member => new UserCandidateDto
                {
                    Id = member.Id,
                    ReferenceCode = member.ReferenceCode,
                    Name = member.Name,
                    Email = member.Email
                }
            )
            .ToList();
    }

    public async Task<List<UserCandidateDto>> GetEligibleEmployeesAsync(int organizationId)
    {
        var employees = await _unitOfWork.EmployeeRepository.GetAllAsync(organizationId);

        return employees
            .Where(employee => string.IsNullOrWhiteSpace(employee.UserId))
            .OrderBy(employee => employee.Name)
            .Select(
                employee => new UserCandidateDto
                {
                    Id = employee.Id,
                    ReferenceCode = employee.ReferenceCode,
                    Name = employee.Name,
                    Email = employee.Email
                }
            )
            .ToList();
    }

    public async Task<RegisterResponseDto> CreateUserForMemberAsync(
        int memberId,
        int organizationId
    )
    {
        var member = await _unitOfWork.MemberRepository.GetByIdAsync(memberId);

        if (member == null || member.OrganizationId != organizationId)
        {
            throw new KeyNotFoundException("El socio no existe.");
        }

        if (!string.IsNullOrWhiteSpace(member.UserId))
        {
            throw new InvalidOperationException("El socio ya tiene un usuario asociado.");
        }

        return await CreateLinkedUserAsync(
            email: member.Email,
            organizationId: organizationId,
            role: AppRole.Member,
            onLinked: userId => member.UserId = userId,
            successMessage: "Se creó el usuario para el socio y se envió el correo de confirmación."
        );
    }

    public async Task<RegisterResponseDto> CreateUserForEmployeeAsync(
        int employeeId,
        int organizationId
    )
    {
        var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);

        if (employee == null || employee.OrganizationId != organizationId)
        {
            throw new KeyNotFoundException("El empleado no existe.");
        }

        if (!string.IsNullOrWhiteSpace(employee.UserId))
        {
            throw new InvalidOperationException("El empleado ya tiene un usuario asociado.");
        }

        return await CreateLinkedUserAsync(
            email: employee.Email,
            organizationId: organizationId,
            role: AppRole.Employee,
            onLinked: userId => employee.UserId = userId,
            successMessage: "Se creó el usuario para el empleado y se envió el correo de confirmación."
        );
    }

    public async Task DeleteAsync(string id, int organizationId, string loggedUserId)
    {
        var user = await _unitOfWork.AppUserRepository.GetByIdAsync(id, organizationId);

        if (user == null)
        {
            throw new KeyNotFoundException("El usuario no existe.");
        }

        if (string.Equals(user.Id, loggedUserId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("No podés eliminar tu propio usuario.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            if (user.Employee != null)
            {
                user.Employee.UserId = null;
            }

            if (user.Member != null)
            {
                user.Member.UserId = null;
            }

            var result = await _unitOfWork.IdentityService.DeleteUser(user);

            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    string.Join(", ", result.Errors.Select(error => error.Description))
                );
            }

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task<UserReadDto> MapToReadDtoAsync(AppUser user)
    {
        var roles = await _unitOfWork.IdentityService.GetRoles(user);

        return new UserReadDto
        {
            Id = user.Id,
            ReferenceCode = user.ReferenceCode,
            Email = user.Email ?? string.Empty,
            EmailConfirmed = user.EmailConfirmed,
            Role = roles.FirstOrDefault() ?? string.Empty,
            LinkedPersonName = user.Employee?.Name ?? user.Member?.Name,
            LinkedPersonType = user.Employee != null ? "Empleado" : user.Member != null ? "Socio" : null
        };
    }

    private async Task<RegisterResponseDto> CreateLinkedUserAsync(
        string email,
        int organizationId,
        AppRole role,
        Action<string> onLinked,
        string successMessage
    )
    {
        if (await _unitOfWork.IdentityService.FindByEmail(email) != null)
        {
            throw new InvalidOperationException("Mail de usuario en uso.");
        }

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = email.ToUpperInvariant(),
            EmailConfirmed = false,
            OrganizationId = organizationId,
        };

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var result = await _unitOfWork.IdentityService.CreateUser(user, GenerateTemporaryPassword());

            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    string.Join(", ", result.Errors.Select(error => error.Description))
                );
            }

            var roleResult = await _unitOfWork.IdentityService.AddToRole(user, role);

            if (!roleResult.Succeeded)
            {
                throw new ApplicationException(
                    string.Join(", ", roleResult.Errors.Select(error => error.Description))
                );
            }

            onLinked(user.Id);

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        await _accountService.SendConfirmationAsync(user);

        return new RegisterResponseDto
        {
            Message = successMessage,
            UserEmail = user.Email ?? email
        };
    }

    private static string GenerateTemporaryPassword()
    {
        return "Password123-";
        // TODO: Enviar por mail
        return $"Tmp!{Guid.NewGuid():N}aA1";
    }
}
