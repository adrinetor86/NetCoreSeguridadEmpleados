using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using NetCoreSeguridadEmpleados.Repositories;

namespace NetCoreSeguridadEmpleados.Policies;

public class SubordinadoRequirement : IAuthorizationRequirement { }

public class SubordinadoHandler : AuthorizationHandler<SubordinadoRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SubordinadoHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, SubordinadoRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        var repo = httpContext.RequestServices.GetRequiredService<RepositoryHospital>();
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim != null)
        {
            int id = int.Parse(userIdClaim.Value);
            if (await repo.TieneSubordinados(id))
            {
                context.Succeed(requirement);
            }
        }
    }
}
    
