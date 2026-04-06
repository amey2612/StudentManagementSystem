namespace StudentManagement.API.Authentication
{
    /// <summary>
    /// Extension methods for JWT authentication configuration
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// Validates JWT token format and claims
        /// </summary>
        public static bool IsValidToken(this string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;
            
            var parts = token.Split('.');
            return parts.Length == 3; // JWT must have 3 parts: header.payload.signature
        }

        /// <summary>
        /// Extracts user ID from JWT token claims
        /// </summary>
        public static string ExtractUserId(this string token)
        {
            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token)) return null;

                var jsonToken = handler.ReadJwtToken(token);
                return jsonToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
