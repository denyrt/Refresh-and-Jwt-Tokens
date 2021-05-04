using Microsoft.AspNetCore.Http;
using System;

namespace StudyTestingEnvironment.Services.Identity
{
    public static class Extensions
    {
        public const string RefreshTokenCookieKey = "refresh-token";
        public const string RefreshTokenPath = "/api/auth/";

        public const string SecurityTokenCookieKey = "secure";
        public const string SecurityTokenPath = "/api/security/";

        public static string GetRefreshToken(this HttpRequest request)
        {
            request.Cookies.TryGetValue(RefreshTokenCookieKey, out var value);
            return value;
        }

        public static void ResetRefreshTokenCookie(this IResponseCookies responseCookies, string tokenValue, DateTimeOffset expires)
        {
            responseCookies.RemoveRefreshTokenCookie();
            responseCookies.SetRefreshTokenCookie(tokenValue, expires);
        }

        public static void SetRefreshTokenCookie(this IResponseCookies responseCookies, string tokenValue, DateTimeOffset expires)
        {
            responseCookies.Append(RefreshTokenCookieKey,
                tokenValue,
                new CookieOptions
                {
                    Path = RefreshTokenPath,
                    HttpOnly = true,
                    Expires = expires
                });
        }

        public static void RemoveRefreshTokenCookie(this IResponseCookies responseCookies)
        {
            responseCookies.Delete(RefreshTokenCookieKey,
                new CookieOptions
                {
                    Path = RefreshTokenPath,
                    HttpOnly = true
                });
        }

        public static void SetSecurityTokenCookie(this IResponseCookies responseCookies, string value, DateTimeOffset expires)
        {
            responseCookies.Append(SecurityTokenCookieKey,
                value,
                new CookieOptions
                {
                    Path = SecurityTokenPath,                    
                    HttpOnly = true,
                    Expires = expires
                });
        }

        public static string GetSecurityTokenCookie(this HttpRequest httpRequest)
        {
            httpRequest.Cookies.TryGetValue(SecurityTokenCookieKey, out var secureToken);
            return secureToken;
        }
    }
}