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
                services.AddScoped<DoctorRepository>();
                services.AddScoped<TimeKeepingRepository>();
                services.AddScoped<RewardPenaltyRepository>();

                services.AddTransient<LoginView>();
                services.AddTransient<MainWindow>();
                services.AddTransient<AttendanceTrackingView>();
                services.AddTransient<AssignRoleView>();
                services.AddTransient<DoctorWorkReportView>();

                services.AddScoped<AuthenticationService>();
                services.AddScoped<EmailService>();
                services.AddScoped<UserRoleService>();
                services.AddScoped<DoctorScheduleService>();
                services.AddScoped<AttendanceService>();
                services.AddScoped<WorkReportService>();

                _serviceProvider = services.BuildServiceProvider();

                //var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                //var navigateService = (NavigateService)_serviceProvider.GetRequiredService<INavigateService>();
                //navigateService.SetNavigateService(mainViewModel);

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