using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.DTOs.Group;

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