using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web_API.Models;
using Web_API.Models.Dto;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UsersController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet("Login")]
        public ActionResult Login(string email, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email) && u.Password.Equals(password));
                if(user == null)
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(new LoginDto
                    {
                        UserId = user.UserId,
                        Email = email,
                        Username = user.FirstName + user.LastName,
                        Role = "Teacher",
                        Token = GenerateJwtToken(user.UserId)
                    }); ;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Profile")]
        public ActionResult Profile(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(new
                    {
                        user.Email,
                        user.FirstName,
                        user.LastName
                    }); ;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProfile")]
        public ActionResult Register(ProfileDto profile)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u=>u.UserId ==  profile.UserId);
                if (user == null)
                {
                    return Unauthorized();
                }
                user.Email = profile.Email;
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                _context.Users.Update(user);
                _context.SaveChanges();
                return Ok("Success Update Profile");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Register")]
        public ActionResult Register(RegisterInput register)
        {
            try
            {
                var user = new User
                {
                    Email = register.Email,
                    Password = register.Password,
                    FirstName = register.FirstName,
                    LastName = register.LastName
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok("Success Register");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        private static string GenerateJwtToken(int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key_with_at_least_32_characters"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7181",
                audience: "https://localhost:7181",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
