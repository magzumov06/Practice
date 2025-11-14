namespace Domain.Entities;

public class Enrollment : BaseEntities
{
    public int StudentId { get; set; }
    public decimal Grade { get; set; }
    public Student? Student { get; set; }
}