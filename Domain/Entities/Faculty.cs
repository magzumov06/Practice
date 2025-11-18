namespace Domain.Entities;

public class Faculty : BaseEntities
{
    public required string Name { get; set; }
    public required string DecanName { get; set; }
    public  List<Specialty> Specialties { get; set; }
    public List<Student> Students { get; set; }
    public List<Teacher> Teachers { get; set; }
    public  List<Groups> Groups { get; set; }
}