namespace Domain.DTOs.StudentGroupDto;

public class GetStudentGroup : UpdateStudentGroup
{
    public int StudentId { get; set; }
    public DateTime JoinedDate { get; set; }
    public DateTime LeaveDate { get; set; } = DateTime.UtcNow;
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; } 
}