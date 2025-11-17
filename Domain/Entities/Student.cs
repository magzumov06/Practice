using Domain.Enum;

namespace Domain.Entities;

public class Student : BaseEntities
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }
}