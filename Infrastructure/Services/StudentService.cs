using System.Net;
using Domain.DTOs.StudentDto;
using Domain.Entities;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class StudentService(DataContext context) : IStudentServices 
{
  public async Task<Responce<string>> AddStudent(AddStudent student)
{
    try
    {
        Log.Information("AddStudent called with FirstName={FirstName}, LastName={LastName}, FacultyId={FacultyId}, SpecialtyId={SpecialtyId}",
            student.FirstName, student.LastName, student.FacultyId, student.SpecialtyId);

        var faculty = await context.Faculties.FirstOrDefaultAsync(f => f.Id == student.FacultyId && !f.IsDeleted);
        if (faculty == null)
        {
            Log.Warning("Faculty not found. FacultyId={FacultyId}", student.FacultyId);
            return new Responce<string>(HttpStatusCode.BadRequest, "Чунин факултет вуҷуд надорад!");
        }

        var specialty = await context.Specialties.FirstOrDefaultAsync(s => s.Id == student.SpecialtyId && !s.IsDeleted);
        if (specialty == null)
        {
            Log.Warning("Specialty not found. SpecialtyId={SpecialtyId}", student.SpecialtyId);
            return new Responce<string>(HttpStatusCode.BadRequest, "Чунин ихтисос вуҷуд надорад!");
        }

        var newStudent = new Student
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

        if (res > 0)
        {
            Log.Information("Student added successfully. StudentId={StudentId}", newStudent.Id);
            return new Responce<string>(HttpStatusCode.Created, "Student added successfully");
        }

        Log.Warning("Student could not be added.");
        return new Responce<string>(HttpStatusCode.BadRequest, "Student could not be added");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error in AddStudent");
        return new Responce<string>(HttpStatusCode.InternalServerError, ex.Message);
    }
}


    public async Task<Responce<string>> UpdateStudent(UpdateStudent student)
    {
        try
        {
            Log.Information("UpdateStudent called: Id={Id}", student.Id);

            var updatStudent = await context.Students.FirstOrDefaultAsync(x => x.Id == student.Id && !x.IsDeleted);

            if (updatStudent == null)
            {
                Log.Warning("Student not found. Id={Id}", student.Id);
                return new Responce<string>(HttpStatusCode.NotFound, "Student not found");
            }

            Log.Information("Updating Student {Id}. OldValues: {OldFirstName} {OldLastName}", 
                updatStudent.Id, updatStudent.FirstName, updatStudent.LastName);

            updatStudent.FirstName = student.FirstName;
            updatStudent.LastName = student.LastName;
            updatStudent.BirthDate = student.BirthDate;
            updatStudent.Gender = student.Gender;
            updatStudent.FacultyId = student.FacultyId;
            updatStudent.SpecialtyId = student.SpecialtyId;
            updatStudent.UpdatedDate = DateTime.UtcNow;

            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                Log.Information("Student updated successfully. Id={Id}", student.Id);
                return new Responce<string>(HttpStatusCode.OK, "Student updated successfully");
            }

            Log.Warning("Student update failed. Id={Id}", student.Id);
            return new Responce<string>(HttpStatusCode.BadRequest, "Student could not be updated");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in UpdateStudent");
            return new Responce<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }


    public async Task<Responce<string>> DeleteStudent(int id)
    {
        try
        {
            Log.Information("DeleteStudent called. Id={Id}", id);

            var student = await context.Students.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (student == null)
            {
                Log.Warning("Student not found for delete. Id={Id}", id);
                return new Responce<string>(HttpStatusCode.NotFound, "Student not found");
            }

            student.IsDeleted = true;

            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                Log.Warning("Student marked as deleted. Id={Id}", id);
                return new Responce<string>(HttpStatusCode.OK, "Student deleted successfully");
            }

            Log.Warning("Delete failed. Id={Id}", id);
            return new Responce<string>(HttpStatusCode.BadRequest, "Student could not be deleted");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in DeleteStudent");
            return new Responce<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }


    public async Task<Responce<GetStudent>> GetStudentById(int id)
    {
        try
        {
            Log.Information("Getting Student");
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
            Log.Error("Error in GetStudentById");
            return new Responce<GetStudent>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetStudent>>> GetStudents(StudentFilter filter)
{
    try
    {
        Log.Information("GetStudents called with Filter: {@Filter}", filter);
        var query = context.Students.AsQueryable();

        if (filter.Id.HasValue)
            query = query.Where(x => x.Id == filter.Id);

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            query = query.Where(x => x.FirstName.ToLower().Contains(filter.FirstName.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.LastName))
            query = query.Where(x => x.LastName.ToLower().Contains(filter.LastName.ToLower()));

        if (filter.BirthDate.HasValue)
            query = query.Where(x => x.BirthDate == filter.BirthDate);

        if (filter.Gender.HasValue)
            query = query.Where(x => x.Gender == filter.Gender);

        if (filter.FacultyId.HasValue)
            query = query.Where(x => x.FacultyId == filter.FacultyId);

        if (filter.SpecialtyId.HasValue)
            query = query.Where(x => x.SpecialtyId == filter.SpecialtyId);

        query = query.Where(x => !x.IsDeleted);

        var total = await query.CountAsync();
        var skip = (filter.PageNumber - 1) * filter.PageSize;
        var students = await query.Skip(skip).Take(filter.PageSize).ToListAsync();

        if (students.Count == 0)
        {
            Log.Warning("No students found with filter={@Filter}", filter);
            return new PaginationResponce<List<GetStudent>>(HttpStatusCode.NotFound, "No students found");
        }
        Log.Information("{Count} students found.", students.Count);
        var dtos = students.Select(x => new GetStudent
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

        return new PaginationResponce<List<GetStudent>>(dtos, total, filter.PageNumber, filter.PageSize);
    }
    catch (Exception ex)
    {
        Log.Error("Error in GetStudents");
        return new PaginationResponce<List<GetStudent>>(HttpStatusCode.InternalServerError, ex.Message);
    }
}

}