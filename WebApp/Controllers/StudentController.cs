using Domain.DTOs.StudentDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController(IStudentServices services) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Post(AddStudent student)
    {
        var res = await services.AddStudent(student);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateStudent student)
    {
        var res = await services.UpdateStudent(student);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await services.DeleteStudent(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var res = await services.GetStudentById(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents([FromQuery] StudentFilter filter)
    {
        var res = await services.GetStudents(filter);
        return StatusCode((int)res.StatusCode, res);
    }

}