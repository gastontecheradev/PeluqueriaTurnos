using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeluqueriaTurnos.Data;

namespace PeluqueriaTurnos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public HomeController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Price)
                .ToListAsync();

            var stylists = await _context.Stylists
                .Where(s => s.IsActive)
                .ToListAsync();

            ViewBag.SalonName = _config["NovaFade:SalonName"];
            ViewBag.City = _config["NovaFade:City"];

            return View((services, stylists));
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Phone = _config["NovaFade:Phone"];
            ViewBag.Email = _config["NovaFade:Email"];
            ViewBag.City = _config["NovaFade:City"];
            return View();
        }
    }
}
