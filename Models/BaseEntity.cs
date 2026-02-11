using System.ComponentModel.DataAnnotations;

namespace ZetaSaasHRMSBackend.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long TenantId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
