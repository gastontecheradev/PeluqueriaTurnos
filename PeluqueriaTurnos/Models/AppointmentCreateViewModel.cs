using System.ComponentModel.DataAnnotations;

namespace PeluqueriaTurnos.Models
{
    public class AppointmentCreateViewModel
    {
        [Required(ErrorMessage = "Seleccioná un servicio.")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Seleccioná un estilista.")]
        public int StylistId { get; set; }

        [Required(ErrorMessage = "Ingresá fecha y hora.")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Ingresá un nombre.")]
        [StringLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [StringLength(30)]
        public string? ClientPhone { get; set; }
    }
}
