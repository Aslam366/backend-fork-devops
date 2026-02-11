using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZetaSaasHRMSBackend.Models
{
    [Index(nameof(TenantId))]
    public class UserRole :BaseEntity
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long RoleId { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; } = null!;
    }
}
