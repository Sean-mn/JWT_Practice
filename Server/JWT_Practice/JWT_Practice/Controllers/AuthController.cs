using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT_Practice.DBContexts;
using JWT_Practice.Models;
using JWT_Practice.Request;
using JWT_Practice.Setting;
using Microsoft.Extensions.Logging;
using System;

namespace JWT_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginDBContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(LoginDBContext context, IOptions<JwtSettings> jwtSettings, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);
                if (user == null || user.Password != request.Password)
                {
                    _logger.LogWarning("로그인 실패 - 유저이름: {Username}", request.Username);
                    return Unauthorized("알맞지 않은 유저 이름 또는 비밀번호");
                }

                _logger.LogInformation("로그인 성공 - 유저이름: {Username}", user.Username);

                var token = GenerateToken(user);
                return Ok(new { token = token, username = user.Username });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "로그인 중 예외 발생");
                return StatusCode(500, "서버 오류가 발생했습니다.");
            }
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            _logger.LogInformation("회원가입 요청 - 유저이름: {Username}, 비밀번호: {Password}", request.Username, request.Password);

            if (_context.Users.Any(u => u.Username == request.Username))
                return Conflict("이미 존재하는 유저 이름입니다.");

            var newUser = new User
            {
                Username = request.Username,
                Password = request.Password
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("회원가입 성공");
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
