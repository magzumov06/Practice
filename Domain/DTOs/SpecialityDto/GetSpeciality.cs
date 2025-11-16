namespace Domain.DTOs.SpecialityDto;

public class GetSpeciality : UpdateSpeciality
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}