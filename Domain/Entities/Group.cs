using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Group : BaseEntities
{
    [Required]
    public required string Name { get; set; }
    
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }
    
    public int MentorId { get; set; }
    public Teacher? Mentor { get; set; }
    
    public int FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    
    public TimeOnly? LessonStartTime { get; set; }
    public TimeOnly? LessonEndTime { get; set; }
    
    
}