using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZetaSaasHRMSBackend.CustomModels;
using ZetaSaasHRMSBackend.Repository.Auth;
using ZetaSaasHRMSBackend.Repository.Common;
using ZetaSaasHRMSBackend.Models;
using ZetaSaasHRMSBackend.Repository.User;


namespace ZetaSaasHRMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthRepository authRepository,ICommonRepository commonRepository,IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _commonRepository = commonRepository;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authRepository.LoginAsync(request.UserName, request.Password);
            if (response.JwtToken != "" && response.RefreshToken != "")
            {
                // store refresh token in HttpOnly cookie
                Response.Cookies.Append("refreshToken_hrms", response.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,          // HTTPS only
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Ok(new
                {
                    Success = true,
                    Data = "login success",
                    Token = response.JwtToken,
                    TokenType = "Bearer"
                });
            }
            else
            {
                return Ok(new
                {
                    Success = false,
                    Data = "login failed"
                });
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken_hrms"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var newAccessToken = await _authRepository.GenerateJwtToken(refreshToken);

            if (newAccessToken == null || newAccessToken == "")
                return Unauthorized();

            return Ok(new
            {
                token = newAccessToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            request.Password = await _commonRepository.EncriptPwd(request.Password);
            User user = new User();
            user.UserName = request.UserName;
            user.PasswordHash = request.Password;
            user.IsActive = true;
            user.CreatedAt = DateTime.UtcNow;
            await _userRepository.CreateAsync(user);
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken_hrms"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authRepository.DeleteRefreshToken(refreshToken);
            }

            // IMPORTANT: must match cookie creation options
            Response.Cookies.Delete("refreshToken_hrms");

            return Ok(new { message = "Logged out successfully" });
        }


    }
}
