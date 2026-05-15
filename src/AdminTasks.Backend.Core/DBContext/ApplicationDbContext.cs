using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models.Input;

namespace DBContext.ApplicationDbContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tarea> Tareas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tarea>().ToTable("tareas");
    }

}

public class Tarea
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
}
