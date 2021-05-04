using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyTestingEnvironment.Models.Identity;
using StudyTestingEnvironment.Services.Identity;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            var loginResult = await _authService.Login(login);
            if (!loginResult.IsSuccess) return Unauthorized();            
            return Ok(loginResult);
        }

        [HttpPost("registry")]
        public async Task<IActionResult> Registry(RegistryRequest registry)
        {
            var registryResult = await _authService.Registry(registry);
            if (!registryResult.IsSuccess) return BadRequest(registryResult);
            return Ok(registryResult);
        }       

        [HttpPost("logout")]    
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] string fingerPrint)
        {
            var logout = await _authService.Logout(fingerPrint);
            return logout ? Ok() : BadRequest();
        }

        [HttpPost("logout-all")]
        [Authorize]
        public async Task<IActionResult> LogoutAll()
        {
            await _authService.LogoutFromAllDevices();
            return Ok();
        }        

        [HttpPost("refresh-token")]        
        public async Task<IActionResult> RefreshToken([FromBody] string fingerPrint)
        {
            var refreshResult = await _authService.RefreshToken(fingerPrint);
            if (!refreshResult.IsSuccess)
            {
                return BadRequest(refreshResult);
            }
            
            return Ok(refreshResult);
        }
    }
}