namespace Domain.DTOs.TeacherDto;

public class GetTeacher : UpdateTeacher
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}