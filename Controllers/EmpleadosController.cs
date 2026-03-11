using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;

namespace NetCoreSeguridadEmpleados.Controllers;

public class EmpleadosController : Controller
{
    private RepositoryHospital _repoHosp;

    public EmpleadosController(RepositoryHospital repoHosp)
    {
        _repoHosp = repoHosp;
    }
    
    public async Task<IActionResult> Index()
    {
        List<Empleado> empleados = await _repoHosp.GetEmpleadosAsync();
        return View(empleados);
    }  
    
    public async Task<IActionResult> Details(int id)
    {
        Empleado empleado = await _repoHosp.FindEmpleadoAsync(id);
        return View(empleado);
    }

    [AuthorizeEmpleados]
    public IActionResult PerfilEmpleado()
    {
        return View();
    }
    
    
}