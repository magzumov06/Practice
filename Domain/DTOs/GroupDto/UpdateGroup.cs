namespace Domain.DTOs.GroupDto;

public class UpdateGroup
{

    public int Id { get; set; }
    public required string Name { get; set; }
    public int SpecialtyId { get; set; }
    public int MentorId { get; set; }
    public int FacultyId { get; set; }
    public TimeOnly? LessonStartTime { get; set; }
    public TimeOnly? LessonEndTime { get; set; }
}