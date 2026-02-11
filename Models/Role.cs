using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZetaSaasHRMSBackend.Models
{
    [Index(nameof(TenantId))]
    public class Role : BaseEntity
    {
        [Required, MaxLength(150)]
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsSuperRole { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<RoleMenuRight> RoleMenuRight { get; set; } = new List<RoleMenuRight>();
        public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    }
}
