namespace Mvc.Authentication;

public interface ICookieService
{
    public void SetJwtCookie(string token);
    public void DeleteJwtCookie();
}
