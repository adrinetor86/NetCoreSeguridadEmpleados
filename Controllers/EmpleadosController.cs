using System.Security.Claims;
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
    
    [AuthorizeEmpleados]
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

    [AuthorizeEmpleados(Policy = "SoloSub")]
    public async Task<IActionResult> Delete(int id)
    {
        
       await _repoHosp.DeleteEmpleado(id);
        
        return RedirectToAction("Index");    
    }
    
    [AuthorizeEmpleados(Policy = "SOLOJEFES")]
    public async Task<IActionResult> Compis()
    {
        //RECUPERAMOS EL CLAIM DEL USUARIO VALIDADO
        string dato= HttpContext.User.FindFirstValue("Departamento");
        int idDept = int.Parse(dato);
        List<Empleado> empleados=await _repoHosp.GetEmpleadosDepartamentoAsync(idDept); 
        
        return View(empleados);
    }   
    [HttpPost]
    [AuthorizeEmpleados(Policy = "SOLOJEFES")]
    public async Task<IActionResult> Compis(int incremento)
    {
        string dato= HttpContext.User.FindFirstValue("Departamento");

        int idDept = int.Parse(dato);
        
        await _repoHosp.UpdateSalarioEmpleadosAsync(idDept,incremento);
        List<Empleado> empleados = await _repoHosp.GetEmpleadosDepartamentoAsync(idDept);
        return View(empleados);
    }

    [AuthorizeEmpleados(Policy = "ADMINONLY")]
    public IActionResult AdminEmpleados()
    {
        return View();
    }
    
    [AuthorizeEmpleados(Policy = "SoloRicos")]
    public IActionResult ZonaNoble()
    {
        return View();
    }
    
    
}