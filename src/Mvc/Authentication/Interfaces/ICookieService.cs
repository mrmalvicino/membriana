namespace Mvc.Authentication.Interfaces;

public interface ICookieService
{
    public void SetJwtCookie(string token);
    public void DeleteJwtCookie();
}
