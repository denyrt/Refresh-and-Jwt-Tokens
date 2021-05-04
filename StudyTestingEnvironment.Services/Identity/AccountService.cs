using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyTestingEnvironment.Data.Models;
using StudyTestingEnvironment.Models.Common;
using StudyTestingEnvironment.Models.Identity;
using StudyTestingEnvironment.Models.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StudyTestingEnvironment.Services.Identity
{
    public class AccountService : IAccountService
    {
        private readonly AspNetUserManager<User> _aspNetUserManager;
        private readonly IPwdRecoveryCacheService _pwdRecoveryCacheService;
        private readonly IEmailService _emailService;
        private readonly FrontendRoutingOptions _frontendRoutingOptions;
        private readonly ResultFactory _resultFactory = new ResultFactory();

        public AccountService(AspNetUserManager<User> aspNetUserManager,
            IPwdRecoveryCacheService pwdRecoveryCacheService,
            IEmailService emailService,
            IOptions<FrontendRoutingOptions> frontendOptions)
        {
            _aspNetUserManager = aspNetUserManager;
            _pwdRecoveryCacheService = pwdRecoveryCacheService;
            _emailService = emailService;
            _frontendRoutingOptions = frontendOptions.Value;
        }

        public async Task<bool> ConfirmRegistry(ConfirmRegistryRequest request)
        {
            var user = await _aspNetUserManager.FindByEmailAsync(request.Email);
            if (user == null) return false;

            var confirmResult = await _aspNetUserManager.ConfirmEmailAsync(user, request.ConfirmToken);
            return confirmResult.Succeeded;
        }

        public async Task<bool> SendConfirmRegistryLetter(string email)
        {
            var user = await _aspNetUserManager.FindByEmailAsync(email);
            if (user == null || user.EmailConfirmed) return false;

            var confirmToken = await _aspNetUserManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmUrl = string.Format("{0}?token={1}&email={2}",
                _frontendRoutingOptions.ConfirmEmailUrl,
                confirmToken,
                user.Email);
            bool sent = await _emailService.SendMessage(user.Email,
                 subject: "Confirm Registry",
                 content: "Open this link in browser to confirm you mail: " + HttpUtility.HtmlEncode(confirmUrl),
                 htmlContent: $"<html><body>Click <a href=\"{confirmUrl}\">here</a> to confirm your mail.</html></body>");
            
            return sent;
        }

        public async Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _aspNetUserManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return _resultFactory.ForgotPasswordFailed("Something went wrong!");
            }

            var date = await _pwdRecoveryCacheService.GetMailExpiresDateUtc(request.Email);
            if (date != null)
            {
                return _resultFactory.ForgotPasswordFailed("You cannot recovery password now. Try after this 'timeout'.",
                    user.Email,
                    date.Value.Subtract(DateTime.Now).TotalSeconds);                
            }

            var token = await _aspNetUserManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = string.Format("{0}{1}", _frontendRoutingOptions.ResetPasswordUrl, token);
            var sent = await _emailService.SendMessage(user.Email,
                subject: "Password Recovery",
                content: "Open this link in browser to reset your password: " + HttpUtility.HtmlEncode(resetUrl),
                htmlContent: $"<html><body>Click <a href=\"{resetUrl}\">here</a> to reset your password.</html></body>");

            if (!sent)
            {
                return _resultFactory.ForgotPasswordFailed("Error while sending letter. Try again later.");
            }

            var expires = await _pwdRecoveryCacheService.CacheMail(request.Email);           
            return _resultFactory.ForgotPasswordSuccess("Letter to reset password was sent in your mailbox.",
                user.Email,
                expires);
        }

        public async Task<OperationResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _aspNetUserManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return _resultFactory.CreateOperationResult(false, "Reseting error.");
            }

            var result = await _aspNetUserManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return _resultFactory.CreateOperationResult(true, "Password success changed.");                
            }

            var message = string.Join(Environment.NewLine, result.Errors.Select(error => error.Description));
            return _resultFactory.CreateOperationResult(false, message);
        }        
    }
}