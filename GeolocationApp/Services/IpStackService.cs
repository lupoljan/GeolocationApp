using GeolocationApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationApp.Services
{
    public class IpStackService
    {
        private const string ApiKey = "YOUR_API_KEY";
        private readonly HttpClient _httpClient;

        public IpStackService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<Geolocation> GetGeolocationAsync(string input)
        {
            var response = await _httpClient.GetStringAsync($"http://api.ipstack.com/{input}?access_key={ApiKey}");
            return JsonConvert.DeserializeObject<Geolocation>(response);
        }
    }
}
