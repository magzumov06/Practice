using Domain.Enum;

namespace Domain.DTOs.StudentDto;

public class AddStudent
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int FacultyId { get; set; }
    public int SpecialtyId { get; set; }
}