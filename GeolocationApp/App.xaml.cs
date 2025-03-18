using GeolocationApp.Models;
using GeolocationApp.Services;
using GeolocationApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GeolocationApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                // First create the database
                context.Database.EnsureCreated();
            }


            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Get the main window from DI container
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext Factory
            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlite("Data Source=geolocations.db"));
            services.AddHttpClient<IpStackService>();
            services.AddSingleton<GeolocationRepository>();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }

}
