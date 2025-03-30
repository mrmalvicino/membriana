﻿using Application.Repositories;
using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
