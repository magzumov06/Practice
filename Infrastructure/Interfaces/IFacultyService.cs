using Domain.DTOs.FacyltyDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IFacultyService
{
    Task<Responce<string>> AddFaculty(AddFaculty faculty);
    Task<Responce<string>> UpdateFaculty(UpdateFaculty faculty);
    Task<Responce<string>> DeleteFaculty(int id);
    Task<Responce<GetFaculty>> GetFacultyById(int id);
    Task<Responce<List<GetFaculty>>> GetAllFaculties();
}