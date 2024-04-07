using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using studak.spbrtk.API.Context;
using studak.spbrtk.API.Models;
using Microsoft.EntityFrameworkCore;
using studak.spbrtk.API.Context;
using studak.spbrtk.API.DTO;
using studak.spbrtk.API.Models;


namespace studak.spbrtk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Admin user = new Admin();
        private readonly IConfiguration _configuration;
        private readonly IcawqbetContext _context;

        public AuthController(IConfiguration configuration, IcawqbetContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        /*  В методе Register происходит регистрация нового пользователя
            Пользователь передаёт связку ключ:значение (key:value)
            Сначала проверяется, если ли уже пользователь с таким именем. В случае, если нет -
            пароль передаётся в метод CreatePasswordHash(), который хэширует его, а также создаёт
            соль-ключ (Silt) для безопасности и возможности дальнейшей верификации
            После этого данные заносятся новой записью в БД
         */
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(
            [FromForm] int userId,
            [FromForm] string userLogin,
            [FromForm] string password)
        {
            var verify = await _context.Admins
                .Where(x => x.Userid == userId)
                .ToListAsync();
            
            if (verify.Count > 0)
                return BadRequest("Пользователь уже зарегистрирован!");
            
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Userid = userId;
            user.Userlogin = userLogin;
            user.Userpasswordhash = passwordHash;
            user.Userpasswordsalt = passwordSalt;

            _context.Admins.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
        
        /*  Ниже представлен метод Login (авторизация пользователя)
            Пользователь передаёт связку ключ:значение (key:value)
            По имени пользователя ищется запись в БД, из которой берётся Hash пароля и Silt пароля,
            которые вместе с введённым паролем пользователя при авторизации передаются в метод VerifyPasswordHash,
            где генерируется Hash на введёный пароль по ключу соли (Silt) и сравнивается на Истину
            
            Если Hash's совпали, тогда создаётся токен сессии, рефреш токен,
            после чего обновляется запись о пользователе в БД в соответствии с новыми данными,
            а именно рефреш токен, время его создания и время его жизни.
            
            Возвращается актуальный токен сессии (время жизни 5 минут)
         */
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(
            [FromForm] string username,
            [FromForm] string password)
        {
            var verify = await _context.Admins
                .Where(x => x.Userlogin == username) 
                .ToListAsync();
            
            if (verify.Count == 0)
                return BadRequest("Пользователь не найден!");
            
            foreach (var v in verify)
            {
                if (VerifyPasswordHash(password, v.Userpasswordhash, v.Userpasswordsalt) == false)
                    return BadRequest("Неверное имя пользователя или пароль!");
            }
            
            string token = CreateToken(verify[0]);

            await _context.SaveChangesAsync();
            return Ok(token);
        }

        private string CreateToken(Admin user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Userid)),
                new Claim(ClaimTypes.Name, user.Userlogin)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        
        

        /*  Метод GetLastId служит для получения последнего значения ID в таблице,
            т.к. автоинкремент не работает :(
         */
        private int GetLastId()
        {
            int lastId = _context.Users
                .OrderByDescending(u => u.Id)
                .Select(u => u.Id)
                .FirstOrDefault();

            return ++lastId;
        }
    }
}
