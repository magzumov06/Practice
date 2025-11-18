using Domain.DTOs.StudentGroupDto;
using Domain.Entities;

namespace Domain.DTOs.GroupDto;

public class GetGroup : UpdateGroup
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<GetStudentGroup> StudentGroups { get; set; }

}