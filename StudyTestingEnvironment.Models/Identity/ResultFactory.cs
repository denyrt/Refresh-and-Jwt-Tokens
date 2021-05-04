using StudyTestingEnvironment.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyTestingEnvironment.Models.Identity
{
    public class ResultFactory
    {
        public LoginResult LoginFailed() => new LoginResult { IsSuccess = false };

        public LoginResult LoginSuccess(string accessToken, string refreshToken, DateTime refreshExpires)
            => new LoginResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshExpiresInUTC = refreshExpires,
                IsSuccess = true
            };

        public RegistryResult RegistryFailed(string description) => new RegistryResult
        {
            Description = description,
            IsSuccess = false
        };

        public RegistryResult RegistryFailed() => RegistryFailed("Registry failed. Try again.");

        public RegistryResult SuccessRegistry(string destinationEmail) => new RegistryResult
        {
            IsSuccess = true,
            DestinationEmail = destinationEmail,
            Description = "Confirm registry using letter in your mailbox."
        };

        public ForgotPasswordResult ForgotPasswordFailed(string description,
                                                         string destinationEmail = null,
                                                         double timeout = 0) 
        => new ForgotPasswordResult
        {
            IsSuccess = false,
            Description = description,
            DestinationEmail = destinationEmail,
            Timeout = timeout
        };

        public ForgotPasswordResult ForgotPasswordSuccess(string description,
                                                         string destinationEmail = null,
                                                         double timeout = 0)
        => new ForgotPasswordResult
        {
            IsSuccess = true,
            Description = description,
            DestinationEmail = destinationEmail,
            Timeout = timeout
        };

        public OperationResult CreateOperationResult(bool isSuccess, string description) => new OperationResult
        {
            IsSuccess = isSuccess,
            Description = description
        };

        public RefreshTokenResult CreateRefreshTokenResultSuccess(string accessToken, string refreshToken) => new RefreshTokenResult
        {
            IsSuccess = true,
            Description = "",
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        public RefreshTokenResult CreateRefreshTokenExpiredResult() => new RefreshTokenResult
        {
            IsSuccess = false,
            Description = "Token Expired"
        };
    }
}