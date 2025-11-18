using Domain.DTOs.StudentGroupDto;
using Domain.Filter;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IStudentGroupService
{
    Task<Responce<string>> AddStudentGroup(AddStudentGroup add);
    Task<Responce<string>> UpdateStudentGroup(UpdateStudentGroup update);
    Task<Responce<string>> DeleteStudentGroup(int id);
    Task<Responce<GetStudentGroup>> GetStudentGroupById(int id);
    Task<PaginationResponce<List<GetStudentGroup>>> GetStudentGroup(StudentGroupFilter filter);
    
}