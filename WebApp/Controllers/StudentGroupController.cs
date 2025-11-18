using Domain.DTOs.StudentGroupDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]

public class StudentGroupController(IStudentGroupService service) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddStudentGroup studentGroup)
    {
        var res = await service.AddStudentGroup(studentGroup);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateStudentGroup studentGroup)
    {
        var res = await service.UpdateStudentGroup(studentGroup);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeleteStudentGroup(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetStudentGroupById(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] StudentGroupFilter filter)
    {
        var res =  await service.GetStudentGroup(filter);
        return StatusCode((int)res.StatusCode, res);
    }
}