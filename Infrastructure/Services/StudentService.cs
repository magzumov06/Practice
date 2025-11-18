using System.Net;
using Domain.DTOs.StudentDto;
using Domain.Entities;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StudentService(DataContext context) : IStudentServices 
{
    public async Task<Responce<string>> AddStudent(AddStudent student)
    {
        try
        {
            var faculty = await context.Faculties.FirstOrDefaultAsync(f => f.Id == student.FacultyId &&  f.IsDeleted == false);
            if (faculty == null) 
                return new Responce<string>(HttpStatusCode.BadRequest, "Чунин факултет вуҷуд надорад!");
            var specialty = await context.Specialties.FirstOrDefaultAsync(s => s.Id == student.SpecialtyId && s.IsDeleted == false);
            if (specialty == null) 
                return new Responce<string>(HttpStatusCode.BadRequest, "Чунин ихтисос вуҷуд надорад!");
            var newStudent = new Student()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                Gender = student.Gender,
                FacultyId = student.FacultyId,
                SpecialtyId = student.SpecialtyId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            await context.Students.AddAsync(newStudent);
            var res = await context.SaveChangesAsync();
            return res  > 0
                ? new Responce<string>(HttpStatusCode.Created, "Student added successfully")
                : new Responce<string>(HttpStatusCode.BadRequest, "Student could not be added");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateStudent(UpdateStudent student)
    {
        try
        {
            var updatStudent = await context.Students.FirstOrDefaultAsync(x=> x.Id == student.Id &&  x.IsDeleted == false);
            if(updatStudent == null) return new Responce<string>(HttpStatusCode.NotFound, "Student not found");
            updatStudent.FirstName = student.FirstName;
            updatStudent.LastName = student.LastName;
            updatStudent.BirthDate = student.BirthDate;
            updatStudent.Gender = student.Gender;
            updatStudent.FacultyId = student.FacultyId;
            updatStudent.SpecialtyId = student.SpecialtyId;
            updatStudent.UpdatedDate = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Student updated successfully")
                : new Responce<string>(HttpStatusCode.BadRequest, "Student could not be updated");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteStudent(int id)
    {
        try
        {
            var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id &&  x.IsDeleted == false);
            if(student == null) return new Responce<string>(HttpStatusCode.NotFound, "Student not found");
            student.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Student deleted successfully")
                : new Responce<string>(HttpStatusCode.BadRequest, "Student could not be deleted");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetStudent>> GetStudentById(int id)
    {
        try
        {
            var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if(student == null) return new Responce<GetStudent>(HttpStatusCode.NotFound, "Student not found");
            if(student.IsDeleted) return new Responce<GetStudent>(HttpStatusCode.NotFound, "Student deleted");
            var dto = new GetStudent()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                Gender = student.Gender,
                FacultyId = student.FacultyId,
                SpecialtyId = student.SpecialtyId,
                CreatedDate = student.CreatedDate,
                UpdatedDate = student.UpdatedDate,
            };
            return new Responce<GetStudent>(dto);
        }
        catch (Exception e)
        {
            return new Responce<GetStudent>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetStudent>>> GetStudents(StudentFilter filter)
    {
        try
        {
            var query = context.Students.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id);
            }

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                query = query.Where(x => x.FirstName.Contains(filter.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                query = query.Where(x => x.LastName.Contains(filter.LastName));
            }

            if (!filter.BirthDate.HasValue)
            {
                query = query.Where(x => x.BirthDate >= filter.BirthDate);
            }

            if (!filter.Gender.HasValue)
            {
                query = query.Where(x => x.Gender == filter.Gender);
            }

            if (!filter.FacultyId.HasValue)
            {
                query = query.Where(x => x.FacultyId == filter.FacultyId);
            }

            if (!filter.SpecialtyId.HasValue)
            {
                query = query.Where(x => x.SpecialtyId == filter.SpecialtyId);
            }

            query = query.Where(x => x.IsDeleted == false);
            var total = await query.CountAsync();
            var skip =  (filter.PageNumber - 1) * filter.PageSize;
            var student = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(student.Count == 0) return new PaginationResponce<List<GetStudent>>(HttpStatusCode.NotFound,"No students found");
            var dtos = student.Select(x=> new GetStudent()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                FacultyId = x.FacultyId,
                SpecialtyId = x.SpecialtyId,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate
            }).ToList();
            return new PaginationResponce<List<GetStudent>>(dtos, total,filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PaginationResponce<List<GetStudent>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}