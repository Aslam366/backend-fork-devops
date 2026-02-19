namespace ZetaSaasHRMSBackend.CustomModels
{
    public class UserRequest
    {
        public int userId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        public List<long> RoleIds { get; set; } = new();
    }
}
