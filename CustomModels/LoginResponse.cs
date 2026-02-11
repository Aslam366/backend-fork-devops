namespace ZetaSaasHRMSBackend.CustomModels
{
    public class LoginResponse
    {
        public string JwtToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
