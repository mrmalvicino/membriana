using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class TenancyQueryFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;

        public TenancyQueryFilter(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            string? queryParam = filterContext.HttpContext.Request.Query["organizationId"];
            int queryOrgId;
            int.TryParse(queryParam, out queryOrgId);
            int userOrgId = await _userService.GetOrganizationIdAsync();

            if (queryOrgId != userOrgId)
            {
                filterContext.Result = new NotFoundResult();
                return;
            }

            await next();
        }
    }
}
