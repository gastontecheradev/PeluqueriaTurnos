using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeluqueriaTurnos.Data;
using PeluqueriaTurnos.Models;

namespace PeluqueriaTurnos.Controllers
{
    [Authorize] // Cualquier usuario logueado puede entrar a este controlador
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // PANEL ADMIN: todos los turnos
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

        // MIS TURNOS: del usuario actual
        [Authorize] // cualquier usuario logueado
        public async Task<IActionResult> My()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .Where(a => a.ClientId == user.Id)
                .OrderBy(a => a.Start)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Crear turno
        [Authorize]
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

            var vm = new AppointmentCreateViewModel
            {
                Start = DateTime.Today.AddHours(14),
                ClientName = user?.FullName ?? user?.Email ?? string.Empty
            };

            return View(vm);
        }

        // POST: Crear turno
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(AppointmentCreateViewModel vm)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            if (vm.Start < DateTime.Now)
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

                return View(vm);
            }

            // Buscar servicio para setear duración desde ahí
            var service = await _context.Services.FirstAsync(s => s.Id == vm.ServiceId);

            var appointment = new Appointment
            {
                ServiceId = vm.ServiceId,
                StylistId = vm.StylistId,
                Start = vm.Start,
                DurationMinutes = service.DurationMinutes,
                ClientId = user.Id,
                ClientName = vm.ClientName,
                ClientPhone = vm.ClientPhone,
                Status = AppointmentStatus.Booked
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(My));
        }

        // GET: Cancelar (confirmación)
        [Authorize]
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
        [Authorize]
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
            return RedirectToAction(nameof(My));
        }
    }
}
