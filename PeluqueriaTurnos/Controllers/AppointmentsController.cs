using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeluqueriaTurnos.Data;
using PeluqueriaTurnos.Models;

namespace PeluqueriaTurnos.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Admin ve todos los turnos
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .Include(a => a.Client)
                .OrderBy(a => a.Start)
                .ToListAsync();

            return View(appointments);
        }

        // Cliente ve sus turnos
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> My()
        {
            var user = await _userManager.GetUserAsync(User);
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .Where(a => a.ClientId == user!.Id)
                .OrderBy(a => a.Start)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Crear turno
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            ViewBag.Stylists = await _context.Stylists
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            var user = await _userManager.GetUserAsync(User);

            var model = new Appointment
            {
                Start = DateTime.Today.AddHours(14),
                DurationMinutes = 30,
                ClientName = user?.FullName ?? user?.Email ?? ""
            };

            return View(model);
        }

        // POST: Crear turno
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Create(Appointment model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            if (model.Start < DateTime.Now)
            {
                ModelState.AddModelError("Start", "El turno debe ser en una fecha/hora futura.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Services = await _context.Services
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                ViewBag.Stylists = await _context.Stylists
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                return View(model);
            }

            model.ClientId = user.Id;
            model.Status = AppointmentStatus.Booked;

            // Si duración no viene seteada, usar la del servicio
            var service = await _context.Services.FirstAsync(s => s.Id == model.ServiceId);
            if (model.DurationMinutes <= 0)
            {
                model.DurationMinutes = service.DurationMinutes;
            }

            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(My));
        }

        // GET: Cancelar (confirmación)
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (appointment.ClientId != user!.Id)
                    return Forbid();
            }

            return View(appointment);
        }

        // POST: Confirmar cancelación
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client,Admin")]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (appointment.ClientId != user!.Id)
                    return Forbid();

                appointment.Status = AppointmentStatus.CancelledByClient;
            }
            else
            {
                appointment.Status = AppointmentStatus.CancelledByAdmin;
            }

            await _context.SaveChangesAsync();
            if (User.IsInRole("Admin"))
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction(nameof(My));
        }
    }
}
