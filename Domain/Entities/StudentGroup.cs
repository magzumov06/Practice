namespace Domain.Entities;

public class StudentGroup : BaseEntities
{
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    
    public int GroupId { get; set; }
    public Group? Group { get; set; }
    
    public bool IsActive { get; set; } =  true;
    public bool IsLeft { get; set; } =  false;
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    public DateTime LeaveDate { get; set; } = DateTime.UtcNow;
}