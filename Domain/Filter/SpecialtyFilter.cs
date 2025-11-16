namespace Domain.Filter;

public class SpecialtyFilter : BaseFilter
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? FacultyId { get; set; }
}