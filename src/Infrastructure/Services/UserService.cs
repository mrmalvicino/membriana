using Application.Repositories;
using Application.Services;
using Contracts.Dtos.Authentication;
using Contracts.Dtos.User;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

/// <summary>
/// Servicio para gestionar el acceso a información del usuario en sesión
/// y a la generación de tokens de autenticación.
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    /// <summary>
    /// Constructor principal.
    /// </summary>
    public UserService(
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IOrganizationRepository organizationRepository,
        IAppUserRepository appUserRepository,
        IUnitOfWork unitOfWork,
        IImageService imageService
    )
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _organizationRepository = organizationRepository;
        _appUserRepository = appUserRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    /// <summary>
    /// Obtiene un DTO con información relevante del usuario autenticado.
    /// </summary>
    public async Task<LoggedUserContextDto> GetLoggedUserContextAsync()
    {
        AppUser appUser = await GetLoggedUserHydratedAsync();
        Organization organization = await GetLoggedOrganizationAsync();

        LoggedUserContextDto loggedUserContextDto = new();

        loggedUserContextDto.UserId = appUser.Id;
        loggedUserContextDto.UserEmail = appUser.Email ?? "";
        loggedUserContextDto.OrganizationId = organization.Id;
        loggedUserContextDto.OrganizationName = organization.Name;
        loggedUserContextDto.MemberId = appUser.Member?.Id;
        loggedUserContextDto.EmployeeId = appUser.Employee?.Id;

        return loggedUserContextDto;
    }

    /// <summary>
    /// Obtiene la organización a la que pertenece el usuario autenticado en el contexto HTTP actual.
    /// </summary>
    public async Task<Organization> GetLoggedOrganizationAsync()
    {
        int organizationId = await GetOrganizationIdAsync();
        var organization = await _organizationRepository.GetByIdAsync(organizationId);

        if (organization == null)
        {
            throw new InvalidOperationException("No se encontró la organización del usuario.");
        }

        return organization;
    }

    /// <summary>
    /// Obtiene el usuario actualmente autenticado en el contexto HTTP actual.
    /// </summary>
    /// <remarks>
    /// Cuando un usuario autenticado realiza una petición HTTP al servidor, el
    /// middleware de autenticación valida el token JWT (extraído desde la cookie
    /// o header Authorization) y, si es válido, construye un objeto ClaimsPrincipal
    /// con los datos del usuario mediante <see cref="IHttpContextAccessor"/> y
    /// utiliza <see cref="UserManager{TUser}"/> para recuperar desde la base de datos
    /// la entidad <see cref="AppUser"/> completa.
    /// </remarks>
    public async Task<AppUser> GetLoggedUserAsync()
    {
        var appUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

        if (appUser == null)
        {
            throw new UnauthorizedAccessException("No hay usuario en sesión.");
        }

        return appUser;
    }

    /// <summary>
    /// Obtiene el usuario actualmente autenticado en el contexto HTTP actual.
    /// </summary>
    /// <remarks>
    /// A diferencia de <see cref="GetLoggedUserAsync"/>, este método devuelve el usuario con todos
    /// los atributos hidratados para escenarios de eager loading.
    /// </remarks>
    public async Task<AppUser> GetLoggedUserHydratedAsync()
    {
        AppUser appUser = await GetLoggedUserAsync();

        var appUserHydrated = await _appUserRepository.GetByIdAsync(
            appUser.Id,
            appUser.OrganizationId
        );

        if (appUserHydrated == null)
        {
            throw new InvalidOperationException("No se encontró el usuario autenticado.");
        }

        return appUserHydrated;
    }

    /// <summary>
    /// Obtiene el ID de la organización (tenant) del usuario autenticado en el request actual.
    /// </summary>
    /// <remarks>
    /// Este método es utilizado por controladores y servicios de aplicación para
    /// validar y reforzar el aislamiento multi-tenant, evitando que un usuario
    /// opere sobre datos de una organización distinta a la propia.
    /// </remarks>
    public async Task<int> GetOrganizationIdAsync()
    {
        var user = await GetLoggedUserAsync();

        if (user.OrganizationId == 0)
        {
            throw new InvalidOperationException("No se encontró la organización del usuario.");
        }

        return user.OrganizationId;
    }

    /// <summary>
    /// Genera un token JWT firmado para el usuario indicado, incluyendo claims de
    /// identidad, roles y tenant.
    /// </summary>
    /// <remarks>
    /// Este método suele ser invocado desde flujos de login o emisión de credenciales,
    /// mientras que el consumo del token queda a cargo de los middlewares de
    /// autenticación configurados en la aplicación.
    /// </remarks>
    public async Task<JwtSecurityToken> GenerateTokenAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim("OrganizationId", user.OrganizationId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
            claims: authClaims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    /// <summary>
	/// Obtiene los datos personales del usuario autenticado.
	/// </summary>
	public async Task<UserProfileReadDto> GetUserProfileAsync()
    {
        AppUser appUser = await GetLoggedUserHydratedAsync();
        Person? person = GetAssociatedPerson(appUser);

        return MapProfile(appUser, person);
    }

    /// <summary>
    /// Actualiza los datos personales del usuario autenticado.
    /// </summary>
    public async Task<UserProfileReadDto> UpdateUserProfileAsync(
        UserProfileUpdateDto profileUpdateDto
    )
    {
        if (string.IsNullOrWhiteSpace(profileUpdateDto.UserEmail))
        {
            throw new ArgumentException("El email es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(profileUpdateDto.Name))
        {
            throw new ArgumentException("El nombre es obligatorio.");
        }

        if (!profileUpdateDto.BirthDate.HasValue)
        {
            throw new ArgumentException("La fecha de nacimiento es obligatoria.");
        }

        if (profileUpdateDto.BirthDate.Value.Date > DateTime.Today)
        {
            throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
        }

        AppUser appUser = await GetLoggedUserHydratedAsync();

        Person? person = GetAssociatedPerson(appUser);

        if (person == null)
        {
            throw new InvalidOperationException(
                "El usuario no tiene una persona asociada para actualizar."
            );
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            person.Name = profileUpdateDto.Name.Trim();
            person.Email = profileUpdateDto.UserEmail.Trim();

            person.Phone =
                string.IsNullOrWhiteSpace(profileUpdateDto.Phone) ?
                null : profileUpdateDto.Phone.Trim();

            person.Dni =
                string.IsNullOrWhiteSpace(profileUpdateDto.Dni) ?
                null : profileUpdateDto.Dni.Trim();

            person.BirthDate = profileUpdateDto.BirthDate!.Value.Date;

            _imageService.ApplyProfileImage(person, profileUpdateDto.ProfileImageUrl);

            if (person is Employee employee)
            {
                await _unitOfWork.EmployeeRepository.UpdateAsync(employee);
            }
            else if (person is Member member)
            {
                await _unitOfWork.MemberRepository.UpdateAsync(member);
            }

            await _unitOfWork.CommitAsync();

			return MapProfile(appUser, person);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

	private static Person? GetAssociatedPerson(AppUser appUser)
	{
		if (appUser.Employee != null && appUser.Member != null)
		{
			throw new InvalidOperationException(
				"El usuario tiene más de una persona asociada."
			);
		}

		if (appUser.Employee != null)
		{
			if (appUser.Employee.OrganizationId != appUser.OrganizationId)
			{
				throw new KeyNotFoundException("No se encontró el perfil del usuario.");
			}

			return appUser.Employee;
		}

		if (appUser.Member != null)
		{
			if (appUser.Member.OrganizationId != appUser.OrganizationId)
			{
				throw new KeyNotFoundException("No se encontró el perfil del usuario.");
			}

			return appUser.Member;
		}

		return null;
	}

	private static UserProfileReadDto MapProfile(AppUser appUser, Person? person)
	{
		return new UserProfileReadDto
		{
			Name = person?.Name,
			Email = person?.Email ?? appUser.Email ?? string.Empty,
			Phone = person?.Phone,
			Dni = person?.Dni,
			BirthDate = person?.BirthDate,
			ProfileImageUrl = person?.ProfileImage?.Url,
			HasAssociatedPerson = person != null
		};
	}
}
