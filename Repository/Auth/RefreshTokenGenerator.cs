using System.Security.Cryptography;

namespace ZetaSaasHRMSBackend.Repository.Auth
{
    public static class RefreshTokenGenerator
    {
        public static string Generate()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
