using Microsoft.EntityFrameworkCore;
using SourcefitClone.Api.Models;

namespace SourcefitClone.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Resolve the two-FK-to-Employee ambiguity on Department
        modelBuilder.Entity<Department>()
            .HasOne(d => d.PrimaryContact)
            .WithMany()
            .HasForeignKey(d => d.PrimaryContactId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Department>()
            .HasOne(d => d.SecondaryContact)
            .WithMany()
            .HasForeignKey(d => d.SecondaryContactId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Employee>()
            .HasQueryFilter(e => e.DeletedAt == null);
    }
}