using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyTestingEnvironment.Models.Identity;
using StudyTestingEnvironment.Services.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpPost("confirm-registry")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmRegistry([FromBody] ConfirmRegistryRequest confirmRegistry)
        {
            var confirmed = await _accountService.ConfirmRegistry(confirmRegistry);
            if (confirmed) return Ok();
            return BadRequest();
        }

        [HttpGet("send-confirm-letter")]
        [AllowAnonymous]
        public async Task<IActionResult> SendConfirmRegistryLetter([FromQuery, EmailAddress] string email)
        {
            var sent = await _accountService.SendConfirmRegistryLetter(email);
            return sent ? Ok() : BadRequest();
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPassword)
        {
            var result = await _accountService.ForgotPassword(forgotPassword);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            var result = await _accountService.ResetPassword(resetPassword);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result);
        }
    }
}