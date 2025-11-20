using System.Net;
using Domain.DTOs.FacyltyDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class FacultyService(DataContext context) : IFacultyService
{
    public async Task<Responce<string>> AddFaculty(AddFaculty faculty)
    {
        try
        {
            Log.Information("Adding faculty");
            var existsFaculty = await context.Faculties.FirstOrDefaultAsync(x=> x.Name == faculty.Name);
            if (existsFaculty != null)
            {
                return new Responce<string>(HttpStatusCode.BadRequest,"Faculty already exists");
            }

            var newFaculty = new Faculty()
            {
                Name = faculty.Name,
                DecanName = faculty.DecanName,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            await context.Faculties.AddAsync(newFaculty);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created,"Faculty added")
                : new Responce<string>(HttpStatusCode.NotFound,"Something went wrong");
        }
        catch (Exception e)
        {
            Log.Error("Error adding faculty");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateFaculty(UpdateFaculty faculty)
    {
        try
        {
            Log.Information("Updating faculty");
            var existsFaculty = await context.Faculties.FirstOrDefaultAsync(x => x.Id == faculty.Id &&  x.IsDeleted == false);
            if (existsFaculty == null) return new Responce<string>(HttpStatusCode.NotFound,"Faculty not found");
            existsFaculty.Name = faculty.Name;
            existsFaculty.DecanName = faculty.DecanName;
            existsFaculty.UpdatedDate = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Faculty updated")
                : new Responce<string>(HttpStatusCode.NotFound,"Something went wrong");
        }
        catch (Exception e)
        {
            Log.Error("Error updating faculty");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteFaculty(int id)
    {
        try
        {
            Log.Information("Deleting faculty");
            var existsFaculty = await context.Faculties.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if(existsFaculty == null) return new Responce<string>(HttpStatusCode.NotFound,"Faculty not found");
            existsFaculty.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Faculty deleted")
                : new Responce<string>(HttpStatusCode.NotFound,"Something went wrong");
        }
        catch (Exception e)
        {
            Log.Error("Error deleting faculty");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetFaculty>> GetFacultyById(int id)
    {
        try
        {
            Log.Information("Getting faculty");
            var existsFaculty = await context.Faculties.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if(existsFaculty == null) return new Responce<GetFaculty>(HttpStatusCode.NotFound,"Faculty not found");
            var dto = new GetFaculty()
            {
                Id = existsFaculty.Id,
                Name = existsFaculty.Name,
                DecanName = existsFaculty.DecanName,
                CreatedDate = existsFaculty.CreatedDate,
                UpdatedDate = existsFaculty.UpdatedDate,
            };
            return new Responce<GetFaculty>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error getting faculty");
            return new Responce<GetFaculty>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetFaculty>>> GetAllFaculties()
    {
        try
        {
            Log.Information("Getting all faculties");
            var faculties = await context.Faculties.Where(x=> x.IsDeleted == false).ToListAsync();
            if(faculties.Count == 0) return new Responce<List<GetFaculty>>(HttpStatusCode.NotFound,"Faculty not found");
            var dtos = faculties.Select(x => new GetFaculty()
            {
                Id = x.Id,
                Name = x.Name,
                DecanName = x.DecanName,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
            }).ToList();
            return new Responce<List<GetFaculty>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error("Error getting all faculties");
            return new Responce<List<GetFaculty>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}