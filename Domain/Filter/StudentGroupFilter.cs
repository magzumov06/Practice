namespace Domain.Filter;

public class StudentGroupFilter : BaseFilter
{
    public int? Id { get; set; }
    public int? StudentId { get; set; }
    public int? GroupId { get; set; }
    public bool? IsActive { get; set; } 
    public bool? IsLeft { get; set; }
}