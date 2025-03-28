﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationApp.Models;
using GeolocationApp.Repositories;
using GeolocationApp.Repositories.Interfaces;
using GeolocationApp.Services;
using GeolocationApp.Services.Interfaces;
using System.Collections.ObjectModel;

namespace GeolocationApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IIpStackService _ipStackService;
        private readonly IGeolocationRepository _repository;

        [ObservableProperty]
        private ObservableCollection<Geolocation> _locations = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FetchGeolocationCommand))]
        private string _input;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveToDatabaseCommand))]
        private Geolocation _selectedLocation;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteFromDatabaseCommand))]
        private Geolocation _selectedLocationFromDb;
        
        [ObservableProperty]
        private string _errorMessage;

        private bool CanFetch => !string.IsNullOrWhiteSpace(Input);
        private bool CanSave => SelectedLocation != null;
        private bool CanDelete => SelectedLocationFromDb != null;
        public MainViewModel(IIpStackService ipStackService, IGeolocationRepository repository)
        {
            _ipStackService = ipStackService;
            _repository = repository;
            LoadLocations();
        }
        private async void LoadLocations()
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

            Locations = new ObservableCollection<Geolocation>(locations);
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

        [RelayCommand(CanExecute = nameof(CanFetch))]
        private async Task FetchGeolocation()
        {
            try
            {
                ErrorMessage = string.Empty;
                var result = await _ipStackService.GetGeolocationAsync(Input);
                SelectedLocation = result;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveToDatabase()
        {
            try
            {
                if (SelectedLocation != null)
                {
                    await _repository.AddAsync(SelectedLocation);
                    Locations.Add(SelectedLocation);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Save error: {ex.Message}";
            }
        }

        [RelayCommand(CanExecute = nameof(CanDelete))]
        private async Task DeleteFromDatabase()
        {
            try
            {
                if (SelectedLocationFromDb != null)
                {
                    await _repository.DeleteAsync(SelectedLocationFromDb.Id);
                    Locations.Remove(SelectedLocationFromDb);
                    SelectedLocationFromDb = null;
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Delete error: {ex.Message}";
            }
        }
    }
}