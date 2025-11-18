namespace Domain.DTOs.StudentGroupDto;

public class UpdateStudentGroup 
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public bool IsActive { get; set; } =  true;
    public bool IsLeft { get; set; } =  false;
}