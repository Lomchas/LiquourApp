using LiquourApp.Models.Auth;
using LiquourApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiquourApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(model);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar edad
            if (!_authService.IsAdult(model.DateOfBirth))
            {
                return BadRequest(new { Success = false, Message = "Debes ser mayor de 18 años para registrarte" });
            }

            var result = await _authService.Register(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("verify-age")]
        public IActionResult VerifyAge([FromQuery] DateTime dateOfBirth)
        {
            var isAdult = _authService.IsAdult(dateOfBirth);

            return Ok(new { IsAdult = isAdult });
        }
    }
}