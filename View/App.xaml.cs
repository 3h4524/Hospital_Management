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
                    options.UseSqlServer("Data Source=3H;Initial Catalog=Hospital_Management;User ID=sa;Password=3H452004hh@;TrustServerCertificate=True;"));
                services.AddScoped<SystemUserRepository>();
                services.AddScoped<EmailPasswordResetRepository>();
                services.AddScoped<AuthenticationService>();
                services.AddScoped<AppointmentBookingService>();
                services.AddScoped<EmailService>();
                services.AddTransient<LoginView>();
                services.AddTransient<MainWindow>();
                services.AddScoped<PatientRepository>();
                services.AddScoped<DoctorScheduleRepository>();
                services.AddScoped<DoctorRepository>();
                services.AddScoped<AppointmentRepository>();




                _serviceProvider = services.BuildServiceProvider();

                //var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                //var navigateService = (NavigateService)_serviceProvider.GetRequiredService<INavigateService>();
                //navigateService.SetNavigateService(mainViewModel);

                var mainWindow = new MainWindow(_serviceProvider.GetRequiredService<AuthenticationService>());
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