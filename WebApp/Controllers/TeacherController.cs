using Domain.DTOs.TeacherDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherController(ITeacherService service) : Controller
{
    [HttpPost]
    public async Task<IActionResult> AddTeacher([FromBody] AddTeacher teacher)
    {
        var res = await service.AddTeacher(teacher);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTeacher([FromBody] UpdateTeacher teacher)
    {
        var res = await service.UpdateTeacher(teacher);
        return StatusCode((int)res.StatusCode, res);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var res = await service.DeleteTeacher(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var res = await service.GetTeacher(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> GetTeachers([FromQuery] TeacherFilter filter)
    {
        var res = await service.GetTeachers(filter);
        return StatusCode((int)res.StatusCode, res);
    }
}