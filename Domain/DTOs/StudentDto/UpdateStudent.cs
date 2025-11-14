using Domain.Enum;

namespace Domain.DTOs.StudentDto;

public class UpdateStudent
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public int FacultyId { get; set; }
    public int SpecialtyId { get; set; }
}