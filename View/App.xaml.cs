using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Navigation;
using View;

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
                    options.UseSqlServer("Server=localhost,1433;Initial Catalog=Hospital_Management;User ID=sa;Password=Phuchaomb#16;TrustServerCertificate=True;"));
                services.AddScoped<SystemUserRepository>();
                services.AddScoped<EmailPasswordResetRepository>();
                services.AddScoped<DoctorScheduleRepository>();
                services.AddScoped<AppointmentRepository>();
                services.AddScoped<MedicalRecordRepository>();
                services.AddScoped<SpecializationRepository>();
                services.AddScoped<PatientRepository>();
                services.AddScoped<AuthenticationService>();
                services.AddScoped<EmailService>();
                services.AddScoped<AppointmentService>();
                services.AddScoped<MedicalRecordService>();
                services.AddScoped<PatientService>();
                services.AddScoped<DoctorScheduleService>();
                services.AddTransient<LoginView>();
                services.AddTransient<MainWindow>();
                services.AddScoped<DoctorScheduleService>();
                services.AddTransient<AdminDashboardView>();
                services.AddTransient<ReceptionistDashboardView>();
                services.AddTransient<CreateAppointmentView>();
                services.AddTransient<DoctorDashboardView>();
                services.AddTransient<CreateMedicalRecordView>();
                services.AddTransient<MedicalRecordsView>();
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