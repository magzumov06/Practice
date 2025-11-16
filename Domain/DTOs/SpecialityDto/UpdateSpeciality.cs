namespace Domain.DTOs.SpecialityDto;

public class UpdateSpeciality
{
    public int Id { get; set; }
    public string Name { get; set; }
    public required string SpecialityLanguage { get; set; }

    public int FacultyId { get; set; }
}