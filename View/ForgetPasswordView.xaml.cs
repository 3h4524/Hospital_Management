using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Service;

namespace View
{

    public partial class ForgetPasswordView : UserControl, INotifyPropertyChanged
    {
        private AuthenticationService _authenticationService;
        private string _email;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private bool SetPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        public string Email
        {
            get => _email;
            set
            {
                if(SetPropertyChanged(ref _email, value))
                {
                    MessageBox.Text = ValidateEmail(value);
                }
            }
        }
        public ForgetPasswordView(AuthenticationService authenticationService)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            DataContext = this;
        }
     

        private void BackToLogin(object sender, EventArgs e)
        {
            Window mainWindow = Window.GetWindow(this);
            mainWindow.Content = new LoginView(_authenticationService);
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ValidateEmail(Email)))
            {
                MessageBox.Text = "Please fix errors before receive reset code.";
                return;
            }

            bool success = await _authenticationService.ForgetPassword(Email);

            if (success)
            {
                MessageBox.Text = "Reset password code sent successful";
                Window mainWindow = Window.GetWindow(this);
                mainWindow.Content = new ResetPasswordView(_authenticationService, _email); 
            } else
            {
                MessageBox.Text = "Please check correct email.";
            }
        }

        private string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return "Email cannot be empty";
            } else if (!Regex.IsMatch(email ,@"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return "Email is invalid";
            }

            return "";
        }
    }
}
