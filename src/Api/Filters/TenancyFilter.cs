using Application.Repositories;
using Application.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class TenancyFilter<T, R> : IAsyncActionFilter
        where T : class, IIdentifiable
        where R : IBaseRepository<T>
    {
        private readonly IUserService _userService;
        private readonly R _repository;

        public TenancyFilter(IUserService userService, R repository)
        {
            _userService = userService;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            int id;
            var urlId = filterContext.RouteData.Values["id"] as string;
            int.TryParse(urlId, out id);

            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                filterContext.Result = new NotFoundResult();
                return;
            }

            var organizationIdProperty = typeof(T).GetProperty("OrganizationId");

            if (organizationIdProperty == null)
            {
                filterContext.Result = new BadRequestResult();
                return;
            }

            int organizationId = await _userService.GetOrganizationIdAsync();
            var entityOrganizationId = (int)organizationIdProperty.GetValue(entity);

            if (entityOrganizationId != organizationId)
            {
                filterContext.Result = new NotFoundResult();
                return;
            }

            await next();
        }
    }
}
