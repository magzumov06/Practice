namespace Domain.DTOs.StudentDto;

public class GetStudent : UpdateStudent
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}