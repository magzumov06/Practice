using Domain.DTOs.FacyltyDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FacultyController(IFacultyService service) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Add(AddFaculty faculty)
    {
        var res = await service.AddFaculty(faculty);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateFaculty faculty)
    {
        var res = await service.UpdateFaculty(faculty);
        return StatusCode((int)res.StatusCode, res);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeleteFaculty(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetFacultyById(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var res = await service.GetAllFaculties();
        return StatusCode((int)res.StatusCode,res);
    }
}