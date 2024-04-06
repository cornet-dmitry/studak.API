using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studak.spbrtk.API.Context;
using studak.spbrtk.API.DTO;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class EventController : Controller
{
    
    private readonly IcawqbetContext _context;

    public EventController(IcawqbetContext context)
    {
        _context = context;
    }
    
    [HttpGet("GetEvents")]
    public async Task<ActionResult> GetEvents()
    {
        var events = await _context.Events
            .Select(x => new EventDTO()
            {
                Responsible = x.ResponsibleNavigation.Surname + " " + x.ResponsibleNavigation.Name,
                Direction = x.DirectionNavigation.DirectionLongName,
                Name = x.Name,
                Description = x.Description,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Rate = x.Rate
            })
            .OrderBy(x => x.StartDate)
            .OrderBy(x => x.StartTime)
            .ToListAsync();

        return Ok(events);
    }
    
    [HttpPost("AddEvent")]
    public async Task<ActionResult<EventDTO>> AddEvent(
        [FromForm] int responsible,
        [FromForm] int direction,
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] DateTime startDate,
        [FromForm] DateTime endDate,
        [FromForm] DateTime startTime,
        [FromForm] DateTime endTime,
        [FromForm] int rate
        )
    {
        var newEvent = new Event
        {
            Responsible = responsible,
            Direction = direction,
            Name = name,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            StartTime = startTime,
            EndTime = endTime,
            Rate = rate
            // Добавьте другие поля, если необходимо
        };

        // Добавление нового события в контекст и сохранение изменений
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return Ok(newEvent);
    }
    
    [HttpPost("EditEvent/{id}")]
    public async Task<ActionResult<EventDTO>> EditEvent(int id,
        [FromForm] int responsible,
        [FromForm] int direction,
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] DateTime startDate,
        [FromForm] DateTime endDate,
        [FromForm] DateTime startTime,
        [FromForm] DateTime endTime,
        [FromForm] int rate
    )
    {
        try
        {
            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }

            events.Responsible = responsible;
            events.Direction = direction;
            events.Name = name;
            events.Description = description;
            events.StartDate = startDate;
            events.EndDate = endDate;
            events.StartTime = startTime;
            events.EndTime = endTime;
            events.Rate = rate;
            
            _context.Events.Update(events);
            await _context.SaveChangesAsync();

            return Ok(events);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("DeleteEvent/{id}")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        try
        {
            var events = await _context.Events.FindAsync(id);
 
            if (events == null)
            {
                return NotFound();
            }

            _context.Events.Remove(events);
            await _context.SaveChangesAsync();

            return Ok(events);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}