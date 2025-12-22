using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PeluqueriaTurnos.Models;

namespace PeluqueriaTurnos.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var context = provider.GetRequiredService<ApplicationDbContext>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var config = provider.GetRequiredService<IConfiguration>(); // 👈 agregamos esto

            await context.Database.MigrateAsync();

            // 1. Roles
            string[] roles = new[] { "Admin", "Client" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Usuario admin por configuración
            var adminEmail = config["AdminUser:Email"];
            var adminPassword = config["AdminUser:Password"];

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "Faltan las claves AdminUser:Email o AdminUser:Password en la configuración.");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Admin NovaFade"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new Exception("No se pudo crear el usuario admin: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // 3. Servicios demo
            if (!context.Services.Any())
            {
                var corte = new Service
                {
                    Name = "Corte Clásico",
                    ShortDescription = "Corte de cabello clásico con acabado prolijo.",
                    DurationMinutes = 30,
                    Price = 700,
                    ImagePath = "/img/services/corte-clasico.jpg"
                };
                var fade = new Service
                {
                    Name = "Skin Fade",
                    ShortDescription = "Fade moderno con terminaciones al ras.",
                    DurationMinutes = 45,
                    Price = 900,
                    ImagePath = "/img/services/skin-fade.jpg"
                };
                var barba = new Service
                {
                    Name = "Perfilado de Barba",
                    ShortDescription = "Diseño y perfilado de barba a navaja.",
                    DurationMinutes = 30,
                    Price = 650,
                    ImagePath = "/img/services/barba.jpg"
                };

                context.Services.AddRange(corte, fade, barba);
                await context.SaveChangesAsync();
            }

            // 4. Estilistas demo
            if (!context.Stylists.Any())
            {
                var lucas = new Stylist
                {
                    Name = "Lucas Andrade",
                    Bio = "Especialista en fades y degradados modernos.",
                    PhotoPath = "/img/stylists/lucas.jpg"
                };
                var sofia = new Stylist
                {
                    Name = "Sofía Méndez",
                    Bio = "Cortes de tendencia y colorimetría básica.",
                    PhotoPath = "/img/stylists/sofia.jpg"
                };

                context.Stylists.AddRange(lucas, sofia);
                await context.SaveChangesAsync();

                var allServices = await context.Services.ToListAsync();

                foreach (var svc in allServices)
                {
                    context.StylistServices.Add(new StylistService
                    {
                        StylistId = lucas.Id,
                        ServiceId = svc.Id
                    });
                    context.StylistServices.Add(new StylistService
                    {
                        StylistId = sofia.Id,
                        ServiceId = svc.Id
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
