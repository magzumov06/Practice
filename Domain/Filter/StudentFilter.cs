using Domain.Enum;

namespace Domain.Filter;

public class StudentFilter : BaseFilter
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public int? FacultyId { get; set; }
    public int? SpecialtyId { get; set; }
}