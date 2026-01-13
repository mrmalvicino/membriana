using Api.Filters;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

            /// <summary>
            /// Cadena de conexión principal utilizada por la infraestructura (EF Core y persistencia).
            /// </summary>
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            /// <summary>
            /// Configuración de JWT (Issuer, Audience, Key) utilizada por autenticación.
            /// </summary>
            var jwtSettings = builder.Configuration.GetSection("Jwt");

            /// <summary>
            /// Clave simétrica utilizada para firmar y validar tokens JWT.
            /// </summary>
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            #endregion

            #region Dependency Injection - Infrastructure

            /// <summary>
            /// Registra dependencias de infraestructura
            /// </summary>
            builder.Services.AddInfrastructure(connectionString);

            #endregion

            #region Dependency Injection - Filters

            /// <summary>
            /// Filtro para validar tenancy por querystring (organizationId) y evitar acceso cross-tenant.
            /// </summary>
            builder.Services.AddScoped<TenancyQueryFilter>();

            /// <summary>
            /// Filtro para validar tenancy por ruta (organizationId) y evitar acceso cross-tenant.
            /// </summary>
            builder.Services.AddScoped(typeof(TenancyRouteFilter<,>));

            #endregion

            #region Identity

            /// <summary>
            /// Configura Identity con almacenamiento EF Core sobre <see cref="AppDbContext"/>.
            /// </summary>
            builder.Services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            #endregion

            #region Authentication - JWT Bearer

            /// <summary>
            /// Configura autenticación basada en JWT Bearer.
            /// </summary>
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

            /// <summary>
            /// Políticas de autorización por roles.
            /// </summary>
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

            /// <summary>
            /// Política CORS para permitir que el frontend MVC consuma la API en desarrollo.
            /// </summary>
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

            /// <summary>
            /// Registra AutoMapper escaneando los assemblies de Application/Infrastructure/Api.
            /// </summary>
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            /// <summary>
            /// Habilita controllers para endpoints REST.
            /// </summary>
            builder.Services.AddControllers();

            /// <summary>
            /// Permite descubrir endpoints para Swagger/OpenAPI.
            /// </summary>
            builder.Services.AddEndpointsApiExplorer();

            /// <summary>
            /// Configura Swagger/OpenAPI e incorpora esquema de seguridad JWT Bearer para "Authorize".
            /// </summary>
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

            #region Build

            var app = builder.Build();

            #endregion

            #region Middleware Pipeline

            /// <summary>
            /// Swagger habilitado solo en Development.
            /// </summary>
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            /// <summary>
            /// Redirección a HTTPS.
            /// </summary>
            app.UseHttpsRedirection();

            /// <summary>
            /// Aplica la política CORS para permitir el consumo desde el frontend MVC.
            /// </summary>
            app.UseCors("AllowMvc");

            /// <summary>
            /// Autenticación y autorización, en ese orden.
            /// </summary>
            app.UseAuthentication();
            app.UseAuthorization();

            /// <summary>
            /// Mapea controllers como endpoints.
            /// </summary>
            app.MapControllers();

            #endregion

            #region Run

            app.Run();

            #endregion
        }
    }
}
