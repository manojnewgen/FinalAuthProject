using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using FinalAuthProject.Services;
using FinalAuthProject.Models;

namespace FinalAuthProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _authService.Authenticate(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = _authService.GenerateJwtToken(user);

            return Ok(new
            {
                Username = user.UserName,
                Role = user.Role,
                Token = token
            });
        }
    }
}