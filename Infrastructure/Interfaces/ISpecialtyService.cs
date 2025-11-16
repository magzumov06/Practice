using Domain.DTOs.SpecialityDto;
using Domain.Filter;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ISpecialtyService
{
    Task<Responce<string>> AddSpecialty(AddSpeciality specialty);
    Task<Responce<string>> UpdateSpecialty(UpdateSpeciality specialty);
    Task<Responce<string>> DeleteSpecialty(int id);
    Task<Responce<GetSpeciality>> GetSpecialtyId(int id);
    Task<PaginationResponce<List<GetSpeciality>>> GetSpecialties(SpecialtyFilter filter);
}