using EmployeeTaskManagement.DbContext;
using EmployeeTaskManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{ 
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _applicationDbContext;
    public AuthController( IConfiguration configuration, ApplicationDbContext applicationDbContext)
    { 
        _configuration = configuration;
        _applicationDbContext = applicationDbContext;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    { 
        var user = await _applicationDbContext.UserModels
        .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null || user.Password != model.Password)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        } 
        var roles = user.Role ?? "Employee";

        var secretKey = _configuration["JWT:SecretKey"];
        var issuer = _configuration["JWT:Issuer"];
        var audience = _configuration["JWT:Audience"];
        //if (roles.Contains("Admin"))
       // { 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                   new Claim("UserId", user.UserId.ToString()),
                   new Claim(ClaimTypes.Name, user.FirstName),
                   new Claim(ClaimTypes.Role, roles)
               }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { message = "Login successful for admin, token generated", Token = tokenHandler.WriteToken(token) , Role = roles, AssignedFromId = user.UserId });
       // }

       // return Ok(new { message = "Login successful for employee, no token generated" , Role = roles });
    }
}
 