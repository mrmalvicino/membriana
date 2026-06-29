using Application.Repositories;
using Application.Services;
using Contracts.Dtos.User;
using Domain.Entities;

namespace Infrastructure.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserManagementService(
        IUnitOfWork unitOfWork
    )
    {
        _unitOfWork = unitOfWork;
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
}
