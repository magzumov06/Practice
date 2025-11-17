using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.TeacherDto;

public class AddTeacher
{
    public required string FullName { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public int FacultyId { get; set; }
}