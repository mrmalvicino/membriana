using Mvc.Services.Api;
using Mvc.Services.Api.Interfaces;
using Mvc.Services.Handlers;
using Mvc.Services.Utilities;
using Mvc.Services.Utilities.Interfaces;

namespace Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient<IAuthenticationApiService, AuthenticationApiService>();
            builder.Services.AddHttpClient<IMemberApiService, MemberApiService>();
            builder.Services.AddHttpClient<IMembershipPlanApiService, MembershipPlanApiService>();
            builder.Services.AddHttpClient<IUserApiService, UserApiService>();
            builder.Services.AddScoped<ICookieService, CookieService>();

            builder.Services.ConfigureApplicationCookie(
                options =>
                {
                    options.LoginPath = "/Authentication/Login";
                    options.AccessDeniedPath = "/Authentication/AccessDenied";
                }
            );

            builder.Services.AddTransient<JwtCookieHandler>();

            builder.Services.AddHttpClient<IMemberApiService, MemberApiService>()
                .AddHttpMessageHandler<JwtCookieHandler>();

            builder.Services.AddHttpClient<IMembershipPlanApiService, MembershipPlanApiService>()
                .AddHttpMessageHandler<JwtCookieHandler>();
            
            builder.Services.AddHttpClient<IUserApiService, UserApiService>()
                .AddHttpMessageHandler<JwtCookieHandler>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
