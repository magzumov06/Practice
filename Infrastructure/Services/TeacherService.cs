using System.Net;
using Domain.DTOs.TeacherDto;
using Domain.Entities;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class TeacherService(DataContext context) : ITeacherService
{
    public async Task<Responce<string>> AddTeacher(AddTeacher teacher)
    {
        try
        {
            Log.Information("Adding teacher: {FullName}, {Email},{FacultyId}", teacher.FullName, teacher.Email, teacher.FacultyId);
            var existsFaculty = await context.Faculties.FirstOrDefaultAsync(x=>x.Id == teacher.FacultyId && x.IsDeleted == false);
            if (existsFaculty == null)
            {
                Log.Warning("Faculty not found:{FacultyId}" , teacher.FacultyId);
                return new Responce<string>(HttpStatusCode.BadRequest,"Хатоги ҳангоми интихоби факултет!");
            }
            var newTeacher = new Teacher()
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                FacultyId = teacher.FacultyId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            await context.Teachers.AddAsync(newTeacher);
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Teacer {TeacherId} added", newTeacher.Id);
                return new Responce<string>(HttpStatusCode.Created, "Teacher successfully added!");
            }
            Log.Warning("Teacher {TeacherId} could not be added", newTeacher.Id);
            return new Responce<string>(HttpStatusCode.BadRequest, "Something went wrong");
        }
        catch (Exception e)
        {
            Log.Error("Error adding teacher");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateTeacher(UpdateTeacher teacher)
    {
        try
        {
            Log.Information("Updating teacher");
            var existsTeacher = await context.Teachers.FirstOrDefaultAsync(x => x.Id == teacher.Id && x.IsDeleted == false);
            if (existsTeacher == null)
                return new Responce<string>(HttpStatusCode.NotFound,"Teacher not found!");
            Log.Information("Updating teacher {id}.OldValue:  {OldFullName},{OldEmail},{OldFacultyId}",existsTeacher.Id,existsTeacher.FullName,existsTeacher.Email,existsTeacher.FacultyId);
            existsTeacher.FullName = teacher.FullName;
            existsTeacher.Email = teacher.Email;
            existsTeacher.FacultyId = teacher.FacultyId;
            existsTeacher.UpdatedDate = DateTime.UtcNow;
            var res =  await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Teacher successfully updated!")
                : new Responce<string>(HttpStatusCode.BadRequest, "Something went wrong");
        }
        catch (Exception e)
        {
            Log.Error("Error updating teacher");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteTeacher(int id)
    {
        try
        {
            Log.Information("Deleting teacher {id}",id);
            var existsTeacher = await context.Teachers.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (existsTeacher == null)
                return new Responce<string>(HttpStatusCode.NotFound,"Teacher not found!");
            existsTeacher.IsDeleted = true;
            existsTeacher.UpdatedDate = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Teacher successfully deleted!")
                : new Responce<string>(HttpStatusCode.BadRequest, "Something went wrong");
        }
        catch (Exception e)
        {
            Log.Error("Error deleting teacher");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetTeacher>> GetTeacher(int id)
    {
        try
        {
            Log.Information("Getting teacher");
            var existsTeacher = await context.Teachers.FirstOrDefaultAsync(x => x.Id == id  && x.IsDeleted == false);
            if (existsTeacher == null)
                return new Responce<GetTeacher>(HttpStatusCode.NotFound, "Teacher not found!");
            var dto = new GetTeacher()
            {
                Id = existsTeacher.Id,
                FullName = existsTeacher.FullName,
                Email = existsTeacher.Email,
                FacultyId = existsTeacher.FacultyId,
                CreatedDate = existsTeacher.CreatedDate,
                UpdatedDate = existsTeacher.UpdatedDate,
            };
            return new Responce<GetTeacher>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error getting teacher");
            return new Responce<GetTeacher>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetTeacher>>> GetTeachers(TeacherFilter filter)
    {
        try
        {
            Log.Information("Getting teachers");
            var query = context.Teachers.AsQueryable();
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id.Value);
            
            if(!string.IsNullOrWhiteSpace(filter.FullName))
                query = query.Where(x => x.FullName.ToLower().Contains(filter.FullName.ToLower()));
            
            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(x => x.Email.ToLower().Contains(filter.Email.ToLower()));
            
            if(filter.FacultyId.HasValue)
                query = query.Where(x => x.FacultyId == filter.FacultyId.Value);

            query = query.Where(x=>x.IsDeleted==false);
            var total = await query.CountAsync();
            var skip =  (filter.PageNumber - 1) * filter.PageSize;
            var teachers = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(teachers.Count == 0)
                return new PaginationResponce<List<GetTeacher>>(HttpStatusCode.NotFound,"Teacher not found!");
            Log.Information("Found {Total} teachers", total);
            var dtos = teachers.Select(x=> new GetTeacher()
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                FacultyId = x.FacultyId,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
            }).ToList();
            return new PaginationResponce<List<GetTeacher>>(dtos, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            Log.Error("Error getting teachers");
            return new PaginationResponce<List<GetTeacher>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}