using Application.Repositories;
using Application.Services;
using Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    /// <summary>
    /// Filtro de acción que aplica validaciones de multi-tenancy sobre recursos
    /// (entidades) identificables por un <c>ID</c> obtenido de la ruta.
    /// </summary>
    /// <remarks>
    /// Este filtro garantiza que el recurso solicitado:
    /// <list type="bullet">
    /// <item>Exista en el sistema.</item>
    /// <item>Pertenezca a la organización (tenant) del usuario autenticado.</item>
    /// </list>
    ///
    /// Si el recurso no existe o pertenece a otra organización, el filtro
    /// corta la ejecución y devuelve <see cref="NotFoundResult"/>.
    /// 
    /// La respuesta 404 es intencional y evita filtrar información sobre la
    /// existencia de recursos de otros tenants.
    /// </remarks>
    /// <typeparam name="T">
    /// Tipo de la entidad a validar. Debe implementar <see cref="IIdentifiable"/>
    /// e <see cref="ITenantable"/>.
    /// </typeparam>
    /// <typeparam name="R">
    /// Repositorio de acceso a datos para la entidad <typeparamref name="T"/>.
    /// </typeparam>
    public class TenancyRouteFilter<T, R> : IAsyncActionFilter
        where T : class, IIdentifiable, ITenantable
        where R : IBaseRepository<T>
    {
        private readonly IUserService _userService;
        private readonly R _repository;

        /// <summary>
        /// Constructor principal.
        /// </summary>
        public TenancyRouteFilter(IUserService userService, R repository)
        {
            _userService = userService;
            _repository = repository;
        }

        /// <summary>
        /// Ejecuta la validación de acceso al recurso antes de que se invoque
        /// una acción del controlador.
        /// </summary>
        /// <param name="filterContext">
        /// Contexto de ejecución de la acción, utilizado para obtener el
        /// identificador del recurso desde la ruta.
        /// </param>
        /// <param name="next">
        /// Delegado que continúa la ejecución del pipeline si la validación es exitosa.
        /// </param>
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
