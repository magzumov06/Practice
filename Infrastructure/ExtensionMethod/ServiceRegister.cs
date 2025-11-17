using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExtensionMethod;

public static class ServiceRegister
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentServices, StudentService>();
        services.AddScoped<IFacultyService, FacultyService>();
        services.AddScoped<ISpecialtyService, SpecialtyService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<ITeacherService, TeacherService>();
    }
}