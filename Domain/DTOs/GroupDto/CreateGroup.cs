using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.GroupDto;

public class CreateGroup
{
    [Required]
    public required string Name { get; set; }
    public int SpecialtyId { get; set; }
    public int MentorId { get; set; }
    public int FacultyId { get; set; }
    public TimeOnly? LessonStartTime { get; set; }
    public TimeOnly? LessonEndTime { get; set; }
}