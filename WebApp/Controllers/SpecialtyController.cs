using Domain.DTOs.SpecialityDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SpecialtyController(ISpecialtyService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSpecialty(AddSpeciality specialty)
    {
        var res = await service.AddSpecialty(specialty);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSpecialty(UpdateSpeciality specialty)
    {
        var res = await service.UpdateSpecialty(specialty);
        return StatusCode((int)res.StatusCode, res);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteSpecialty(int id)
    {
        var res = await service.DeleteSpecialty(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecialty(int id)
    {
        var res = await service.GetSpecialtyId(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> GetSpecialties([FromQuery]SpecialtyFilter filter)
    {
        var res = await service.GetSpecialties(filter);
        return StatusCode((int)res.StatusCode, res);
    }
}