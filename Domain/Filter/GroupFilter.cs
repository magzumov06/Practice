namespace Domain.Filter;

public class GroupFilter : BaseFilter
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? SpecialtyId { get; set; }
    public int? MentorId { get; set; }
    public int? FacultyId { get; set; }
}