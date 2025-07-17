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
                    options.UseSqlServer("Server=localhost,1433;Initial Catalog=Hospital_Management;User ID=sa;Password=1;TrustServerCertificate=True;"));
                services.AddScoped<SystemUserRepository>();
                services.AddScoped<EmailPasswordResetRepository>();
                services.AddScoped<DoctorScheduleRepository>();
                services.AddScoped<AttendanceRepository>();
                services.AddScoped<AuthenticationService>();
                services.AddScoped<EmailService>();
                services.AddTransient<LoginView>();
                services.AddTransient<MainWindow>();
                services.AddScoped<DoctorScheduleService>();
                services.AddScoped<UserRoleService>();
                services.AddTransient<AssignRoleView>();
                services.AddScoped<AttendanceService>();
                services.AddTransient<AttendanceTrackingView>();


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