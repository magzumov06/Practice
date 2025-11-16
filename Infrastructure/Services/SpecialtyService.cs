using System.Net;
using Domain.DTOs.SpecialityDto;
using Domain.Entities;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class SpecialtyService(DataContext context) : ISpecialtyService
{
    public async Task<Responce<string>> AddSpecialty(AddSpeciality specialty)
    {
        try
        {
            var faculty = await context.Faculties.FirstOrDefaultAsync(f => f.Id == specialty.FacultyId && f.IsDeleted == false);
            if (faculty == null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Хатоги ҳангоми интихоби факултет!");
            var exexistsSpeciality = await context.Specialties.FirstOrDefaultAsync(x => x.Name == specialty.Name);
            if (exexistsSpeciality != null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Ихтисос алакай вуҷуд дорад!");
            var newSpecialty = new Specialty()
            {
                Name = specialty.Name,
                SpecialityLanguage = specialty.SpecialityLanguage,
                FacultyId = specialty.FacultyId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            await context.Specialties.AddAsync(newSpecialty);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created, "Specialty added")
                : new Responce<string>(HttpStatusCode.BadRequest, "Something went wrong");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> UpdateSpecialty(UpdateSpeciality specialty)
    {
        try
        {
            var existingSpecialty = await context.Specialties.FirstOrDefaultAsync(x=> x.Id == specialty.Id);
            if (existingSpecialty == null) return new Responce<string>(HttpStatusCode.NotFound, "Specialty not found");
            existingSpecialty.Name = specialty.Name;
            existingSpecialty.SpecialityLanguage = specialty.SpecialityLanguage;
            existingSpecialty.FacultyId = specialty.FacultyId;
            existingSpecialty.UpdatedDate = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Specialty updated")
                : new Responce<string>(HttpStatusCode.BadRequest, "Something went wrong");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Responce<string>> DeleteSpecialty(int id)
    {
        try
        {
            var specialty = await context.Specialties.FirstOrDefaultAsync(x => x.Id == id);
            if (specialty == null) return new Responce<string>(HttpStatusCode.NotFound, "Specialty not found");
            specialty.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Specialty deleted")
                : new Responce<string>(HttpStatusCode.BadRequest, "Something went wrong");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError,e.Message);
        }    }

    public async Task<Responce<GetSpeciality>> GetSpecialtyId(int id)
    {
        try
        {
            var specialty = await context.Specialties.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if(specialty == null) return new Responce<GetSpeciality>(HttpStatusCode.NotFound, "Specialty not found");
            var dto = new GetSpeciality()
            {
                Id = specialty.Id,
                Name = specialty.Name,
                SpecialityLanguage = specialty.SpecialityLanguage,
                FacultyId = specialty.FacultyId,
                CreatedDate = specialty.CreatedDate,
                UpdatedDate = specialty.UpdatedDate
            };
            return new Responce<GetSpeciality>(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    }

    public async Task<PaginationResponce<List<GetSpeciality>>> GetSpecialties(SpecialtyFilter filter)
    {
        try
        {
            var query =  context.Specialties.AsQueryable();
            if(filter.Id.HasValue) query = query.Where(x=> x.Id == filter.Id);
            if(!string.IsNullOrEmpty(filter.Name)) query = query.Where(x => x.Name.Contains(filter.Name));
            if(filter.FacultyId.HasValue) query = query.Where(x => x.FacultyId == filter.FacultyId);
            query = query.Where(x => x.IsDeleted == false);
            var total = await query.CountAsync();
            var skip =  (filter.PageNumber - 1) * filter.PageSize;
            var specialties = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(specialties.Count == 0) return new PaginationResponce<List<GetSpeciality>>(HttpStatusCode.NotFound, "Specialties not found");
            var dtos = specialties.Select(x=> new GetSpeciality()
            {
                Id = x.Id,
                Name = x.Name,
                SpecialityLanguage = x.SpecialityLanguage,
                FacultyId = x.FacultyId,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            }).ToList();
            return new PaginationResponce<List<GetSpeciality>>(dtos, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PaginationResponce<List<GetSpeciality>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}