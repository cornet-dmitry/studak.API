using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studak.spbrtk.API.Context;
using studak.spbrtk.API.DTO;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Controllers
{
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
                Id = x.Id,
                Responsible = x.ResponsibleNavigation.Surname + " " + x.ResponsibleNavigation.Name,
                Direction = x.DirectionNavigation.DirectionLongName,
                Name = x.Name,
                Description = x.Description,
                Place = x.Place,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Rate = x.Rate,
                Isactice = x.Isactice
            })
            .OrderBy(x => x.StartDate)
            .OrderBy(x => x.StartTime)
            .ToListAsync();

        return Ok(events);
    }
    
    [HttpPost("AddEvent")]
    public async Task<ActionResult<EventDTO>> AddEvent(
        [FromForm] string responsible,
        [FromForm] string direction,
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] string place,
        [FromForm] string startDate,
        [FromForm] string endDate,
        [FromForm] string startTime,
        [FromForm] string endTime,
        [FromForm] string rate,
        [FromForm] bool isActive
        )
    {
        var newEvent = new Event
        {
            Responsible = Convert.ToInt32(responsible),
            Direction = Convert.ToInt32(direction),
            Name = name,
            Description = description,
            Place = place,
            StartDate = DateTime.Parse(startDate),
            EndDate = DateTime.Parse(endDate),
            StartTime = DateTime.Parse(startTime),
            EndTime = DateTime.Parse(endTime),
            Rate = Convert.ToInt32(rate),
            Isactice = Convert.ToBoolean(isActive)
            // Добавьте другие поля, если необходимо
        };

        // Добавление нового события в контекст и сохранение изменений
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return Ok(newEvent);
    }
    
    [HttpPost("EditEvent/{id}")]
    public async Task<ActionResult<EventDTO>> EditEvent(int id,
        [FromForm] string name,
        [FromForm] string description,
        [FromForm] string place,
        [FromForm] DateTime startDate,
        [FromForm] DateTime endDate,
        [FromForm] DateTime startTime,
        [FromForm] DateTime endTime,
        [FromForm] string rate,
        [FromForm] string isActive
    )
    {
        try
        {
            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }

            events.Responsible = events.Responsible;
            events.Direction = events.Direction;
            events.Name = name;
            events.Description = description;
            events.Place = place;
            events.StartDate = startDate;
            events.EndDate = endDate;
            events.StartTime = startTime;
            events.EndTime = endTime;
            events.Rate = Convert.ToInt32(rate);
            events.Isactice = Convert.ToBoolean(isActive);
            
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
    public async Task<ActionResult> DeleteEvent(string id)
    {
        try
        {
            var events = await _context.Events.FindAsync(Convert.ToInt32(id));
 
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
}

