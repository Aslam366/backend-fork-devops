using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZetaSaasHRMSBackend.Models
{
    [Index(nameof(TenantId))]
    public class RefreshToken : BaseEntity
    {
        public long UserId { get; set; }

        [Required]
        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        // Navigation
        public User User { get; set; } = null!;
    }
}
