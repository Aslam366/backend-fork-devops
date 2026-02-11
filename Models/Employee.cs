using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZetaSaasHRMSBackend.Models
{
    [Index(nameof(TenantId))]
    public class Employee : BaseEntity
    {
        public long UserId { get; set; } // HRMS login user

        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? Mobile { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
