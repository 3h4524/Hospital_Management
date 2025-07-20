using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;
using View;
using ViewModel;

namespace View
{
    public partial class App : Application
    {
        public static IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                var services = new ServiceCollection();
                services.AddDbContext<HospitalManagementContext>(options =>
                    options.UseSqlServer("Server=localhost,1433;Initial Catalog=Hospital_Management;User ID=sa;Password=3H452004hh@;TrustServerCertificate=True;"));
                services.AddScoped<SystemUserRepository>();
                services.AddScoped<EmailPasswordResetRepository>();
                services.AddScoped<DoctorScheduleRepository>();
                services.AddScoped<DoctorRepository>();
                services.AddScoped<AuthenticationService>();
                services.AddScoped<EmailService>();
                services.AddTransient<LoginView>();
                services.AddTransient<MainWindow>();
                services.AddScoped<DoctorScheduleService>();
                services.AddScoped<AttendanceService>();
                _serviceProvider = services.BuildServiceProvider();

                //var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                //var navigateService = (NavigateService)_serviceProvider.GetRequiredService<INavigateService>();
                //navigateService.SetNavigateService(mainViewModel);

                AttendanceService s = _serviceProvider.GetRequiredService<AttendanceService>();

                s.Watch();

                var mainWindow = new MainWindow(_serviceProvider.GetRequiredService<LoginView>());
                MainWindow = mainWindow;
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup failed: {ex.Message}\n{ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}