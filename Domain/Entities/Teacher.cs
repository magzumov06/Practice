namespace Domain.Entities;

public class Teacher : BaseEntities
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public int FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    
}