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
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                var services = new ServiceCollection();
                services.AddDbContext<HospitalManagementContext>(options =>
                    options.UseSqlServer("server=(local);database=HOSPITAL_MANAGEMENT;uid=sa;pwd=1;encrypt=true;trustServerCertificate=true"));
                services.AddScoped<SystemUserRepository>();
                services.AddScoped<EmailPasswordResetRepository>();
                services.AddScoped<DoctorScheduleRepository>();
                services.AddScoped<AuthenticationService>();
                services.AddScoped<EmailService>();
                services.AddScoped<DoctorScheduleService>();
                services.AddScoped<UserRoleService>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<INavigateService, NavigateService>();
                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegisterViewModel>();
                services.AddTransient<ForgetPasswordViewModel>();
                services.AddTransient<ResetPasswordViewModel>();
                services.AddTransient<DoctorSchedulesViewModel>();
                services.AddTransient<AssignRoleView>();
                services.AddTransient<AttendanceTrackingView>();

                _serviceProvider = services.BuildServiceProvider();
                //var assignRoleView = _serviceProvider.GetRequiredService<AssignRoleView>();
                //var mainWindow = new MainWindow(assignRoleView as AssignRoleView);

                var attendanceTrackingView = _serviceProvider.GetRequiredService<AttendanceTrackingView>();
                var mainWindow = new MainWindow(attendanceTrackingView as AttendanceTrackingView);

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