namespace Domain.DTOs.SpecialityDto;

public class AddSpeciality
{
    public required string Name { get; set; }
    public required string SpecialityLanguage { get; set; }
    public int FacultyId { get; set; }
}