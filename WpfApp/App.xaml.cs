using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;
using ViewModel;

namespace WpfApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        
        public App()
        {
            var services = new ServiceCollection();

            services.AddDbContext<HospitalManagementContext>(options => options.UseSqlServer("Data Source=3H;Initial Catalog=Hospital_Management;User ID=sa;Password=3H452004hh@;TrustServerCertificate=True;"));
            services.AddScoped<SystemUserRepository>();
            services.AddScoped<EmailPasswordResetRepository>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<EmailService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<INavigateService, NavigateService>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<ForgetPasswordViewModel>();
            services.AddTransient<ResetPasswordViewModel>();
            _serviceProvider = services.BuildServiceProvider();

        }
    }

}
