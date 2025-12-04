using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeluqueriaTurnos.Models;

namespace PeluqueriaTurnos.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services => Set<Service>();
        public DbSet<Stylist> Stylists => Set<Stylist>();
        public DbSet<StylistService> StylistServices => Set<StylistService>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Muchos a muchos Stylist-Service
            builder.Entity<StylistService>()
                .HasKey(ss => new { ss.StylistId, ss.ServiceId });

            builder.Entity<StylistService>()
                .HasOne(ss => ss.Stylist)
                .WithMany(s => s.StylistServices)
                .HasForeignKey(ss => ss.StylistId);

            builder.Entity<StylistService>()
                .HasOne(ss => ss.Service)
                .WithMany()
                .HasForeignKey(ss => ss.ServiceId);
        }
    }
}
