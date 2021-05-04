namespace StudyTestingEnvironment.Models.Options
{
    /// <summary>
    /// Some routes for frontend SPA.
    /// </summary>
    public class FrontendRoutingOptions
    {
        /// <summary>
        /// Route to email confirming in our frontend.
        /// </summary>
        public string ConfirmEmailUrl { get; set; }

        /// <summary>
        /// Route to password recovery using email in our frontend.
        /// </summary>
        public string ResetPasswordUrl { get; set; }
    }
}