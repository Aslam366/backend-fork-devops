namespace ZetaSaasHRMSBackend.Middleware
{
    public class TenantContext
    {
        public long TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
    }
}
