using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Geolocation> Geolocations { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=geolocations.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Geolocation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IPAddress).IsUnique();
                entity.HasIndex(e => e.Url).IsUnique();
            });
        }

    }
}
