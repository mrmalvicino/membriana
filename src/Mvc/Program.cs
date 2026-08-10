using Mvc.Authentication;
using Mvc.Clients;
using Mvc.Clients.Interfaces;
using Mvc.ModelBinders;
using Microsoft.AspNetCore.Localization;
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
        builder.Services.AddHttpClient<IAuthenticationClient, AuthenticationClient>();
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

        builder.Services.AddHttpClient<IEmployeeClient, EmployeeClient>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IMemberClient, MemberClient>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IMembershipPlanClient, MembershipPlanClient>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IPaymentClient, PaymentClient>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IMemberStatusClient, MemberStatusClient>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        builder.Services.AddHttpClient<IUserClient, UserClient>()
            .AddHttpMessageHandler<JwtCookieHandler>();

        #endregion

        #region MVC

        var supportedCultures = new[] { new CultureInfo("es-AR") };

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("es-AR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        builder.Services.AddControllersWithViews(options =>
        {
            options.ModelBinderProviders.Insert(0, new FlexibleDecimalModelBinderProvider());
        });
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
        app.UseRequestLocalization();
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
