using Contracts.Dtos.Authentication;
using Contracts.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Mvc.Authentication;
using Mvc.Clients.Interfaces;
using Mvc.ViewModels;

namespace Mvc.Controllers;

[JwtAuthorizationFilter]
public class ProfileController : Controller
{
	private readonly IUserClient _userClient;

	public ProfileController(IUserClient userClient)
	{
		_userClient = userClient;
	}

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		try
		{
			LoggedUserContextDto loggedUserContext = await _userClient.GetLoggedUserContextAsync();
			UserProfileReadDto profile = await _userClient.GetUserProfileAsync();

            var profileViewModel = new ProfileViewModel
            {
                Name = profile.Name,
                UserEmail = profile.Email,
                Phone = profile.Phone,
                BirthDate = profile.BirthDate,
                Dni = profile.Dni,
                ProfileImageUrl = profile.ProfileImageUrl,
                Role = loggedUserContext.MemberId.HasValue ? "Member" : string.Empty,
                HasAssociatedPerson = profile.HasAssociatedPerson
            };

            return View(profileViewModel);
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, ex.Message);
			return View(new ProfileViewModel { LoadFailed = true });
		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(ProfileViewModel profileViewModel)
	{
        try
        {
            var loggedUserContext = await _userClient.GetLoggedUserContextAsync();
            profileViewModel.Role = loggedUserContext.MemberId.HasValue ? "Member" : string.Empty;

            profileViewModel.HasAssociatedPerson =
				loggedUserContext.MemberId.HasValue || loggedUserContext.EmployeeId.HasValue;
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

		if (!ModelState.IsValid)
		{
			return View(nameof(Index), profileViewModel);
		}

		try
		{
			await _userClient.UpdateUserProfileAsync(
				new UserProfileUpdateDto
				{
					Name = profileViewModel.Name,
					UserEmail = profileViewModel.UserEmail,
					Phone = profileViewModel.Phone,
					BirthDate = profileViewModel.BirthDate,
					Dni = profileViewModel.Dni,
					ProfileImageUrl = profileViewModel.ProfileImageUrl
				}
			);

			TempData["ProfileSuccess"] = "Tus datos se actualizaron correctamente.";
			return RedirectToAction(nameof(Index));
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, ex.Message);
			return View(nameof(Index), profileViewModel);
		}
	}
}
