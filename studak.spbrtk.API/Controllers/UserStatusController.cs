using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studak.spbrtk.API.Context;
using studak.spbrtk.API.DTO;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserStatusController : Controller
{
    
    private readonly IcawqbetContext _context;

    public UserStatusController(IcawqbetContext context)
    {
        _context = context;
    }
    
    [HttpGet("GetUserStatus")]
    public async Task<ActionResult> GetUserStatus()
    {
        var events = await _context.UserStatuses
            .Select(x => new UserStatusDTO()
            {
                Id = x.Id,
                StatusName = x.StatusName
            })
            .OrderBy(x => x.Id)
            .ToListAsync();

        return Ok(events);
    }
    
    [HttpPost("AddUserStatus")]
    public async Task<ActionResult> AddUserStatus([FromForm] string statusName)
    {
        try
        {
            UserStatus userStatus = new UserStatus();
            userStatus.StatusName = statusName;

            _context.UserStatuses.Add(userStatus);
            _context.SaveChanges();
            
            return Ok(userStatus);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost("EditUserStatus/{id}")]
    public async Task<ActionResult> EditUserStatus(int id, [FromForm] string statusName)
    {
        try
        {
            var userStatus = await _context.UserStatuses.FindAsync(id);

            if (userStatus == null)
            {
                return NotFound();
            }

            userStatus.StatusName = statusName;
            _context.UserStatuses.Update(userStatus);
            await _context.SaveChangesAsync();

            return Ok(userStatus);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpDelete("DeleteUserStatus/{id}")]
    public async Task<ActionResult> DeleteUserStatus(int id)
    {
        try
        {
            var userStatus = await _context.UserStatuses.FindAsync(id);

            if (userStatus == null)
            {
                return NotFound();
            }

            _context.UserStatuses.Remove(userStatus);
            await _context.SaveChangesAsync();

            return Ok(userStatus);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

}