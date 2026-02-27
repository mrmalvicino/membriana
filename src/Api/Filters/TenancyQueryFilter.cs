using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

/// <summary>
/// Filtro de acción que aplica validaciones de multi-tenancy sobre
/// parámetros recibidos por query string.
/// </summary>
/// <remarks>
/// Este filtro valida que el parámetro <c>organizationId</c> recibido
/// en la URL coincida con la organización asociada al usuario autenticado.
///
/// Si el valor no coincide, la ejecución de la acción se interrumpe y se
/// devuelve <see cref="NotFoundResult"/>.
/// 
/// La respuesta 404 es intencional y evita filtrar información sobre la
/// existencia de recursos pertenecientes a otros tenants.
/// </remarks>
public class TenancyQueryFilter : IAsyncActionFilter
{
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor principal.
    /// </summary>
    public TenancyQueryFilter(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Ejecuta la validación de acceso multi-tenant antes de que se invoque
    /// la acción del controlador.
    /// </summary>
    /// <param name="filterContext">
    /// Contexto de ejecución que permite acceder a los parámetros de la
    /// solicitud HTTP.
    /// </param>
    /// <param name="next">
    /// Delegado que continúa la ejecución del pipeline si la validación es exitosa.
    /// </param>
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
