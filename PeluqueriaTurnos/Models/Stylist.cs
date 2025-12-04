using System.ComponentModel.DataAnnotations;

namespace PeluqueriaTurnos.Models
{
    public class Stylist
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Bio { get; set; }

        [StringLength(200)]
        public string? PhotoPath { get; set; } // /img/stylists/lucas.jpg

        public bool IsActive { get; set; } = true;

        public ICollection<StylistService> StylistServices { get; set; } = new List<StylistService>();
    }

    // Relación muchos-a-muchos Stylist-Services
    public class StylistService
    {
        public int StylistId { get; set; }
        public Stylist Stylist { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
    }
}
