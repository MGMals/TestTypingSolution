using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TestTypingApi.Models;
using TestTypingApi.Models.Context;
using TestTypingApi.Models.DB;
using TestTypingApi.Service;

namespace TestTypingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(TestTypingSpeedsDbContext db, PasswordService passwordService, TokenService tokenService) : ControllerBase
    {
        private readonly TestTypingSpeedsDbContext _db = db;
        private readonly PasswordService _passwordService = passwordService;
        private readonly TokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var email = model.Email.Trim().ToLower();

            if (await _db.TestTypeUsers.AnyAsync(u => u.Email == email))
                return BadRequest("Email already registered");

            var user = new TestTypeUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = email,
                Country = model.Country,
                PasswordHash = _passwordService.HashPassword(model.Password)
            };

            try
            {
                _db.TestTypeUsers.Add(user);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("User with this email or username already exists");
            }

            return Ok(new { message = "User registered" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //Normalize before checking:
            var email = model.Email.Trim().ToLower();

            var user = await _db.TestTypeUsers
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !_passwordService.VerifyPassword(user, model.Password))
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new { token });

           
        }


    }
}
