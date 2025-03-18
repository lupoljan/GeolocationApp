using GeolocationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp.Services.Interfaces
{
    public interface IIpStackService
    {
        Task<Geolocation> GetGeolocationAsync(string input);
    }
}
