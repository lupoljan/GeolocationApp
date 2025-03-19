using GeolocationApp.Models;
using GeolocationApp.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GeolocationApp.Services
{
    public class IpStackService : IIpStackService
    {
        private readonly string _apiKey;
        private const string BaseUrl = "http://api.ipstack.com/";
        private readonly HttpClient _httpClient;

        public IpStackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = ConfigurationManager.AppSettings["IPStackApiKey"] ?? throw new InvalidOperationException("API key is missing in App.config");
        }
        public async Task<Geolocation> GetGeolocationAsync(string input)
        {
            try
            {
                string ipAddress = await ResolveIpAddressAsync(input);
                var response = await _httpClient.GetAsync($"{BaseUrl}{ipAddress}?access_key={_apiKey}&hostname=1");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Error fetching geolocation: {response.StatusCode}");
                }

                var responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<GeolocationResponse>(responseData);

                return new Geolocation
                {
                    IPAddress = ipAddress,
                    Url = data?.Hostname ?? "Unknown",
                    Country = data?.Country_Name ?? "Unknown",
                    City = data?.City ?? "Unknown",
                    Latitude = data?.Latitude ?? 0,
                    Longitude = data?.Longitude ?? 0
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching geolocation: {ex.Message}");
                return null;
            }

        }
        private async Task<string> ResolveIpAddressAsync(string input)
        {
            if (IsValidIpAddress(input))
            {
                return input;
            }

            if (TryParseUrl(input, out Uri uri))
            {
                return await ResolveDnsAsync(uri);
            }

            throw new ArgumentException("Invalid input - must be a valid IP address or URL");
        }
        private bool IsValidIpAddress(string input)
        {
            return IPAddress.TryParse(input, out _);
        }
        private bool TryParseUrl(string input, out Uri uri)
        {
            if (Uri.TryCreate(input, UriKind.Absolute, out uri))
            {
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }

            // Try adding http:// prefix for URLs without scheme
            if (Uri.TryCreate($"http://{input}", UriKind.Absolute, out uri))
            {
                return true;
            }

            return false;
        }
        private async Task<string> ResolveDnsAsync(Uri uri)
        {
            try
            {
                var hostEntry = await Dns.GetHostAddressesAsync(uri.Host);
                return hostEntry.FirstOrDefault()?.ToString()
                    ?? throw new InvalidOperationException("No IP addresses found for the URL");
            }
            catch (SocketException ex)
            {
                throw new InvalidOperationException($"Could not resolve DNS for {uri.Host}", ex);
            }
        }
    }
}
