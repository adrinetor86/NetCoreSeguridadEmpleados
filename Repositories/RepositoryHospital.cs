using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Repositories;

public class RepositoryHospital
{

    private DataContext _context;


    public RepositoryHospital(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Empleado>> GetEmpleadosAsync()
    {
        return await _context.Empleados.ToListAsync();
    }    
    
    public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
    {
        return await _context.Empleados
            .FirstOrDefaultAsync(x=>x.IdEmpleado==idEmpleado);
    }
    
    public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync(int idDept)
    {
        return await _context.Empleados
            .Where(z=>z.IdDepartamento==idDept)
            .ToListAsync();
    }

    public async Task UpdateSalarioEmpleadosAsync(int idDept, int incremento)
    {
        List<Empleado> empleados = await GetEmpleadosDepartamentoAsync(idDept);

        foreach (Empleado emp in empleados)
        {
            emp.Salario += incremento;
        }

        await _context.SaveChangesAsync();
    }   
    
    public async Task DeleteEmpleado(int id)
    {
        
        Empleado empleado = await FindEmpleadoAsync(id);

        _context.Empleados.Remove(empleado);
        
        await _context.SaveChangesAsync();
        
    }


    public async Task<Empleado> LogInEmpleadosAsync(string apellido, int idEmpleado)
    {
        Empleado empleado = await _context.Empleados.FirstOrDefaultAsync
        (z => z.Apellido == apellido
              && z.IdEmpleado == idEmpleado);
        return empleado;
    }


    public async Task<Boolean> TieneSubordinados(int id)
    {


        if (await _context.Empleados.Where(z => z.dir == id).CountAsync() != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    
    
}