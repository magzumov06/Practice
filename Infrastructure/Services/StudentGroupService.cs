using System.Net;
using Domain.DTOs.StudentGroupDto;
using Domain.Entities;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class StudentGroupService(DataContext context) : IStudentGroupService
{
    public async Task<Responce<string>> AddStudentGroup(AddStudentGroup add)
    {
        try
        {
            var existsStudent = await context.Students.FirstOrDefaultAsync(x=>x.Id == add.StudentId && x.IsDeleted == false);
            if (existsStudent == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Донишҷу вуҷуд надорад!");
            
            var existsGroup = await context.Groups.FirstOrDefaultAsync(x => x.Id == add.GroupId && x.IsDeleted == false);
            if (existsGroup == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Гуруҳ вуҷуд надорад!");
            
            var doublStudent = await context.StudentGroups.FirstOrDefaultAsync(x => x.StudentId == add.StudentId && x.GroupId == add.GroupId && x.IsDeleted == false);
            if (doublStudent != null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Донишҷу алакай да гуруҳ аст!");
            
            var studentGroup = new StudentGroup()
            {
                StudentId = add.StudentId,
                GroupId = add.GroupId,
                JoinedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            await context.StudentGroups.AddAsync(studentGroup);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>($"StudentGroup {studentGroup.Id} has been added successfully")
                : new Responce<string>($"StudentGroup {studentGroup.Id} could not be added");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateStudentGroup(UpdateStudentGroup update)
    {
        try
        {
            var existsStudentGroup = await context.StudentGroups.FirstOrDefaultAsync(x => x.Id == update.Id && x.IsDeleted == false);
            if (existsStudentGroup == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Not found");
            existsStudentGroup.GroupId = update.GroupId;
            existsStudentGroup.IsActive = update.IsActive;
            existsStudentGroup.IsLeft = update.IsLeft;
            existsStudentGroup.UpdatedDate = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"StudentGroup has been updated successfully")
                : new Responce<string>(HttpStatusCode.BadRequest, "Not updated");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteStudentGroup(int id)
    {
        try
        {
            var studentGroup = await context.StudentGroups.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (studentGroup == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Not found");
            studentGroup.IsDeleted = true;
            studentGroup.IsLeft = true;
            studentGroup.LeaveDate = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "StudentGroup has been deleted successfully")
                : new Responce<string>(HttpStatusCode.BadRequest, "Not deleted");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Responce<GetStudentGroup>> GetStudentGroupById(int id)
    {
        try
        {
            var studentGroup = await context.StudentGroups.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (studentGroup == null)
                return new Responce<GetStudentGroup>(HttpStatusCode.NotFound, "Not found");
            var dto = new GetStudentGroup()
            {
                Id = studentGroup.Id,
                StudentId = studentGroup.StudentId,
                GroupId = studentGroup.GroupId,
                IsActive = studentGroup.IsActive,
                IsLeft = studentGroup.IsLeft,
                JoinedDate = studentGroup.JoinedDate,
                LeaveDate = studentGroup.LeaveDate,
                CreateDate = studentGroup.CreatedDate,
                UpdateDate = studentGroup.UpdatedDate
            };
            return new Responce<GetStudentGroup>(dto);
        }
        catch (Exception e)
        {
            return new Responce<GetStudentGroup>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetStudentGroup>>> GetStudentGroup(StudentGroupFilter filter)
    {
        try
        {
            var query = context.StudentGroups.AsQueryable();
            if(filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id.Value);
            if(filter.GroupId.HasValue)
                query = query.Where(x => x.GroupId == filter.GroupId.Value);
            if(filter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == filter.IsActive.Value);
            if(filter.IsLeft.HasValue)
                query = query.Where(x => x.IsLeft == filter.IsLeft.Value);
            query = query.Where(x=> x.IsDeleted == false);
            var total = await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var studentGroup  = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(studentGroup.Count == 0)
                return new PaginationResponce<List<GetStudentGroup>>(HttpStatusCode.NotFound,"No student groups found");
            var dtos = studentGroup.Select(x=> new GetStudentGroup()
            {
                Id = x.Id,
                StudentId = x.StudentId,
                GroupId = x.GroupId,
                IsActive = x.IsActive,
                IsLeft = x.IsLeft,
                LeaveDate = x.LeaveDate,
                JoinedDate = x.JoinedDate,
                CreateDate = x.CreatedDate,
                UpdateDate = x.UpdatedDate
            }).ToList();
            return new PaginationResponce<List<GetStudentGroup>>(dtos, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PaginationResponce<List<GetStudentGroup>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}