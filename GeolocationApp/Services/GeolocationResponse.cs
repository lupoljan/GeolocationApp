using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp.Services
{
    public class GeolocationResponse
    {
        public string? Ip { get; set; }
        public string? Hostname { get; set; }
        public string? Country_Name { get; set; }
        public string? City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
