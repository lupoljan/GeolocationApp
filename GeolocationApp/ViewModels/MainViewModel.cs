using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationApp.Models;
using GeolocationApp.Services;
using System.Collections.ObjectModel;

namespace GeolocationApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IpStackService _ipStackService;
        private readonly GeolocationRepository _repository;

        [ObservableProperty]
        private ObservableCollection<Geolocation> _geolocations = new();

        [ObservableProperty]
        private string _input;

        [ObservableProperty]
        private Geolocation _selectedGeolocation;

        public MainViewModel(IpStackService ipStackService, GeolocationRepository repository)
        {
            _ipStackService = ipStackService;
            _repository = repository;
            LoadSampleGeolocations();
        }

        private async void LoadGeolocations()
        {
            var locations = await _repository.GetAllAsync();
            Geolocations = new ObservableCollection<Geolocation>(locations);
        }

        private async void LoadSampleGeolocations()
        {
            var locations = await _repository.GetAllAsync();

            // If no data in database, create sample data
            if (!locations.Any())
            {
                var sampleData = GetSampleGeolocations();
                foreach (var item in sampleData)
                {
                    await _repository.AddAsync(item);
                }
                locations = await _repository.GetAllAsync();
            }

            Geolocations = new ObservableCollection<Geolocation>(locations);
        }

        private List<Geolocation> GetSampleGeolocations()
        {
            return new List<Geolocation>
        {
            new Geolocation
            {
                IPAddress = "8.8.8.8",
                Url = "dns.google",
                Country = "United States",
                City = "Mountain View",
                Latitude = 37.4056,
                Longitude = -122.0775
            },
            new Geolocation
            {
                IPAddress = "104.16.249.249",
                Url = "github.com",
                Country = "Germany",
                City = "Frankfurt",
                Latitude = 50.1188,
                Longitude = 8.6843
            },
            new Geolocation
            {
                IPAddress = "172.217.16.206",
                Url = "google.com",
                Country = "Japan",
                City = "Tokyo",
                Latitude = 35.6895,
                Longitude = 139.6917
            }
        };
        }


        [RelayCommand]
        private async Task LookupAsync()
        {
            var geolocation = await _ipStackService.GetGeolocationAsync(Input);
            await _repository.AddAsync(geolocation);
            Geolocations.Add(geolocation);
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (SelectedGeolocation != null)
            {
                await _repository.DeleteAsync(SelectedGeolocation.Id);
                Geolocations.Remove(SelectedGeolocation);
            }
        }
    }
}