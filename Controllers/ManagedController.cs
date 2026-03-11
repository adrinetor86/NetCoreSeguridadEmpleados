using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;

namespace NetCoreSeguridadEmpleados.Controllers;

public class ManagedController : Controller
{
    private RepositoryHospital _repoHosp;

    public ManagedController(RepositoryHospital repoHosp)
    {
        _repoHosp = repoHosp;
    }
    
    public IActionResult Login()
    {
        return View();
    }  
    
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        int idEmpleado = int.Parse(password);
        Empleado empleado=await _repoHosp.LogInEmpleadosAsync(username,idEmpleado);
        if (empleado != null)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name, ClaimTypes.Role);
            Claim claimName=new Claim(ClaimTypes.Name,username);
            identity.AddClaim(claimName);
            Claim claimId = new Claim(ClaimTypes.NameIdentifier
                ,empleado.IdEmpleado.ToString());
            identity.AddClaim(claimId);
            //COMO ROLE, UTILIZAMOS EL OFICIO (LOS CLAIMS SIEMPRE SON STRING)

            Claim claimRole = new Claim(ClaimTypes.Role, empleado.Oficio);
            identity.AddClaim(claimRole);
            Claim claimSalario = new Claim("Salario",empleado.Salario.ToString());
            identity.AddClaim(claimSalario);
            Claim claimDept= new Claim("Departamento",empleado.IdDepartamento.ToString());
            identity.AddClaim(claimDept);
            
            //COMO POR AHORA NO VOY A UTILIZAR ROLES NO LO INDICAMOS
            ClaimsPrincipal userPrincipal= new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
            
            //POR AHORA LO ENVIAMOS A UNA VISTA QUE HAREMOS EN BREVE
            
            return RedirectToAction("PerfilEmpleado", "Empleados");
        }
        else
        {
            ViewData["MENSAJE"]="Credenciales incorrectas";
            return View();
        }
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync
            (CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    
}