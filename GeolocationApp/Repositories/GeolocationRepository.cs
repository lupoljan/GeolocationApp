using GeolocationApp.Models;
using GeolocationApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp.Repositories
{
    public class GeolocationRepository : IGeolocationRepository
    {
        private readonly AppDbContext _context;

        public GeolocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Geolocation geolocation)
        {
            await _context.Geolocations.AddAsync(geolocation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Geolocations.FindAsync(id);
            if (entity != null)
            {
                _context.Geolocations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Geolocation>> GetAllAsync()
        {
            return await _context.Geolocations.AsNoTracking().ToListAsync();
        }

        public async Task<Geolocation> GetByIpOrUrlAsync(string input)
        {
            return await _context.Geolocations
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.IPAddress == input || g.Url == input);
        }
    }
}
