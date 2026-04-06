using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Authentication;
using StudentManagement.Core.Models;

namespace StudentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJwtTokenGenerator jwtTokenGenerator, ILogger<AuthController> logger)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Login endpoint to get JWT token
        /// </summary>
        /// <remarks>
        /// For demo purposes, this endpoint accepts any username/password.
        /// In a real application, validate against a database.
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<ApiResponse<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(ApiResponse.ErrorResponse("Username and password are required."));
            }

            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", request.Username);

                // For demo purposes, accept any valid credentials
                // In production, validate against actual user database
                var token = _jwtTokenGenerator.GenerateToken("1", request.Username);

                var response = new LoginResponse
                {
                    Access_token = token,
                    Token_type = "Bearer",
                    Expires_in = 3600
                };

                _logger.LogInformation("User {Username} logged in successfully", request.Username);
                return Ok(ApiResponse<LoginResponse>.SuccessResponse(response, "Login successful."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user: {Username}", request.Username);
                return BadRequest(ApiResponse.ErrorResponse("Login failed. Please try again."));
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Access_token { get; set; } = string.Empty;
        public string Token_type { get; set; } = string.Empty;
        public int Expires_in { get; set; }
    }
}
