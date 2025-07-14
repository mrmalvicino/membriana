using Application.Repositories;
using Application.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class TenancyRouteFilter<T, R> : IAsyncActionFilter
        where T : class, IIdentifiable, ITenantable
        where R : IBaseRepository<T>
    {
        private readonly IUserService _userService;
        private readonly R _repository;

        public TenancyRouteFilter(IUserService userService, R repository)
        {
            _userService = userService;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var urlId = filterContext.RouteData.Values["id"] as string;
            int id;
            int.TryParse(urlId, out id);

            T? entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                filterContext.Result = new NotFoundResult();
                return;
            }

            int userOrgId = await _userService.GetOrganizationIdAsync();

            if (entity.OrganizationId != userOrgId)
            {
                filterContext.Result = new NotFoundResult();
                return;
            }

            await next();
        }
    }
}
