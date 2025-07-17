using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Enum;
using Microsoft.Extensions.DependencyInjection;
using Service;
using ViewModel;

namespace View
{
    public partial class LoginView : UserControl
    {
        AuthenticationService _authenticationService;

        public LoginView(AuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
        }


        private void OnForgotPasswordClick(object sender, MouseButtonEventArgs e)
        {
            Window main = Window.GetWindow(this);
            main.Content = new ForgetPasswordView(_authenticationService);
        }

        private void OnRegisterClick(object sender, MouseButtonEventArgs e)
        {
            Window main = Window.GetWindow(this);
            main.Content = new RegisterView(_authenticationService);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;

            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email or password can not empty", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Email must be containt @ and .", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = await _authenticationService.Login(email, password);

            if(user == null)
            {
                MessageBox.Show("Invalid email or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else
            {
                Application.Current.Properties["CurrentUser"] = user;

                if (user.Role == UserRole.Doctor.ToString())
                {
                    Window main = Window.GetWindow(this);
                    main.Content = new DoctorSchedulesView(App._serviceProvider.GetRequiredService<DoctorScheduleService>());

                } else if  (user.Role == UserRole.Admin.ToString())
                {
                    Window main = Window.GetWindow(this);
                    main.Content = new AttendanceTrackingView(App._serviceProvider.GetRequiredService<AttendanceService>());

                    //Window main = Window.GetWindow(this);
                    //main.Content = new AssignRoleView(App._serviceProvider.GetRequiredService<UserRoleService>());
                } else
                {

                    MessageBox.Show("Hahaha Tao La Nhat", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}