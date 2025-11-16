namespace Domain.DTOs.FacyltyDto;

public class GetFaculty : UpdateFaculty
{
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}