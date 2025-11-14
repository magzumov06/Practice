using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<Faculty> Faculties { get; set; }

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
        
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(e => e.Enrollments)
            .HasForeignKey(e => e.StudentId);
        
    }
}