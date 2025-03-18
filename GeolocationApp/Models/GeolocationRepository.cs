using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp.Models
{
    public class GeolocationRepository
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public GeolocationRepository(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Geolocation>> GetAllAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Geolocations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Geolocation> GetByIpOrUrlAsync(string input)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Geolocations
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.IP == input || g.Url == input);
        }

        public async Task AddAsync(Geolocation geolocation)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await context.Geolocations.AddAsync(geolocation);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Geolocation geolocation)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Geolocations.Update(geolocation);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var entity = await context.Geolocations.FindAsync(id);
            if (entity != null)
            {
                context.Geolocations.Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
