using System.ComponentModel.DataAnnotations;

namespace ZetaSaasHRMSBackend.Data
{
    public class Tenant
    {
        public long Id { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public bool IsActive { get; set; } 
    }
}
