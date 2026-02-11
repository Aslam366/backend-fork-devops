using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZetaSaasHRMSBackend.Models
{
    [Index(nameof(TenantId))]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = null!;

        [MaxLength(150)]
        public string? Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } = null!;

        public bool IsSuperUser { get; set; } = false;

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    }
}
