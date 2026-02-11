using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZetaSaasHRMSBackend.Models
{
    [Index(nameof(TenantId))]
    public class RoleMenuRight : BaseEntity
    {
        [Required]
        public long RoleId { get; set; }

        [Required]
        public long MenuId { get; set; }

        public bool CanView { get; set; } = false;
        public bool CanCreate { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;

        // Navigation properties
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; } = null!;

        [ForeignKey(nameof(MenuId))]
        public Menu Menu { get; set; } = null!;
    }
}
