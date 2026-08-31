namespace Mvc.ViewModels;

public class UserMenuViewModel
{
	public string Name { get; set; } = "Usuario";
	public string Email { get; set; } = string.Empty;
	public string? ProfileImageUrl { get; set; }
	public bool LoadFailed { get; set; }

	public string Initials
	{
		get
		{
			var words = Name.Split(
				' ',
				StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
			);

			if (words.Length == 0)
			{
				return "U";
			}

			return string.Concat(words.Take(2).Select(word => char.ToUpperInvariant(word[0])));
		}
	}
}
