using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZetaSaasHRMSBackend.CustomModels;
using ZetaSaasHRMSBackend.Data;
using ZetaSaasHRMSBackend.Middleware;
using ZetaSaasHRMSBackend.Models;
using ZetaSaasHRMSBackend.Repository.Common;


namespace ZetaSaasHRMSBackend.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HrmsDbContext _context;
        private readonly TenantContext _tenantContext;
        private readonly ICommonRepository _commonRepository;
        private readonly IConfiguration _configuration;

        public AuthRepository(HrmsDbContext context, TenantContext tenantContext, ICommonRepository commonRepository, IConfiguration configuration)
        {
            _context = context;
            _tenantContext = tenantContext;
            _commonRepository = commonRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(string userName, string password)
        {
            password= await _commonRepository.EncriptPwd(password);

            var user = await _context.User.AsNoTracking().FirstOrDefaultAsync(u =>
            u.UserName == userName &&
            u.PasswordHash == password &&
            u.IsActive);

            if (user != null)
            {

                var token = GenerateJwtToken(user.Id, user.UserName, _tenantContext.TenantId);

                var refreshToken = new RefreshToken
                {
                    UserId = user.Id,
                    TenantId = _tenantContext.TenantId,
                    Token = RefreshTokenGenerator.Generate(),
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                _context.RefreshToken.Add(refreshToken);
                await _context.SaveChangesAsync();

                return new LoginResponse
                {
                    JwtToken = token,
                    RefreshToken = refreshToken.Token
                };
            }

            return new LoginResponse();
            
        }

        public string GenerateJwtToken(long userId, string userName, long tenantId)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim("TenantId", tenantId.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string?> GenerateJwtToken(string refreshToken)
        {
            var tokenRecord = await _context.RefreshToken
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken && r.ExpiresAt > DateTime.UtcNow);

            if (tokenRecord == null)
                return "";

            var newAccessToken = "";
            if (tokenRecord.User != null)
            {
                newAccessToken = GenerateJwtToken(tokenRecord.User.Id, tokenRecord.User.UserName,tokenRecord.User.TenantId);
            }

            return newAccessToken;
        }

        public async Task DeleteRefreshToken(string refreshToken)
        {
            var token = await _context.RefreshToken
                     .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (token != null)
            {
                _context.RefreshToken.Remove(token);
                await _context.SaveChangesAsync();
            }
        }

    }
}
