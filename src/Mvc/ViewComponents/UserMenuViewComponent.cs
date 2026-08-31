using Microsoft.AspNetCore.Mvc;
using Mvc.Clients.Interfaces;
using Mvc.ViewModels;

namespace Mvc.ViewComponents;

public class UserMenuViewComponent : ViewComponent
{
	private readonly IUserClient _userClient;

	public UserMenuViewComponent(IUserClient userClient)
	{
		_userClient = userClient;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		try
		{
			var profile = await _userClient.GetUserProfileAsync();

			return View(
				new UserMenuViewModel
				{
					Name = profile.Name ?? "Usuario",
					Email = profile.Email,
					ProfileImageUrl = profile.ProfileImageUrl
				}
			);
		}
		catch (Exception)
		{
			return View(new UserMenuViewModel { LoadFailed = true });
		}
	}
}
