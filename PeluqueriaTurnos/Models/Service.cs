using System.ComponentModel.DataAnnotations;

namespace PeluqueriaTurnos.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ShortDescription { get; set; }

        [Range(10, 240)]
        public int DurationMinutes { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(200)]
        public string? ImagePath { get; set; } // /img/services/corte.jpg
    }
}
