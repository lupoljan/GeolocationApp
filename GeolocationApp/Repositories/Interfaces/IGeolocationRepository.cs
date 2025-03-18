using GeolocationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp.Repositories.Interfaces
{
    public interface IGeolocationRepository
    {
        Task AddAsync(Geolocation geolocation);
        Task DeleteAsync(int id);
        Task<IEnumerable<Geolocation>> GetAllAsync();
        Task<Geolocation> GetByIpOrUrlAsync(string input);
    }
}
