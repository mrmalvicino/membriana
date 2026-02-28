using Mvc.Services.Api;
using Mvc.Services.Api.Interfaces;
using Mvc.Services.Handlers;
using Mvc.Services.Utilities;
using Mvc.Services.Utilities.Interfaces;
using System.Globalization;

namespace Mvc;

/// <summary>
/// Punto de entrada de la aplicación y bootstrapper del frontend MVC.
/// Configura el contenedor de dependencias (DI), middleware pipeline y rutas.
/// </summary>
public class Program
{
    /// <summary>
    /// Método principal que inicializa y ejecuta la aplicación web.
    /// </summary>
    /// <param name="args">Argumentos de línea de comandos.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        #region Authentication

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient<IAuthenticationApiService, AuthenticationApiService>();
        builder.Services.AddScoped<ICookieService, CookieService>();

        builder.Services.ConfigureApplicationCookie(
            options =>
            {
                options.LoginPath = "/Authentication/Login";
                options.AccessDeniedPath = "/Authentication/AccessDenied";
            }
        );

        #endregion

        #region HTTP Client

        builder.Services.AddTransient<JwtCookieHandler>();

        builder.Services.AddHttpClient<IEmployeeApiService, EmployeeApiService>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IMemberApiService, MemberApiService>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IMembershipPlanApiService, MembershipPlanApiService>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IPaymentApiService, PaymentApiService>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IMemberStatusApiService, MemberStatusApiService>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IUserApiService, UserApiService>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        #endregion

        #region MVC

        builder.Services.AddControllersWithViews();
        builder.Services.AddSession();

        #endregion

        var app = builder.Build();

        #region Middleware Pipeline

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAreaControllerRoute(
            name: "admin",
            areaName: "Admin",
            pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
        );

        app.MapAreaControllerRoute(
            name: "portal",
            areaName: "Portal",
            pattern: "Portal/{controller=Dashboard}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        #endregion

        #region Globalization

        var culture = new CultureInfo("es-AR");

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        #endregion

        app.Run();
    }
}
