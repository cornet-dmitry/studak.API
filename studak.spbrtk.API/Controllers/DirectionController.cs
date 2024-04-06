using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studak.spbrtk.API.Context;
using studak.spbrtk.API.DTO;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class DirectionController : Controller
{

    private readonly IcawqbetContext _context;

    public DirectionController(IcawqbetContext context)
    {
        _context = context;
    }

    [HttpGet("GetDirection")]
    public async Task<ActionResult> GetEvents()
    {
        try
        {
            var events = await _context.Directions
                .Select(x => new Direction()
                {
                    DirectionShortName = x.DirectionShortName,
                    DirectionLongName = x.DirectionLongName
                })
                .ToListAsync();

            return Ok(events);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost("AddDirection")]
    public async Task<ActionResult> AddDirection(
        [FromForm] string directionShortName,
        [FromForm] string directionLongName)
    {
        try
        {
            Direction direction = new Direction();
            direction.DirectionShortName = directionShortName;
            direction.DirectionLongName = directionLongName;

            _context.Directions.Add(direction);
            _context.SaveChanges();
            
            return Ok(direction);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost("EditDirection/{id}")]
    public async Task<ActionResult<Direction>> EditDirection(int id,
        [FromForm] string directionShortName,
        [FromForm] string directionLongName
        )
    {
        try
        {
            var existingDirection = await _context.Directions.FindAsync(id);
            
            if (existingDirection == null)
            {
                return NotFound($"Направления с ID {id} не найдено");
            }

            existingDirection.DirectionShortName = directionShortName;
            existingDirection.DirectionLongName = directionLongName;
        
            _context.Directions.Update(existingDirection);
            await _context.SaveChangesAsync();

            return Ok(existingDirection);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpDelete("DeleteUserStatus/{id}")]
    public async Task<ActionResult> DeleteUserStatus(int id)
    {
        try
        {
            var direction = await _context.Directions.FindAsync(id);

            if (direction == null)
            {
                return NotFound();
            }

            _context.Directions.Remove(direction);
            await _context.SaveChangesAsync();

            return Ok(direction);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}