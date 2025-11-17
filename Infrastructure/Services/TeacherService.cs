using System.Net;
using Domain.DTOs.TeacherDto;
using Domain.Entities;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TeacherService(DataContext context) : ITeacherService
{
    public async Task<Responce<string>> AddTeacher(AddTeacher teacher)
    {
        try
        {
            var existsFaculty = await context.Faculties.FirstOrDefaultAsync(x=>x.Id == teacher.FacultyId && x.IsDeleted == false);
            if (existsFaculty == null)
                return new Responce<string>(HttpStatusCode.BadRequest,"Хатоги ҳангоми интихоби факултет!");
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
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created, "Teacher successfully added!")
                : new Responce<string>(HttpStatusCode.BadRequest, "Something went wrong");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateTeacher(UpdateTeacher teacher)
    {
        try
        {
            var existsTeacher = await context.Teachers.FirstOrDefaultAsync(x => x.Id == teacher.Id && x.IsDeleted == false);
            if (existsTeacher == null)
                return new Responce<string>(HttpStatusCode.NotFound,"Teacher not found!");
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
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteTeacher(int id)
    {
        try
        {
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
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetTeacher>> GetTeacher(int id)
    {
        try
        {
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
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PaginationResponce<List<GetTeacher>>> GetTeachers(TeacherFilter filter)
    {
        try
        {
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
            return new PaginationResponce<List<GetTeacher>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}