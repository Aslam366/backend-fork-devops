using System.ComponentModel.DataAnnotations;

namespace ZetaSaasHRMSBackend.Models
{
    public class Menu 
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string MenuName { get; set; } = null!;

        public long? ParentMenuId { get; set; } // Self reference for submenus

        [MaxLength(200)]
        public string? RouteUrl { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

        public decimal DisplayOrder { get; set; } = 0.0m;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<RoleMenuRight> RoleMenuRight { get; set; } = new List<RoleMenuRight>();
    }
}
