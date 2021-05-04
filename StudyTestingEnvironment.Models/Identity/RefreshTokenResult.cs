using StudyTestingEnvironment.Models.Common;

namespace StudyTestingEnvironment.Models.Identity
{
    public class RefreshTokenResult : OperationResult
    {
        /// <summary>
        /// Jwt access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// For mobile devices (or desktop devices).
        /// </summary>
        public string RefreshToken { get; set; }
    }
}