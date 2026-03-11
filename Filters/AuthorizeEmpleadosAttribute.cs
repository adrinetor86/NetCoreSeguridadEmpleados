using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSeguridadEmpleados.Filters;

public class AuthorizeEmpleadosAttribute :AuthorizeAttribute,IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //POR AHORA, SOLO NOS INTERESA 
        //VALIDAR SI EXISTE O NO EL USUARIO

        var user = context.HttpContext.User;
        if (user.Identity.IsAuthenticated == false)
        {
            context.Result = GetRoute("Managed", "Login");
        }
        
    }
    //EN ALGUN MOMENTO TENDREMOS MAS REDIRECCIONES QUE SOLO A LOGIN,
    //POR LO QUE CREAMOS UN METODO PARA REDIRECCIONAR

    private RedirectToRouteResult GetRoute(string controller, string action)
    {
        RouteValueDictionary ruta =
            new RouteValueDictionary(new
            {
                Controller = controller,
                Action = action
            });
        RedirectToRouteResult result = new RedirectToRouteResult(ruta);
        return result;
    }
}