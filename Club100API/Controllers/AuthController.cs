using Club100API.Models;
using Club100API.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Club100API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] MemberLoginRequest login)
        {
            // 1. Validate the user (In a real scenario, check your database here)
            // Here we use a simple hardcoded check:
            if (login.Username == "admin" && login.Password == "1234")
            {
                var role = "Admin";
                var token = GenerateJwtToken(login.Username, role);
                return Ok(new { token, role });
            }
            else if (login.Username == "anders" && login.Password == "Secret12")
            {
                var role = "User";
                var token = GenerateJwtToken(login.Username, role);
                return Ok(new { token, role });
            }
            else if (login.Username == "peter" && login.Password == "password")
            {
                var role = "User";
                var token = GenerateJwtToken(login.Username, role);
                return Ok(new { token, role });
            }

            return Unauthorized("Invalid username or password.");
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims are the pieces of information "baked" into the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role) // You can add roles here
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Token is valid for 2 hours
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}