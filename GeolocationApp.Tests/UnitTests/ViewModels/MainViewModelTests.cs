using GeolocationApp.Models;
using Moq;
using GeolocationApp.ViewModels;
using GeolocationApp.Repositories.Interfaces;
using GeolocationApp.Services.Interfaces;

namespace GeolocationApp.Tests.UnitTests.ViewModels
{
    public class MainViewModelTests
    {
        private readonly Mock<IIpStackService> _mockIpStackService;
        private readonly Mock<IGeolocationRepository> _mockRepo;
        private readonly MainViewModel _vm;

        public MainViewModelTests()
        {
            _mockIpStackService = new Mock<IIpStackService>();
            _mockRepo = new Mock<IGeolocationRepository>();
            _vm = new MainViewModel(_mockIpStackService.Object, _mockRepo.Object);
        }

        [Fact]
        public async Task FetchGeolocationCommand_ValidInput_UpdatesSelectedLocation()
        {
            // Arrange
            var expected = new Geolocation { IPAddress = "8.8.8.8" };
            _mockIpStackService.Setup(s => s.GetGeolocationAsync(It.IsAny<string>()))
                .ReturnsAsync(expected);
            _vm.Input = "8.8.8.8";

            // Act
            await _vm.FetchGeolocationCommand.ExecuteAsync(null);

            // Assert
            Assert.Equal(expected.IPAddress, _vm.SelectedLocation?.IPAddress);
        }

        [Fact]
        public async Task SaveToDatabaseCommand_ValidLocation_AddsToRepository()
        {
            // Arrange
            var geo = new Geolocation { IPAddress = "8.8.8.8" };
            _vm.SelectedLocation = geo;

            // Act
            await _vm.SaveToDatabaseCommand.ExecuteAsync(null);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(geo), Times.Once);
        }
    }
}
