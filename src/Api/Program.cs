using Api.Filters;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;

namespace Api
{
    /// <summary>
    /// Punto de entrada de la aplicación y bootstrapper de la API.
    /// Configura el contenedor de dependencias (DI), middleware pipeline y endpoints.
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

            #region Configuration

            var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            #endregion

            #region Logging

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .WriteTo.Console()
                    .WriteTo.MSSqlServer(
                        connectionString: dbConnectionString,
                        sinkOptions: new MSSqlServerSinkOptions
                        {
                            TableName = "Logs",
                            AutoCreateSqlTable = true
                        });
            });

            #endregion

            #region Dependency Injection

            builder.Services.AddInfrastructure(dbConnectionString);
            builder.Services.AddScoped<TenancyQueryFilter>();
            builder.Services.AddScoped(typeof(TenancyRouteFilter<,>));

            #endregion

            #region Identity

            builder.Services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            #endregion

            #region Authentication - JWT Bearer

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],

                        ValidateAudience = true,
                        ValidAudience = jwtSettings["Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            #endregion

            #region Authorization - Policies

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireRole(Domain.Enums.AppRole.Admin.ToString())
                );

                options.AddPolicy("Employee", policy =>
                    policy.RequireRole(
                        Domain.Enums.AppRole.Admin.ToString(),
                        Domain.Enums.AppRole.Employee.ToString()
                    )
                );

                options.AddPolicy("Member", policy =>
                    policy.RequireRole(
                        Domain.Enums.AppRole.Admin.ToString(),
                        Domain.Enums.AppRole.Employee.ToString(),
                        Domain.Enums.AppRole.Member.ToString()
                    )
                );
            });

            #endregion

            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMvc", policy =>
                {
                    policy.WithOrigins("https://localhost:5001")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            #endregion

            #region MVC

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new()
                    {
                        Title = "Membriana API",
                        Version = "v1"
                    }
                );

                var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Description = "Introduce tu token JWT como: Bearer {token}",
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                options.AddSecurityRequirement(
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        { jwtSecurityScheme, Array.Empty<string>() }
                    }
                );
            });

            #endregion

            var app = builder.Build();

            #region Middleware Pipeline

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowMvc");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
