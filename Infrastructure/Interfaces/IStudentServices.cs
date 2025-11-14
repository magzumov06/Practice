using Domain.DTOs.StudentDto;
using Domain.Filter;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IStudentServices
{
    Task<Responce<string>> AddStudent(AddStudent student);
    Task<Responce<string>> UpdateStudent(UpdateStudent student);
    Task<Responce<string>> DeleteStudent(int id);
    Task<Responce<GetStudent>> GetStudentById(int id);
    Task<PaginationResponce<List<GetStudent>>>GetStudents(StudentFilter filter);
    
}