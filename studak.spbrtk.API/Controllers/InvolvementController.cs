using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studak.spbrtk.API.Context;
using studak.spbrtk.API.DTO;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class InvolvementController : Controller
{

    private readonly IcawqbetContext _context;

    public InvolvementController(IcawqbetContext context)
    {
        _context = context;
    }

    [HttpPost("GetInvolvementByEventId")]
    public async Task<ActionResult> GetInvolvementByEventId([FromForm] int id)
    {
        var events = await _context.Involvements
            .Where(x => x.Eventid == id)
            .Select(x => new Involvement()
            {
                Eventid = x.Eventid,
                Userid = x.Userid,
                Status = x.Status,
                Createtime = x.Createtime
            })
            .ToListAsync();

        return Ok(events);
    }
    
    [HttpGet("GetInvolvement")]
    public async Task<ActionResult> GetInvolvement()
    {
        var events = await _context.Involvements
            .Select(x => new Involvement()
            {
                Eventid = x.Eventid,
                Userid = x.Userid,
                Status = x.Status,
                Createtime = x.Createtime
            })
            .ToListAsync();

        return Ok(events);
    }

    [HttpPost("AddInvolvement")]
    public async Task<ActionResult> AddInvolvement(
        [FromForm] int eventID,
        [FromForm] int userID
        )
    {
        try
        {
            var check = await _context.Involvements
                .Where(x => x.Eventid == eventID)
                .Where(x => x.Userid == userID).ToListAsync();

            if (check.Count > 0)
            {
                return BadRequest($"Пользователь {userID} уже подал заявку на мероприятие {eventID}");
            }
            
            Involvement involvement = new Involvement()
            {
                Eventid = eventID,
                Userid = userID,
                Status = 1,
                Createtime = DateTime.Now
            };

            _context.Involvements.Add(involvement);
            _context.SaveChanges();

            return Ok(involvement);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    [HttpPost("EditInvolvementStatus")]
    public async Task<ActionResult> EditInvolvementStatus(
        [FromForm] int eventID,
        [FromForm] int userID,
        [FromForm] int statusID
    )
    {
        try
        {
            var check = await _context.Involvements
                .Where(x => x.Eventid == eventID)
                .Where(x => x.Userid == userID).ToListAsync();

            if (check.Count == 0)
            {
                return BadRequest($"Пользователь {userID} не оставлял заявку на мероприятие {eventID}");
            }
            
            Involvement involvement = new Involvement()
            {
                Eventid = eventID,
                Userid = userID,
                Status = statusID,
                Createtime = DateTime.Now
            };

            _context.Involvements.Update(involvement);
            _context.SaveChanges();

            return Ok(involvement);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}