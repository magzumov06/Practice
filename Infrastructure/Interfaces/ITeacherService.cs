using Domain.DTOs.TeacherDto;
using Domain.Filter;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ITeacherService
{
    Task<Responce<string>> AddTeacher(AddTeacher teacher);
    Task<Responce<string>> UpdateTeacher(UpdateTeacher teacher);
    Task<Responce<string>> DeleteTeacher(int id);
    Task<Responce<GetTeacher>> GetTeacher(int id);
    Task<PaginationResponce<List<GetTeacher>>> GetTeachers(TeacherFilter filter);
}