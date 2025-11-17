using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.TeacherDto;

public class UpdateTeacher
{
    public int Id { get; set; }
    public string FullName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public int FacultyId { get; set; }
}