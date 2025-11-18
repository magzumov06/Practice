using Domain.DTOs.GroupDto;
using Domain.Filter;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IGroupService
{
    Task<Responce<string>> CreateGroup(CreateGroup group);
    Task<Responce<string>> UpdateGroup(UpdateGroup group);
    Task<Responce<string>> DeleteGroup(int id);
    Task<Responce<GetGroup>> GetGroupById(int id);
    Task<PaginationResponce<List<GetGroup>>> GetGroups(GroupFilter filter);
}