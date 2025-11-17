using System.Net;
using Domain.DTOs.Group;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Group = Domain.Entities.Group;

namespace Infrastructure.Services;

public class GroupService(DataContext context) : IGroupService
{
    public async Task<Responce<string>> CreateGroup(CreateGroup group)
    {
        try
        {
            var existsMentor = await context.Teachers.FirstOrDefaultAsync(x=> x.Id == group.MentorId && x.IsDeleted == false);
            if (existsMentor == null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Oмузгор вуҷуд надорад!");
            
            var existsFaculty = await context.Faculties.FirstOrDefaultAsync(x => x.Id == group.FacultyId  && x.IsDeleted == false);
            if (existsFaculty == null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Хатоги ҳангоми интихоби факултет!");
            
            var existsSpecialty = await context.Specialties.FirstOrDefaultAsync(x=> x.Id == group.SpecialtyId  && x.IsDeleted == false);
            if (existsSpecialty == null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Чунин ихтисос вуҷуд надорад!");
                
            var newGroup = new Group()
            {
                Name = group.Name,
                SpecialtyId = group.SpecialtyId,
                MentorId = group.MentorId,
                FacultyId = group.FacultyId,
                LessonStartTime = group.LessonStartTime,
                LessonEndTime = group.LessonEndTime,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            await context.Groups.AddAsync(newGroup);
            var res = await context.SaveChangesAsync();
            return res > 0 
                ? new Responce<string>(HttpStatusCode.Created,"Group created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Group not found");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateGroup(UpdateGroup group)
    {
        try
        {
            var  existsGroup = await context.Groups.FirstOrDefaultAsync(x=> x.Id == group.Id && x.IsDeleted == false);
            if (existsGroup == null)
                 return new Responce<string>(HttpStatusCode.NotFound,"group not found");
            existsGroup.Name = group.Name;
            existsGroup.MentorId = group.MentorId;
            existsGroup.FacultyId = group.FacultyId;
            existsGroup.SpecialtyId = group.SpecialtyId;
            existsGroup.LessonStartTime = group.LessonStartTime;
            existsGroup.LessonEndTime = group.LessonEndTime;
            existsGroup.UpdatedDate = DateTime.UtcNow;
            var res  = await context.SaveChangesAsync();
            return res > 0 
                ? new Responce<string>(HttpStatusCode.OK,"Group updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Something went wrong");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteGroup(int id)
    {
        try
        {
            var existsGroup = await context.Groups.FirstOrDefaultAsync(x=> x.Id == id && x.IsDeleted == false);
            if (existsGroup == null)
                return new Responce<string>(HttpStatusCode.NotFound,"group not found");
            existsGroup.IsDeleted = true;
            existsGroup.UpdatedDate = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Group deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Something went wrong");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Responce<GetGroup>> GetGroupById(int id)
    {
        try
        {
            var existsGroup = await context.Groups.FirstOrDefaultAsync(x=> x.Id == id && x.IsDeleted == false);
            if (existsGroup == null)
                return new Responce<GetGroup>(HttpStatusCode.NotFound,"group not found");
            var dto = new GetGroup()
            {
                Id = existsGroup.Id,
                Name = existsGroup.Name,
                MentorId = existsGroup.MentorId,
                FacultyId = existsGroup.FacultyId,
                SpecialtyId = existsGroup.SpecialtyId,
                LessonStartTime = existsGroup.LessonStartTime,
                LessonEndTime = existsGroup.LessonEndTime,
                CreatedDate = existsGroup.CreatedDate,
                UpdatedDate = existsGroup.UpdatedDate,
            };
            return new Responce<GetGroup>(dto);
        }
        catch (Exception e)
        {
            return new Responce<GetGroup>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetGroup>>> GetGroups(GroupFilter filter)
    {
        try
        {
            var query = context.Groups.AsQueryable();
            
            if(filter.Id.HasValue) 
                query = query.Where(x => x.Id == filter.Id);
            
            if(!string.IsNullOrEmpty(filter.Name))
                query = query.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            
            if(filter.SpecialtyId.HasValue)
                query = query.Where(x => x.SpecialtyId == filter.SpecialtyId);
            
            if(filter.MentorId.HasValue)
                query = query.Where(x => x.MentorId == filter.MentorId);
            
            if(filter.FacultyId.HasValue)
                query = query.Where(x => x.FacultyId == filter.FacultyId);
            
            query = query.Where(x=> x.IsDeleted == false);
            var totalCount = await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var group = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(group.Count == 0)
                return new PaginationResponce<List<GetGroup>>(HttpStatusCode.NotFound,"group not found");
            var dtos = group.Select(x=> new GetGroup()
            {
                Id = x.Id,
                Name = x.Name,
                MentorId = x.MentorId,
                FacultyId = x.FacultyId,
                SpecialtyId = x.SpecialtyId,
                LessonStartTime = x.LessonStartTime,
                LessonEndTime = x.LessonEndTime,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
            }).ToList();
            return new PaginationResponce<List<GetGroup>>(dtos, totalCount, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PaginationResponce<List<GetGroup>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}