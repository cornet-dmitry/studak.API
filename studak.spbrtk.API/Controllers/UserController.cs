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

public class UserController : Controller
{
    
    private readonly IcawqbetContext _context;

    public UserController(IcawqbetContext context)
    {
        _context = context;
    }
    
    [HttpGet("GetUsers")]
    public async Task<ActionResult> GetUsers()
    {
        var active = await _context.Users
            .Where(x => x.Status == 1)
            .Select(x => new User()
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Group = x.Group,
                DateBirth = x.DateBirth,
                Phone = x.Phone,
                Email = x.Email,
                VkLink = x.VkLink,
                TgLink = x.TgLink,
                Kpi = x.Kpi,
                Status = x.Status,
                OrderNumber = x.OrderNumber,
                StartDate = x.StartDate
            })
            .OrderBy(x => x.Id)
            .ToListAsync();
        
        if (active.Count > 0)
        {
            return Ok(active);
        }
        return BadRequest();
    }
    
    [HttpGet("GetManagers")]
    public async Task<ActionResult> GetManagers()
    {
        var active = await _context.Users
            .Where(x => x.Status != 1)
            .Select(x => new UserDTO()
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Group = x.Group,
                DateBirth = x.DateBirth,
                Phone = x.Phone,
                Email = x.Email,
                VkLink = x.VkLink,
                TgLink = x.TgLink,
                Kpi = x.Kpi,
                Status = x.Status,
                OrderNumber = x.OrderNumber,
                StartDate = x.StartDate
            })
            .ToListAsync();

        if (active.Count > 0)
        {
            return Ok(active);
        }
        return BadRequest();
    }
    
    [HttpPost("GetUserByID")]
    public async Task<ActionResult<User>> GetUserByID([FromForm] string userId)
    {
        var active = await _context.Users
            .Where(x => x.Id == int.Parse(userId))
            .ToListAsync();

        if (active.Count > 0)
        {
            return Ok(active);
        }
        return BadRequest();
    }
    
    [HttpPost("AddUser")]
    public async Task<ActionResult> AddUser(
        [FromForm] string surname,
        [FromForm] string name,
        [FromForm] string patronymic,
        [FromForm] string group,
        [FromForm] string dateBirth,
        [FromForm] string phone,
        [FromForm] string email,
        [FromForm] string vkLink,
        [FromForm] string tgLink,
        [FromForm] string kpi,
        [FromForm] string status,
        [FromForm] string orderNumber,
        [FromForm] string startDate
        )
    {
        try
        {
            User user = new User();

            user.Surname = surname;
            user.Name = name;
            user.Patronymic = patronymic;
            user.Group = group;
            user.DateBirth = DateTime.Parse(dateBirth);
            user.Phone = phone;
            user.Email = email;
            user.VkLink = vkLink;
            user.TgLink = tgLink;
            user.Kpi = Convert.ToInt32(kpi);
            user.Status = Convert.ToInt32(status);
            user.OrderNumber = orderNumber;
            user.StartDate = DateTime.Parse(startDate);
        
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }

    }
    
    [HttpPost("EditUser/{id}")]
    public async Task<ActionResult> EditUser(string id,
        [FromForm] string surname,
        [FromForm] string name,
        [FromForm] string patronymic,
        [FromForm] string group,
        [FromForm] string dateBirth,
        [FromForm] string phone,
        [FromForm] string email,
        [FromForm] string vkLink,
        [FromForm] string tgLink,
        [FromForm] string kpi,
        [FromForm] string status,
        [FromForm] string orderNumber,
        [FromForm] string startDate
    )
    {
        try
        {
            var user = await _context.Users.FindAsync(Convert.ToInt32(id));

            if (user == null)
            {
                return NotFound();
            }

            user.Surname = surname;
            user.Name = name;
            user.Patronymic = patronymic;
            user.Group = group;
            user.DateBirth = DateTime.Parse(dateBirth);
            user.Phone = phone;
            user.Email = email;
            user.VkLink = vkLink;
            user.TgLink = tgLink;
            user.Kpi = Convert.ToInt32(kpi);
            user.Status = Convert.ToInt32(status);
            user.OrderNumber = orderNumber;
            user.StartDate = DateTime.Parse(startDate);
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    
    [HttpDelete("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}
}
