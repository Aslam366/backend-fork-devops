using ZetaSaasHRMSBackend.CustomModels;
using ZetaSaasHRMSBackend.Models;

namespace ZetaSaasHRMSBackend.Repository.Auth
{
    public interface IAuthRepository
    {
        Task<LoginResponse> LoginAsync(string userName, string password);
        string GenerateJwtToken(long userId, string userName, long tenantId);
        Task<string?> GenerateJwtToken(string refreshToken);
        Task DeleteRefreshToken(string refreshToken);
    }
}
