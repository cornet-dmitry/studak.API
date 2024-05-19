using System.Collections.Generic;
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
    
    public class KPIController : Controller
    {
        
        private readonly IcawqbetContext _context;
    
        public KPIController(IcawqbetContext context)
        {
            _context = context;
        }
    
        [HttpGet("GetUsersKPI")]
        public async Task<ActionResult> GetUsersKPI()
        {
            var active = await _context.Users
                .Select(x => new User()
                {
                    Id = x.Id,
                    Kpi = x.Kpi
                })
                .ToListAsync();
            
            if (active.Count > 0)
            {
                return Ok(active);
            }
            return BadRequest();
        }
        
        [HttpPost("ChangeUserKPI")]
        public async Task<ActionResult> ChangeUserKPI(
            [FromForm] string userId,
            [FromForm] string changeAmount)
        {
            var user = await _context.Users.FindAsync(Convert.ToInt32(userId));
    
            if (user == null)
            {
                return NotFound($"Пользователь с ID {userId} не найден");
            }
    
            user.Kpi += Convert.ToInt32(changeAmount);
    
            _context.Users.Update(user);
            await _context.SaveChangesAsync(); 
    
            return Ok(user);
        }
        
        [HttpGet("GetUserWithHighestKPI")]
        public async Task<ActionResult<User>> GetUserWithHighestKPI()
        {
            var userWithHighestKPI = await _context.Users.OrderByDescending(u => u.Kpi).FirstOrDefaultAsync();
    
            if (userWithHighestKPI == null)
            {
                return NotFound("Пользователи не найдены");
            }
    
            return Ok(userWithHighestKPI);
        }
    
        [HttpGet("GetTopUsersWithHighestKPI")]
        public async Task<ActionResult<List<User>>> GetTopUsersWithHighestKPI()
        {
            var topUsersWithHighestKPI = await _context.Users.OrderByDescending(u => u.Kpi).Take(10).ToListAsync();
    
            if (topUsersWithHighestKPI == null || !topUsersWithHighestKPI.Any())
            {
                return NotFound("Пользователи не найдены");
            }
    
            return Ok(topUsersWithHighestKPI);
        }
    
    }
}

