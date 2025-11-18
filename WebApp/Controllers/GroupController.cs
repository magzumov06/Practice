using Domain.DTOs.GroupDto;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController(IGroupService service) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroup group)
    {
        var res = await service.CreateGroup(group);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroup group)
    {
        var res = await service.UpdateGroup(group);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var res = await service.DeleteGroup(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var res = await service.GetGroupById(id);
        return StatusCode((int)res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups([FromQuery]GroupFilter filter)
    {
        var res =  await service.GetGroups(filter);
        return StatusCode((int)res.StatusCode, res);
    }
}