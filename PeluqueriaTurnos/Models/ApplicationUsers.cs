using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PeluqueriaTurnos.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(80)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Notes { get; set; }
    }
}
