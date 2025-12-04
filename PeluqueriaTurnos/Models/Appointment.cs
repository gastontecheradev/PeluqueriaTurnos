using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeluqueriaTurnos.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        [Required]
        public int StylistId { get; set; }
        public Stylist Stylist { get; set; } = null!;

        [Required]
        public DateTime Start { get; set; }

        [Range(10, 240)]
        public int DurationMinutes { get; set; }

        [Required]
        public string ClientId { get; set; } = string.Empty;

        public ApplicationUser Client { get; set; } = null!;

        [Required, StringLength(80)]
        public string ClientName { get; set; } = string.Empty;

        [StringLength(30)]
        public string? ClientPhone { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
