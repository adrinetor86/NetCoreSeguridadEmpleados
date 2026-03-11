using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Data;

public class DataContext : DbContext
{

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Empleado> Empleados { get; set; }

}
