using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Groups> Groups { get; set; }
    public DbSet<StudentGroup> StudentGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Specialties)
            .WithOne(s => s.Faculty)
            .HasForeignKey(s => s.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Faculty>()
            .HasMany(f => f.Students)
            .WithOne(s => s.Faculty)
            .HasForeignKey(s => s.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Specialty>()
            .HasMany(s => s.Students)
            .WithOne(s => s.Specialty)
            .HasForeignKey(s => s.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);
        
      
        
    }
}