namespace Domain.Entities;

public class Specialty : BaseEntities
{
    public required string Name { get; set; }
    public int FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    public  List<Student>? Students { get; set; }
}