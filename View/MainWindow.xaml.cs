using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Service;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(AuthenticationService authenticationService)
        {
            InitializeComponent();
            //MainContent.Content = new LoginView(authenticationService);
            //Content = new ResetPasswordView(authenticationService, "acsacac");
            //MainContent.Content = new DoctorSchedulesView(doctorScheduleService);
            MainContent.Content = new PendingDayOffRequestsView(App._serviceProvider.GetRequiredService<DoctorScheduleService>());
            
        }
    }
}